using AutoMapper;
using Microsoft.EntityFrameworkCore;
using HRManagement.Data;
using HRManagement.Models.DTOs;
using HRManagement.Models.Entities;
using HRManagement.Services.Interfaces;

namespace HRManagement.Services.Implementations
{
    /// <summary>
    /// Implementation of benefits management service
    /// </summary>
    public class BenefitsService : IBenefitsService
    {
        private readonly HRContext _context;
        private readonly IMapper _mapper;

        public BenefitsService(HRContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // ==================== BenefitType Operations ====================
        public async Task<PagedResponse<BenefitTypeDto>> GetAllBenefitTypesAsync(long organizationId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.BenefitTypes
                .Where(bt => bt.OrganizationId == organizationId && bt.IsActive)
                .AsNoTracking();

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(bt => bt.TypeName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = new List<BenefitTypeDto>();
            foreach (var item in items)
            {
                var dto = _mapper.Map<BenefitTypeDto>(item);
                dto.PlanCount = await _context.BenefitPlans
                    .Where(bp => bp.BenefitTypeId == item.BenefitTypeId && bp.IsActive)
                    .CountAsync();
                dtos.Add(dto);
            }

            return new PagedResponse<BenefitTypeDto>
            {
                Items = dtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<BenefitTypeDto> GetBenefitTypeByIdAsync(long benefitTypeId)
        {
            var benefitType = await _context.BenefitTypes
                .FirstOrDefaultAsync(bt => bt.BenefitTypeId == benefitTypeId);

            if (benefitType == null)
                throw new Exception($"Benefit type with ID {benefitTypeId} not found");

            var dto = _mapper.Map<BenefitTypeDto>(benefitType);
            dto.PlanCount = await _context.BenefitPlans
                .Where(bp => bp.BenefitTypeId == benefitTypeId && bp.IsActive)
                .CountAsync();

            return dto;
        }

        public async Task<BenefitTypeDto> CreateBenefitTypeAsync(long organizationId, CreateBenefitTypeRequest request)
        {
            var benefitType = new BenefitType
            {
                OrganizationId = organizationId,
                TypeName = request.TypeName,
                Description = request.Description,
                Category = request.Category,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.BenefitTypes.Add(benefitType);
            await _context.SaveChangesAsync();

            return await GetBenefitTypeByIdAsync(benefitType.BenefitTypeId);
        }

        public async Task<BenefitTypeDto> UpdateBenefitTypeAsync(long benefitTypeId, UpdateBenefitTypeRequest request)
        {
            var benefitType = await _context.BenefitTypes
                .FirstOrDefaultAsync(bt => bt.BenefitTypeId == benefitTypeId);

            if (benefitType == null)
                throw new Exception($"Benefit type with ID {benefitTypeId} not found");

            benefitType.TypeName = request.TypeName;
            benefitType.Description = request.Description;
            benefitType.Category = request.Category;
            benefitType.IsActive = request.IsActive;
            benefitType.UpdatedAt = DateTime.UtcNow;

            _context.BenefitTypes.Update(benefitType);
            await _context.SaveChangesAsync();

            return await GetBenefitTypeByIdAsync(benefitTypeId);
        }

        public async Task<bool> DeleteBenefitTypeAsync(long benefitTypeId)
        {
            var benefitType = await _context.BenefitTypes
                .FirstOrDefaultAsync(bt => bt.BenefitTypeId == benefitTypeId);

            if (benefitType == null)
                throw new Exception($"Benefit type with ID {benefitTypeId} not found");

            _context.BenefitTypes.Remove(benefitType);
            await _context.SaveChangesAsync();

            return true;
        }

        // ==================== BenefitPlan Operations ====================
        public async Task<PagedResponse<BenefitPlanDto>> GetPlansByBenefitTypeAsync(long benefitTypeId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.BenefitPlans
                .Where(bp => bp.BenefitTypeId == benefitTypeId && bp.IsActive)
                .Include(bp => bp.BenefitType)
                .AsNoTracking();

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(bp => bp.DisplayOrder)
                .ThenBy(bp => bp.PlanName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = new List<BenefitPlanDto>();
            foreach (var item in items)
            {
                var dto = _mapper.Map<BenefitPlanDto>(item);
                dto.EnrolledEmployees = await _context.EmployeeBenefits
                    .Where(eb => eb.BenefitPlanId == item.BenefitPlanId && eb.IsActive)
                    .CountAsync();
                dto.DeductionCount = await _context.BenefitDeductions
                    .Where(bd => bd.BenefitPlanId == item.BenefitPlanId && bd.IsActive)
                    .CountAsync();
                dtos.Add(dto);
            }

            return new PagedResponse<BenefitPlanDto>
            {
                Items = dtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
        }

        public async Task<PagedResponse<BenefitPlanDto>> GetAllBenefitPlansAsync(long organizationId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.BenefitPlans
                .Where(bp => bp.OrganizationId == organizationId && bp.IsActive)
                .Include(bp => bp.BenefitType)
                .AsNoTracking();

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(bp => bp.DisplayOrder)
                .ThenBy(bp => bp.PlanName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = new List<BenefitPlanDto>();
            foreach (var item in items)
            {
                var dto = _mapper.Map<BenefitPlanDto>(item);
                dto.EnrolledEmployees = await _context.EmployeeBenefits
                    .Where(eb => eb.BenefitPlanId == item.BenefitPlanId && eb.IsActive)
                    .CountAsync();
                dto.DeductionCount = await _context.BenefitDeductions
                    .Where(bd => bd.BenefitPlanId == item.BenefitPlanId && bd.IsActive)
                    .CountAsync();
                dtos.Add(dto);
            }

            return new PagedResponse<BenefitPlanDto>
            {
                Items = dtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
        }

        public async Task<BenefitPlanDto> GetBenefitPlanByIdAsync(long benefitPlanId)
        {
            var benefitPlan = await _context.BenefitPlans
                .Include(bp => bp.BenefitType)
                .FirstOrDefaultAsync(bp => bp.BenefitPlanId == benefitPlanId);

            if (benefitPlan == null)
                throw new Exception($"Benefit plan with ID {benefitPlanId} not found");

            var dto = _mapper.Map<BenefitPlanDto>(benefitPlan);
            dto.EnrolledEmployees = await _context.EmployeeBenefits
                .Where(eb => eb.BenefitPlanId == benefitPlanId && eb.IsActive)
                .CountAsync();
            dto.DeductionCount = await _context.BenefitDeductions
                .Where(bd => bd.BenefitPlanId == benefitPlanId && bd.IsActive)
                .CountAsync();

            return dto;
        }

        public async Task<BenefitPlanDto> CreateBenefitPlanAsync(long organizationId, CreateBenefitPlanRequest request)
        {
            var benefitPlan = new BenefitPlan
            {
                BenefitTypeId = request.BenefitTypeId,
                OrganizationId = organizationId,
                PlanName = request.PlanName,
                Description = request.Description,
                PlanCode = request.PlanCode,
                EmployeeContribution = request.EmployeeContribution,
                EmployerContribution = request.EmployerContribution,
                ContributionFrequency = request.ContributionFrequency,
                EffectiveDate = request.EffectiveDate,
                EndDate = request.EndDate,
                CoverageAmount = request.CoverageAmount,
                CoverageDetails = request.CoverageDetails,
                IsActive = true,
                IsDefaultPlan = request.IsDefaultPlan,
                DisplayOrder = request.DisplayOrder,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.BenefitPlans.Add(benefitPlan);
            await _context.SaveChangesAsync();

            return await GetBenefitPlanByIdAsync(benefitPlan.BenefitPlanId);
        }

        public async Task<BenefitPlanDto> UpdateBenefitPlanAsync(long benefitPlanId, UpdateBenefitPlanRequest request)
        {
            var benefitPlan = await _context.BenefitPlans
                .FirstOrDefaultAsync(bp => bp.BenefitPlanId == benefitPlanId);

            if (benefitPlan == null)
                throw new Exception($"Benefit plan with ID {benefitPlanId} not found");

            benefitPlan.PlanName = request.PlanName;
            benefitPlan.Description = request.Description;
            benefitPlan.PlanCode = request.PlanCode;
            benefitPlan.EmployeeContribution = request.EmployeeContribution;
            benefitPlan.EmployerContribution = request.EmployerContribution;
            benefitPlan.ContributionFrequency = request.ContributionFrequency;
            benefitPlan.EffectiveDate = request.EffectiveDate;
            benefitPlan.EndDate = request.EndDate;
            benefitPlan.CoverageAmount = request.CoverageAmount;
            benefitPlan.CoverageDetails = request.CoverageDetails;
            benefitPlan.IsActive = request.IsActive;
            benefitPlan.IsDefaultPlan = request.IsDefaultPlan;
            benefitPlan.DisplayOrder = request.DisplayOrder;
            benefitPlan.UpdatedAt = DateTime.UtcNow;

            _context.BenefitPlans.Update(benefitPlan);
            await _context.SaveChangesAsync();

            return await GetBenefitPlanByIdAsync(benefitPlanId);
        }

        public async Task<bool> DeleteBenefitPlanAsync(long benefitPlanId)
        {
            var benefitPlan = await _context.BenefitPlans
                .FirstOrDefaultAsync(bp => bp.BenefitPlanId == benefitPlanId);

            if (benefitPlan == null)
                throw new Exception($"Benefit plan with ID {benefitPlanId} not found");

            _context.BenefitPlans.Remove(benefitPlan);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<PagedResponse<BenefitPlanDto>> GetActiveBenefitPlansAsync(long organizationId, int pageNumber = 1, int pageSize = 10)
        {
            var now = DateTime.UtcNow;
            var query = _context.BenefitPlans
                .Where(bp => bp.OrganizationId == organizationId &&
                             bp.IsActive &&
                             bp.EffectiveDate <= now &&
                             (bp.EndDate == null || bp.EndDate >= now))
                .Include(bp => bp.BenefitType)
                .AsNoTracking();

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(bp => bp.DisplayOrder)
                .ThenBy(bp => bp.PlanName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = items.Select(_mapper.Map<BenefitPlanDto>).ToList();

            return new PagedResponse<BenefitPlanDto>
            {
                Items = dtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
        }

        // ==================== EmployeeBenefit Operations ====================
        public async Task<PagedResponse<EmployeeBenefitDto>> GetEmployeeBenefitsAsync(long employeeId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.EmployeeBenefits
                .Where(eb => eb.EmployeeId == employeeId && eb.IsActive)
                .Include(eb => eb.Employee)
                .Include(eb => eb.BenefitPlan)
                .ThenInclude(bp => bp.BenefitType)
                .AsNoTracking();

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(eb => eb.EnrolledDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = items.Select(_mapper.Map<EmployeeBenefitDto>).ToList();

            return new PagedResponse<EmployeeBenefitDto>
            {
                Items = dtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
        }

        public async Task<PagedResponse<EmployeeBenefitDto>> GetPlanEnrolleesAsync(long benefitPlanId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.EmployeeBenefits
                .Where(eb => eb.BenefitPlanId == benefitPlanId && eb.IsActive)
                .Include(eb => eb.Employee)
                .Include(eb => eb.BenefitPlan)
                .ThenInclude(bp => bp.BenefitType)
                .AsNoTracking();

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(eb => eb.Employee.FirstName)
                .ThenBy(eb => eb.Employee.LastName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = items.Select(_mapper.Map<EmployeeBenefitDto>).ToList();

            return new PagedResponse<EmployeeBenefitDto>
            {
                Items = dtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
        }

        public async Task<EmployeeBenefitDto> GetEmployeeBenefitByIdAsync(long employeeBenefitId)
        {
            var employeeBenefit = await _context.EmployeeBenefits
                .Include(eb => eb.Employee)
                .Include(eb => eb.BenefitPlan)
                .ThenInclude(bp => bp.BenefitType)
                .FirstOrDefaultAsync(eb => eb.EmployeeBenefitId == employeeBenefitId);

            if (employeeBenefit == null)
                throw new Exception($"Employee benefit with ID {employeeBenefitId} not found");

            return _mapper.Map<EmployeeBenefitDto>(employeeBenefit);
        }

        public async Task<EmployeeBenefitDto> EnrollEmployeeBenefitAsync(CreateEmployeeBenefitRequest request)
        {
            // Verify employee exists
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeId == request.EmployeeId);
            if (employee == null)
                throw new Exception($"Employee with ID {request.EmployeeId} not found");

            // Verify benefit plan exists
            var benefitPlan = await _context.BenefitPlans.FirstOrDefaultAsync(bp => bp.BenefitPlanId == request.BenefitPlanId);
            if (benefitPlan == null)
                throw new Exception($"Benefit plan with ID {request.BenefitPlanId} not found");

            // Check if employee is already enrolled
            var existing = await _context.EmployeeBenefits
                .FirstOrDefaultAsync(eb => eb.EmployeeId == request.EmployeeId &&
                                           eb.BenefitPlanId == request.BenefitPlanId &&
                                           eb.IsActive);

            if (existing != null)
                throw new Exception($"Employee is already enrolled in this benefit plan");

            var employeeBenefit = new EmployeeBenefit
            {
                EmployeeId = request.EmployeeId,
                BenefitPlanId = request.BenefitPlanId,
                EnrolledDate = request.EnrolledDate,
                EnrollmentStatus = "Active",
                EmployeeContributionOverride = request.EmployeeContributionOverride,
                EmployerContributionOverride = request.EmployerContributionOverride,
                BeneficiaryInfo = request.BeneficiaryInfo,
                Remarks = request.Remarks,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.EmployeeBenefits.Add(employeeBenefit);
            await _context.SaveChangesAsync();

            return await GetEmployeeBenefitByIdAsync(employeeBenefit.EmployeeBenefitId);
        }

        public async Task<EmployeeBenefitDto> UpdateEmployeeBenefitAsync(long employeeBenefitId, UpdateEmployeeBenefitRequest request)
        {
            var employeeBenefit = await _context.EmployeeBenefits
                .FirstOrDefaultAsync(eb => eb.EmployeeBenefitId == employeeBenefitId);

            if (employeeBenefit == null)
                throw new Exception($"Employee benefit with ID {employeeBenefitId} not found");

            employeeBenefit.EnrolledDate = request.EnrolledDate;
            employeeBenefit.TerminationDate = request.TerminationDate;
            employeeBenefit.EnrollmentStatus = request.EnrollmentStatus;
            employeeBenefit.EmployeeContributionOverride = request.EmployeeContributionOverride;
            employeeBenefit.EmployerContributionOverride = request.EmployerContributionOverride;
            employeeBenefit.BeneficiaryInfo = request.BeneficiaryInfo;
            employeeBenefit.UsedAmount = request.UsedAmount;
            employeeBenefit.ClaimNotes = request.ClaimNotes;
            employeeBenefit.Remarks = request.Remarks;
            employeeBenefit.IsActive = request.IsActive;
            employeeBenefit.UpdatedAt = DateTime.UtcNow;

            _context.EmployeeBenefits.Update(employeeBenefit);
            await _context.SaveChangesAsync();

            return await GetEmployeeBenefitByIdAsync(employeeBenefitId);
        }

        public async Task<bool> TerminateEmployeeBenefitAsync(long employeeBenefitId, DateTime terminationDate)
        {
            var employeeBenefit = await _context.EmployeeBenefits
                .FirstOrDefaultAsync(eb => eb.EmployeeBenefitId == employeeBenefitId);

            if (employeeBenefit == null)
                throw new Exception($"Employee benefit with ID {employeeBenefitId} not found");

            employeeBenefit.TerminationDate = terminationDate;
            employeeBenefit.EnrollmentStatus = "Terminated";
            employeeBenefit.IsActive = false;
            employeeBenefit.UpdatedAt = DateTime.UtcNow;

            _context.EmployeeBenefits.Update(employeeBenefit);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<EmployeeBenefitDto>> GetActiveEmployeeBenefitsAsync(long employeeId, DateTime onDate)
        {
            var benefits = await _context.EmployeeBenefits
                .Where(eb => eb.EmployeeId == employeeId &&
                             eb.EnrolledDate <= onDate &&
                             (eb.TerminationDate == null || eb.TerminationDate >= onDate) &&
                             eb.IsActive)
                .Include(eb => eb.Employee)
                .Include(eb => eb.BenefitPlan)
                .ThenInclude(bp => bp.BenefitType)
                .AsNoTracking()
                .ToListAsync();

            return benefits.Select(_mapper.Map<EmployeeBenefitDto>).ToList();
        }

        public async Task<PagedResponse<EmployeeBenefitDto>> GetEmployeeBenefitHistoryAsync(long employeeId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.EmployeeBenefits
                .Where(eb => eb.EmployeeId == employeeId)
                .Include(eb => eb.Employee)
                .Include(eb => eb.BenefitPlan)
                .ThenInclude(bp => bp.BenefitType)
                .AsNoTracking();

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(eb => eb.EnrolledDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = items.Select(_mapper.Map<EmployeeBenefitDto>).ToList();

            return new PagedResponse<EmployeeBenefitDto>
            {
                Items = dtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
        }

        // ==================== BenefitDeduction Operations ====================
        public async Task<PagedResponse<BenefitDeductionDto>> GetDeductionsForPlanAsync(long benefitPlanId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.BenefitDeductions
                .Where(bd => bd.BenefitPlanId == benefitPlanId && bd.IsActive)
                .AsNoTracking();

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(bd => bd.DisplayOrder)
                .ThenBy(bd => bd.DeductionName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = items.Select(_mapper.Map<BenefitDeductionDto>).ToList();

            return new PagedResponse<BenefitDeductionDto>
            {
                Items = dtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
        }

        public async Task<BenefitDeductionDto> GetBenefitDeductionByIdAsync(long benefitDeductionId)
        {
            var benefitDeduction = await _context.BenefitDeductions
                .FirstOrDefaultAsync(bd => bd.BenefitDeductionId == benefitDeductionId);

            if (benefitDeduction == null)
                throw new Exception($"Benefit deduction with ID {benefitDeductionId} not found");

            return _mapper.Map<BenefitDeductionDto>(benefitDeduction);
        }

        public async Task<BenefitDeductionDto> CreateBenefitDeductionAsync(CreateBenefitDeductionRequest request)
        {
            var benefitDeduction = new BenefitDeduction
            {
                BenefitPlanId = request.BenefitPlanId,
                DeductionName = request.DeductionName,
                DeductionCode = request.DeductionCode,
                DeductionType = request.DeductionType,
                Amount = request.Amount,
                Percentage = request.Percentage,
                IsPercentageBased = request.IsPercentageBased,
                IsFixed = request.IsFixed,
                IsTaxable = request.IsTaxable,
                IsTaxDeductible = request.IsTaxDeductible,
                DeductionFrequency = request.DeductionFrequency,
                IsActive = true,
                DisplayOrder = request.DisplayOrder,
                EffectiveDate = request.EffectiveDate,
                EndDate = request.EndDate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.BenefitDeductions.Add(benefitDeduction);
            await _context.SaveChangesAsync();

            return await GetBenefitDeductionByIdAsync(benefitDeduction.BenefitDeductionId);
        }

        public async Task<BenefitDeductionDto> UpdateBenefitDeductionAsync(long benefitDeductionId, UpdateBenefitDeductionRequest request)
        {
            var benefitDeduction = await _context.BenefitDeductions
                .FirstOrDefaultAsync(bd => bd.BenefitDeductionId == benefitDeductionId);

            if (benefitDeduction == null)
                throw new Exception($"Benefit deduction with ID {benefitDeductionId} not found");

            benefitDeduction.DeductionName = request.DeductionName;
            benefitDeduction.DeductionCode = request.DeductionCode;
            benefitDeduction.DeductionType = request.DeductionType;
            benefitDeduction.Amount = request.Amount;
            benefitDeduction.Percentage = request.Percentage;
            benefitDeduction.IsPercentageBased = request.IsPercentageBased;
            benefitDeduction.IsFixed = request.IsFixed;
            benefitDeduction.IsTaxable = request.IsTaxable;
            benefitDeduction.IsTaxDeductible = request.IsTaxDeductible;
            benefitDeduction.DeductionFrequency = request.DeductionFrequency;
            benefitDeduction.IsActive = request.IsActive;
            benefitDeduction.DisplayOrder = request.DisplayOrder;
            benefitDeduction.EffectiveDate = request.EffectiveDate;
            benefitDeduction.EndDate = request.EndDate;
            benefitDeduction.UpdatedAt = DateTime.UtcNow;

            _context.BenefitDeductions.Update(benefitDeduction);
            await _context.SaveChangesAsync();

            return await GetBenefitDeductionByIdAsync(benefitDeductionId);
        }

        public async Task<bool> DeleteBenefitDeductionAsync(long benefitDeductionId)
        {
            var benefitDeduction = await _context.BenefitDeductions
                .FirstOrDefaultAsync(bd => bd.BenefitDeductionId == benefitDeductionId);

            if (benefitDeduction == null)
                throw new Exception($"Benefit deduction with ID {benefitDeductionId} not found");

            _context.BenefitDeductions.Remove(benefitDeduction);
            await _context.SaveChangesAsync();

            return true;
        }

        // ==================== Calculations and Analytics ====================
        public async Task<decimal> CalculateEmployeeBenefitCostAsync(long benefitPlanId)
        {
            var enrollees = await _context.EmployeeBenefits
                .Where(eb => eb.BenefitPlanId == benefitPlanId && eb.IsActive)
                .CountAsync();

            var plan = await _context.BenefitPlans.FirstOrDefaultAsync(bp => bp.BenefitPlanId == benefitPlanId);
            if (plan == null)
                return 0;

            return enrollees * plan.EmployeeContribution;
        }

        public async Task<decimal> CalculateEmployerBenefitCostAsync(long benefitPlanId)
        {
            var enrollees = await _context.EmployeeBenefits
                .Where(eb => eb.BenefitPlanId == benefitPlanId && eb.IsActive)
                .CountAsync();

            var plan = await _context.BenefitPlans.FirstOrDefaultAsync(bp => bp.BenefitPlanId == benefitPlanId);
            if (plan == null)
                return 0;

            return enrollees * plan.EmployerContribution;
        }

        public async Task<BenefitsSummaryDto> GetEmployeeBenefitsSummaryAsync(long employeeId)
        {
            var benefits = await GetActiveEmployeeBenefitsAsync(employeeId, DateTime.UtcNow);
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

            var totalEmployeeContribution = benefits.Sum(b =>
                b.EmployeeContributionOverride != 0 ? b.EmployeeContributionOverride : b.PlanEmployeeContribution);

            var totalEmployerContribution = benefits.Sum(b =>
                b.EmployerContributionOverride != 0 ? b.EmployerContributionOverride : b.PlanEmployerContribution);

            return new BenefitsSummaryDto
            {
                EmployeeId = employeeId,
                EmployeeName = employee != null ? $"{employee.FirstName} {employee.LastName}" : "",
                TotalActiveBenefits = benefits.Count,
                TotalEmployeeContribution = totalEmployeeContribution,
                TotalEmployerContribution = totalEmployerContribution,
                BenefitNames = benefits.Select(b => b.PlanName).ToList(),
                BenefitStatuses = benefits.Select(b => b.EnrollmentStatus).ToList()
            };
        }

        public async Task<BenefitsUtilizationReportDto> GetBenefitUtilizationReportAsync(long benefitPlanId)
        {
            var plan = await _context.BenefitPlans.FirstOrDefaultAsync(bp => bp.BenefitPlanId == benefitPlanId);
            if (plan == null)
                throw new Exception($"Benefit plan with ID {benefitPlanId} not found");

            var benefits = await _context.EmployeeBenefits
                .Where(eb => eb.BenefitPlanId == benefitPlanId)
                .ToListAsync();

            var totalEnrolled = benefits.Count;
            var activeEnrollees = benefits.Count(b => b.IsActive && b.EnrollmentStatus == "Active");
            var suspendedEnrollees = benefits.Count(b => b.IsActive && b.EnrollmentStatus == "Suspended");
            var terminatedEnrollees = benefits.Count(b => !b.IsActive || b.EnrollmentStatus == "Terminated");

            var totalClaimsAmount = benefits.Sum(b => b.UsedAmount);
            var totalCoverageAmount = activeEnrollees * plan.CoverageAmount;
            var utilizationPercentage = totalCoverageAmount > 0
                ? (totalClaimsAmount / totalCoverageAmount) * 100
                : 0;

            return new BenefitsUtilizationReportDto
            {
                BenefitPlanId = benefitPlanId,
                PlanName = plan.PlanName,
                TotalEnrolled = totalEnrolled,
                ActiveEnrollees = activeEnrollees,
                SuspendedEnrollees = suspendedEnrollees,
                TerminatedEnrollees = terminatedEnrollees,
                TotalClaimsAmount = totalClaimsAmount,
                TotalCoverageAmount = totalCoverageAmount,
                CoverageUtilizationPercentage = (decimal)utilizationPercentage,
                EmployeeContributionTotal = benefits.Sum(b =>
                    b.EmployeeContributionOverride.HasValue ? b.EmployeeContributionOverride.Value : plan.EmployeeContribution),
                EmployerContributionTotal = benefits.Sum(b =>
                    b.EmployerContributionOverride.HasValue ? b.EmployerContributionOverride.Value : plan.EmployerContribution)
            };
        }
    }
}
