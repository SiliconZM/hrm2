namespace HRManagement.Models.DTOs
{
    // SalaryStructure DTOs
    public class SalaryStructureDto
    {
        public long SalaryStructureId { get; set; }
        public long OrganizationId { get; set; }
        public string StructureName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal BasicSalary { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<SalaryComponentDto>? SalaryComponents { get; set; }
    }

    public class CreateSalaryStructureRequest
    {
        public long OrganizationId { get; set; }
        public string StructureName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal BasicSalary { get; set; }
    }

    public class UpdateSalaryStructureRequest
    {
        public string StructureName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal BasicSalary { get; set; }
        public bool IsActive { get; set; }
    }

    // SalaryComponent DTOs
    public class SalaryComponentDto
    {
        public long SalaryComponentId { get; set; }
        public long SalaryStructureId { get; set; }
        public string ComponentName { get; set; } = string.Empty;
        public string ComponentType { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public decimal? Percentage { get; set; }
        public bool IsTaxable { get; set; }
        public bool IsPercentageBased { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public string? Description { get; set; }
    }

    public class CreateSalaryComponentRequest
    {
        public long SalaryStructureId { get; set; }
        public string ComponentName { get; set; } = string.Empty;
        public string ComponentType { get; set; } = "Earning";
        public decimal Amount { get; set; }
        public decimal? Percentage { get; set; }
        public bool IsTaxable { get; set; }
        public bool IsPercentageBased { get; set; }
        public int DisplayOrder { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateSalaryComponentRequest
    {
        public string ComponentName { get; set; } = string.Empty;
        public string ComponentType { get; set; } = "Earning";
        public decimal Amount { get; set; }
        public decimal? Percentage { get; set; }
        public bool IsTaxable { get; set; }
        public bool IsPercentageBased { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public string? Description { get; set; }
    }

    // EmployeeSalary DTOs
    public class EmployeeSalaryDto
    {
        public long EmployeeSalaryId { get; set; }
        public long EmployeeId { get; set; }
        public long SalaryStructureId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string SalaryStructureName { get; set; } = string.Empty;
        public DateTime EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? OverrideBasicSalary { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal NetSalary { get; set; }
        public bool IsActive { get; set; }
        public string? Remarks { get; set; }
    }

    public class CreateEmployeeSalaryRequest
    {
        public long EmployeeId { get; set; }
        public long SalaryStructureId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public decimal? OverrideBasicSalary { get; set; }
        public string? Remarks { get; set; }
    }

    public class UpdateEmployeeSalaryRequest
    {
        public long SalaryStructureId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public decimal? OverrideBasicSalary { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal NetSalary { get; set; }
        public bool IsActive { get; set; }
        public string? Remarks { get; set; }
    }

    // Payroll DTOs
    public class PayrollDto
    {
        public long PayrollId { get; set; }
        public long OrganizationId { get; set; }
        public string PayrollName { get; set; } = string.Empty;
        public string PayrollFrequency { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalGrossSalary { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalNetSalary { get; set; }
        public int EmployeeCount { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public string? Remarks { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreatePayrollRequest
    {
        public long OrganizationId { get; set; }
        public string PayrollName { get; set; } = string.Empty;
        public string PayrollFrequency { get; set; } = "Monthly";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Remarks { get; set; }
    }

    public class UpdatePayrollRequest
    {
        public string PayrollName { get; set; } = string.Empty;
        public string PayrollFrequency { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Remarks { get; set; }
    }

    // PayrollDetail DTOs
    public class PayrollDetailDto
    {
        public long PayrollDetailId { get; set; }
        public long PayrollId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public decimal TotalEarnings { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal TotalTax { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal NetSalary { get; set; }
        public int? WorkingDays { get; set; }
        public int? DaysWorked { get; set; }
        public int? LeavesDays { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Remarks { get; set; }
    }

    public class CreatePayrollDetailRequest
    {
        public long PayrollId { get; set; }
        public long EmployeeId { get; set; }
        public int? WorkingDays { get; set; }
        public int? DaysWorked { get; set; }
        public int? LeavesDays { get; set; }
        public string? Remarks { get; set; }
    }

    // SalarySlip DTOs
    public class SalarySlipDto
    {
        public long SalarySlipId { get; set; }
        public long PayrollDetailId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string SlipNumber { get; set; } = string.Empty;
        public string Period { get; set; } = string.Empty;
        public decimal GrossSalary { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal IncomeTax { get; set; }
        public decimal NetPayable { get; set; }
        public DateTime? SalaryCreditedDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Remarks { get; set; }
        public List<SalarySlipComponentDto>? Components { get; set; }
    }

    public class SalarySlipComponentDto
    {
        public long SalarySlipComponentId { get; set; }
        public string ComponentName { get; set; } = string.Empty;
        public string ComponentType { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class CreateSalarySlipRequest
    {
        public long PayrollDetailId { get; set; }
        public long EmployeeId { get; set; }
        public string Period { get; set; } = string.Empty;
        public string? Remarks { get; set; }
    }
}
