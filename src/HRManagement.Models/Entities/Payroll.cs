namespace HRManagement.Models.Entities
{
    /// <summary>
    /// Represents a payroll run/cycle (monthly, bi-weekly, etc.)
    /// </summary>
    public class Payroll
    {
        public long PayrollId { get; set; }
        public long OrganizationId { get; set; }

        /// <summary>
        /// Payroll name/description (e.g., "December 2024")
        /// </summary>
        public string PayrollName { get; set; } = string.Empty;

        /// <summary>
        /// Payroll frequency: Monthly, Bi-weekly, Weekly, Quarterly
        /// </summary>
        public string PayrollFrequency { get; set; } = "Monthly";

        /// <summary>
        /// Start date of this payroll period
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date of this payroll period
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Payroll status: Draft, Processing, Completed, Paid, Cancelled
        /// </summary>
        public string Status { get; set; } = "Draft";

        /// <summary>
        /// Total gross salary for all employees in this payroll
        /// </summary>
        public decimal TotalGrossSalary { get; set; }

        /// <summary>
        /// Total deductions for all employees
        /// </summary>
        public decimal TotalDeductions { get; set; }

        /// <summary>
        /// Total tax for all employees
        /// </summary>
        public decimal TotalTax { get; set; }

        /// <summary>
        /// Total net salary to be paid
        /// </summary>
        public decimal TotalNetSalary { get; set; }

        /// <summary>
        /// Number of employees in this payroll
        /// </summary>
        public int EmployeeCount { get; set; }

        /// <summary>
        /// Date when payroll was processed/finalized
        /// </summary>
        public DateTime? ProcessedDate { get; set; }

        /// <summary>
        /// Date when salary was actually paid
        /// </summary>
        public DateTime? PaidDate { get; set; }

        /// <summary>
        /// Remarks or notes for this payroll
        /// </summary>
        public string? Remarks { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Organization? Organization { get; set; }
        public virtual ICollection<PayrollDetail> PayrollDetails { get; set; } = new List<PayrollDetail>();
    }
}
