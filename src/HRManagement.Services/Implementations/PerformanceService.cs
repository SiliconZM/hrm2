using HRManagement.Data;
using HRManagement.Models.DTOs;
using HRManagement.Models.Entities;
using HRManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Services.Implementations
{
    public class PerformanceService : IPerformanceService
    {
        private readonly HRContext _context;

        public PerformanceService(HRContext context)
        {
            _context = context;
        }

        #region Evaluation Operations

        public async Task<PagedResponse<EvaluationDto>> GetAllEvaluationsAsync(long organizationId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Evaluations
                .Include(e => e.Employee)
                .Include(e => e.Evaluator)
                .Where(e => e.Employee.OrganizationId == organizationId);

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(e => e.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            var dtos = items.Select(e => MapEvaluationToDto(e)).ToList();

            return new PagedResponse<EvaluationDto>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<PagedResponse<EvaluationDto>> GetEvaluationsByEmployeeAsync(long employeeId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Evaluations
                .Include(e => e.Employee)
                .Include(e => e.Evaluator)
                .Where(e => e.EmployeeId == employeeId);

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(e => e.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            var dtos = items.Select(e => MapEvaluationToDto(e)).ToList();

            return new PagedResponse<EvaluationDto>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<EvaluationDto> GetEvaluationByIdAsync(long evaluationId)
        {
            var evaluation = await _context.Evaluations
                .Include(e => e.Employee)
                .Include(e => e.Evaluator)
                .FirstOrDefaultAsync(e => e.EvaluationId == evaluationId);

            if (evaluation == null)
                throw new InvalidOperationException($"Evaluation {evaluationId} not found");

            return MapEvaluationToDto(evaluation);
        }

        public async Task<long> CreateEvaluationAsync(CreateEvaluationRequest request)
        {
            // Validate employee
            var employee = await _context.Employees.FindAsync(request.EmployeeId);
            if (employee == null)
                throw new InvalidOperationException($"Employee {request.EmployeeId} not found");

            // Validate evaluator if provided
            if (request.EvaluatorId.HasValue)
            {
                var evaluator = await _context.Employees.FindAsync(request.EvaluatorId.Value);
                if (evaluator == null)
                    throw new InvalidOperationException($"Evaluator {request.EvaluatorId} not found");
            }

            var evaluation = new Evaluation
            {
                EmployeeId = request.EmployeeId,
                EvaluatorId = request.EvaluatorId,
                EvaluationType = request.EvaluationType,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Status = "Draft",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Evaluations.Add(evaluation);
            await _context.SaveChangesAsync();

            return evaluation.EvaluationId;
        }

        public async Task UpdateEvaluationAsync(long evaluationId, UpdateEvaluationRequest request)
        {
            var evaluation = await _context.Evaluations.FindAsync(evaluationId);
            if (evaluation == null)
                throw new InvalidOperationException($"Evaluation {evaluationId} not found");

            evaluation.EvaluationType = request.EvaluationType;
            evaluation.StartDate = request.StartDate;
            evaluation.EndDate = request.EndDate;
            evaluation.OverallRating = request.OverallRating;
            evaluation.Comments = request.Comments;
            evaluation.StrengthAreas = request.StrengthAreas;
            evaluation.ImprovementAreas = request.ImprovementAreas;
            evaluation.Status = request.Status;
            evaluation.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task SubmitEvaluationAsync(long evaluationId)
        {
            var evaluation = await _context.Evaluations.FindAsync(evaluationId);
            if (evaluation == null)
                throw new InvalidOperationException($"Evaluation {evaluationId} not found");

            evaluation.Status = "Submitted";
            evaluation.SubmittedDate = DateTime.UtcNow;
            evaluation.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task ApproveEvaluationAsync(long evaluationId)
        {
            var evaluation = await _context.Evaluations.FindAsync(evaluationId);
            if (evaluation == null)
                throw new InvalidOperationException($"Evaluation {evaluationId} not found");

            evaluation.Status = "Approved";
            evaluation.ApprovedDate = DateTime.UtcNow;
            evaluation.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteEvaluationAsync(long evaluationId)
        {
            var evaluation = await _context.Evaluations.FindAsync(evaluationId);
            if (evaluation == null)
                throw new InvalidOperationException($"Evaluation {evaluationId} not found");

            // Delete associated goals
            var goals = await _context.Goals.Where(g => g.EvaluationId == evaluationId).ToListAsync();
            _context.Goals.RemoveRange(goals);

            _context.Evaluations.Remove(evaluation);
            await _context.SaveChangesAsync();
        }

        #endregion

        #region Goal Operations

        public async Task<PagedResponse<GoalDto>> GetAllGoalsAsync(long organizationId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Goals
                .Include(g => g.Employee)
                .Where(g => g.Employee.OrganizationId == organizationId);

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(g => g.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            var dtos = items.Select(g => MapGoalToDto(g)).ToList();

            return new PagedResponse<GoalDto>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<PagedResponse<GoalDto>> GetGoalsByEmployeeAsync(long employeeId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Goals.Where(g => g.EmployeeId == employeeId);

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(g => g.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            var dtos = items.Select(g => MapGoalToDto(g)).ToList();

            return new PagedResponse<GoalDto>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<PagedResponse<GoalDto>> GetGoalsByEvaluationAsync(long evaluationId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Goals.Where(g => g.EvaluationId == evaluationId);

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(g => g.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            var dtos = items.Select(g => MapGoalToDto(g)).ToList();

            return new PagedResponse<GoalDto>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<GoalDto> GetGoalByIdAsync(long goalId)
        {
            var goal = await _context.Goals
                .FirstOrDefaultAsync(g => g.GoalId == goalId);

            if (goal == null)
                throw new InvalidOperationException($"Goal {goalId} not found");

            return MapGoalToDto(goal);
        }

        public async Task<long> CreateGoalAsync(CreateGoalRequest request)
        {
            // Validate employee
            var employee = await _context.Employees.FindAsync(request.EmployeeId);
            if (employee == null)
                throw new InvalidOperationException($"Employee {request.EmployeeId} not found");

            var goal = new Goal
            {
                EmployeeId = request.EmployeeId,
                EvaluationId = request.EvaluationId,
                GoalTitle = request.GoalTitle,
                Description = request.Description,
                Status = "Not Started",
                TargetDate = request.TargetDate,
                ProgressPercentage = 0,
                OwnerId = request.OwnerId,
                Alignment = request.Alignment,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Goals.Add(goal);
            await _context.SaveChangesAsync();

            return goal.GoalId;
        }

        public async Task UpdateGoalAsync(long goalId, UpdateGoalRequest request)
        {
            var goal = await _context.Goals.FindAsync(goalId);
            if (goal == null)
                throw new InvalidOperationException($"Goal {goalId} not found");

            goal.GoalTitle = request.GoalTitle;
            goal.Description = request.Description;
            goal.Status = request.Status;
            goal.TargetDate = request.TargetDate;
            goal.ProgressPercentage = request.ProgressPercentage;
            goal.OwnerId = request.OwnerId;
            goal.Alignment = request.Alignment;
            goal.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateGoalProgressAsync(long goalId, int progressPercentage)
        {
            var goal = await _context.Goals.FindAsync(goalId);
            if (goal == null)
                throw new InvalidOperationException($"Goal {goalId} not found");

            goal.ProgressPercentage = Math.Min(Math.Max(progressPercentage, 0), 100);
            goal.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task CompleteGoalAsync(long goalId)
        {
            var goal = await _context.Goals.FindAsync(goalId);
            if (goal == null)
                throw new InvalidOperationException($"Goal {goalId} not found");

            goal.Status = "Completed";
            goal.ProgressPercentage = 100;
            goal.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteGoalAsync(long goalId)
        {
            var goal = await _context.Goals.FindAsync(goalId);
            if (goal == null)
                throw new InvalidOperationException($"Goal {goalId} not found");

            _context.Goals.Remove(goal);
            await _context.SaveChangesAsync();
        }

        #endregion

        #region Helper Methods

        private EvaluationDto MapEvaluationToDto(Evaluation evaluation)
        {
            return new EvaluationDto
            {
                EvaluationId = evaluation.EvaluationId,
                EmployeeId = evaluation.EmployeeId,
                EvaluatorId = evaluation.EvaluatorId,
                EmployeeName = evaluation.Employee != null ? $"{evaluation.Employee.FirstName} {evaluation.Employee.LastName}".Trim() : "N/A",
                EvaluatorName = evaluation.Evaluator != null ? $"{evaluation.Evaluator.FirstName} {evaluation.Evaluator.LastName}".Trim() : null,
                EvaluationType = evaluation.EvaluationType,
                StartDate = evaluation.StartDate,
                EndDate = evaluation.EndDate,
                OverallRating = evaluation.OverallRating,
                Comments = evaluation.Comments,
                StrengthAreas = evaluation.StrengthAreas,
                ImprovementAreas = evaluation.ImprovementAreas,
                Status = evaluation.Status,
                SubmittedDate = evaluation.SubmittedDate,
                ApprovedDate = evaluation.ApprovedDate,
                CreatedAt = evaluation.CreatedAt,
                UpdatedAt = evaluation.UpdatedAt
            };
        }

        private GoalDto MapGoalToDto(Goal goal)
        {
            return new GoalDto
            {
                GoalId = goal.GoalId,
                EmployeeId = goal.EmployeeId,
                EvaluationId = goal.EvaluationId,
                GoalTitle = goal.GoalTitle,
                Description = goal.Description,
                Status = goal.Status,
                TargetDate = goal.TargetDate,
                ProgressPercentage = goal.ProgressPercentage,
                OwnerId = goal.OwnerId,
                Alignment = goal.Alignment,
                CreatedAt = goal.CreatedAt,
                UpdatedAt = goal.UpdatedAt
            };
        }

        #endregion
    }
}
