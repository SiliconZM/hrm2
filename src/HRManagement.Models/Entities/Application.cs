namespace HRManagement.Models.Entities
{
    /// <summary>
    /// Represents a job application from a candidate
    /// </summary>
    public class Application
    {
        public long ApplicationId { get; set; }
        public long CandidateId { get; set; }
        public long JobPostingId { get; set; }

        public DateTime ApplicationDate { get; set; }

        /// <summary>
        /// Status: Applied, Screening, Interview, OfferSent, Hired, Rejected
        /// </summary>
        public string Status { get; set; } = "Applied";

        public string? RejectionReason { get; set; }

        /// <summary>
        /// HR person or recruiter assigned to this application
        /// </summary>
        public long? AssignedToId { get; set; }

        public DateTime? InterviewDate { get; set; }
        public string? InterviewerNotes { get; set; }

        /// <summary>
        /// Status: NotOffered, Pending, Accepted, Rejected
        /// </summary>
        public string? OfferStatus { get; set; }

        public decimal? OfferAmount { get; set; }
        public DateTime? HiredDate { get; set; }

        /// <summary>
        /// If hired, link to employee record
        /// </summary>
        public long? LinkedEmployeeId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Candidate? Candidate { get; set; }
        public virtual JobPosting? JobPosting { get; set; }
        public virtual Employee? AssignedTo { get; set; }
        public virtual Employee? LinkedEmployee { get; set; }
    }
}
