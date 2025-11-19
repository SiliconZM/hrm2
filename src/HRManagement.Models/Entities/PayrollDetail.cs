namespace HRManagement.Models.Entities
{
    /// <summary>
    /// Represents individual employee payroll details for a specific payroll cycle
    /// </summary>
    public class PayrollDetail
    {
        public long PayrollDetailId { get; set; }
        public long PayrollId { get; set; }
        public long EmployeeId { get; set; }

        /// <summary>
        /// Total earnings for this employee in this period
        /// </summary>
        public decimal TotalEarnings { get; set; }

        /// <summary>
        /// Total deductions for this employee
        /// </summary>
        public decimal TotalDeductions { get; set; }

        /// <summary>
        /// Total tax for this employee
        /// </summary>
        public decimal TotalTax { get; set; }

        /// <summary>
        /// Gross salary (basic + allowances)
        /// </summary>
        public decimal GrossSalary { get; set; }

        /// <summary>
        /// Net salary (gross - deductions - tax)
        /// </summary>
        public decimal NetSalary { get; set; }

        /// <summary>
        /// Number of working days in this period
        /// </summary>
        public int? WorkingDays { get; set; }

        /// <summary>
        /// Number of days actually worked (for proration)
        /// </summary>
        public int? DaysWorked { get; set; }

        /// <summary>
        /// Number of leaves taken
        /// </summary>
        public int? LeavesDays { get; set; }

        /// <summary>
        /// Status of this payroll detail: Draft, Approved, Paid, Cancelled
        /// </summary>
        public string Status { get; set; } = "Draft";

        /// <summary>
        /// Remarks for this specific payroll detail
        /// </summary>
        public string? Remarks { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Payroll? Payroll { get; set; }
        public virtual Employee? Employee { get; set; }
        public virtual ICollection<SalarySlip> SalarySlips { get; set; } = new List<SalarySlip>();
    }
}
