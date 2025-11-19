namespace HRManagement.Models.DTOs
{
    public class LeaveTypeDto
    {
        public long LeaveTypeId { get; set; }
        public long OrganizationId { get; set; }
        public string LeaveTypeName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DefaultDaysPerYear { get; set; }
        public bool IsCarryOverAllowed { get; set; }
        public int? MaxCarryOver { get; set; }
        public bool RequiresApproval { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateLeaveTypeRequest
    {
        public long OrganizationId { get; set; }
        public string LeaveTypeName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DefaultDaysPerYear { get; set; }
        public bool IsCarryOverAllowed { get; set; }
        public int? MaxCarryOver { get; set; }
        public bool RequiresApproval { get; set; } = true;
    }

    public class UpdateLeaveTypeRequest
    {
        public string LeaveTypeName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DefaultDaysPerYear { get; set; }
        public bool IsCarryOverAllowed { get; set; }
        public int? MaxCarryOver { get; set; }
        public bool RequiresApproval { get; set; }
        public bool IsActive { get; set; }
    }
}
