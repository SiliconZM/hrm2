namespace HRManagement.Models.DTOs
{
    /// <summary>
    /// DTO for job title data
    /// </summary>
    public class JobTitleDto
    {
        public long JobTitleId { get; set; }
        public long OrganizationId { get; set; }
        public string TitleName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal? SalaryFloor { get; set; }
        public decimal? SalaryCeiling { get; set; }
        public long? ReportsToJobTitleId { get; set; }
        public string? ReportsToJobTitleName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
