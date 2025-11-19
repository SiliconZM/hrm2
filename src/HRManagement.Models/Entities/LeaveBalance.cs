namespace HRManagement.Models.Entities
{
    /// <summary>
    /// Tracks leave balance for an employee for a specific leave type and year
    /// </summary>
    public class LeaveBalance
    {
        public long LeaveBalanceId { get; set; }
        public long EmployeeId { get; set; }
        public long LeaveTypeId { get; set; }

        /// <summary>
        /// Financial year this balance applies to
        /// </summary>
        public int FinancialYear { get; set; }

        /// <summary>
        /// Total days allocated for this year
        /// </summary>
        public decimal TotalDays { get; set; }

        /// <summary>
        /// Days already used
        /// </summary>
        public decimal UsedDays { get; set; }

        /// <summary>
        /// Days carried over from previous year
        /// </summary>
        public decimal CarryOverDays { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Employee? Employee { get; set; }
        public virtual LeaveType? LeaveType { get; set; }

        // Computed property
        public decimal AvailableDays => TotalDays + CarryOverDays - UsedDays;
    }
}
