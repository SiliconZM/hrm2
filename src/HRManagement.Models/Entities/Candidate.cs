namespace HRManagement.Models.Entities
{
    /// <summary>
    /// Represents a job candidate/applicant
    /// </summary>
    public class Candidate
    {
        public long CandidateId { get; set; }
        public long OrganizationId { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string? PhonePrimary { get; set; }

        public string? CurrentCompany { get; set; }
        public string? CurrentDesignation { get; set; }

        /// <summary>
        /// Years of work experience
        /// </summary>
        public int? ExperienceYears { get; set; }

        public string? ResumeUrl { get; set; }
        public string? LinkedInUrl { get; set; }

        /// <summary>
        /// Source: Job Board, Referral, Direct, LinkedIn, etc.
        /// </summary>
        public string? Source { get; set; }

        /// <summary>
        /// Skills as JSON or comma-separated
        /// </summary>
        public string? Skills { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Organization? Organization { get; set; }
        public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

        // Computed property
        public string FullName => $"{FirstName} {LastName}".Trim();
    }
}
