namespace HRManagement.Models.Entities
{
    /// <summary>
    /// Represents an employee in the organization
    /// </summary>
    public class Employee
    {
        public long EmployeeId { get; set; }
        public long OrganizationId { get; set; }

        /// <summary>
        /// Unique employee code/ID (e.g., EMP001)
        /// </summary>
        public string EmployeeCode { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// M = Male, F = Female, O = Other
        /// </summary>
        public char? Gender { get; set; }

        public string? MaritalStatus { get; set; }
        public string? Nationality { get; set; }
        public string? EmailPersonal { get; set; }

        /// <summary>
        /// Official work email
        /// </summary>
        public string EmailOfficial { get; set; } = string.Empty;

        public string? PhonePrimary { get; set; }
        public string? PhoneSecondary { get; set; }

        // Organization relationships
        public long DepartmentId { get; set; }
        public long? JobTitleId { get; set; }

        /// <summary>
        /// Direct manager ID
        /// </summary>
        public long? ReportingManagerId { get; set; }

        // Employment details
        public string EmploymentType { get; set; } = "Full-time"; // Full-time, Part-time, Contract
        public string EmploymentStatus { get; set; } = "Active"; // Active, Inactive, On Leave, Terminated
        public DateTime HireDate { get; set; }
        public DateTime? TerminationDate { get; set; }

        // Compensation
        public decimal? Salary { get; set; }
        public string SalaryFrequency { get; set; } = "Monthly"; // Monthly, Annual, Hourly
        public string? CurrencyCode { get; set; } = "USD";

        // Personal details
        public string? PermanentAddress { get; set; }
        public string? CurrentAddress { get; set; }
        public string? ProfileImageUrl { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        // Navigation properties
        public virtual Organization? Organization { get; set; }
        public virtual Department? Department { get; set; }
        public virtual JobTitle? JobTitle { get; set; }
        public virtual Employee? ReportingManager { get; set; }

        /// <summary>
        /// Direct reports (employees reporting to this employee)
        /// </summary>
        public virtual ICollection<Employee> DirectReports { get; set; } = new List<Employee>();

        public virtual ICollection<EmploymentHistory> EmploymentHistories { get; set; } = new List<EmploymentHistory>();

        // Computed property
        public string FullName => $"{FirstName} {LastName}".Trim();
    }
}
