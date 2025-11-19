using HRManagement.Data;
using HRManagement.Models.DTOs;
using HRManagement.Models.Entities;
using HRManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Services.Implementations
{
    public class ContractsService : IContractsService
    {
        private readonly HRContext _context;

        public ContractsService(HRContext context)
        {
            _context = context;
        }

        public async Task<PagedResponse<ContractDto>> GetAllContractsAsync(long organizationId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Contracts
                .Include(c => c.Employee)
                .Where(c => c.Employee.OrganizationId == organizationId);

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            var dtos = items.Select(c => MapContractToDto(c)).ToList();

            return new PagedResponse<ContractDto>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<PagedResponse<ContractDto>> GetEmployeeContractsAsync(long employeeId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Contracts
                .Include(c => c.Employee)
                .Where(c => c.EmployeeId == employeeId);

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            var dtos = items.Select(c => MapContractToDto(c)).ToList();

            return new PagedResponse<ContractDto>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<ContractDto> GetContractByIdAsync(long contractId)
        {
            var contract = await _context.Contracts
                .Include(c => c.Employee)
                .FirstOrDefaultAsync(c => c.ContractId == contractId);

            if (contract == null)
                throw new InvalidOperationException($"Contract {contractId} not found");

            return MapContractToDto(contract);
        }

        public async Task<long> CreateContractAsync(CreateContractRequest request)
        {
            // Validate employee
            var employee = await _context.Employees.FindAsync(request.EmployeeId);
            if (employee == null)
                throw new InvalidOperationException($"Employee {request.EmployeeId} not found");

            var contract = new Contract
            {
                EmployeeId = request.EmployeeId,
                ContractType = request.ContractType,
                FileName = request.FileName,
                FileUrl = request.FileUrl,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Terms = request.Terms,
                Status = "Draft",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();

            return contract.ContractId;
        }

        public async Task UpdateContractAsync(long contractId, UpdateContractRequest request)
        {
            var contract = await _context.Contracts.FindAsync(contractId);
            if (contract == null)
                throw new InvalidOperationException($"Contract {contractId} not found");

            contract.ContractType = request.ContractType;
            contract.FileName = request.FileName;
            contract.FileUrl = request.FileUrl;
            contract.StartDate = request.StartDate;
            contract.EndDate = request.EndDate;
            contract.Terms = request.Terms;
            contract.Status = request.Status;
            contract.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task SignContractAsync(long contractId, SignContractRequest request)
        {
            var contract = await _context.Contracts.FindAsync(contractId);
            if (contract == null)
                throw new InvalidOperationException($"Contract {contractId} not found");

            contract.Status = "Signed";
            contract.SignedDate = request.SignedDate;
            contract.SignedBy = request.SignedBy;
            contract.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task RenewContractAsync(long contractId, DateTime newEndDate)
        {
            var contract = await _context.Contracts.FindAsync(contractId);
            if (contract == null)
                throw new InvalidOperationException($"Contract {contractId} not found");

            contract.EndDate = newEndDate;
            contract.Status = "Renewed";
            contract.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task TerminateContractAsync(long contractId)
        {
            var contract = await _context.Contracts.FindAsync(contractId);
            if (contract == null)
                throw new InvalidOperationException($"Contract {contractId} not found");

            contract.Status = "Terminated";
            contract.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteContractAsync(long contractId)
        {
            var contract = await _context.Contracts.FindAsync(contractId);
            if (contract == null)
                throw new InvalidOperationException($"Contract {contractId} not found");

            _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResponse<ContractDto>> GetExpiringContractsAsync(long organizationId, int daysToExpire = 30, int pageNumber = 1, int pageSize = 10)
        {
            var expiryDate = DateTime.Today.AddDays(daysToExpire);

            var query = _context.Contracts
                .Include(c => c.Employee)
                .Where(c => c.Employee.OrganizationId == organizationId &&
                           c.EndDate.HasValue &&
                           c.EndDate.Value <= expiryDate &&
                           c.EndDate.Value > DateTime.Today &&
                           c.Status != "Terminated");

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(c => c.EndDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            var dtos = items.Select(c => MapContractToDto(c)).ToList();

            return new PagedResponse<ContractDto>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        #region Helper Methods

        private ContractDto MapContractToDto(Contract contract)
        {
            return new ContractDto
            {
                ContractId = contract.ContractId,
                EmployeeId = contract.EmployeeId,
                EmployeeName = contract.Employee != null ? $"{contract.Employee.FirstName} {contract.Employee.LastName}".Trim() : "N/A",
                ContractType = contract.ContractType,
                FileName = contract.FileName,
                FileUrl = contract.FileUrl,
                StartDate = contract.StartDate,
                EndDate = contract.EndDate,
                Terms = contract.Terms,
                Status = contract.Status,
                SignedDate = contract.SignedDate,
                SignedBy = contract.SignedBy,
                CreatedAt = contract.CreatedAt,
                UpdatedAt = contract.UpdatedAt,
                IsExpired = contract.IsExpired
            };
        }

        #endregion
    }
}
