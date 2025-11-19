using System;

namespace HRManagement.Models.Entities
{
    /// <summary>
    /// Represents the enrollment of an employee in a specific benefit plan
    /// </summary>
    public class EmployeeBenefit
    {
        public long EmployeeBenefitId { get; set; }
        public long EmployeeId { get; set; }
        public long BenefitPlanId { get; set; }

        // Enrollment Information
        public DateTime EnrolledDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string EnrollmentStatus { get; set; } = "Active"; // Active, Suspended, Terminated, Pending

        // Customization for Employee
        public decimal? EmployeeContributionOverride { get; set; } // Override plan's default contribution
        public decimal? EmployerContributionOverride { get; set; } // Override employer contribution
        public string BeneficiaryInfo { get; set; } = string.Empty; // For insurance: beneficiary details

        // Coverage and Claims
        public decimal UsedAmount { get; set; } = 0; // Amount used/claimed under the plan
        public DateTime? LastClaimDate { get; set; }
        public string ClaimNotes { get; set; } = string.Empty;

        // Notes
        public string Remarks { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Employee Employee { get; set; } = null!;
        public BenefitPlan BenefitPlan { get; set; } = null!;
    }
}
