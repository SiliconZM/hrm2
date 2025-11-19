namespace HRManagement.Models.Entities
{
    /// <summary>
    /// Represents a performance goal or objective
    /// </summary>
    public class Goal
    {
        public long GoalId { get; set; }
        public long EmployeeId { get; set; }
        public long? EvaluationId { get; set; }

        public string GoalTitle { get; set; } = string.Empty;
        public string? Description { get; set; }

        /// <summary>
        /// Status: Not Started, In Progress, Completed, On Hold, Cancelled
        /// </summary>
        public string Status { get; set; } = "Not Started";

        public DateTime? TargetDate { get; set; }

        /// <summary>
        /// Progress percentage (0-100)
        /// </summary>
        public int ProgressPercentage { get; set; }

        /// <summary>
        /// Employee who owns/is responsible for the goal
        /// </summary>
        public long? OwnerId { get; set; }

        /// <summary>
        /// Alignment with organizational strategic goals
        /// </summary>
        public string? Alignment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Employee? Employee { get; set; }
        public virtual Employee? Owner { get; set; }
        public virtual Evaluation? Evaluation { get; set; }
    }
}
