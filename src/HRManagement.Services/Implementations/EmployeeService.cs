using Microsoft.EntityFrameworkCore;
using HRManagement.Data;
using HRManagement.Models.DTOs;
using HRManagement.Models.Entities;
using HRManagement.Services.Interfaces;
using AutoMapper;

namespace HRManagement.Services.Implementations
{
    /// <summary>
    /// Service for managing employee operations
    /// </summary>
    public class EmployeeService : IEmployeeService
    {
        private readonly HRContext _context;
        private readonly IMapper _mapper;

        public EmployeeService(HRContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedResponse<EmployeeDto>> GetAllAsync(long organizationId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Employees
                .AsNoTracking()
                .Where(e => e.OrganizationId == organizationId && e.IsActive)
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName);

            var totalCount = await query.CountAsync();

            var employees = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(e => e.Department)
                .Include(e => e.JobTitle)
                .Include(e => e.ReportingManager)
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return new PagedResponse<EmployeeDto>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<EmployeeDto?> GetByIdAsync(long employeeId)
        {
            var employee = await _context.Employees
                .AsNoTracking()
                .Where(e => e.EmployeeId == employeeId && e.IsActive)
                .Include(e => e.Department)
                .Include(e => e.JobTitle)
                .Include(e => e.ReportingManager)
                .FirstOrDefaultAsync();

            if (employee == null)
                return null;

            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<EmployeeDto?> GetByCodeAsync(long organizationId, string employeeCode)
        {
            var employee = await _context.Employees
                .AsNoTracking()
                .Where(e => e.OrganizationId == organizationId &&
                            e.EmployeeCode == employeeCode &&
                            e.IsActive)
                .Include(e => e.Department)
                .Include(e => e.JobTitle)
                .Include(e => e.ReportingManager)
                .FirstOrDefaultAsync();

            if (employee == null)
                return null;

            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<EmployeeDto?> GetByEmailAsync(string email)
        {
            var employee = await _context.Employees
                .AsNoTracking()
                .Where(e => e.EmailOfficial == email && e.IsActive)
                .Include(e => e.Department)
                .Include(e => e.JobTitle)
                .Include(e => e.ReportingManager)
                .FirstOrDefaultAsync();

            if (employee == null)
                return null;

            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<long> CreateAsync(CreateEmployeeRequest request)
        {
            // Validate organization exists
            var org = await _context.Organizations.FindAsync(request.OrganizationId);
            if (org == null)
                throw new InvalidOperationException($"Organization {request.OrganizationId} not found");

            // Validate department exists
            var dept = await _context.Departments
                .Where(d => d.DepartmentId == request.DepartmentId &&
                            d.OrganizationId == request.OrganizationId)
                .FirstOrDefaultAsync();
            if (dept == null)
                throw new InvalidOperationException($"Department {request.DepartmentId} not found");

            // Check if employee code already exists
            var exists = await _context.Employees
                .AnyAsync(e => e.OrganizationId == request.OrganizationId &&
                               e.EmployeeCode == request.EmployeeCode);
            if (exists)
                throw new InvalidOperationException($"Employee code {request.EmployeeCode} already exists");

            // Check if email already exists
            var emailExists = await _context.Employees
                .AnyAsync(e => e.EmailOfficial == request.EmailOfficial);
            if (emailExists)
                throw new InvalidOperationException($"Email {request.EmailOfficial} already exists");

            var employee = _mapper.Map<Employee>(request);
            employee.IsActive = true;
            employee.CreatedAt = DateTime.UtcNow;
            employee.UpdatedAt = DateTime.UtcNow;

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return employee.EmployeeId;
        }

        public async Task UpdateAsync(long employeeId, UpdateEmployeeRequest request)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null)
                throw new InvalidOperationException($"Employee {employeeId} not found");

            if (employee.IsActive == false)
                throw new InvalidOperationException("Cannot update an inactive employee");

            // Update properties if provided
            if (!string.IsNullOrWhiteSpace(request.FirstName))
                employee.FirstName = request.FirstName;

            if (!string.IsNullOrWhiteSpace(request.LastName))
                employee.LastName = request.LastName;

            if (!string.IsNullOrWhiteSpace(request.MiddleName))
                employee.MiddleName = request.MiddleName;

            if (request.DateOfBirth.HasValue)
                employee.DateOfBirth = request.DateOfBirth;

            if (request.Gender.HasValue)
                employee.Gender = request.Gender;

            if (!string.IsNullOrWhiteSpace(request.MaritalStatus))
                employee.MaritalStatus = request.MaritalStatus;

            if (!string.IsNullOrWhiteSpace(request.Nationality))
                employee.Nationality = request.Nationality;

            if (!string.IsNullOrWhiteSpace(request.EmailPersonal))
                employee.EmailPersonal = request.EmailPersonal;

            if (!string.IsNullOrWhiteSpace(request.EmailOfficial))
            {
                // Check if new email is unique
                var emailExists = await _context.Employees
                    .AnyAsync(e => e.EmployeeId != employeeId &&
                                   e.EmailOfficial == request.EmailOfficial);
                if (emailExists)
                    throw new InvalidOperationException($"Email {request.EmailOfficial} already exists");

                employee.EmailOfficial = request.EmailOfficial;
            }

            if (!string.IsNullOrWhiteSpace(request.PhonePrimary))
                employee.PhonePrimary = request.PhonePrimary;

            if (!string.IsNullOrWhiteSpace(request.PhoneSecondary))
                employee.PhoneSecondary = request.PhoneSecondary;

            if (request.DepartmentId.HasValue)
                employee.DepartmentId = request.DepartmentId.Value;

            if (request.JobTitleId.HasValue)
                employee.JobTitleId = request.JobTitleId;

            if (request.ReportingManagerId.HasValue)
                employee.ReportingManagerId = request.ReportingManagerId;

            if (!string.IsNullOrWhiteSpace(request.EmploymentType))
                employee.EmploymentType = request.EmploymentType;

            if (!string.IsNullOrWhiteSpace(request.EmploymentStatus))
                employee.EmploymentStatus = request.EmploymentStatus;

            if (request.TerminationDate.HasValue)
                employee.TerminationDate = request.TerminationDate;

            if (request.Salary.HasValue)
                employee.Salary = request.Salary;

            if (!string.IsNullOrWhiteSpace(request.SalaryFrequency))
                employee.SalaryFrequency = request.SalaryFrequency;

            if (!string.IsNullOrWhiteSpace(request.CurrencyCode))
                employee.CurrencyCode = request.CurrencyCode;

            if (!string.IsNullOrWhiteSpace(request.PermanentAddress))
                employee.PermanentAddress = request.PermanentAddress;

            if (!string.IsNullOrWhiteSpace(request.CurrentAddress))
                employee.CurrentAddress = request.CurrentAddress;

            if (!string.IsNullOrWhiteSpace(request.ProfileImageUrl))
                employee.ProfileImageUrl = request.ProfileImageUrl;

            employee.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task TerminateAsync(long employeeId, DateTime terminationDate)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null)
                throw new InvalidOperationException($"Employee {employeeId} not found");

            if (!employee.IsActive)
                throw new InvalidOperationException("Employee is already terminated");

            employee.TerminationDate = terminationDate;
            employee.EmploymentStatus = "Terminated";
            employee.IsActive = false;
            employee.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task ReactivateAsync(long employeeId)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null)
                throw new InvalidOperationException($"Employee {employeeId} not found");

            employee.TerminationDate = null;
            employee.EmploymentStatus = "Active";
            employee.IsActive = true;
            employee.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EmployeeDto>> GetDirectReportsAsync(long managerId)
        {
            var employees = await _context.Employees
                .AsNoTracking()
                .Where(e => e.ReportingManagerId == managerId && e.IsActive)
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .Include(e => e.Department)
                .Include(e => e.JobTitle)
                .ToListAsync();

            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        public async Task<IEnumerable<EmployeeDto>> GetByDepartmentAsync(long departmentId)
        {
            var employees = await _context.Employees
                .AsNoTracking()
                .Where(e => e.DepartmentId == departmentId && e.IsActive)
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .Include(e => e.JobTitle)
                .Include(e => e.ReportingManager)
                .ToListAsync();

            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        public async Task<IEnumerable<EmployeeDto>> GetByJobTitleAsync(long jobTitleId)
        {
            var employees = await _context.Employees
                .AsNoTracking()
                .Where(e => e.JobTitleId == jobTitleId && e.IsActive)
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .Include(e => e.Department)
                .Include(e => e.ReportingManager)
                .ToListAsync();

            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }
    }
}
