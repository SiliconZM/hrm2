using HRManagement.Models.DTOs;

namespace HRManagement.Services.Interfaces
{
    public interface ILeaveService
    {
        #region Leave Type Management
        Task<PagedResponse<LeaveTypeDto>> GetAllLeaveTypesAsync(long organizationId, int pageNumber = 1, int pageSize = 10);
        Task<LeaveTypeDto> GetLeaveTypeByIdAsync(long leaveTypeId);
        Task<long> CreateLeaveTypeAsync(CreateLeaveTypeRequest request);
        Task UpdateLeaveTypeAsync(long leaveTypeId, UpdateLeaveTypeRequest request);
        Task DeleteLeaveTypeAsync(long leaveTypeId);
        #endregion

        #region Leave Request Management
        Task<PagedResponse<LeaveRequestDto>> GetAllLeaveRequestsAsync(long organizationId, int pageNumber = 1, int pageSize = 10);
        Task<PagedResponse<LeaveRequestDto>> GetLeaveRequestsByEmployeeAsync(long employeeId, int pageNumber = 1, int pageSize = 10);
        Task<PagedResponse<LeaveRequestDto>> GetPendingLeaveRequestsAsync(long organizationId, int pageNumber = 1, int pageSize = 10);
        Task<LeaveRequestDto> GetLeaveRequestByIdAsync(long leaveRequestId);
        Task<long> CreateLeaveRequestAsync(CreateLeaveRequestRequest request);
        Task UpdateLeaveRequestAsync(long leaveRequestId, UpdateLeaveRequestRequest request);
        Task ApproveLeaveRequestAsync(long leaveRequestId, ApproveLeaveRequestRequest request);
        Task RejectLeaveRequestAsync(long leaveRequestId, string rejectionReason);
        Task CancelLeaveRequestAsync(long leaveRequestId);
        Task DeleteLeaveRequestAsync(long leaveRequestId);
        #endregion

        #region Leave Balance Management
        Task<PagedResponse<LeaveBalanceDto>> GetAllLeaveBalancesAsync(long organizationId, int pageNumber = 1, int pageSize = 10);
        Task<PagedResponse<LeaveBalanceDto>> GetEmployeeLeaveBalancesAsync(long employeeId, int pageNumber = 1, int pageSize = 10);
        Task<LeaveBalanceDto> GetLeaveBalanceByIdAsync(long leaveBalanceId);
        Task<LeaveBalanceSummaryDto> GetEmployeeLeaveBalanceSummaryAsync(long employeeId, int year);
        Task<long> CreateLeaveBalanceAsync(CreateLeaveBalanceRequest request);
        Task UpdateLeaveBalanceAsync(long leaveBalanceId, UpdateLeaveBalanceRequest request);
        Task UpdateLeaveBalanceUsageAsync(long leaveRequestId, bool approved);
        Task ResetLeaveBalanceAsync(long leaveBalanceId);
        #endregion
    }
}
