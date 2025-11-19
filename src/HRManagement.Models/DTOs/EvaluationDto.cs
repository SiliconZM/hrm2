namespace HRManagement.Models.DTOs
{
    public class EvaluationDto
    {
        public long EvaluationId { get; set; }
        public long EmployeeId { get; set; }
        public long? EvaluatorId { get; set; }

        public string EmployeeName { get; set; }
        public string EvaluatorName { get; set; }

        public string EvaluationType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public decimal? OverallRating { get; set; }
        public string Comments { get; set; }
        public string StrengthAreas { get; set; }
        public string ImprovementAreas { get; set; }

        public string Status { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateEvaluationRequest
    {
        public long EmployeeId { get; set; }
        public long? EvaluatorId { get; set; }
        public string EvaluationType { get; set; } = "Annual";
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class UpdateEvaluationRequest
    {
        public string EvaluationType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? OverallRating { get; set; }
        public string Comments { get; set; }
        public string StrengthAreas { get; set; }
        public string ImprovementAreas { get; set; }
        public string Status { get; set; }
    }

    public class GoalDto
    {
        public long GoalId { get; set; }
        public long EmployeeId { get; set; }
        public long? EvaluationId { get; set; }

        public string GoalTitle { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime? TargetDate { get; set; }
        public int ProgressPercentage { get; set; }

        public long? OwnerId { get; set; }
        public string Alignment { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateGoalRequest
    {
        public long EmployeeId { get; set; }
        public long? EvaluationId { get; set; }
        public string GoalTitle { get; set; }
        public string Description { get; set; }
        public DateTime? TargetDate { get; set; }
        public long? OwnerId { get; set; }
        public string Alignment { get; set; }
    }

    public class UpdateGoalRequest
    {
        public string GoalTitle { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime? TargetDate { get; set; }
        public int ProgressPercentage { get; set; }
        public long? OwnerId { get; set; }
        public string Alignment { get; set; }
    }
}
