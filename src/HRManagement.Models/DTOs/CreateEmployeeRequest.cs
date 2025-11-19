namespace HRManagement.Models.DTOs
{
    /// <summary>
    /// Request DTO for creating a new employee
    /// </summary>
    public class CreateEmployeeRequest
    {
        public long OrganizationId { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public char? Gender { get; set; }
        public string? MaritalStatus { get; set; }
        public string? Nationality { get; set; }
        public string? EmailPersonal { get; set; }
        public string EmailOfficial { get; set; } = string.Empty;
        public string? PhonePrimary { get; set; }
        public string? PhoneSecondary { get; set; }

        public long DepartmentId { get; set; }
        public long? JobTitleId { get; set; }
        public long? ReportingManagerId { get; set; }

        public string EmploymentType { get; set; } = "Full-time";
        public string EmploymentStatus { get; set; } = "Active";
        public DateTime HireDate { get; set; }

        public decimal? Salary { get; set; }
        public string SalaryFrequency { get; set; } = "Monthly";
        public string? CurrencyCode { get; set; } = "USD";

        public string? PermanentAddress { get; set; }
        public string? CurrentAddress { get; set; }
    }
}
