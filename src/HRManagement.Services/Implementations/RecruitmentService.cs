using HRManagement.Data;
using HRManagement.Models.DTOs;
using HRManagement.Models.Entities;
using HRManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Services.Implementations
{
    public class RecruitmentService : IRecruitmentService
    {
        private readonly HRContext _context;

        public RecruitmentService(HRContext context)
        {
            _context = context;
        }

        #region Job Posting Operations

        public async Task<PagedResponse<JobPostingDto>> GetAllJobPostingsAsync(long organizationId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.JobPostings
                .Include(jp => jp.Department)
                .Include(jp => jp.JobTitle)
                .Where(jp => jp.OrganizationId == organizationId);

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(jp => jp.PostedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            var dtos = items.Select(jp => MapJobPostingToDto(jp)).ToList();

            return new PagedResponse<JobPostingDto>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<JobPostingDto> GetJobPostingByIdAsync(long jobPostingId)
        {
            var jobPosting = await _context.JobPostings
                .Include(jp => jp.Department)
                .Include(jp => jp.JobTitle)
                .FirstOrDefaultAsync(jp => jp.JobPostingId == jobPostingId);

            if (jobPosting == null)
                throw new InvalidOperationException($"Job posting {jobPostingId} not found");

            return MapJobPostingToDto(jobPosting);
        }

        public async Task<long> CreateJobPostingAsync(CreateJobPostingRequest request)
        {
            // Validate organization
            var org = await _context.Organizations.FindAsync(request.OrganizationId);
            if (org == null)
                throw new InvalidOperationException($"Organization {request.OrganizationId} not found");

            var jobPosting = new JobPosting
            {
                OrganizationId = request.OrganizationId,
                JobTitleId = request.JobTitleId,
                DepartmentId = request.DepartmentId,
                NumberOfOpenings = request.NumberOfOpenings,
                Description = request.Description,
                Requirements = request.Requirements,
                Qualifications = request.Qualifications,
                SalaryRangeMin = request.SalaryRangeMin,
                SalaryRangeMax = request.SalaryRangeMax,
                EmploymentType = request.EmploymentType,
                Location = request.Location,
                Status = "Draft",
                ClosingDate = request.ClosingDate,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.JobPostings.Add(jobPosting);
            await _context.SaveChangesAsync();

            return jobPosting.JobPostingId;
        }

        public async Task UpdateJobPostingAsync(long jobPostingId, UpdateJobPostingRequest request)
        {
            var jobPosting = await _context.JobPostings.FindAsync(jobPostingId);
            if (jobPosting == null)
                throw new InvalidOperationException($"Job posting {jobPostingId} not found");

            jobPosting.NumberOfOpenings = request.NumberOfOpenings;
            jobPosting.Description = request.Description;
            jobPosting.Requirements = request.Requirements;
            jobPosting.Qualifications = request.Qualifications;
            jobPosting.SalaryRangeMin = request.SalaryRangeMin;
            jobPosting.SalaryRangeMax = request.SalaryRangeMax;
            jobPosting.EmploymentType = request.EmploymentType;
            jobPosting.Location = request.Location;
            jobPosting.Status = request.Status;
            jobPosting.ClosingDate = request.ClosingDate;
            jobPosting.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task PublishJobPostingAsync(long jobPostingId)
        {
            var jobPosting = await _context.JobPostings.FindAsync(jobPostingId);
            if (jobPosting == null)
                throw new InvalidOperationException($"Job posting {jobPostingId} not found");

            jobPosting.Status = "Open";
            jobPosting.PostedDate = DateTime.UtcNow;
            jobPosting.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task CloseJobPostingAsync(long jobPostingId)
        {
            var jobPosting = await _context.JobPostings.FindAsync(jobPostingId);
            if (jobPosting == null)
                throw new InvalidOperationException($"Job posting {jobPostingId} not found");

            jobPosting.Status = "Closed";
            jobPosting.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        #endregion

        #region Candidate Operations

        public async Task<PagedResponse<CandidateDto>> GetAllCandidatesAsync(long organizationId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Candidates
                .Where(c => c.OrganizationId == organizationId);

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            var dtos = items.Select(c => MapCandidateToDto(c)).ToList();

            return new PagedResponse<CandidateDto>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<CandidateDto> GetCandidateByIdAsync(long candidateId)
        {
            var candidate = await _context.Candidates
                .FirstOrDefaultAsync(c => c.CandidateId == candidateId);

            if (candidate == null)
                throw new InvalidOperationException($"Candidate {candidateId} not found");

            return MapCandidateToDto(candidate);
        }

        public async Task<CandidateDto> GetCandidateByEmailAsync(string email)
        {
            var candidate = await _context.Candidates
                .FirstOrDefaultAsync(c => c.Email == email);

            if (candidate == null)
                throw new InvalidOperationException($"Candidate with email {email} not found");

            return MapCandidateToDto(candidate);
        }

        public async Task<long> CreateCandidateAsync(CreateCandidateRequest request)
        {
            // Validate organization
            var org = await _context.Organizations.FindAsync(request.OrganizationId);
            if (org == null)
                throw new InvalidOperationException($"Organization {request.OrganizationId} not found");

            // Check if email already exists
            var exists = await _context.Candidates.AnyAsync(c => c.Email == request.Email);
            if (exists)
                throw new InvalidOperationException($"Candidate with email {request.Email} already exists");

            var candidate = new Candidate
            {
                OrganizationId = request.OrganizationId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhonePrimary = request.PhonePrimary,
                CurrentCompany = request.CurrentCompany,
                CurrentDesignation = request.CurrentDesignation,
                ExperienceYears = request.ExperienceYears,
                ResumeUrl = request.ResumeUrl,
                LinkedInUrl = request.LinkedInUrl,
                Source = request.Source,
                Skills = request.Skills,
                Notes = request.Notes,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Candidates.Add(candidate);
            await _context.SaveChangesAsync();

            return candidate.CandidateId;
        }

        public async Task UpdateCandidateAsync(long candidateId, UpdateCandidateRequest request)
        {
            var candidate = await _context.Candidates.FindAsync(candidateId);
            if (candidate == null)
                throw new InvalidOperationException($"Candidate {candidateId} not found");

            candidate.FirstName = request.FirstName;
            candidate.LastName = request.LastName;
            candidate.Email = request.Email;
            candidate.PhonePrimary = request.PhonePrimary;
            candidate.CurrentCompany = request.CurrentCompany;
            candidate.CurrentDesignation = request.CurrentDesignation;
            candidate.ExperienceYears = request.ExperienceYears;
            candidate.ResumeUrl = request.ResumeUrl;
            candidate.LinkedInUrl = request.LinkedInUrl;
            candidate.Skills = request.Skills;
            candidate.Notes = request.Notes;
            candidate.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCandidateAsync(long candidateId)
        {
            var candidate = await _context.Candidates.FindAsync(candidateId);
            if (candidate == null)
                throw new InvalidOperationException($"Candidate {candidateId} not found");

            _context.Candidates.Remove(candidate);
            await _context.SaveChangesAsync();
        }

        #endregion

        #region Application Operations

        public async Task<PagedResponse<ApplicationDto>> GetApplicationsByJobPostingAsync(long jobPostingId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Applications
                .Include(a => a.Candidate)
                .Include(a => a.JobPosting)
                .Include(a => a.AssignedTo)
                .Where(a => a.JobPostingId == jobPostingId);

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(a => a.ApplicationDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            var dtos = items.Select(a => MapApplicationToDto(a)).ToList();

            return new PagedResponse<ApplicationDto>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<PagedResponse<ApplicationDto>> GetApplicationsByCandidateAsync(long candidateId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Applications
                .Include(a => a.Candidate)
                .Include(a => a.JobPosting)
                .Where(a => a.CandidateId == candidateId);

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(a => a.ApplicationDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            var dtos = items.Select(a => MapApplicationToDto(a)).ToList();

            return new PagedResponse<ApplicationDto>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<ApplicationDto> GetApplicationByIdAsync(long applicationId)
        {
            var application = await _context.Applications
                .Include(a => a.Candidate)
                .Include(a => a.JobPosting)
                .Include(a => a.AssignedTo)
                .FirstOrDefaultAsync(a => a.ApplicationId == applicationId);

            if (application == null)
                throw new InvalidOperationException($"Application {applicationId} not found");

            return MapApplicationToDto(application);
        }

        public async Task<long> CreateApplicationAsync(CreateApplicationRequest request)
        {
            // Validate candidate
            var candidate = await _context.Candidates.FindAsync(request.CandidateId);
            if (candidate == null)
                throw new InvalidOperationException($"Candidate {request.CandidateId} not found");

            // Validate job posting
            var jobPosting = await _context.JobPostings.FindAsync(request.JobPostingId);
            if (jobPosting == null)
                throw new InvalidOperationException($"Job posting {request.JobPostingId} not found");

            // Check if candidate already applied for this position
            var exists = await _context.Applications
                .AnyAsync(a => a.CandidateId == request.CandidateId && a.JobPostingId == request.JobPostingId);
            if (exists)
                throw new InvalidOperationException("Candidate has already applied for this position");

            var application = new Application
            {
                CandidateId = request.CandidateId,
                JobPostingId = request.JobPostingId,
                ApplicationDate = DateTime.UtcNow,
                Status = "Applied",
                AssignedToId = request.AssignedToId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            return application.ApplicationId;
        }

        public async Task UpdateApplicationAsync(long applicationId, UpdateApplicationRequest request)
        {
            var application = await _context.Applications.FindAsync(applicationId);
            if (application == null)
                throw new InvalidOperationException($"Application {applicationId} not found");

            application.Status = request.Status;
            application.RejectionReason = request.RejectionReason;
            application.InterviewDate = request.InterviewDate;
            application.InterviewerNotes = request.InterviewerNotes;
            application.AssignedToId = request.AssignedToId;
            application.OfferStatus = request.OfferStatus;
            application.OfferAmount = request.OfferAmount;
            application.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task HireCandidateAsync(long jobPostingId, HireApplicationRequest request)
        {
            // Validate job posting
            var jobPosting = await _context.JobPostings.FindAsync(jobPostingId);
            if (jobPosting == null)
                throw new InvalidOperationException($"Job posting {jobPostingId} not found");

            // Get application
            var application = await _context.Applications.FindAsync(request.ApplicationId);
            if (application == null)
                throw new InvalidOperationException($"Application {request.ApplicationId} not found");

            // Get candidate
            var candidate = await _context.Candidates.FindAsync(request.CandidateId);
            if (candidate == null)
                throw new InvalidOperationException($"Candidate {request.CandidateId} not found");

            // Validate department
            var dept = await _context.Departments.FindAsync(request.DepartmentId);
            if (dept == null)
                throw new InvalidOperationException($"Department {request.DepartmentId} not found");

            // Validate job title
            var jobTitle = await _context.JobTitles.FindAsync(request.JobTitleId);
            if (jobTitle == null)
                throw new InvalidOperationException($"Job title {request.JobTitleId} not found");

            // Create employee from candidate
            var employee = new Employee
            {
                OrganizationId = jobPosting.OrganizationId,
                EmployeeCode = request.EmployeeCode,
                FirstName = candidate.FirstName,
                LastName = candidate.LastName,
                EmailOfficial = candidate.Email,
                PhonePrimary = candidate.PhonePrimary,
                DepartmentId = request.DepartmentId,
                JobTitleId = request.JobTitleId,
                EmploymentType = request.EmploymentType,
                HireDate = request.HireDate,
                EmploymentStatus = "Active",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // Update application with linked employee and status
            application.LinkedEmployeeId = employee.EmployeeId;
            application.Status = "Hired";
            application.HiredDate = DateTime.UtcNow;
            application.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task RejectApplicationAsync(long applicationId, string reason = "")
        {
            var application = await _context.Applications.FindAsync(applicationId);
            if (application == null)
                throw new InvalidOperationException($"Application {applicationId} not found");

            application.Status = "Rejected";
            application.RejectionReason = reason;
            application.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        #endregion

        #region Helper Methods

        private JobPostingDto MapJobPostingToDto(JobPosting jobPosting)
        {
            return new JobPostingDto
            {
                JobPostingId = jobPosting.JobPostingId,
                OrganizationId = jobPosting.OrganizationId,
                JobTitleId = jobPosting.JobTitleId,
                DepartmentId = jobPosting.DepartmentId,
                DepartmentName = jobPosting.Department?.Name ?? "N/A",
                JobTitleName = jobPosting.JobTitle?.TitleName ?? "N/A",
                NumberOfOpenings = jobPosting.NumberOfOpenings,
                Description = jobPosting.Description,
                Requirements = jobPosting.Requirements,
                Qualifications = jobPosting.Qualifications,
                SalaryRangeMin = jobPosting.SalaryRangeMin,
                SalaryRangeMax = jobPosting.SalaryRangeMax,
                EmploymentType = jobPosting.EmploymentType,
                Location = jobPosting.Location,
                PostedDate = jobPosting.PostedDate,
                ClosingDate = jobPosting.ClosingDate,
                Status = jobPosting.Status,
                CreatedBy = jobPosting.CreatedBy,
                CreatedAt = jobPosting.CreatedAt,
                UpdatedAt = jobPosting.UpdatedAt
            };
        }

        private CandidateDto MapCandidateToDto(Candidate candidate)
        {
            return new CandidateDto
            {
                CandidateId = candidate.CandidateId,
                OrganizationId = candidate.OrganizationId,
                FirstName = candidate.FirstName,
                LastName = candidate.LastName,
                Email = candidate.Email,
                PhonePrimary = candidate.PhonePrimary,
                CurrentCompany = candidate.CurrentCompany,
                CurrentDesignation = candidate.CurrentDesignation,
                ExperienceYears = candidate.ExperienceYears,
                ResumeUrl = candidate.ResumeUrl,
                LinkedInUrl = candidate.LinkedInUrl,
                Source = candidate.Source,
                Skills = candidate.Skills,
                Notes = candidate.Notes,
                CreatedAt = candidate.CreatedAt,
                UpdatedAt = candidate.UpdatedAt
            };
        }

        private ApplicationDto MapApplicationToDto(Application application)
        {
            return new ApplicationDto
            {
                ApplicationId = application.ApplicationId,
                CandidateId = application.CandidateId,
                JobPostingId = application.JobPostingId,
                CandidateName = $"{application.Candidate?.FirstName} {application.Candidate?.LastName}".Trim(),
                JobPostingTitle = application.JobPosting?.Description ?? "N/A",
                ApplicationDate = application.ApplicationDate,
                Status = application.Status,
                RejectionReason = application.RejectionReason,
                AssignedToId = application.AssignedToId,
                AssignedToName = application.AssignedTo != null ? $"{application.AssignedTo.FirstName} {application.AssignedTo.LastName}".Trim() : null,
                InterviewDate = application.InterviewDate,
                InterviewerNotes = application.InterviewerNotes,
                OfferStatus = application.OfferStatus,
                OfferAmount = application.OfferAmount,
                HiredDate = application.HiredDate,
                LinkedEmployeeId = application.LinkedEmployeeId,
                CreatedAt = application.CreatedAt,
                UpdatedAt = application.UpdatedAt
            };
        }

        #endregion
    }
}
