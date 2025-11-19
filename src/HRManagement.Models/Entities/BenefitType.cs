using System;

namespace HRManagement.Models.Entities
{
    /// <summary>
    /// Represents a type or category of benefit (Health Insurance, Life Insurance, Retirement Plans, etc.)
    /// </summary>
    public class BenefitType
    {
        public long BenefitTypeId { get; set; }
        public long OrganizationId { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // Health, Retirement, Insurance, Wellness, Financial, etc.
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Organization Organization { get; set; } = null!;
        public ICollection<BenefitPlan> BenefitPlans { get; set; } = new List<BenefitPlan>();
    }
}
