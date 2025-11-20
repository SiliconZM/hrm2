using HRManagement.Data;
using HRManagement.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Services.Validations
{
    public class PayrollValidationService
    {
        private readonly HRContext _context;

        public PayrollValidationService(HRContext context)
        {
            _context = context;
        }

        #region Payroll Validations

        /// <summary>
        /// Validates payroll creation/update request
        /// </summary>
        public async Task<ValidationResult> ValidatePayrollAsync(CreatePayrollRequest request)
        {
            var errors = new List<string>();

            // Validate dates
            if (request.StartDate >= request.EndDate)
            {
                errors.Add("Payroll start date must be before end date");
            }

            if (request.StartDate > DateTime.UtcNow.AddMonths(1))
            {
                errors.Add("Payroll start date cannot be more than 1 month in the future");
            }

            // Validate organization exists
            var orgExists = await _context.Organizations.AnyAsync(o => o.OrganizationId == request.OrganizationId);
            if (!orgExists)
            {
                errors.Add($"Organization with ID {request.OrganizationId} not found");
            }

            // Check for overlapping payroll runs
            var overlappingPayroll = await _context.Payrolls
                .Where(p => p.OrganizationId == request.OrganizationId
                    && p.Status != "Cancelled"
                    && p.StartDate < request.EndDate
                    && p.EndDate > request.StartDate)
                .AnyAsync();

            if (overlappingPayroll)
            {
                errors.Add("This date range overlaps with an existing payroll run");
            }

            return new ValidationResult { IsValid = errors.Count == 0, Errors = errors };
        }

        #endregion

        #region PayrollDetail Validations

        /// <summary>
        /// Validates payroll detail creation request
        /// </summary>
        public async Task<ValidationResult> ValidatePayrollDetailAsync(CreatePayrollDetailRequest request)
        {
            var errors = new List<string>();

            // Check if payroll exists
            var payroll = await _context.Payrolls.FindAsync(request.PayrollId);
            if (payroll == null)
            {
                errors.Add($"Payroll with ID {request.PayrollId} not found");
                return new ValidationResult { IsValid = false, Errors = errors };
            }

            // Check if employee exists
            var employee = await _context.Employees.FindAsync(request.EmployeeId);
            if (employee == null)
            {
                errors.Add($"Employee with ID {request.EmployeeId} not found");
                return new ValidationResult { IsValid = false, Errors = errors };
            }

            // Check if employee already exists in this payroll
            var isDuplicate = await _context.PayrollDetails
                .AnyAsync(pd => pd.PayrollId == request.PayrollId && pd.EmployeeId == request.EmployeeId);

            if (isDuplicate)
            {
                errors.Add($"Employee {employee.FirstName} {employee.LastName} is already added to this payroll");
            }

            // Check if employee has active salary
            var activeSalary = await _context.EmployeeSalaries
                .FirstOrDefaultAsync(es => es.EmployeeId == request.EmployeeId
                    && es.IsActive
                    && (!es.EndDate.HasValue || es.EndDate > DateTime.UtcNow));

            if (activeSalary == null)
            {
                errors.Add($"Employee {employee.FirstName} {employee.LastName} does not have an active salary structure");
            }

            // Validate working days
            if (request.WorkingDays.HasValue)
            {
                if (request.WorkingDays < 0 || request.WorkingDays > 31)
                {
                    errors.Add("Working days must be between 0 and 31");
                }
            }

            // Validate days worked
            if (request.DaysWorked.HasValue)
            {
                if (request.DaysWorked < 0 || request.DaysWorked > 31)
                {
                    errors.Add("Days worked must be between 0 and 31");
                }

                if (request.WorkingDays.HasValue && request.DaysWorked > request.WorkingDays)
                {
                    errors.Add("Days worked cannot exceed working days");
                }
            }

            // Validate leave days
            if (request.LeavesDays.HasValue)
            {
                if (request.LeavesDays < 0 || request.LeavesDays > 31)
                {
                    errors.Add("Leave days must be between 0 and 31");
                }

                if (request.WorkingDays.HasValue && request.LeavesDays > request.WorkingDays)
                {
                    errors.Add("Leave days cannot exceed working days");
                }
            }

            // Validate working days + days worked + leave days = total working days
            if (request.WorkingDays.HasValue && request.DaysWorked.HasValue && request.LeavesDays.HasValue)
            {
                var total = request.DaysWorked.Value + request.LeavesDays.Value;
                if (total > request.WorkingDays.Value)
                {
                    errors.Add("Days worked + leave days cannot exceed working days");
                }
            }

            return new ValidationResult { IsValid = errors.Count == 0, Errors = errors };
        }

        #endregion

        #region Salary Component Validations

        /// <summary>
        /// Validates salary component creation/update request
        /// </summary>
        public async Task<ValidationResult> ValidateSalaryComponentAsync(CreateSalaryComponentRequest request, long? editingComponentId = null)
        {
            var errors = new List<string>();

            // Validate component name is not empty
            if (string.IsNullOrWhiteSpace(request.ComponentName))
            {
                errors.Add("Component name is required");
            }

            // Validate component type
            var validTypes = new[] { "Earning", "Deduction", "Tax" };
            if (!validTypes.Contains(request.ComponentType))
            {
                errors.Add("Component type must be Earning, Deduction, or Tax");
            }

            // Check for duplicate component names in the same structure
            var duplicateQuery = _context.SalaryComponents
                .Where(sc => sc.SalaryStructureId == request.SalaryStructureId
                    && sc.ComponentName == request.ComponentName);

            // If editing, exclude the current component
            if (editingComponentId.HasValue)
            {
                duplicateQuery = duplicateQuery.Where(sc => sc.SalaryComponentId != editingComponentId.Value);
            }

            var isDuplicate = await duplicateQuery.AnyAsync();
            if (isDuplicate)
            {
                errors.Add($"A component with name '{request.ComponentName}' already exists in this salary structure");
            }

            // Validate amount/percentage
            if (request.IsPercentageBased)
            {
                if (!request.Percentage.HasValue || request.Percentage <= 0 || request.Percentage > 100)
                {
                    errors.Add("Percentage must be between 0 and 100");
                }
            }
            else
            {
                if (request.Amount < 0)
                {
                    errors.Add("Amount cannot be negative");
                }
            }

            // Validate display order
            if (request.DisplayOrder < 0)
            {
                errors.Add("Display order cannot be negative");
            }

            return new ValidationResult { IsValid = errors.Count == 0, Errors = errors };
        }

        #endregion

        #region Employee Salary Validations

        /// <summary>
        /// Validates employee salary creation/update request
        /// </summary>
        public async Task<ValidationResult> ValidateEmployeeSalaryAsync(CreateEmployeeSalaryRequest request)
        {
            var errors = new List<string>();

            // Check if employee exists
            var employee = await _context.Employees.FindAsync(request.EmployeeId);
            if (employee == null)
            {
                errors.Add($"Employee with ID {request.EmployeeId} not found");
                return new ValidationResult { IsValid = false, Errors = errors };
            }

            // Check if salary structure exists
            var structure = await _context.SalaryStructures.FindAsync(request.SalaryStructureId);
            if (structure == null)
            {
                errors.Add($"Salary structure with ID {request.SalaryStructureId} not found");
                return new ValidationResult { IsValid = false, Errors = errors };
            }

            // Validate effective date
            if (request.EffectiveDate > DateTime.UtcNow.AddMonths(3))
            {
                errors.Add("Effective date cannot be more than 3 months in the future");
            }

            // Validate override salary if provided
            if (request.OverrideBasicSalary.HasValue && request.OverrideBasicSalary < 0)
            {
                errors.Add("Override basic salary cannot be negative");
            }

            // Check for overlapping salary assignments
            var overlappingSalary = await _context.EmployeeSalaries
                .Where(es => es.EmployeeId == request.EmployeeId
                    && es.IsActive
                    && es.EffectiveDate <= request.EffectiveDate
                    && (!es.EndDate.HasValue || es.EndDate > request.EffectiveDate))
                .AnyAsync();

            if (overlappingSalary)
            {
                errors.Add($"Employee already has an active salary assignment that overlaps with the effective date");
            }

            return new ValidationResult { IsValid = errors.Count == 0, Errors = errors };
        }

        #endregion

        #region Salary Structure Validations

        /// <summary>
        /// Validates salary structure creation/update request
        /// </summary>
        public async Task<ValidationResult> ValidateSalaryStructureAsync(CreateSalaryStructureRequest request)
        {
            var errors = new List<string>();

            // Validate structure name
            if (string.IsNullOrWhiteSpace(request.StructureName))
            {
                errors.Add("Salary structure name is required");
            }

            // Check for duplicate structure names in the organization
            var isDuplicate = await _context.SalaryStructures
                .AnyAsync(ss => ss.OrganizationId == request.OrganizationId
                    && ss.StructureName == request.StructureName);

            if (isDuplicate)
            {
                errors.Add($"A salary structure with name '{request.StructureName}' already exists in this organization");
            }

            // Validate basic salary
            if (request.BasicSalary <= 0)
            {
                errors.Add("Basic salary must be greater than 0");
            }

            // Check if organization exists
            var orgExists = await _context.Organizations.AnyAsync(o => o.OrganizationId == request.OrganizationId);
            if (!orgExists)
            {
                errors.Add($"Organization with ID {request.OrganizationId} not found");
            }

            return new ValidationResult { IsValid = errors.Count == 0, Errors = errors };
        }

        #endregion
    }

    /// <summary>
    /// Result of validation operation
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new();

        public override string ToString()
        {
            return IsValid ? "Valid" : string.Join(", ", Errors);
        }
    }
}
