namespace HRManagement.Models.Entities
{
    /// <summary>
    /// Represents a department within an organization
    /// </summary>
    public class Department
    {
        public long DepartmentId { get; set; }
        public long OrganizationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? DepartmentCode { get; set; }

        /// <summary>
        /// Parent department for sub-departments
        /// </summary>
        public long? ParentDepartmentId { get; set; }

        /// <summary>
        /// Department head employee ID
        /// </summary>
        public long? DepartmentHeadId { get; set; }

        public decimal? BudgetAllocated { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        // Navigation properties
        public virtual Organization? Organization { get; set; }
        public virtual Department? ParentDepartment { get; set; }
        public virtual ICollection<Department> SubDepartments { get; set; } = new List<Department>();
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
