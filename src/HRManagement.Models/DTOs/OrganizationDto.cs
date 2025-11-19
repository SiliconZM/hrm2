namespace HRManagement.Models.DTOs
{
    /// <summary>
    /// DTO for organization data
    /// </summary>
    public class OrganizationDto
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
        public int? FiscalYearStart { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
