using HRManagement.Models.DTOs;

namespace HRManagement.Services.Interfaces
{
    /// <summary>
    /// Service for managing performance evaluations and goals
    /// </summary>
    public interface IPerformanceService
    {
        // Evaluation operations
        Task<PagedResponse<EvaluationDto>> GetAllEvaluationsAsync(long organizationId, int pageNumber = 1, int pageSize = 10);
        Task<PagedResponse<EvaluationDto>> GetEvaluationsByEmployeeAsync(long employeeId, int pageNumber = 1, int pageSize = 10);
        Task<EvaluationDto> GetEvaluationByIdAsync(long evaluationId);
        Task<long> CreateEvaluationAsync(CreateEvaluationRequest request);
        Task UpdateEvaluationAsync(long evaluationId, UpdateEvaluationRequest request);
        Task SubmitEvaluationAsync(long evaluationId);
        Task ApproveEvaluationAsync(long evaluationId);
        Task DeleteEvaluationAsync(long evaluationId);

        // Goal operations
        Task<PagedResponse<GoalDto>> GetAllGoalsAsync(long organizationId, int pageNumber = 1, int pageSize = 10);
        Task<PagedResponse<GoalDto>> GetGoalsByEmployeeAsync(long employeeId, int pageNumber = 1, int pageSize = 10);
        Task<PagedResponse<GoalDto>> GetGoalsByEvaluationAsync(long evaluationId, int pageNumber = 1, int pageSize = 10);
        Task<GoalDto> GetGoalByIdAsync(long goalId);
        Task<long> CreateGoalAsync(CreateGoalRequest request);
        Task UpdateGoalAsync(long goalId, UpdateGoalRequest request);
        Task UpdateGoalProgressAsync(long goalId, int progressPercentage);
        Task CompleteGoalAsync(long goalId);
        Task DeleteGoalAsync(long goalId);
    }
}
