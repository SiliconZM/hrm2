using HRManagement.Models.DTOs;

namespace HRManagement.Services.Interfaces
{
    /// <summary>
    /// Service interface for managing employee benefits including benefit types, plans, enrollments, and deductions
    /// </summary>
    public interface IBenefitsService
    {
        // ==================== BenefitType Operations ====================
        /// <summary>
        /// Get all benefit types for an organization with pagination
        /// </summary>
        Task<PagedResponse<BenefitTypeDto>> GetAllBenefitTypesAsync(long organizationId, int pageNumber = 1, int pageSize = 10);

        /// <summary>
        /// Get a specific benefit type by ID
        /// </summary>
        Task<BenefitTypeDto> GetBenefitTypeByIdAsync(long benefitTypeId);

        /// <summary>
        /// Create a new benefit type
        /// </summary>
        Task<BenefitTypeDto> CreateBenefitTypeAsync(long organizationId, CreateBenefitTypeRequest request);

        /// <summary>
        /// Update an existing benefit type
        /// </summary>
        Task<BenefitTypeDto> UpdateBenefitTypeAsync(long benefitTypeId, UpdateBenefitTypeRequest request);

        /// <summary>
        /// Delete a benefit type
        /// </summary>
        Task<bool> DeleteBenefitTypeAsync(long benefitTypeId);

        // ==================== BenefitPlan Operations ====================
        /// <summary>
        /// Get all benefit plans for a benefit type
        /// </summary>
        Task<PagedResponse<BenefitPlanDto>> GetPlansByBenefitTypeAsync(long benefitTypeId, int pageNumber = 1, int pageSize = 10);

        /// <summary>
        /// Get all benefit plans for an organization
        /// </summary>
        Task<PagedResponse<BenefitPlanDto>> GetAllBenefitPlansAsync(long organizationId, int pageNumber = 1, int pageSize = 10);

        /// <summary>
        /// Get a specific benefit plan by ID
        /// </summary>
        Task<BenefitPlanDto> GetBenefitPlanByIdAsync(long benefitPlanId);

        /// <summary>
        /// Create a new benefit plan
        /// </summary>
        Task<BenefitPlanDto> CreateBenefitPlanAsync(long organizationId, CreateBenefitPlanRequest request);

        /// <summary>
        /// Update an existing benefit plan
        /// </summary>
        Task<BenefitPlanDto> UpdateBenefitPlanAsync(long benefitPlanId, UpdateBenefitPlanRequest request);

        /// <summary>
        /// Delete a benefit plan
        /// </summary>
        Task<bool> DeleteBenefitPlanAsync(long benefitPlanId);

        /// <summary>
        /// Get active benefit plans for new employee enrollment
        /// </summary>
        Task<PagedResponse<BenefitPlanDto>> GetActiveBenefitPlansAsync(long organizationId, int pageNumber = 1, int pageSize = 10);

        // ==================== EmployeeBenefit Operations ====================
        /// <summary>
        /// Get all enrolled benefits for an employee
        /// </summary>
        Task<PagedResponse<EmployeeBenefitDto>> GetEmployeeBenefitsAsync(long employeeId, int pageNumber = 1, int pageSize = 10);

        /// <summary>
        /// Get all enrolled employees for a benefit plan
        /// </summary>
        Task<PagedResponse<EmployeeBenefitDto>> GetPlanEnrolleesAsync(long benefitPlanId, int pageNumber = 1, int pageSize = 10);

        /// <summary>
        /// Get a specific employee benefit enrollment
        /// </summary>
        Task<EmployeeBenefitDto> GetEmployeeBenefitByIdAsync(long employeeBenefitId);

        /// <summary>
        /// Enroll an employee in a benefit plan
        /// </summary>
        Task<EmployeeBenefitDto> EnrollEmployeeBenefitAsync(CreateEmployeeBenefitRequest request);

        /// <summary>
        /// Update an employee's benefit enrollment (change status, contributions, etc.)
        /// </summary>
        Task<EmployeeBenefitDto> UpdateEmployeeBenefitAsync(long employeeBenefitId, UpdateEmployeeBenefitRequest request);

        /// <summary>
        /// Terminate an employee's benefit enrollment
        /// </summary>
        Task<bool> TerminateEmployeeBenefitAsync(long employeeBenefitId, DateTime terminationDate);

        /// <summary>
        /// Get active benefits for an employee on a specific date
        /// </summary>
        Task<List<EmployeeBenefitDto>> GetActiveEmployeeBenefitsAsync(long employeeId, DateTime onDate);

        /// <summary>
        /// Get benefit enrollment history for an employee
        /// </summary>
        Task<PagedResponse<EmployeeBenefitDto>> GetEmployeeBenefitHistoryAsync(long employeeId, int pageNumber = 1, int pageSize = 10);

        // ==================== BenefitDeduction Operations ====================
        /// <summary>
        /// Get all deductions for a benefit plan
        /// </summary>
        Task<PagedResponse<BenefitDeductionDto>> GetDeductionsForPlanAsync(long benefitPlanId, int pageNumber = 1, int pageSize = 10);

        /// <summary>
        /// Get a specific benefit deduction
        /// </summary>
        Task<BenefitDeductionDto> GetBenefitDeductionByIdAsync(long benefitDeductionId);

        /// <summary>
        /// Create a new deduction for a benefit plan
        /// </summary>
        Task<BenefitDeductionDto> CreateBenefitDeductionAsync(CreateBenefitDeductionRequest request);

        /// <summary>
        /// Update an existing benefit deduction
        /// </summary>
        Task<BenefitDeductionDto> UpdateBenefitDeductionAsync(long benefitDeductionId, UpdateBenefitDeductionRequest request);

        /// <summary>
        /// Delete a benefit deduction
        /// </summary>
        Task<bool> DeleteBenefitDeductionAsync(long benefitDeductionId);

        // ==================== Calculations and Analytics ====================
        /// <summary>
        /// Calculate total employee benefit cost for a specific plan
        /// </summary>
        Task<decimal> CalculateEmployeeBenefitCostAsync(long benefitPlanId);

        /// <summary>
        /// Calculate total employer benefit cost for a specific plan
        /// </summary>
        Task<decimal> CalculateEmployerBenefitCostAsync(long benefitPlanId);

        /// <summary>
        /// Get benefits summary for an employee
        /// </summary>
        Task<BenefitsSummaryDto> GetEmployeeBenefitsSummaryAsync(long employeeId);

        /// <summary>
        /// Get benefits utilization report for a plan
        /// </summary>
        Task<BenefitsUtilizationReportDto> GetBenefitUtilizationReportAsync(long benefitPlanId);
    }

    /// <summary>
    /// DTO for benefits summary
    /// </summary>
    public class BenefitsSummaryDto
    {
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public int TotalActiveBenefits { get; set; }
        public decimal TotalEmployeeContribution { get; set; }
        public decimal TotalEmployerContribution { get; set; }
        public List<string> BenefitNames { get; set; } = new();
        public List<string> BenefitStatuses { get; set; } = new();
    }

    /// <summary>
    /// DTO for benefits utilization report
    /// </summary>
    public class BenefitsUtilizationReportDto
    {
        public long BenefitPlanId { get; set; }
        public string PlanName { get; set; } = string.Empty;
        public int TotalEnrolled { get; set; }
        public int ActiveEnrollees { get; set; }
        public int SuspendedEnrollees { get; set; }
        public int TerminatedEnrollees { get; set; }
        public decimal TotalClaimsAmount { get; set; }
        public decimal TotalCoverageAmount { get; set; }
        public decimal CoverageUtilizationPercentage { get; set; }
        public decimal EmployeeContributionTotal { get; set; }
        public decimal EmployerContributionTotal { get; set; }
    }
}
