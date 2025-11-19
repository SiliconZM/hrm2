namespace HRManagement.Models.Entities
{
    /// <summary>
    /// Represents daily attendance record for an employee
    /// </summary>
    public class Attendance
    {
        public long AttendanceId { get; set; }
        public long EmployeeId { get; set; }

        public DateTime AttendanceDate { get; set; }

        /// <summary>
        /// Time when employee checked in
        /// </summary>
        public DateTime? CheckInTime { get; set; }

        /// <summary>
        /// Time when employee checked out
        /// </summary>
        public DateTime? CheckOutTime { get; set; }

        /// <summary>
        /// Status: Present, Absent, Half-day, Late, Excused, OnLeave
        /// </summary>
        public string Status { get; set; } = "Absent";

        /// <summary>
        /// Total work hours for the day
        /// </summary>
        public decimal? WorkHours { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Employee? Employee { get; set; }
    }
}
