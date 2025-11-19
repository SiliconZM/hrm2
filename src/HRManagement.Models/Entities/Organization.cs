namespace HRManagement.Models.Entities
{
    /// <summary>
    /// Represents an organization/company in the HR system
    /// </summary>
    public class Organization
    {
        public long OrganizationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? IndustryType { get; set; }
        public string? CountryCode { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public string? ZipCode { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public string? LogoUrl { get; set; }

        /// <summary>
        /// Fiscal year start month (1-12)
        /// </summary>
        public int? FiscalYearStart { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        // Navigation properties
        public virtual ICollection<Department> Departments { get; set; } = new List<Department>();
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public virtual ICollection<JobTitle> JobTitles { get; set; } = new List<JobTitle>();
    }
}
