namespace HRManagement.Models.DTOs
{
    /// <summary>
    /// Request DTO for updating an existing employee
    /// </summary>
    public class UpdateEmployeeRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public char? Gender { get; set; }
        public string? MaritalStatus { get; set; }
        public string? Nationality { get; set; }
        public string? EmailPersonal { get; set; }
        public string? EmailOfficial { get; set; }
        public string? PhonePrimary { get; set; }
        public string? PhoneSecondary { get; set; }

        public long? DepartmentId { get; set; }
        public long? JobTitleId { get; set; }
        public long? ReportingManagerId { get; set; }

        public string? EmploymentType { get; set; }
        public string? EmploymentStatus { get; set; }
        public DateTime? TerminationDate { get; set; }

        public decimal? Salary { get; set; }
        public string? SalaryFrequency { get; set; }
        public string? CurrencyCode { get; set; }

        public string? PermanentAddress { get; set; }
        public string? CurrentAddress { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
}
