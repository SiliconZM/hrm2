namespace HRManagement.Models.Entities
{
    /// <summary>
    /// Represents a type of leave (Sick, Vacation, etc.)
    /// </summary>
    public class LeaveType
    {
        public long LeaveTypeId { get; set; }
        public long OrganizationId { get; set; }
        public string LeaveTypeName { get; set; } = string.Empty;
        public string? Description { get; set; }

        /// <summary>
        /// Default number of days per year for this leave type
        /// </summary>
        public int DefaultDaysPerYear { get; set; }

        public bool IsCarryOverAllowed { get; set; }
        public int? MaxCarryOver { get; set; }

        /// <summary>
        /// Whether this leave type requires manager approval
        /// </summary>
        public bool RequiresApproval { get; set; } = true;

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Organization? Organization { get; set; }
        public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
        public virtual ICollection<LeaveBalance> LeaveBalances { get; set; } = new List<LeaveBalance>();
    }
}
