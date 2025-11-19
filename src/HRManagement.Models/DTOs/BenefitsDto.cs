using System;

namespace HRManagement.Models.DTOs
{
    // ==================== BenefitType DTOs ====================
    public class BenefitTypeDto
    {
        public long BenefitTypeId { get; set; }
        public long OrganizationId { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int PlanCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateBenefitTypeRequest
    {
        public string TypeName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }

    public class UpdateBenefitTypeRequest
    {
        public string TypeName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    // ==================== BenefitPlan DTOs ====================
    public class BenefitPlanDto
    {
        public long BenefitPlanId { get; set; }
        public long BenefitTypeId { get; set; }
        public long OrganizationId { get; set; }
        public string BenefitTypeName { get; set; } = string.Empty;
        public string PlanName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PlanCode { get; set; } = string.Empty;
        public decimal EmployeeContribution { get; set; }
        public decimal EmployerContribution { get; set; }
        public string ContributionFrequency { get; set; } = string.Empty;
        public DateTime EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal CoverageAmount { get; set; }
        public string CoverageDetails { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsDefaultPlan { get; set; }
        public int DisplayOrder { get; set; }
        public int EnrolledEmployees { get; set; }
        public int DeductionCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateBenefitPlanRequest
    {
        public long BenefitTypeId { get; set; }
        public string PlanName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PlanCode { get; set; } = string.Empty;
        public decimal EmployeeContribution { get; set; }
        public decimal EmployerContribution { get; set; }
        public string ContributionFrequency { get; set; } = "Monthly";
        public DateTime EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal CoverageAmount { get; set; }
        public string CoverageDetails { get; set; } = string.Empty;
        public bool IsDefaultPlan { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class UpdateBenefitPlanRequest
    {
        public string PlanName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PlanCode { get; set; } = string.Empty;
        public decimal EmployeeContribution { get; set; }
        public decimal EmployerContribution { get; set; }
        public string ContributionFrequency { get; set; } = string.Empty;
        public DateTime EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal CoverageAmount { get; set; }
        public string CoverageDetails { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsDefaultPlan { get; set; }
        public int DisplayOrder { get; set; }
    }

    // ==================== EmployeeBenefit DTOs ====================
    public class EmployeeBenefitDto
    {
        public long EmployeeBenefitId { get; set; }
        public long EmployeeId { get; set; }
        public long BenefitPlanId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string EmployeeCode { get; set; } = string.Empty;
        public string BenefitTypeName { get; set; } = string.Empty;
        public string PlanName { get; set; } = string.Empty;
        public string PlanCode { get; set; } = string.Empty;
        public DateTime EnrolledDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string EnrollmentStatus { get; set; } = string.Empty;
        public decimal EmployeeContributionOverride { get; set; }
        public decimal EmployerContributionOverride { get; set; }
        public decimal PlanEmployeeContribution { get; set; }
        public decimal PlanEmployerContribution { get; set; }
        public string BeneficiaryInfo { get; set; } = string.Empty;
        public decimal UsedAmount { get; set; }
        public DateTime? LastClaimDate { get; set; }
        public string ClaimNotes { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateEmployeeBenefitRequest
    {
        public long EmployeeId { get; set; }
        public long BenefitPlanId { get; set; }
        public DateTime EnrolledDate { get; set; }
        public string BeneficiaryInfo { get; set; } = string.Empty;
        public decimal? EmployeeContributionOverride { get; set; }
        public decimal? EmployerContributionOverride { get; set; }
        public string Remarks { get; set; } = string.Empty;
    }

    public class UpdateEmployeeBenefitRequest
    {
        public DateTime EnrolledDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string EnrollmentStatus { get; set; } = string.Empty;
        public decimal? EmployeeContributionOverride { get; set; }
        public decimal? EmployerContributionOverride { get; set; }
        public string BeneficiaryInfo { get; set; } = string.Empty;
        public decimal UsedAmount { get; set; }
        public string ClaimNotes { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    // ==================== BenefitDeduction DTOs ====================
    public class BenefitDeductionDto
    {
        public long BenefitDeductionId { get; set; }
        public long BenefitPlanId { get; set; }
        public string PlanName { get; set; } = string.Empty;
        public string DeductionName { get; set; } = string.Empty;
        public string DeductionCode { get; set; } = string.Empty;
        public string DeductionType { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public decimal Percentage { get; set; }
        public bool IsPercentageBased { get; set; }
        public bool IsFixed { get; set; }
        public bool IsTaxable { get; set; }
        public bool IsTaxDeductible { get; set; }
        public string DeductionFrequency { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateBenefitDeductionRequest
    {
        public long BenefitPlanId { get; set; }
        public string DeductionName { get; set; } = string.Empty;
        public string DeductionCode { get; set; } = string.Empty;
        public string DeductionType { get; set; } = string.Empty; // Employee, Employer, Both
        public decimal Amount { get; set; }
        public decimal Percentage { get; set; }
        public bool IsPercentageBased { get; set; }
        public bool IsFixed { get; set; }
        public bool IsTaxable { get; set; }
        public bool IsTaxDeductible { get; set; }
        public string DeductionFrequency { get; set; } = "Monthly";
        public int DisplayOrder { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class UpdateBenefitDeductionRequest
    {
        public string DeductionName { get; set; } = string.Empty;
        public string DeductionCode { get; set; } = string.Empty;
        public string DeductionType { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public decimal Percentage { get; set; }
        public bool IsPercentageBased { get; set; }
        public bool IsFixed { get; set; }
        public bool IsTaxable { get; set; }
        public bool IsTaxDeductible { get; set; }
        public string DeductionFrequency { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    // ==================== Response Wrapper ====================
    public class BenefitsPaginatedResponse<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }
}
