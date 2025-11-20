using AutoMapper;
using HRManagement.Data;
using HRManagement.Models.DTOs;
using HRManagement.Models.Entities;
using HRManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Services.Implementations
{
    public class TaxService : ITaxService
    {
        private readonly HRContext _context;
        private readonly IMapper _mapper;

        public TaxService(HRContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region Tax Configuration Management

        public async Task<PagedResponse<TaxConfigurationDto>> GetAllTaxConfigurationsAsync(
            long organizationId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.TaxConfigurations
                .AsNoTracking()
                .Where(tc => tc.OrganizationId == organizationId)
                .OrderByDescending(tc => tc.FinancialYear)
                .ThenBy(tc => tc.ConfigurationName);

            var totalCount = await query.CountAsync();

            var configs = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(tc => tc.TaxSlabs.Where(ts => ts.IsActive).OrderBy(ts => ts.DisplayOrder))
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<TaxConfigurationDto>>(configs);

            return new PagedResponse<TaxConfigurationDto>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<TaxConfigurationDto> GetTaxConfigurationByIdAsync(long taxConfigurationId)
        {
            var config = await _context.TaxConfigurations
                .AsNoTracking()
                .Include(tc => tc.TaxSlabs.Where(ts => ts.IsActive).OrderBy(ts => ts.DisplayOrder))
                .FirstOrDefaultAsync(tc => tc.TaxConfigurationId == taxConfigurationId);

            if (config == null)
                throw new InvalidOperationException($"Tax configuration {taxConfigurationId} not found");

            return _mapper.Map<TaxConfigurationDto>(config);
        }

        public async Task<TaxConfigurationDto?> GetActiveTaxConfigurationAsync(long organizationId)
        {
            var config = await _context.TaxConfigurations
                .AsNoTracking()
                .Where(tc => tc.OrganizationId == organizationId && tc.IsActive)
                .OrderByDescending(tc => tc.FinancialYear)
                .ThenByDescending(tc => tc.UpdatedAt)
                .Include(tc => tc.TaxSlabs.Where(ts => ts.IsActive).OrderBy(ts => ts.DisplayOrder))
                .FirstOrDefaultAsync();

            if (config == null)
                return null;

            return _mapper.Map<TaxConfigurationDto>(config);
        }

        public async Task<long> CreateTaxConfigurationAsync(CreateTaxConfigurationRequest request)
        {
            // Validate organization exists
            var org = await _context.Organizations.FindAsync(request.OrganizationId);
            if (org == null)
                throw new InvalidOperationException($"Organization {request.OrganizationId} not found");

            // Check for duplicate configuration name within organization and year
            var exists = await _context.TaxConfigurations
                .AnyAsync(tc => tc.OrganizationId == request.OrganizationId &&
                               tc.FinancialYear == request.FinancialYear &&
                               tc.ConfigurationName == request.ConfigurationName);
            if (exists)
                throw new InvalidOperationException(
                    $"Tax configuration '{request.ConfigurationName}' already exists for year {request.FinancialYear}");

            var config = _mapper.Map<TaxConfiguration>(request);
            config.CreatedAt = DateTime.UtcNow;
            config.UpdatedAt = DateTime.UtcNow;

            _context.TaxConfigurations.Add(config);
            await _context.SaveChangesAsync();

            return config.TaxConfigurationId;
        }

        public async Task UpdateTaxConfigurationAsync(long taxConfigurationId, UpdateTaxConfigurationRequest request)
        {
            var config = await _context.TaxConfigurations.FindAsync(taxConfigurationId);
            if (config == null)
                throw new InvalidOperationException($"Tax configuration {taxConfigurationId} not found");

            config.ConfigurationName = request.ConfigurationName;
            config.FinancialYear = request.FinancialYear;
            config.Country = request.Country;
            config.Region = request.Region;
            config.StandardTaxRate = request.StandardTaxRate;
            config.MinimumTaxableIncome = request.MinimumTaxableIncome;
            config.MonthlyTaxExemption = request.MonthlyTaxExemption;
            config.UseProgressiveTax = request.UseProgressiveTax;
            config.IsActive = request.IsActive;
            config.UpdatedAt = DateTime.UtcNow;

            _context.TaxConfigurations.Update(config);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTaxConfigurationAsync(long taxConfigurationId)
        {
            var config = await _context.TaxConfigurations.FindAsync(taxConfigurationId);
            if (config == null)
                throw new InvalidOperationException($"Tax configuration {taxConfigurationId} not found");

            // Delete associated tax slabs
            var slabs = await _context.TaxSlabs
                .Where(ts => ts.TaxConfigurationId == taxConfigurationId)
                .ToListAsync();

            _context.TaxSlabs.RemoveRange(slabs);
            _context.TaxConfigurations.Remove(config);
            await _context.SaveChangesAsync();
        }

        #endregion

        #region Tax Slab Management

        public async Task<List<TaxSlabDto>> GetTaxSlabsByConfigurationAsync(long taxConfigurationId)
        {
            var config = await _context.TaxConfigurations.FindAsync(taxConfigurationId);
            if (config == null)
                throw new InvalidOperationException($"Tax configuration {taxConfigurationId} not found");

            var slabs = await _context.TaxSlabs
                .AsNoTracking()
                .Where(ts => ts.TaxConfigurationId == taxConfigurationId && ts.IsActive)
                .OrderBy(ts => ts.DisplayOrder)
                .ToListAsync();

            return _mapper.Map<List<TaxSlabDto>>(slabs);
        }

        public async Task<TaxSlabDto> GetTaxSlabByIdAsync(long taxSlabId)
        {
            var slab = await _context.TaxSlabs
                .AsNoTracking()
                .FirstOrDefaultAsync(ts => ts.TaxSlabId == taxSlabId);

            if (slab == null)
                throw new InvalidOperationException($"Tax slab {taxSlabId} not found");

            return _mapper.Map<TaxSlabDto>(slab);
        }

        public async Task<long> CreateTaxSlabAsync(CreateTaxSlabRequest request)
        {
            // Validate configuration exists
            var config = await _context.TaxConfigurations.FindAsync(request.TaxConfigurationId);
            if (config == null)
                throw new InvalidOperationException($"Tax configuration {request.TaxConfigurationId} not found");

            // Validate FromAmount <= ToAmount
            if (request.FromAmount > request.ToAmount)
                throw new InvalidOperationException("FromAmount cannot be greater than ToAmount");

            // Validate tax rate is between 0 and 100
            if (request.TaxRate < 0 || request.TaxRate > 100)
                throw new InvalidOperationException("Tax rate must be between 0 and 100");

            // Check for overlapping slabs
            var overlaps = await _context.TaxSlabs
                .AnyAsync(ts => ts.TaxConfigurationId == request.TaxConfigurationId &&
                               ts.IsActive &&
                               ts.FromAmount < request.ToAmount &&
                               ts.ToAmount > request.FromAmount);
            if (overlaps)
                throw new InvalidOperationException("Tax slab overlaps with existing slabs");

            var slab = _mapper.Map<TaxSlab>(request);
            slab.CreatedAt = DateTime.UtcNow;
            slab.UpdatedAt = DateTime.UtcNow;
            slab.IsActive = true;

            _context.TaxSlabs.Add(slab);
            await _context.SaveChangesAsync();

            return slab.TaxSlabId;
        }

        public async Task UpdateTaxSlabAsync(long taxSlabId, UpdateTaxSlabRequest request)
        {
            var slab = await _context.TaxSlabs.FindAsync(taxSlabId);
            if (slab == null)
                throw new InvalidOperationException($"Tax slab {taxSlabId} not found");

            // Validate FromAmount <= ToAmount
            if (request.FromAmount > request.ToAmount)
                throw new InvalidOperationException("FromAmount cannot be greater than ToAmount");

            // Validate tax rate
            if (request.TaxRate < 0 || request.TaxRate > 100)
                throw new InvalidOperationException("Tax rate must be between 0 and 100");

            // Check for overlapping slabs (excluding current one)
            var overlaps = await _context.TaxSlabs
                .AnyAsync(ts => ts.TaxSlabId != taxSlabId &&
                               ts.TaxConfigurationId == slab.TaxConfigurationId &&
                               ts.IsActive &&
                               ts.FromAmount < request.ToAmount &&
                               ts.ToAmount > request.FromAmount);
            if (overlaps)
                throw new InvalidOperationException("Updated slab would overlap with existing slabs");

            slab.FromAmount = request.FromAmount;
            slab.ToAmount = request.ToAmount;
            slab.TaxRate = request.TaxRate;
            slab.DisplayOrder = request.DisplayOrder;
            slab.IsActive = request.IsActive;
            slab.UpdatedAt = DateTime.UtcNow;

            _context.TaxSlabs.Update(slab);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTaxSlabAsync(long taxSlabId)
        {
            var slab = await _context.TaxSlabs.FindAsync(taxSlabId);
            if (slab == null)
                throw new InvalidOperationException($"Tax slab {taxSlabId} not found");

            _context.TaxSlabs.Remove(slab);
            await _context.SaveChangesAsync();
        }

        #endregion

        #region Tax Calculations

        public async Task<decimal> CalculateIncomeTaxAsync(long taxConfigurationId, decimal grossSalary, int monthsInPayroll = 1)
        {
            var config = await _context.TaxConfigurations
                .Include(tc => tc.TaxSlabs.Where(ts => ts.IsActive).OrderBy(ts => ts.DisplayOrder))
                .FirstOrDefaultAsync(tc => tc.TaxConfigurationId == taxConfigurationId);

            if (config == null)
                throw new InvalidOperationException($"Tax configuration {taxConfigurationId} not found");

            // If salary is below minimum taxable income, no tax
            if (grossSalary < config.MinimumTaxableIncome)
                return 0;

            // Apply monthly tax exemption
            decimal taxableIncome = grossSalary - (config.MonthlyTaxExemption * monthsInPayroll);
            if (taxableIncome <= 0)
                return 0;

            // Use progressive tax slabs if configured, otherwise use standard rate
            if (config.UseProgressiveTax && config.TaxSlabs.Any())
            {
                return CalculateProgressiveTax(taxableIncome, config.TaxSlabs.ToList());
            }
            else
            {
                // Simple percentage-based tax
                return (taxableIncome * config.StandardTaxRate) / 100;
            }
        }

        public async Task<decimal> GetApplicableTaxRateAsync(long taxConfigurationId, decimal income)
        {
            var config = await _context.TaxConfigurations
                .Include(tc => tc.TaxSlabs.Where(ts => ts.IsActive).OrderBy(ts => ts.DisplayOrder))
                .FirstOrDefaultAsync(tc => tc.TaxConfigurationId == taxConfigurationId);

            if (config == null)
                throw new InvalidOperationException($"Tax configuration {taxConfigurationId} not found");

            if (!config.UseProgressiveTax || !config.TaxSlabs.Any())
                return config.StandardTaxRate;

            // Find the applicable slab for this income
            var applicableSlab = config.TaxSlabs
                .FirstOrDefault(ts => income >= ts.FromAmount && income <= ts.ToAmount);

            return applicableSlab?.TaxRate ?? config.StandardTaxRate;
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Calculate tax using progressive tax slabs
        /// Applies marginal tax calculation where different portions of income are taxed at different rates
        /// </summary>
        private decimal CalculateProgressiveTax(decimal taxableIncome, List<TaxSlab> slabs)
        {
            if (!slabs.Any() || taxableIncome <= 0)
                return 0;

            decimal totalTax = 0;
            decimal remainingIncome = taxableIncome;

            foreach (var slab in slabs)
            {
                if (remainingIncome <= 0)
                    break;

                // Calculate income portion within this slab
                decimal incomeInThisSlab;

                if (taxableIncome >= slab.ToAmount)
                {
                    // Full slab range is taxable
                    incomeInThisSlab = slab.ToAmount - slab.FromAmount;
                }
                else if (taxableIncome > slab.FromAmount)
                {
                    // Partial slab is taxable
                    incomeInThisSlab = taxableIncome - slab.FromAmount;
                }
                else
                {
                    // Income doesn't reach this slab
                    incomeInThisSlab = 0;
                }

                if (incomeInThisSlab > 0)
                {
                    totalTax += (incomeInThisSlab * slab.TaxRate) / 100;
                }
            }

            return totalTax;
        }

        #endregion
    }
}
