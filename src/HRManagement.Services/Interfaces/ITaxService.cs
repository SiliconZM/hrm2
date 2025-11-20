using HRManagement.Models.DTOs;

namespace HRManagement.Services.Interfaces
{
    /// <summary>
    /// Service interface for managing tax configurations and calculating taxes
    /// </summary>
    public interface ITaxService
    {
        #region Tax Configuration Management
        Task<PagedResponse<TaxConfigurationDto>> GetAllTaxConfigurationsAsync(long organizationId, int pageNumber = 1, int pageSize = 10);
        Task<TaxConfigurationDto> GetTaxConfigurationByIdAsync(long taxConfigurationId);
        Task<TaxConfigurationDto?> GetActiveTaxConfigurationAsync(long organizationId);
        Task<long> CreateTaxConfigurationAsync(CreateTaxConfigurationRequest request);
        Task UpdateTaxConfigurationAsync(long taxConfigurationId, UpdateTaxConfigurationRequest request);
        Task DeleteTaxConfigurationAsync(long taxConfigurationId);
        #endregion

        #region Tax Slab Management
        Task<List<TaxSlabDto>> GetTaxSlabsByConfigurationAsync(long taxConfigurationId);
        Task<TaxSlabDto> GetTaxSlabByIdAsync(long taxSlabId);
        Task<long> CreateTaxSlabAsync(CreateTaxSlabRequest request);
        Task UpdateTaxSlabAsync(long taxSlabId, UpdateTaxSlabRequest request);
        Task DeleteTaxSlabAsync(long taxSlabId);
        #endregion

        #region Tax Calculations
        /// <summary>
        /// Calculate income tax for a given gross salary and configuration
        /// Supports both fixed percentage and progressive slab-based calculation
        /// </summary>
        Task<decimal> CalculateIncomeTaxAsync(long taxConfigurationId, decimal grossSalary, int monthsInPayroll = 1);

        /// <summary>
        /// Get the applicable tax rate for a given income amount
        /// </summary>
        Task<decimal> GetApplicableTaxRateAsync(long taxConfigurationId, decimal income);
        #endregion
    }
}
