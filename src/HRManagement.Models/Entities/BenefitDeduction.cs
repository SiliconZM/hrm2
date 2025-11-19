using System;

namespace HRManagement.Models.Entities
{
    /// <summary>
    /// Represents how a benefit plan impacts payroll through deductions and contributions
    /// </summary>
    public class BenefitDeduction
    {
        public long BenefitDeductionId { get; set; }
        public long BenefitPlanId { get; set; }

        // Deduction Configuration
        public string DeductionName { get; set; } = string.Empty; // e.g., "Health Insurance Premium"
        public string DeductionCode { get; set; } = string.Empty; // Unique code like "HIP" (Health Insurance Premium)
        public string DeductionType { get; set; } = string.Empty; // Employee, Employer, Both

        // Amount Configuration
        public decimal Amount { get; set; } // Fixed amount
        public decimal Percentage { get; set; } // Percentage of salary (0-100)
        public bool IsPercentageBased { get; set; } = false;
        public bool IsFixed { get; set; } = true;

        // Tax and Compliance
        public bool IsTaxable { get; set; } = false; // Whether this deduction is taxable
        public bool IsTaxDeductible { get; set; } = false; // Tax deductible expense for employee

        // Frequency and Timing
        public string DeductionFrequency { get; set; } = "Monthly"; // Monthly, Quarterly, Yearly, Per Payroll
        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; } = 0;

        // Effective Period
        public DateTime EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public BenefitPlan BenefitPlan { get; set; } = null!;
    }
}
