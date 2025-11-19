namespace HRManagement.Models.Entities
{
    /// <summary>
    /// Represents a leave request from an employee
    /// </summary>
    public class LeaveRequest
    {
        public long LeaveRequestId { get; set; }
        public long EmployeeId { get; set; }
        public long LeaveTypeId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Number of days requested (may be fractional for half days)
        /// </summary>
        public decimal DaysRequested { get; set; }

        public string? ReasonForLeave { get; set; }

        /// <summary>
        /// Status: Pending, Approved, Rejected, Cancelled
        /// </summary>
        public string Status { get; set; } = "Pending";

        /// <summary>
        /// Employee who approved the request
        /// </summary>
        public long? ApprovedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }
        public string? ApprovalComments { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Employee? Employee { get; set; }
        public virtual LeaveType? LeaveType { get; set; }
        public virtual Employee? ApprovedByEmployee { get; set; }
    }
}
