namespace HRManagement.Models.Entities
{
    /// <summary>
    /// Represents a performance evaluation/appraisal
    /// </summary>
    public class Evaluation
    {
        public long EvaluationId { get; set; }
        public long EmployeeId { get; set; }

        /// <summary>
        /// Employee who is evaluating (manager or self-evaluation)
        /// </summary>
        public long? EvaluatorId { get; set; }

        public long? EvaluationCycleId { get; set; }

        /// <summary>
        /// Type: Annual, Half-yearly, Quarterly, Project, 360-Degree
        /// </summary>
        public string EvaluationType { get; set; } = "Annual";

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Overall rating (1-5 scale)
        /// </summary>
        public decimal? OverallRating { get; set; }

        public string? Comments { get; set; }
        public string? StrengthAreas { get; set; }
        public string? ImprovementAreas { get; set; }

        /// <summary>
        /// Status: Draft, Submitted, Approved, Acknowledged
        /// </summary>
        public string Status { get; set; } = "Draft";

        public DateTime? SubmittedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Employee? Employee { get; set; }
        public virtual Employee? Evaluator { get; set; }
        public virtual ICollection<Goal> Goals { get; set; } = new List<Goal>();
    }
}
