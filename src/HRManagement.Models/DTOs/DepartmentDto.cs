namespace HRManagement.Models.DTOs
{
    /// <summary>
    /// DTO for department data
    /// </summary>
    public class DepartmentDto
    {
        public long DepartmentId { get; set; }
        public long OrganizationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? DepartmentCode { get; set; }
        public long? ParentDepartmentId { get; set; }
        public string? ParentDepartmentName { get; set; }
        public long? DepartmentHeadId { get; set; }
        public string? DepartmentHeadName { get; set; }
        public decimal? BudgetAllocated { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
