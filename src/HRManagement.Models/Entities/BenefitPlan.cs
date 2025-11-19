using System;

namespace HRManagement.Models.Entities
{
    /// <summary>
    /// Represents a specific benefit plan (e.g., "Gold Health Plan", "Executive Retirement Plan")
    /// </summary>
    public class BenefitPlan
    {
        public long BenefitPlanId { get; set; }
        public long BenefitTypeId { get; set; }
        public long OrganizationId { get; set; }
        public string PlanName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PlanCode { get; set; } = string.Empty; // Unique identifier like "HEALTH-GOLD-001"

        // Plan Details
        public decimal EmployeeContribution { get; set; } // Employee's monthly/yearly cost
        public decimal EmployerContribution { get; set; } // Employer's contribution
        public string ContributionFrequency { get; set; } = "Monthly"; // Monthly, Quarterly, Yearly
        public DateTime EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Coverage and Limits
        public decimal CoverageAmount { get; set; } // Total coverage limit (for insurance plans)
        public string CoverageDetails { get; set; } = string.Empty; // JSON or text details

        // Status and Tracking
        public bool IsActive { get; set; } = true;
        public bool IsDefaultPlan { get; set; } = false; // Default option for new employees
        public int DisplayOrder { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public BenefitType BenefitType { get; set; } = null!;
        public Organization Organization { get; set; } = null!;
        public ICollection<EmployeeBenefit> EmployeeBenefits { get; set; } = new List<EmployeeBenefit>();
        public ICollection<BenefitDeduction> BenefitDeductions { get; set; } = new List<BenefitDeduction>();
    }
}
