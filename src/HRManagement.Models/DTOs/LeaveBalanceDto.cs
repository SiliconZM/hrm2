namespace HRManagement.Models.DTOs
{
    public class LeaveBalanceDto
    {
        public long LeaveBalanceId { get; set; }
        public long EmployeeId { get; set; }
        public long LeaveTypeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string LeaveTypeName { get; set; } = string.Empty;
        public int FinancialYear { get; set; }
        public decimal TotalDays { get; set; }
        public decimal UsedDays { get; set; }
        public decimal CarryOverDays { get; set; }
        public decimal AvailableDays { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateLeaveBalanceRequest
    {
        public long EmployeeId { get; set; }
        public long LeaveTypeId { get; set; }
        public int FinancialYear { get; set; }
        public decimal TotalDays { get; set; }
        public decimal CarryOverDays { get; set; }
    }

    public class UpdateLeaveBalanceRequest
    {
        public decimal TotalDays { get; set; }
        public decimal UsedDays { get; set; }
        public decimal CarryOverDays { get; set; }
    }

    public class LeaveBalanceSummaryDto
    {
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public int FinancialYear { get; set; }
        public List<LeaveTypeBalance> LeaveTypeBalances { get; set; } = new();
        public decimal TotalAvailableDays { get; set; }
    }

    public class LeaveTypeBalance
    {
        public string LeaveTypeName { get; set; } = string.Empty;
        public decimal TotalDays { get; set; }
        public decimal UsedDays { get; set; }
        public decimal AvailableDays { get; set; }
    }
}
