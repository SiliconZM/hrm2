using AutoMapper;
using HRManagement.Data;
using HRManagement.Models.DTOs;
using HRManagement.Models.Entities;
using HRManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Services.Implementations
{
    public class PayrollService : IPayrollService
    {
        private readonly HRContext _context;
        private readonly IMapper _mapper;

        public PayrollService(HRContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region Salary Structure Management

        public async Task<PagedResponse<SalaryStructureDto>> GetAllSalaryStructuresAsync(
            long organizationId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.SalaryStructures
                .AsNoTracking()
                .Where(ss => ss.OrganizationId == organizationId && ss.IsActive)
                .OrderBy(ss => ss.StructureName);

            var totalCount = await query.CountAsync();

            var structures = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(ss => ss.SalaryComponents)
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<SalaryStructureDto>>(structures);

            return new PagedResponse<SalaryStructureDto>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<SalaryStructureDto> GetSalaryStructureByIdAsync(long salaryStructureId)
        {
            var structure = await _context.SalaryStructures
                .AsNoTracking()
                .Include(ss => ss.SalaryComponents)
                .FirstOrDefaultAsync(ss => ss.SalaryStructureId == salaryStructureId);

            if (structure == null)
                throw new InvalidOperationException($"Salary structure {salaryStructureId} not found");

            return _mapper.Map<SalaryStructureDto>(structure);
        }

        public async Task<long> CreateSalaryStructureAsync(CreateSalaryStructureRequest request)
        {
            // Validate organization exists
            var org = await _context.Organizations.FindAsync(request.OrganizationId);
            if (org == null)
                throw new InvalidOperationException($"Organization {request.OrganizationId} not found");

            // Check for duplicate structure name within organization
            var exists = await _context.SalaryStructures
                .AnyAsync(ss => ss.OrganizationId == request.OrganizationId &&
                                 ss.StructureName == request.StructureName);
            if (exists)
                throw new InvalidOperationException($"Salary structure '{request.StructureName}' already exists in this organization");

            var structure = _mapper.Map<SalaryStructure>(request);
            structure.IsActive = true;
            structure.CreatedAt = DateTime.UtcNow;
            structure.UpdatedAt = DateTime.UtcNow;

            _context.SalaryStructures.Add(structure);
            await _context.SaveChangesAsync();

            return structure.SalaryStructureId;
        }

        public async Task UpdateSalaryStructureAsync(long salaryStructureId, UpdateSalaryStructureRequest request)
        {
            var structure = await _context.SalaryStructures.FindAsync(salaryStructureId);
            if (structure == null)
                throw new InvalidOperationException($"Salary structure {salaryStructureId} not found");

            // Check for duplicate name if updating
            var duplicate = await _context.SalaryStructures
                .AnyAsync(ss => ss.SalaryStructureId != salaryStructureId &&
                                 ss.OrganizationId == structure.OrganizationId &&
                                 ss.StructureName == request.StructureName);
            if (duplicate)
                throw new InvalidOperationException($"Salary structure '{request.StructureName}' already exists");

            structure.StructureName = request.StructureName;
            structure.Description = request.Description;
            structure.BasicSalary = request.BasicSalary;
            structure.IsActive = request.IsActive;
            structure.UpdatedAt = DateTime.UtcNow;

            _context.SalaryStructures.Update(structure);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSalaryStructureAsync(long salaryStructureId)
        {
            var structure = await _context.SalaryStructures.FindAsync(salaryStructureId);
            if (structure == null)
                throw new InvalidOperationException($"Salary structure {salaryStructureId} not found");

            // Check if structure is used by any employees
            var isUsed = await _context.EmployeeSalaries
                .AnyAsync(es => es.SalaryStructureId == salaryStructureId && es.IsActive);

            if (isUsed)
                throw new InvalidOperationException("Cannot delete salary structure that is currently assigned to active employees");

            _context.SalaryStructures.Remove(structure);
            await _context.SaveChangesAsync();
        }

        #endregion

        #region Salary Component Management

        public async Task<List<SalaryComponentDto>> GetComponentsByStructureAsync(long salaryStructureId)
        {
            var structure = await _context.SalaryStructures.FindAsync(salaryStructureId);
            if (structure == null)
                throw new InvalidOperationException($"Salary structure {salaryStructureId} not found");

            var components = await _context.SalaryComponents
                .AsNoTracking()
                .Where(sc => sc.SalaryStructureId == salaryStructureId && sc.IsActive)
                .OrderBy(sc => sc.DisplayOrder)
                .ToListAsync();

            return _mapper.Map<List<SalaryComponentDto>>(components);
        }

        public async Task<SalaryComponentDto> GetSalaryComponentByIdAsync(long componentId)
        {
            var component = await _context.SalaryComponents
                .AsNoTracking()
                .FirstOrDefaultAsync(sc => sc.SalaryComponentId == componentId);

            if (component == null)
                throw new InvalidOperationException($"Salary component {componentId} not found");

            return _mapper.Map<SalaryComponentDto>(component);
        }

        public async Task<long> CreateSalaryComponentAsync(CreateSalaryComponentRequest request)
        {
            // Validate salary structure exists
            var structure = await _context.SalaryStructures.FindAsync(request.SalaryStructureId);
            if (structure == null)
                throw new InvalidOperationException($"Salary structure {request.SalaryStructureId} not found");

            // Validate amount or percentage
            if (!request.IsPercentageBased && request.Amount <= 0)
                throw new InvalidOperationException("Amount must be greater than 0 for fixed components");

            if (request.IsPercentageBased && (request.Percentage == null || request.Percentage <= 0))
                throw new InvalidOperationException("Percentage must be greater than 0 for percentage-based components");

            var component = _mapper.Map<SalaryComponent>(request);
            component.IsActive = true;
            component.CreatedAt = DateTime.UtcNow;
            component.UpdatedAt = DateTime.UtcNow;

            _context.SalaryComponents.Add(component);
            await _context.SaveChangesAsync();

            return component.SalaryComponentId;
        }

        public async Task UpdateSalaryComponentAsync(long componentId, UpdateSalaryComponentRequest request)
        {
            var component = await _context.SalaryComponents.FindAsync(componentId);
            if (component == null)
                throw new InvalidOperationException($"Salary component {componentId} not found");

            // Validate amount or percentage
            if (!request.IsPercentageBased && request.Amount <= 0)
                throw new InvalidOperationException("Amount must be greater than 0 for fixed components");

            if (request.IsPercentageBased && (request.Percentage == null || request.Percentage <= 0))
                throw new InvalidOperationException("Percentage must be greater than 0 for percentage-based components");

            component.ComponentName = request.ComponentName;
            component.ComponentType = request.ComponentType;
            component.Amount = request.Amount;
            component.Percentage = request.Percentage;
            component.IsTaxable = request.IsTaxable;
            component.IsPercentageBased = request.IsPercentageBased;
            component.IsActive = request.IsActive;
            component.DisplayOrder = request.DisplayOrder;
            component.UpdatedAt = DateTime.UtcNow;

            _context.SalaryComponents.Update(component);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSalaryComponentAsync(long componentId)
        {
            var component = await _context.SalaryComponents.FindAsync(componentId);
            if (component == null)
                throw new InvalidOperationException($"Salary component {componentId} not found");

            _context.SalaryComponents.Remove(component);
            await _context.SaveChangesAsync();
        }

        #endregion

        #region Employee Salary Management

        public async Task<PagedResponse<EmployeeSalaryDto>> GetAllEmployeeSalariesAsync(
            long organizationId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.EmployeeSalaries
                .AsNoTracking()
                .Where(es => es.Employee != null && es.Employee.OrganizationId == organizationId)
                .OrderBy(es => es.Employee!.LastName)
                .ThenBy(es => es.Employee!.FirstName);

            var totalCount = await query.CountAsync();

            var salaries = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(es => es.Employee)
                .Include(es => es.SalaryStructure)
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<EmployeeSalaryDto>>(salaries);

            return new PagedResponse<EmployeeSalaryDto>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<EmployeeSalaryDto> GetEmployeeActiveSalaryAsync(long employeeId)
        {
            var salary = await _context.EmployeeSalaries
                .AsNoTracking()
                .Where(es => es.EmployeeId == employeeId &&
                             es.IsActive &&
                             es.EffectiveDate <= DateTime.UtcNow &&
                             (es.EndDate == null || es.EndDate > DateTime.UtcNow))
                .Include(es => es.Employee)
                .Include(es => es.SalaryStructure)
                .FirstOrDefaultAsync();

            if (salary == null)
                throw new InvalidOperationException($"Active salary not found for employee {employeeId}");

            return _mapper.Map<EmployeeSalaryDto>(salary);
        }

        public async Task<List<EmployeeSalaryDto>> GetEmployeeSalaryHistoryAsync(long employeeId)
        {
            var salaries = await _context.EmployeeSalaries
                .AsNoTracking()
                .Where(es => es.EmployeeId == employeeId)
                .Include(es => es.Employee)
                .Include(es => es.SalaryStructure)
                .OrderByDescending(es => es.EffectiveDate)
                .ToListAsync();

            return _mapper.Map<List<EmployeeSalaryDto>>(salaries);
        }

        public async Task<long> CreateEmployeeSalaryAsync(CreateEmployeeSalaryRequest request)
        {
            // Validate employee exists
            var employee = await _context.Employees.FindAsync(request.EmployeeId);
            if (employee == null)
                throw new InvalidOperationException($"Employee {request.EmployeeId} not found");

            // Validate salary structure exists
            var structure = await _context.SalaryStructures.FindAsync(request.SalaryStructureId);
            if (structure == null)
                throw new InvalidOperationException($"Salary structure {request.SalaryStructureId} not found");

            // Deactivate previous active salary
            var previousActive = await _context.EmployeeSalaries
                .Where(es => es.EmployeeId == request.EmployeeId && es.IsActive)
                .ToListAsync();

            foreach (var prev in previousActive)
            {
                prev.IsActive = false;
                prev.EndDate = DateTime.UtcNow;
                prev.UpdatedAt = DateTime.UtcNow;
            }

            var salary = _mapper.Map<EmployeeSalary>(request);
            salary.IsActive = true;
            salary.GrossSalary = await CalculateGrossSalaryFromStructureAsync(structure.SalaryStructureId, request.OverrideBasicSalary);
            salary.NetSalary = await CalculateNetSalaryFromStructureAsync(structure.SalaryStructureId, salary.GrossSalary);
            salary.CreatedAt = DateTime.UtcNow;
            salary.UpdatedAt = DateTime.UtcNow;

            _context.EmployeeSalaries.Add(salary);
            await _context.SaveChangesAsync();

            return salary.EmployeeSalaryId;
        }

        public async Task UpdateEmployeeSalaryAsync(long employeeSalaryId, UpdateEmployeeSalaryRequest request)
        {
            var salary = await _context.EmployeeSalaries.FindAsync(employeeSalaryId);
            if (salary == null)
                throw new InvalidOperationException($"Employee salary {employeeSalaryId} not found");

            // Validate salary structure exists
            var structure = await _context.SalaryStructures.FindAsync(request.SalaryStructureId);
            if (structure == null)
                throw new InvalidOperationException($"Salary structure {request.SalaryStructureId} not found");

            salary.SalaryStructureId = request.SalaryStructureId;
            salary.EffectiveDate = request.EffectiveDate;
            salary.OverrideBasicSalary = request.OverrideBasicSalary;
            salary.GrossSalary = request.GrossSalary;
            salary.NetSalary = request.NetSalary;
            salary.IsActive = request.IsActive;
            salary.Remarks = request.Remarks;
            salary.UpdatedAt = DateTime.UtcNow;

            _context.EmployeeSalaries.Update(salary);
            await _context.SaveChangesAsync();
        }

        public async Task DeactivateEmployeeSalaryAsync(long employeeSalaryId)
        {
            var salary = await _context.EmployeeSalaries.FindAsync(employeeSalaryId);
            if (salary == null)
                throw new InvalidOperationException($"Employee salary {employeeSalaryId} not found");

            salary.IsActive = false;
            salary.EndDate = DateTime.UtcNow;
            salary.UpdatedAt = DateTime.UtcNow;

            _context.EmployeeSalaries.Update(salary);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> CalculateGrossSalaryAsync(long employeeSalaryId)
        {
            var salary = await _context.EmployeeSalaries
                .Include(es => es.SalaryStructure)
                .ThenInclude(ss => ss!.SalaryComponents)
                .FirstOrDefaultAsync(es => es.EmployeeSalaryId == employeeSalaryId);

            if (salary == null)
                throw new InvalidOperationException($"Employee salary {employeeSalaryId} not found");

            return CalculateGrossSalary(salary.SalaryStructure, salary.OverrideBasicSalary);
        }

        public async Task<decimal> CalculateNetSalaryAsync(long employeeSalaryId)
        {
            var salary = await _context.EmployeeSalaries
                .Include(es => es.SalaryStructure)
                .ThenInclude(ss => ss!.SalaryComponents)
                .FirstOrDefaultAsync(es => es.EmployeeSalaryId == employeeSalaryId);

            if (salary == null)
                throw new InvalidOperationException($"Employee salary {employeeSalaryId} not found");

            var grossSalary = CalculateGrossSalary(salary.SalaryStructure, salary.OverrideBasicSalary);
            return CalculateNetSalary(grossSalary, salary.SalaryStructure);
        }

        #endregion

        #region Payroll Management

        public async Task<PagedResponse<PayrollDto>> GetAllPayrollsAsync(
            long organizationId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Payrolls
                .AsNoTracking()
                .Where(p => p.OrganizationId == organizationId)
                .OrderByDescending(p => p.StartDate);

            var totalCount = await query.CountAsync();

            var payrolls = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<PayrollDto>>(payrolls);

            return new PagedResponse<PayrollDto>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<PayrollDto> GetPayrollByIdAsync(long payrollId)
        {
            var payroll = await _context.Payrolls
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PayrollId == payrollId);

            if (payroll == null)
                throw new InvalidOperationException($"Payroll {payrollId} not found");

            return _mapper.Map<PayrollDto>(payroll);
        }

        public async Task<long> CreatePayrollAsync(CreatePayrollRequest request)
        {
            // Validate organization exists
            var org = await _context.Organizations.FindAsync(request.OrganizationId);
            if (org == null)
                throw new InvalidOperationException($"Organization {request.OrganizationId} not found");

            // Validate date range
            if (request.EndDate <= request.StartDate)
                throw new InvalidOperationException("End date must be after start date");

            // Check for overlapping payroll
            var overlapping = await _context.Payrolls
                .AnyAsync(p => p.OrganizationId == request.OrganizationId &&
                               p.StartDate < request.EndDate &&
                               p.EndDate > request.StartDate);
            if (overlapping)
                throw new InvalidOperationException("Overlapping payroll already exists for this period");

            var payroll = _mapper.Map<Payroll>(request);
            payroll.Status = "Draft";
            payroll.TotalGrossSalary = 0;
            payroll.TotalDeductions = 0;
            payroll.TotalTax = 0;
            payroll.TotalNetSalary = 0;
            payroll.EmployeeCount = 0;
            payroll.CreatedAt = DateTime.UtcNow;
            payroll.UpdatedAt = DateTime.UtcNow;

            _context.Payrolls.Add(payroll);
            await _context.SaveChangesAsync();

            return payroll.PayrollId;
        }

        public async Task UpdatePayrollAsync(long payrollId, UpdatePayrollRequest request)
        {
            var payroll = await _context.Payrolls.FindAsync(payrollId);
            if (payroll == null)
                throw new InvalidOperationException($"Payroll {payrollId} not found");

            // Validate date range
            if (request.EndDate <= request.StartDate)
                throw new InvalidOperationException("End date must be after start date");

            // Cannot update processed payroll
            if (payroll.Status != "Draft")
                throw new InvalidOperationException("Cannot update payroll that has been processed");

            payroll.PayrollName = request.PayrollName;
            payroll.PayrollFrequency = request.PayrollFrequency;
            payroll.StartDate = request.StartDate;
            payroll.EndDate = request.EndDate;
            payroll.Status = request.Status;
            payroll.Remarks = request.Remarks;
            payroll.UpdatedAt = DateTime.UtcNow;

            _context.Payrolls.Update(payroll);
            await _context.SaveChangesAsync();
        }

        public async Task ProcessPayrollAsync(long payrollId)
        {
            var payroll = await _context.Payrolls
                .Include(p => p.PayrollDetails)
                .FirstOrDefaultAsync(p => p.PayrollId == payrollId);

            if (payroll == null)
                throw new InvalidOperationException($"Payroll {payrollId} not found");

            if (payroll.Status != "Draft")
                throw new InvalidOperationException("Only draft payroll can be processed");

            if (payroll.PayrollDetails.Count == 0)
                throw new InvalidOperationException("Cannot process payroll without any employee details");

            // Recalculate totals
            payroll.TotalGrossSalary = payroll.PayrollDetails.Sum(pd => pd.GrossSalary);
            payroll.TotalDeductions = payroll.PayrollDetails.Sum(pd => pd.TotalDeductions);
            payroll.TotalTax = payroll.PayrollDetails.Sum(pd => pd.TotalTax);
            payroll.TotalNetSalary = payroll.PayrollDetails.Sum(pd => pd.NetSalary);
            payroll.EmployeeCount = payroll.PayrollDetails.Count;
            payroll.Status = "Processed";
            payroll.ProcessedDate = DateTime.UtcNow;
            payroll.UpdatedAt = DateTime.UtcNow;

            _context.Payrolls.Update(payroll);
            await _context.SaveChangesAsync();
        }

        public async Task MarkPayrollAsPaidAsync(long payrollId, DateTime paidDate)
        {
            var payroll = await _context.Payrolls.FindAsync(payrollId);
            if (payroll == null)
                throw new InvalidOperationException($"Payroll {payrollId} not found");

            if (payroll.Status != "Processed")
                throw new InvalidOperationException("Only processed payroll can be marked as paid");

            payroll.Status = "Paid";
            payroll.PaidDate = paidDate;
            payroll.UpdatedAt = DateTime.UtcNow;

            _context.Payrolls.Update(payroll);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePayrollAsync(long payrollId)
        {
            var payroll = await _context.Payrolls.FindAsync(payrollId);
            if (payroll == null)
                throw new InvalidOperationException($"Payroll {payrollId} not found");

            if (payroll.Status != "Draft")
                throw new InvalidOperationException("Cannot delete processed or paid payroll");

            _context.Payrolls.Remove(payroll);
            await _context.SaveChangesAsync();
        }

        #endregion

        #region Payroll Detail Management

        public async Task<List<PayrollDetailDto>> GetPayrollDetailsAsync(long payrollId)
        {
            var payroll = await _context.Payrolls.FindAsync(payrollId);
            if (payroll == null)
                throw new InvalidOperationException($"Payroll {payrollId} not found");

            var details = await _context.PayrollDetails
                .AsNoTracking()
                .Where(pd => pd.PayrollId == payrollId)
                .Include(pd => pd.Employee)
                .OrderBy(pd => pd.Employee!.LastName)
                .ThenBy(pd => pd.Employee!.FirstName)
                .ToListAsync();

            return _mapper.Map<List<PayrollDetailDto>>(details);
        }

        public async Task<PayrollDetailDto> GetPayrollDetailByIdAsync(long payrollDetailId)
        {
            var detail = await _context.PayrollDetails
                .AsNoTracking()
                .Include(pd => pd.Employee)
                .FirstOrDefaultAsync(pd => pd.PayrollDetailId == payrollDetailId);

            if (detail == null)
                throw new InvalidOperationException($"Payroll detail {payrollDetailId} not found");

            return _mapper.Map<PayrollDetailDto>(detail);
        }

        public async Task<long> CreatePayrollDetailAsync(CreatePayrollDetailRequest request)
        {
            // Validate payroll exists
            var payroll = await _context.Payrolls.FindAsync(request.PayrollId);
            if (payroll == null)
                throw new InvalidOperationException($"Payroll {request.PayrollId} not found");

            // Validate employee exists and is active
            var employee = await _context.Employees.FindAsync(request.EmployeeId);
            if (employee == null || !employee.IsActive)
                throw new InvalidOperationException($"Active employee {request.EmployeeId} not found");

            // Get employee active salary
            var employeeSalary = await _context.EmployeeSalaries
                .Include(es => es.SalaryStructure)
                .ThenInclude(ss => ss!.SalaryComponents)
                .FirstOrDefaultAsync(es => es.EmployeeId == request.EmployeeId && es.IsActive);

            if (employeeSalary == null)
                throw new InvalidOperationException($"No active salary found for employee {request.EmployeeId}");

            // Calculate salary with proration if days worked is specified
            var grossSalary = CalculateGrossSalary(employeeSalary.SalaryStructure, employeeSalary.OverrideBasicSalary);
            var netSalary = CalculateNetSalary(grossSalary, employeeSalary.SalaryStructure);

            if (request.DaysWorked.HasValue && request.WorkingDays.HasValue && request.WorkingDays > 0)
            {
                decimal prorateRatio = (decimal)request.DaysWorked.Value / request.WorkingDays.Value;
                grossSalary *= prorateRatio;
                netSalary *= prorateRatio;
            }

            var detail = _mapper.Map<PayrollDetail>(request);
            detail.TotalEarnings = grossSalary;
            detail.TotalDeductions = grossSalary - netSalary;
            detail.TotalTax = 0; // Can be calculated based on tax rules
            detail.GrossSalary = grossSalary;
            detail.NetSalary = netSalary;
            detail.Status = "Draft";
            detail.CreatedAt = DateTime.UtcNow;
            detail.UpdatedAt = DateTime.UtcNow;

            _context.PayrollDetails.Add(detail);
            await _context.SaveChangesAsync();

            return detail.PayrollDetailId;
        }

        public async Task UpdatePayrollDetailAsync(long payrollDetailId, PayrollDetailDto request)
        {
            var detail = await _context.PayrollDetails.FindAsync(payrollDetailId);
            if (detail == null)
                throw new InvalidOperationException($"Payroll detail {payrollDetailId} not found");

            // Cannot update if payroll is processed
            var payroll = await _context.Payrolls.FindAsync(detail.PayrollId);
            if (payroll != null && payroll.Status != "Draft")
                throw new InvalidOperationException("Cannot update details of a processed payroll");

            detail.TotalEarnings = request.TotalEarnings;
            detail.TotalDeductions = request.TotalDeductions;
            detail.TotalTax = request.TotalTax;
            detail.GrossSalary = request.GrossSalary;
            detail.NetSalary = request.NetSalary;
            detail.WorkingDays = request.WorkingDays;
            detail.DaysWorked = request.DaysWorked;
            detail.LeavesDays = request.LeavesDays;
            detail.Remarks = request.Remarks;
            detail.UpdatedAt = DateTime.UtcNow;

            _context.PayrollDetails.Update(detail);
            await _context.SaveChangesAsync();
        }

        public async Task ApprovePayrollDetailAsync(long payrollDetailId)
        {
            var detail = await _context.PayrollDetails.FindAsync(payrollDetailId);
            if (detail == null)
                throw new InvalidOperationException($"Payroll detail {payrollDetailId} not found");

            detail.Status = "Approved";
            detail.UpdatedAt = DateTime.UtcNow;

            _context.PayrollDetails.Update(detail);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> CalculatePayrollDetailSalaryAsync(long payrollDetailId)
        {
            var detail = await _context.PayrollDetails.FindAsync(payrollDetailId);
            if (detail == null)
                throw new InvalidOperationException($"Payroll detail {payrollDetailId} not found");

            return detail.NetSalary;
        }

        #endregion

        #region Salary Slip Management

        public async Task<SalarySlipDto> GetSalarySlipByIdAsync(long salarySlipId)
        {
            var slip = await _context.SalarySlips
                .AsNoTracking()
                .Include(ss => ss.SalarySlipComponents)
                .FirstOrDefaultAsync(ss => ss.SalarySlipId == salarySlipId);

            if (slip == null)
                throw new InvalidOperationException($"Salary slip {salarySlipId} not found");

            return _mapper.Map<SalarySlipDto>(slip);
        }

        public async Task<List<SalarySlipDto>> GetSalarySlipsByEmployeeAsync(long employeeId, int year)
        {
            var slips = await _context.SalarySlips
                .AsNoTracking()
                .Where(ss => ss.EmployeeId == employeeId &&
                             ss.CreatedAt.Year == year)
                .Include(ss => ss.SalarySlipComponents)
                .OrderByDescending(ss => ss.Period)
                .ToListAsync();

            return _mapper.Map<List<SalarySlipDto>>(slips);
        }

        public async Task<long> GenerateSalarySlipAsync(CreateSalarySlipRequest request)
        {
            // Validate payroll detail exists
            var payrollDetail = await _context.PayrollDetails
                .Include(pd => pd.Payroll)
                .Include(pd => pd.Employee)
                .FirstOrDefaultAsync(pd => pd.PayrollDetailId == request.PayrollDetailId);

            if (payrollDetail == null)
                throw new InvalidOperationException($"Payroll detail {request.PayrollDetailId} not found");

            // Check if slip already exists
            var existingSlip = await _context.SalarySlips
                .FirstOrDefaultAsync(ss => ss.PayrollDetailId == request.PayrollDetailId);

            if (existingSlip != null)
                throw new InvalidOperationException("Salary slip already exists for this payroll detail");

            // Generate slip number
            var slipNumber = $"SS-{payrollDetail.Payroll!.PayrollId}-{payrollDetail.EmployeeId}-{DateTime.UtcNow:yyyyMMdd}";

            var slip = new SalarySlip
            {
                PayrollDetailId = request.PayrollDetailId,
                EmployeeId = request.EmployeeId,
                SlipNumber = slipNumber,
                Period = request.Period,
                GrossSalary = payrollDetail.GrossSalary,
                TotalDeductions = payrollDetail.TotalDeductions,
                IncomeTax = payrollDetail.TotalTax,
                NetPayable = payrollDetail.NetSalary,
                Status = "Generated",
                Remarks = request.Remarks,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Add salary slip components from structure
            var employeeSalary = await _context.EmployeeSalaries
                .Include(es => es.SalaryStructure)
                .ThenInclude(ss => ss!.SalaryComponents)
                .FirstOrDefaultAsync(es => es.EmployeeId == request.EmployeeId && es.IsActive);

            if (employeeSalary?.SalaryStructure?.SalaryComponents != null)
            {
                int displayOrder = 1;
                foreach (var component in employeeSalary.SalaryStructure.SalaryComponents.OrderBy(c => c.DisplayOrder))
                {
                    decimal componentAmount = 0;

                    if (component.IsPercentageBased && component.Percentage.HasValue)
                    {
                        componentAmount = (payrollDetail.GrossSalary * component.Percentage.Value) / 100;
                    }
                    else
                    {
                        componentAmount = component.Amount;
                    }

                    // Apply proration if applicable
                    if (payrollDetail.DaysWorked.HasValue && payrollDetail.WorkingDays.HasValue && payrollDetail.WorkingDays > 0)
                    {
                        decimal prorateRatio = (decimal)payrollDetail.DaysWorked.Value / payrollDetail.WorkingDays.Value;
                        componentAmount *= prorateRatio;
                    }

                    slip.SalarySlipComponents.Add(new SalarySlipComponent
                    {
                        ComponentName = component.ComponentName,
                        ComponentType = component.ComponentType,
                        Amount = componentAmount,
                        DisplayOrder = displayOrder++
                    });
                }
            }

            _context.SalarySlips.Add(slip);
            await _context.SaveChangesAsync();

            return slip.SalarySlipId;
        }

        public async Task UpdateSalarySlipAsync(long salarySlipId, SalarySlipDto request)
        {
            var slip = await _context.SalarySlips
                .Include(ss => ss.SalarySlipComponents)
                .FirstOrDefaultAsync(ss => ss.SalarySlipId == salarySlipId);

            if (slip == null)
                throw new InvalidOperationException($"Salary slip {salarySlipId} not found");

            slip.Period = request.Period;
            slip.GrossSalary = request.GrossSalary;
            slip.TotalDeductions = request.TotalDeductions;
            slip.IncomeTax = request.IncomeTax;
            slip.NetPayable = request.NetPayable;
            slip.Remarks = request.Remarks;
            slip.UpdatedAt = DateTime.UtcNow;

            _context.SalarySlips.Update(slip);
            await _context.SaveChangesAsync();
        }

        public async Task MarkSalarySlipAsApprovedAsync(long salarySlipId)
        {
            var slip = await _context.SalarySlips.FindAsync(salarySlipId);
            if (slip == null)
                throw new InvalidOperationException($"Salary slip {salarySlipId} not found");

            slip.Status = "Approved";
            slip.UpdatedAt = DateTime.UtcNow;

            _context.SalarySlips.Update(slip);
            await _context.SaveChangesAsync();
        }

        public async Task MarkSalarySlipAsSentAsync(long salarySlipId)
        {
            var slip = await _context.SalarySlips.FindAsync(salarySlipId);
            if (slip == null)
                throw new InvalidOperationException($"Salary slip {salarySlipId} not found");

            if (slip.Status != "Approved")
                throw new InvalidOperationException("Only approved salary slips can be sent");

            slip.Status = "Sent";
            slip.UpdatedAt = DateTime.UtcNow;

            _context.SalarySlips.Update(slip);
            await _context.SaveChangesAsync();
        }

        public async Task MarkSalarySlipAsPaidAsync(long salarySlipId, DateTime paidDate)
        {
            var slip = await _context.SalarySlips.FindAsync(salarySlipId);
            if (slip == null)
                throw new InvalidOperationException($"Salary slip {salarySlipId} not found");

            slip.Status = "Paid";
            slip.SalaryCreditedDate = paidDate;
            slip.UpdatedAt = DateTime.UtcNow;

            _context.SalarySlips.Update(slip);
            await _context.SaveChangesAsync();
        }

        public async Task<byte[]> GenerateSalarySlipPdfAsync(long salarySlipId)
        {
            var slip = await _context.SalarySlips
                .Include(ss => ss.SalarySlipComponents)
                .Include(ss => ss.Employee)
                .FirstOrDefaultAsync(ss => ss.SalarySlipId == salarySlipId);

            if (slip == null)
                throw new InvalidOperationException($"Salary slip {salarySlipId} not found");

            // PDF generation would require a library like iTextSharp or SelectPdf
            // For now, returning empty byte array as placeholder
            // In production, implement actual PDF generation here
            return Array.Empty<byte>();
        }

        #endregion

        #region Helper Methods

        private decimal CalculateGrossSalary(SalaryStructure? structure, decimal? overrideBasicSalary)
        {
            if (structure == null)
                return 0;

            decimal basicSalary = overrideBasicSalary ?? structure.BasicSalary;
            decimal totalEarnings = basicSalary;

            if (structure.SalaryComponents != null)
            {
                foreach (var component in structure.SalaryComponents.Where(c => c.ComponentType == "Earning" && c.IsActive))
                {
                    if (component.IsPercentageBased && component.Percentage.HasValue)
                    {
                        totalEarnings += (basicSalary * component.Percentage.Value) / 100;
                    }
                    else
                    {
                        totalEarnings += component.Amount;
                    }
                }
            }

            return totalEarnings;
        }

        private decimal CalculateNetSalary(decimal grossSalary, SalaryStructure? structure)
        {
            decimal netSalary = grossSalary;

            if (structure?.SalaryComponents != null)
            {
                foreach (var component in structure.SalaryComponents.Where(c => c.IsActive && (c.ComponentType == "Deduction" || c.ComponentType == "Tax")))
                {
                    if (component.IsPercentageBased && component.Percentage.HasValue)
                    {
                        netSalary -= (grossSalary * component.Percentage.Value) / 100;
                    }
                    else
                    {
                        netSalary -= component.Amount;
                    }
                }
            }

            return netSalary >= 0 ? netSalary : 0;
        }

        private async Task<decimal> CalculateGrossSalaryFromStructureAsync(long salaryStructureId, decimal? overrideBasicSalary)
        {
            var structure = await _context.SalaryStructures
                .Include(ss => ss.SalaryComponents)
                .FirstOrDefaultAsync(ss => ss.SalaryStructureId == salaryStructureId);

            return CalculateGrossSalary(structure, overrideBasicSalary);
        }

        private async Task<decimal> CalculateNetSalaryFromStructureAsync(long salaryStructureId, decimal grossSalary)
        {
            var structure = await _context.SalaryStructures
                .Include(ss => ss.SalaryComponents)
                .FirstOrDefaultAsync(ss => ss.SalaryStructureId == salaryStructureId);

            return CalculateNetSalary(grossSalary, structure);
        }

        #endregion
    }
}
