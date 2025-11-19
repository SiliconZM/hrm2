using HRManagement.Data;
using HRManagement.Models.DTOs;
using HRManagement.Models.Entities;
using HRManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Services.Implementations
{
    public class SkillsService : ISkillsService
    {
        private readonly HRContext _context;

        public SkillsService(HRContext context)
        {
            _context = context;
        }

        #region Skill Operations

        public async Task<PagedResponse<SkillDto>> GetAllSkillsAsync(long organizationId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Skills
                .Where(s => s.OrganizationId == organizationId);

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(s => s.SkillName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            var dtos = items.Select(s => MapSkillToDto(s)).ToList();

            return new PagedResponse<SkillDto>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<SkillDto> GetSkillByIdAsync(long skillId)
        {
            var skill = await _context.Skills.FindAsync(skillId);

            if (skill == null)
                throw new InvalidOperationException($"Skill {skillId} not found");

            return MapSkillToDto(skill);
        }

        public async Task<SkillDto> GetSkillByNameAsync(long organizationId, string skillName)
        {
            var skill = await _context.Skills
                .FirstOrDefaultAsync(s => s.OrganizationId == organizationId && s.SkillName == skillName);

            if (skill == null)
                throw new InvalidOperationException($"Skill '{skillName}' not found");

            return MapSkillToDto(skill);
        }

        public async Task<long> CreateSkillAsync(CreateSkillRequest request)
        {
            // Validate organization
            var org = await _context.Organizations.FindAsync(request.OrganizationId);
            if (org == null)
                throw new InvalidOperationException($"Organization {request.OrganizationId} not found");

            // Check if skill already exists
            var exists = await _context.Skills
                .AnyAsync(s => s.OrganizationId == request.OrganizationId && s.SkillName == request.SkillName);
            if (exists)
                throw new InvalidOperationException($"Skill '{request.SkillName}' already exists");

            var skill = new Skill
            {
                OrganizationId = request.OrganizationId,
                SkillName = request.SkillName,
                Description = request.Description,
                Category = request.Category,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Skills.Add(skill);
            await _context.SaveChangesAsync();

            return skill.SkillId;
        }

        public async Task UpdateSkillAsync(long skillId, UpdateSkillRequest request)
        {
            var skill = await _context.Skills.FindAsync(skillId);
            if (skill == null)
                throw new InvalidOperationException($"Skill {skillId} not found");

            skill.SkillName = request.SkillName;
            skill.Description = request.Description;
            skill.Category = request.Category;
            skill.IsActive = request.IsActive;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteSkillAsync(long skillId)
        {
            var skill = await _context.Skills.FindAsync(skillId);
            if (skill == null)
                throw new InvalidOperationException($"Skill {skillId} not found");

            // Check if skill is in use
            var inUse = await _context.EmployeeSkills.AnyAsync(es => es.SkillId == skillId);
            if (inUse)
                throw new InvalidOperationException("Cannot delete skill that is assigned to employees");

            _context.Skills.Remove(skill);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResponse<SkillDto>> SearchSkillsAsync(long organizationId, string category = null, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Skills
                .Where(s => s.OrganizationId == organizationId);

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(s => s.Category == category);
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(s => s.SkillName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            var dtos = items.Select(s => MapSkillToDto(s)).ToList();

            return new PagedResponse<SkillDto>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        #endregion

        #region Employee Skill Operations

        public async Task<PagedResponse<EmployeeSkillDto>> GetEmployeeSkillsAsync(long employeeId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.EmployeeSkills
                .Include(es => es.Employee)
                .Include(es => es.Skill)
                .Where(es => es.EmployeeId == employeeId);

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(es => es.IsPrimarySkill)
                .ThenBy(es => es.Skill.SkillName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            var dtos = items.Select(es => MapEmployeeSkillToDto(es)).ToList();

            return new PagedResponse<EmployeeSkillDto>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<EmployeeSkillDto> GetEmployeeSkillByIdAsync(long employeeSkillId)
        {
            var employeeSkill = await _context.EmployeeSkills
                .Include(es => es.Employee)
                .Include(es => es.Skill)
                .FirstOrDefaultAsync(es => es.EmployeeSkillId == employeeSkillId);

            if (employeeSkill == null)
                throw new InvalidOperationException($"Employee skill {employeeSkillId} not found");

            return MapEmployeeSkillToDto(employeeSkill);
        }

        public async Task<long> AddEmployeeSkillAsync(CreateEmployeeSkillRequest request)
        {
            // Validate employee
            var employee = await _context.Employees.FindAsync(request.EmployeeId);
            if (employee == null)
                throw new InvalidOperationException($"Employee {request.EmployeeId} not found");

            // Validate skill
            var skill = await _context.Skills.FindAsync(request.SkillId);
            if (skill == null)
                throw new InvalidOperationException($"Skill {request.SkillId} not found");

            // Check if employee already has this skill
            var exists = await _context.EmployeeSkills
                .AnyAsync(es => es.EmployeeId == request.EmployeeId && es.SkillId == request.SkillId);
            if (exists)
                throw new InvalidOperationException("Employee already has this skill");

            var employeeSkill = new EmployeeSkill
            {
                EmployeeId = request.EmployeeId,
                SkillId = request.SkillId,
                ProficiencyLevel = request.ProficiencyLevel,
                YearsOfExperience = request.YearsOfExperience,
                IsPrimarySkill = request.IsPrimarySkill,
                Notes = request.Notes,
                LastUpdated = DateTime.UtcNow
            };

            _context.EmployeeSkills.Add(employeeSkill);
            await _context.SaveChangesAsync();

            return employeeSkill.EmployeeSkillId;
        }

        public async Task UpdateEmployeeSkillAsync(long employeeSkillId, UpdateEmployeeSkillRequest request)
        {
            var employeeSkill = await _context.EmployeeSkills.FindAsync(employeeSkillId);
            if (employeeSkill == null)
                throw new InvalidOperationException($"Employee skill {employeeSkillId} not found");

            employeeSkill.ProficiencyLevel = request.ProficiencyLevel;
            employeeSkill.YearsOfExperience = request.YearsOfExperience;
            employeeSkill.IsPrimarySkill = request.IsPrimarySkill;
            employeeSkill.Notes = request.Notes;
            employeeSkill.LastUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task RemoveEmployeeSkillAsync(long employeeSkillId)
        {
            var employeeSkill = await _context.EmployeeSkills.FindAsync(employeeSkillId);
            if (employeeSkill == null)
                throw new InvalidOperationException($"Employee skill {employeeSkillId} not found");

            _context.EmployeeSkills.Remove(employeeSkill);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResponse<EmployeeSkillDto>> GetSkillProficiencyAsync(long skillId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.EmployeeSkills
                .Include(es => es.Employee)
                .Include(es => es.Skill)
                .Where(es => es.SkillId == skillId);

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(es => es.Employee.LastName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            var dtos = items.Select(es => MapEmployeeSkillToDto(es)).ToList();

            return new PagedResponse<EmployeeSkillDto>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        #endregion

        #region Helper Methods

        private SkillDto MapSkillToDto(Skill skill)
        {
            return new SkillDto
            {
                SkillId = skill.SkillId,
                OrganizationId = skill.OrganizationId,
                SkillName = skill.SkillName,
                Description = skill.Description,
                Category = skill.Category,
                IsActive = skill.IsActive,
                CreatedAt = skill.CreatedAt
            };
        }

        private EmployeeSkillDto MapEmployeeSkillToDto(EmployeeSkill employeeSkill)
        {
            return new EmployeeSkillDto
            {
                EmployeeSkillId = employeeSkill.EmployeeSkillId,
                EmployeeId = employeeSkill.EmployeeId,
                SkillId = employeeSkill.SkillId,
                EmployeeName = employeeSkill.Employee != null ? $"{employeeSkill.Employee.FirstName} {employeeSkill.Employee.LastName}".Trim() : "N/A",
                SkillName = employeeSkill.Skill?.SkillName ?? "N/A",
                ProficiencyLevel = employeeSkill.ProficiencyLevel,
                YearsOfExperience = employeeSkill.YearsOfExperience,
                IsPrimarySkill = employeeSkill.IsPrimarySkill,
                Notes = employeeSkill.Notes,
                LastUpdated = employeeSkill.LastUpdated
            };
        }

        #endregion
    }
}
