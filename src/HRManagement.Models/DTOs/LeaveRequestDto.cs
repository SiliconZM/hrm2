namespace HRManagement.Models.DTOs
{
    public class LeaveRequestDto
    {
        public long LeaveRequestId { get; set; }
        public long EmployeeId { get; set; }
        public long LeaveTypeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string LeaveTypeName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal DaysRequested { get; set; }
        public string? ReasonForLeave { get; set; }
        public string Status { get; set; } = "Pending";
        public long? ApprovedBy { get; set; }
        public string? ApprovedByName { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string? ApprovalComments { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateLeaveRequestRequest
    {
        public long EmployeeId { get; set; }
        public long LeaveTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal DaysRequested { get; set; }
        public string? ReasonForLeave { get; set; }
    }

    public class UpdateLeaveRequestRequest
    {
        public long LeaveTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal DaysRequested { get; set; }
        public string? ReasonForLeave { get; set; }
        public string Status { get; set; } = "Pending";
    }

    public class ApproveLeaveRequestRequest
    {
        public long ApprovedBy { get; set; }
        public string? ApprovalComments { get; set; }
        public bool IsApproved { get; set; } = true;
    }
}
