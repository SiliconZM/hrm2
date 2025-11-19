namespace HRManagement.Models.Entities
{
    /// <summary>
    /// Represents a job posting/vacancy
    /// </summary>
    public class JobPosting
    {
        public long JobPostingId { get; set; }
        public long OrganizationId { get; set; }
        public long? JobTitleId { get; set; }
        public long? DepartmentId { get; set; }

        public int NumberOfOpenings { get; set; } = 1;
        public string? Description { get; set; }
        public string? Requirements { get; set; }
        public string? Qualifications { get; set; }

        public decimal? SalaryRangeMin { get; set; }
        public decimal? SalaryRangeMax { get; set; }

        public string? EmploymentType { get; set; }
        public string? Location { get; set; }

        public DateTime PostedDate { get; set; }
        public DateTime? ClosingDate { get; set; }

        /// <summary>
        /// Status: Open, Closed, Draft, On Hold
        /// </summary>
        public string Status { get; set; } = "Draft";

        /// <summary>
        /// Employee who created the posting
        /// </summary>
        public long? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Organization? Organization { get; set; }
        public virtual JobTitle? JobTitle { get; set; }
        public virtual Department? Department { get; set; }
        public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}
