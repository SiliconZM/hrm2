using HRManagement.Models.DTOs;

namespace HRManagement.Services.Interfaces
{
    /// <summary>
    /// Service for managing recruitment-related operations (job postings, candidates, applications)
    /// </summary>
    public interface IRecruitmentService
    {
        // Job Posting operations
        Task<PagedResponse<JobPostingDto>> GetAllJobPostingsAsync(long organizationId, int pageNumber = 1, int pageSize = 10);
        Task<JobPostingDto> GetJobPostingByIdAsync(long jobPostingId);
        Task<long> CreateJobPostingAsync(CreateJobPostingRequest request);
        Task UpdateJobPostingAsync(long jobPostingId, UpdateJobPostingRequest request);
        Task PublishJobPostingAsync(long jobPostingId);
        Task CloseJobPostingAsync(long jobPostingId);

        // Candidate operations
        Task<PagedResponse<CandidateDto>> GetAllCandidatesAsync(long organizationId, int pageNumber = 1, int pageSize = 10);
        Task<CandidateDto> GetCandidateByIdAsync(long candidateId);
        Task<CandidateDto> GetCandidateByEmailAsync(string email);
        Task<long> CreateCandidateAsync(CreateCandidateRequest request);
        Task UpdateCandidateAsync(long candidateId, UpdateCandidateRequest request);
        Task DeleteCandidateAsync(long candidateId);

        // Application operations
        Task<PagedResponse<ApplicationDto>> GetApplicationsByJobPostingAsync(long jobPostingId, int pageNumber = 1, int pageSize = 10);
        Task<PagedResponse<ApplicationDto>> GetApplicationsByCandidateAsync(long candidateId, int pageNumber = 1, int pageSize = 10);
        Task<ApplicationDto> GetApplicationByIdAsync(long applicationId);
        Task<long> CreateApplicationAsync(CreateApplicationRequest request);
        Task UpdateApplicationAsync(long applicationId, UpdateApplicationRequest request);
        Task HireCandidateAsync(long jobPostingId, HireApplicationRequest request);
        Task RejectApplicationAsync(long applicationId, string reason = "");
    }
}
