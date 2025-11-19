using HRManagement.Models.DTOs;

namespace HRManagement.Services.Interfaces
{
    /// <summary>
    /// Service for managing skills and employee skill proficiencies
    /// </summary>
    public interface ISkillsService
    {
        // Skill operations
        Task<PagedResponse<SkillDto>> GetAllSkillsAsync(long organizationId, int pageNumber = 1, int pageSize = 10);
        Task<SkillDto> GetSkillByIdAsync(long skillId);
        Task<SkillDto> GetSkillByNameAsync(long organizationId, string skillName);
        Task<long> CreateSkillAsync(CreateSkillRequest request);
        Task UpdateSkillAsync(long skillId, UpdateSkillRequest request);
        Task DeleteSkillAsync(long skillId);
        Task<PagedResponse<SkillDto>> SearchSkillsAsync(long organizationId, string category = null, int pageNumber = 1, int pageSize = 10);

        // Employee Skill operations
        Task<PagedResponse<EmployeeSkillDto>> GetEmployeeSkillsAsync(long employeeId, int pageNumber = 1, int pageSize = 10);
        Task<EmployeeSkillDto> GetEmployeeSkillByIdAsync(long employeeSkillId);
        Task<long> AddEmployeeSkillAsync(CreateEmployeeSkillRequest request);
        Task UpdateEmployeeSkillAsync(long employeeSkillId, UpdateEmployeeSkillRequest request);
        Task RemoveEmployeeSkillAsync(long employeeSkillId);
        Task<PagedResponse<EmployeeSkillDto>> GetSkillProficiencyAsync(long skillId, int pageNumber = 1, int pageSize = 10);
    }
}
