namespace HRManagement.Models.Entities
{
    /// <summary>
    /// Represents a salary structure template for an organization
    /// Defines the base salary components and structure
    /// </summary>
    public class SalaryStructure
    {
        public long SalaryStructureId { get; set; }
        public long OrganizationId { get; set; }
        public string StructureName { get; set; } = string.Empty;
        public string? Description { get; set; }

        /// <summary>
        /// Base salary for this structure
        /// </summary>
        public decimal BasicSalary { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Organization? Organization { get; set; }
        public virtual ICollection<SalaryComponent> SalaryComponents { get; set; } = new List<SalaryComponent>();
        public virtual ICollection<EmployeeSalary> EmployeeSalaries { get; set; } = new List<EmployeeSalary>();
    }
}
