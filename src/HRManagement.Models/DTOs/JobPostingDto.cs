namespace HRManagement.Models.DTOs
{
    public class JobPostingDto
    {
        public long JobPostingId { get; set; }
        public long OrganizationId { get; set; }
        public long? JobTitleId { get; set; }
        public long? DepartmentId { get; set; }

        public string DepartmentName { get; set; }
        public string JobTitleName { get; set; }

        public int NumberOfOpenings { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
        public string Qualifications { get; set; }

        public decimal? SalaryRangeMin { get; set; }
        public decimal? SalaryRangeMax { get; set; }

        public string EmploymentType { get; set; }
        public string Location { get; set; }

        public DateTime PostedDate { get; set; }
        public DateTime? ClosingDate { get; set; }

        public string Status { get; set; } = "Draft";
        public long? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateJobPostingRequest
    {
        public long OrganizationId { get; set; }
        public long? JobTitleId { get; set; }
        public long? DepartmentId { get; set; }

        public int NumberOfOpenings { get; set; } = 1;
        public string Description { get; set; }
        public string Requirements { get; set; }
        public string Qualifications { get; set; }

        public decimal? SalaryRangeMin { get; set; }
        public decimal? SalaryRangeMax { get; set; }

        public string EmploymentType { get; set; }
        public string Location { get; set; }

        public DateTime? ClosingDate { get; set; }
        public long? CreatedBy { get; set; }
    }

    public class UpdateJobPostingRequest
    {
        public int NumberOfOpenings { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
        public string Qualifications { get; set; }

        public decimal? SalaryRangeMin { get; set; }
        public decimal? SalaryRangeMax { get; set; }

        public string EmploymentType { get; set; }
        public string Location { get; set; }

        public string Status { get; set; }
        public DateTime? ClosingDate { get; set; }
    }
}
