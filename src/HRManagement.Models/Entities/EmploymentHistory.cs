namespace HRManagement.Models.Entities
{
    /// <summary>
    /// Tracks employment history changes for an employee
    /// (promotions, transfers, role changes, etc.)
    /// </summary>
    public class EmploymentHistory
    {
        public long EmploymentHistoryId { get; set; }
        public long EmployeeId { get; set; }

        public long? JobTitleId { get; set; }
        public long? DepartmentId { get; set; }
        public long? ReportingManagerId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string? EmploymentType { get; set; }
        public decimal? Salary { get; set; }

        /// <summary>
        /// Reason for the change (Promotion, Transfer, Demotion, etc.)
        /// </summary>
        public string? ReasonForChange { get; set; }

        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Employee? Employee { get; set; }
        public virtual JobTitle? JobTitle { get; set; }
        public virtual Department? Department { get; set; }
    }
}
