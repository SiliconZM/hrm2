namespace HRManagement.Models.DTOs
{
    public class CandidateDto
    {
        public long CandidateId { get; set; }
        public long OrganizationId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhonePrimary { get; set; }

        public string CurrentCompany { get; set; }
        public string CurrentDesignation { get; set; }

        public int? ExperienceYears { get; set; }
        public string ResumeUrl { get; set; }
        public string LinkedInUrl { get; set; }

        public string Source { get; set; }
        public string Skills { get; set; }
        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string FullName => $"{FirstName} {LastName}".Trim();
    }

    public class CreateCandidateRequest
    {
        public long OrganizationId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhonePrimary { get; set; }

        public string CurrentCompany { get; set; }
        public string CurrentDesignation { get; set; }

        public int? ExperienceYears { get; set; }
        public string ResumeUrl { get; set; }
        public string LinkedInUrl { get; set; }

        public string Source { get; set; }
        public string Skills { get; set; }
        public string Notes { get; set; }
    }

    public class UpdateCandidateRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhonePrimary { get; set; }

        public string CurrentCompany { get; set; }
        public string CurrentDesignation { get; set; }

        public int? ExperienceYears { get; set; }
        public string ResumeUrl { get; set; }
        public string LinkedInUrl { get; set; }

        public string Skills { get; set; }
        public string Notes { get; set; }
    }
}
