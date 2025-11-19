using HRManagement.Models.DTOs;

namespace HRManagement.Services.Interfaces
{
    /// <summary>
    /// Service for managing employee contracts
    /// </summary>
    public interface IContractsService
    {
        Task<PagedResponse<ContractDto>> GetAllContractsAsync(long organizationId, int pageNumber = 1, int pageSize = 10);
        Task<PagedResponse<ContractDto>> GetEmployeeContractsAsync(long employeeId, int pageNumber = 1, int pageSize = 10);
        Task<ContractDto> GetContractByIdAsync(long contractId);
        Task<long> CreateContractAsync(CreateContractRequest request);
        Task UpdateContractAsync(long contractId, UpdateContractRequest request);
        Task SignContractAsync(long contractId, SignContractRequest request);
        Task RenewContractAsync(long contractId, DateTime newEndDate);
        Task TerminateContractAsync(long contractId);
        Task DeleteContractAsync(long contractId);
        Task<PagedResponse<ContractDto>> GetExpiringContractsAsync(long organizationId, int daysToExpire = 30, int pageNumber = 1, int pageSize = 10);
    }
}
