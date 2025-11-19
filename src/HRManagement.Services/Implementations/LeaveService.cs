using AutoMapper;
using HRManagement.Data;
using HRManagement.Models.DTOs;
using HRManagement.Models.Entities;
using HRManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Services.Implementations
{
    public class LeaveService : ILeaveService
    {
        private readonly HRContext _context;
        private readonly IMapper _mapper;

        public LeaveService(HRContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region Leave Type Management
        public async Task<PagedResponse<LeaveTypeDto>> GetAllLeaveTypesAsync(long organizationId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.LeaveTypes
                .Where(lt => lt.OrganizationId == organizationId)
                .AsQueryable();

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResponse<LeaveTypeDto>
            {
                Items = _mapper.Map<List<LeaveTypeDto>>(items),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<LeaveTypeDto> GetLeaveTypeByIdAsync(long leaveTypeId)
        {
            var leaveType = await _context.LeaveTypes.FirstOrDefaultAsync(lt => lt.LeaveTypeId == leaveTypeId);
            if (leaveType == null)
                throw new Exception($"Leave Type with ID {leaveTypeId} not found.");

            return _mapper.Map<LeaveTypeDto>(leaveType);
        }

        public async Task<long> CreateLeaveTypeAsync(CreateLeaveTypeRequest request)
        {
            var leaveType = new LeaveType
            {
                OrganizationId = request.OrganizationId,
                LeaveTypeName = request.LeaveTypeName,
                Description = request.Description,
                DefaultDaysPerYear = request.DefaultDaysPerYear,
                IsCarryOverAllowed = request.IsCarryOverAllowed,
                MaxCarryOver = request.MaxCarryOver,
                RequiresApproval = request.RequiresApproval,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.LeaveTypes.Add(leaveType);
            await _context.SaveChangesAsync();
            return leaveType.LeaveTypeId;
        }

        public async Task UpdateLeaveTypeAsync(long leaveTypeId, UpdateLeaveTypeRequest request)
        {
            var leaveType = await _context.LeaveTypes.FirstOrDefaultAsync(lt => lt.LeaveTypeId == leaveTypeId);
            if (leaveType == null)
                throw new Exception($"Leave Type with ID {leaveTypeId} not found.");

            leaveType.LeaveTypeName = request.LeaveTypeName;
            leaveType.Description = request.Description;
            leaveType.DefaultDaysPerYear = request.DefaultDaysPerYear;
            leaveType.IsCarryOverAllowed = request.IsCarryOverAllowed;
            leaveType.MaxCarryOver = request.MaxCarryOver;
            leaveType.RequiresApproval = request.RequiresApproval;
            leaveType.IsActive = request.IsActive;
            leaveType.UpdatedAt = DateTime.UtcNow;

            _context.LeaveTypes.Update(leaveType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLeaveTypeAsync(long leaveTypeId)
        {
            var leaveType = await _context.LeaveTypes.FirstOrDefaultAsync(lt => lt.LeaveTypeId == leaveTypeId);
            if (leaveType == null)
                throw new Exception($"Leave Type with ID {leaveTypeId} not found.");

            _context.LeaveTypes.Remove(leaveType);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Leave Request Management
        public async Task<PagedResponse<LeaveRequestDto>> GetAllLeaveRequestsAsync(long organizationId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.LeaveRequests
                .Include(lr => lr.Employee)
                .Include(lr => lr.LeaveType)
                .Where(lr => lr.Employee!.OrganizationId == organizationId)
                .AsQueryable();

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(lr => lr.StartDate)
                .ToListAsync();

            return new PagedResponse<LeaveRequestDto>
            {
                Items = _mapper.Map<List<LeaveRequestDto>>(items),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<PagedResponse<LeaveRequestDto>> GetLeaveRequestsByEmployeeAsync(long employeeId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.LeaveRequests
                .Include(lr => lr.Employee)
                .Include(lr => lr.LeaveType)
                .Where(lr => lr.EmployeeId == employeeId)
                .AsQueryable();

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(lr => lr.StartDate)
                .ToListAsync();

            return new PagedResponse<LeaveRequestDto>
            {
                Items = _mapper.Map<List<LeaveRequestDto>>(items),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<PagedResponse<LeaveRequestDto>> GetPendingLeaveRequestsAsync(long organizationId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.LeaveRequests
                .Include(lr => lr.Employee)
                .Include(lr => lr.LeaveType)
                .Where(lr => lr.Employee!.OrganizationId == organizationId && lr.Status == "Pending")
                .AsQueryable();

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(lr => lr.StartDate)
                .ToListAsync();

            return new PagedResponse<LeaveRequestDto>
            {
                Items = _mapper.Map<List<LeaveRequestDto>>(items),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<LeaveRequestDto> GetLeaveRequestByIdAsync(long leaveRequestId)
        {
            var leaveRequest = await _context.LeaveRequests
                .Include(lr => lr.Employee)
                .Include(lr => lr.LeaveType)
                .FirstOrDefaultAsync(lr => lr.LeaveRequestId == leaveRequestId);

            if (leaveRequest == null)
                throw new Exception($"Leave Request with ID {leaveRequestId} not found.");

            return _mapper.Map<LeaveRequestDto>(leaveRequest);
        }

        public async Task<long> CreateLeaveRequestAsync(CreateLeaveRequestRequest request)
        {
            var leaveRequest = new LeaveRequest
            {
                EmployeeId = request.EmployeeId,
                LeaveTypeId = request.LeaveTypeId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                DaysRequested = request.DaysRequested,
                ReasonForLeave = request.ReasonForLeave,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.LeaveRequests.Add(leaveRequest);
            await _context.SaveChangesAsync();
            return leaveRequest.LeaveRequestId;
        }

        public async Task UpdateLeaveRequestAsync(long leaveRequestId, UpdateLeaveRequestRequest request)
        {
            var leaveRequest = await _context.LeaveRequests.FirstOrDefaultAsync(lr => lr.LeaveRequestId == leaveRequestId);
            if (leaveRequest == null)
                throw new Exception($"Leave Request with ID {leaveRequestId} not found.");

            if (leaveRequest.Status != "Pending")
                throw new Exception("Only pending leave requests can be updated.");

            leaveRequest.LeaveTypeId = request.LeaveTypeId;
            leaveRequest.StartDate = request.StartDate;
            leaveRequest.EndDate = request.EndDate;
            leaveRequest.DaysRequested = request.DaysRequested;
            leaveRequest.ReasonForLeave = request.ReasonForLeave;
            leaveRequest.UpdatedAt = DateTime.UtcNow;

            _context.LeaveRequests.Update(leaveRequest);
            await _context.SaveChangesAsync();
        }

        public async Task ApproveLeaveRequestAsync(long leaveRequestId, ApproveLeaveRequestRequest request)
        {
            var leaveRequest = await _context.LeaveRequests.FirstOrDefaultAsync(lr => lr.LeaveRequestId == leaveRequestId);
            if (leaveRequest == null)
                throw new Exception($"Leave Request with ID {leaveRequestId} not found.");

            if (request.IsApproved)
            {
                leaveRequest.Status = "Approved";
                leaveRequest.ApprovedBy = request.ApprovedBy;
                leaveRequest.ApprovedDate = DateTime.UtcNow;
                leaveRequest.ApprovalComments = request.ApprovalComments;

                // Update leave balance
                await UpdateLeaveBalanceUsageAsync(leaveRequestId, true);
            }
            else
            {
                leaveRequest.Status = "Rejected";
                leaveRequest.ApprovedBy = request.ApprovedBy;
                leaveRequest.ApprovedDate = DateTime.UtcNow;
                leaveRequest.ApprovalComments = request.ApprovalComments;
            }

            leaveRequest.UpdatedAt = DateTime.UtcNow;
            _context.LeaveRequests.Update(leaveRequest);
            await _context.SaveChangesAsync();
        }

        public async Task RejectLeaveRequestAsync(long leaveRequestId, string rejectionReason)
        {
            var leaveRequest = await _context.LeaveRequests.FirstOrDefaultAsync(lr => lr.LeaveRequestId == leaveRequestId);
            if (leaveRequest == null)
                throw new Exception($"Leave Request with ID {leaveRequestId} not found.");

            leaveRequest.Status = "Rejected";
            leaveRequest.ApprovalComments = rejectionReason;
            leaveRequest.UpdatedAt = DateTime.UtcNow;

            _context.LeaveRequests.Update(leaveRequest);
            await _context.SaveChangesAsync();
        }

        public async Task CancelLeaveRequestAsync(long leaveRequestId)
        {
            var leaveRequest = await _context.LeaveRequests.FirstOrDefaultAsync(lr => lr.LeaveRequestId == leaveRequestId);
            if (leaveRequest == null)
                throw new Exception($"Leave Request with ID {leaveRequestId} not found.");

            if (leaveRequest.Status == "Approved")
            {
                // Reverse the leave balance impact
                var leaveBalance = await _context.LeaveBalances.FirstOrDefaultAsync(
                    lb => lb.EmployeeId == leaveRequest.EmployeeId && lb.LeaveTypeId == leaveRequest.LeaveTypeId);

                if (leaveBalance != null)
                {
                    leaveBalance.UsedDays -= leaveRequest.DaysRequested;
                    _context.LeaveBalances.Update(leaveBalance);
                }
            }

            leaveRequest.Status = "Cancelled";
            leaveRequest.UpdatedAt = DateTime.UtcNow;

            _context.LeaveRequests.Update(leaveRequest);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLeaveRequestAsync(long leaveRequestId)
        {
            var leaveRequest = await _context.LeaveRequests.FirstOrDefaultAsync(lr => lr.LeaveRequestId == leaveRequestId);
            if (leaveRequest == null)
                throw new Exception($"Leave Request with ID {leaveRequestId} not found.");

            _context.LeaveRequests.Remove(leaveRequest);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Leave Balance Management
        public async Task<PagedResponse<LeaveBalanceDto>> GetAllLeaveBalancesAsync(long organizationId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.LeaveBalances
                .Include(lb => lb.Employee)
                .Include(lb => lb.LeaveType)
                .Where(lb => lb.Employee!.OrganizationId == organizationId)
                .AsQueryable();

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(lb => lb.Employee!.FirstName)
                .ToListAsync();

            return new PagedResponse<LeaveBalanceDto>
            {
                Items = _mapper.Map<List<LeaveBalanceDto>>(items),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<PagedResponse<LeaveBalanceDto>> GetEmployeeLeaveBalancesAsync(long employeeId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.LeaveBalances
                .Include(lb => lb.Employee)
                .Include(lb => lb.LeaveType)
                .Where(lb => lb.EmployeeId == employeeId)
                .AsQueryable();

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(lb => lb.FinancialYear)
                .ToListAsync();

            return new PagedResponse<LeaveBalanceDto>
            {
                Items = _mapper.Map<List<LeaveBalanceDto>>(items),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<LeaveBalanceDto> GetLeaveBalanceByIdAsync(long leaveBalanceId)
        {
            var leaveBalance = await _context.LeaveBalances
                .Include(lb => lb.Employee)
                .Include(lb => lb.LeaveType)
                .FirstOrDefaultAsync(lb => lb.LeaveBalanceId == leaveBalanceId);

            if (leaveBalance == null)
                throw new Exception($"Leave Balance with ID {leaveBalanceId} not found.");

            return _mapper.Map<LeaveBalanceDto>(leaveBalance);
        }

        public async Task<LeaveBalanceSummaryDto> GetEmployeeLeaveBalanceSummaryAsync(long employeeId, int year)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
            if (employee == null)
                throw new Exception($"Employee with ID {employeeId} not found.");

            var balances = await _context.LeaveBalances
                .Include(lb => lb.LeaveType)
                .Where(lb => lb.EmployeeId == employeeId && lb.FinancialYear == year)
                .ToListAsync();

            var summary = new LeaveBalanceSummaryDto
            {
                EmployeeId = employeeId,
                EmployeeName = $"{employee.FirstName} {employee.LastName}",
                FinancialYear = year,
                LeaveTypeBalances = balances.Select(b => new LeaveTypeBalance
                {
                    LeaveTypeName = b.LeaveType?.LeaveTypeName ?? "Unknown",
                    TotalDays = b.TotalDays,
                    UsedDays = b.UsedDays,
                    AvailableDays = b.AvailableDays
                }).ToList(),
                TotalAvailableDays = balances.Sum(b => b.AvailableDays)
            };

            return summary;
        }

        public async Task<long> CreateLeaveBalanceAsync(CreateLeaveBalanceRequest request)
        {
            var leaveBalance = new LeaveBalance
            {
                EmployeeId = request.EmployeeId,
                LeaveTypeId = request.LeaveTypeId,
                FinancialYear = request.FinancialYear,
                TotalDays = request.TotalDays,
                UsedDays = 0,
                CarryOverDays = request.CarryOverDays,
                UpdatedAt = DateTime.UtcNow
            };

            _context.LeaveBalances.Add(leaveBalance);
            await _context.SaveChangesAsync();
            return leaveBalance.LeaveBalanceId;
        }

        public async Task UpdateLeaveBalanceAsync(long leaveBalanceId, UpdateLeaveBalanceRequest request)
        {
            var leaveBalance = await _context.LeaveBalances.FirstOrDefaultAsync(lb => lb.LeaveBalanceId == leaveBalanceId);
            if (leaveBalance == null)
                throw new Exception($"Leave Balance with ID {leaveBalanceId} not found.");

            leaveBalance.TotalDays = request.TotalDays;
            leaveBalance.UsedDays = request.UsedDays;
            leaveBalance.CarryOverDays = request.CarryOverDays;
            leaveBalance.UpdatedAt = DateTime.UtcNow;

            _context.LeaveBalances.Update(leaveBalance);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateLeaveBalanceUsageAsync(long leaveRequestId, bool approved)
        {
            var leaveRequest = await _context.LeaveRequests.FirstOrDefaultAsync(lr => lr.LeaveRequestId == leaveRequestId);
            if (leaveRequest == null)
                throw new Exception($"Leave Request with ID {leaveRequestId} not found.");

            if (!approved)
                return;

            var leaveBalance = await _context.LeaveBalances.FirstOrDefaultAsync(
                lb => lb.EmployeeId == leaveRequest.EmployeeId && lb.LeaveTypeId == leaveRequest.LeaveTypeId);

            if (leaveBalance != null)
            {
                leaveBalance.UsedDays += leaveRequest.DaysRequested;
                leaveBalance.UpdatedAt = DateTime.UtcNow;
                _context.LeaveBalances.Update(leaveBalance);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ResetLeaveBalanceAsync(long leaveBalanceId)
        {
            var leaveBalance = await _context.LeaveBalances.FirstOrDefaultAsync(lb => lb.LeaveBalanceId == leaveBalanceId);
            if (leaveBalance == null)
                throw new Exception($"Leave Balance with ID {leaveBalanceId} not found.");

            leaveBalance.UsedDays = 0;
            leaveBalance.UpdatedAt = DateTime.UtcNow;

            _context.LeaveBalances.Update(leaveBalance);
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
