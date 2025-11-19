namespace HRManagement.Models.Entities
{
    /// <summary>
    /// Represents a generated salary slip for an employee in a specific payroll period
    /// </summary>
    public class SalarySlip
    {
        public long SalarySlipId { get; set; }
        public long PayrollDetailId { get; set; }
        public long EmployeeId { get; set; }

        /// <summary>
        /// Salary slip reference number
        /// </summary>
        public string SlipNumber { get; set; } = string.Empty;

        /// <summary>
        /// Period month and year (e.g., "December 2024")
        /// </summary>
        public string Period { get; set; } = string.Empty;

        /// <summary>
        /// Total gross salary
        /// </summary>
        public decimal GrossSalary { get; set; }

        /// <summary>
        /// Total deductions
        /// </summary>
        public decimal TotalDeductions { get; set; }

        /// <summary>
        /// Total income tax
        /// </summary>
        public decimal IncomeTax { get; set; }

        /// <summary>
        /// Net payable amount
        /// </summary>
        public decimal NetPayable { get; set; }

        /// <summary>
        /// Date when salary was credited
        /// </summary>
        public DateTime? SalaryCreditedDate { get; set; }

        /// <summary>
        /// Status: Draft, Generated, Approved, Sent, Acknowledged
        /// </summary>
        public string Status { get; set; } = "Draft";

        /// <summary>
        /// Additional notes or remarks
        /// </summary>
        public string? Remarks { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual PayrollDetail? PayrollDetail { get; set; }
        public virtual Employee? Employee { get; set; }
        public virtual ICollection<SalarySlipComponent> SalarySlipComponents { get; set; } = new List<SalarySlipComponent>();
    }
}
