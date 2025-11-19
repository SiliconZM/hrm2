namespace HRManagement.Models.Entities
{
    /// <summary>
    /// Represents a job title/position within an organization
    /// </summary>
    public class JobTitle
    {
        public long JobTitleId { get; set; }
        public long OrganizationId { get; set; }
        public string TitleName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal? SalaryFloor { get; set; }
        public decimal? SalaryCeiling { get; set; }

        /// <summary>
        /// Reports to (parent job title in hierarchy)
        /// </summary>
        public long? ReportsToJobTitleId { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Organization? Organization { get; set; }
        public virtual JobTitle? ReportsToJobTitle { get; set; }
        public virtual ICollection<JobTitle> SubordinateJobTitles { get; set; } = new List<JobTitle>();
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
