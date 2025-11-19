namespace HRManagement.Models.DTOs
{
    public class ApplicationDto
    {
        public long ApplicationId { get; set; }
        public long CandidateId { get; set; }
        public long JobPostingId { get; set; }

        public string CandidateName { get; set; }
        public string JobPostingTitle { get; set; }

        public DateTime ApplicationDate { get; set; }
        public string Status { get; set; } = "Applied";

        public string RejectionReason { get; set; }

        public long? AssignedToId { get; set; }
        public string AssignedToName { get; set; }

        public DateTime? InterviewDate { get; set; }
        public string InterviewerNotes { get; set; }

        public string OfferStatus { get; set; }
        public decimal? OfferAmount { get; set; }
        public DateTime? HiredDate { get; set; }

        public long? LinkedEmployeeId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateApplicationRequest
    {
        public long CandidateId { get; set; }
        public long JobPostingId { get; set; }
        public long? AssignedToId { get; set; }
    }

    public class UpdateApplicationRequest
    {
        public string Status { get; set; }
        public string RejectionReason { get; set; }

        public long? AssignedToId { get; set; }
        public DateTime? InterviewDate { get; set; }
        public string InterviewerNotes { get; set; }

        public string OfferStatus { get; set; }
        public decimal? OfferAmount { get; set; }
    }

    public class HireApplicationRequest
    {
        public long CandidateId { get; set; }
        public long ApplicationId { get; set; }

        public string EmployeeCode { get; set; }
        public long DepartmentId { get; set; }
        public long JobTitleId { get; set; }

        public DateTime HireDate { get; set; }
        public string EmploymentType { get; set; } = "Full-time";
    }
}
