using HRManagement.Models.DTOs;

namespace HRManagement.Services.Interfaces
{
    /// <summary>
    /// Service interface for employee management operations
    /// </summary>
    public interface IEmployeeService
    {
        /// <summary>
        /// Get all employees for an organization with pagination
        /// </summary>
        Task<PagedResponse<EmployeeDto>> GetAllAsync(long organizationId, int pageNumber = 1, int pageSize = 10);

        /// <summary>
        /// Get employee by ID
        /// </summary>
        Task<EmployeeDto?> GetByIdAsync(long employeeId);

        /// <summary>
        /// Get employee by employee code
        /// </summary>
        Task<EmployeeDto?> GetByCodeAsync(long organizationId, string employeeCode);

        /// <summary>
        /// Get employee by email
        /// </summary>
        Task<EmployeeDto?> GetByEmailAsync(string email);

        /// <summary>
        /// Create a new employee
        /// </summary>
        Task<long> CreateAsync(CreateEmployeeRequest request);

        /// <summary>
        /// Update an existing employee
        /// </summary>
        Task UpdateAsync(long employeeId, UpdateEmployeeRequest request);

        /// <summary>
        /// Terminate an employee
        /// </summary>
        Task TerminateAsync(long employeeId, DateTime terminationDate);

        /// <summary>
        /// Reactivate a terminated employee
        /// </summary>
        Task ReactivateAsync(long employeeId);

        /// <summary>
        /// Get direct reports for a manager
        /// </summary>
        Task<IEnumerable<EmployeeDto>> GetDirectReportsAsync(long managerId);

        /// <summary>
        /// Get employees by department
        /// </summary>
        Task<IEnumerable<EmployeeDto>> GetByDepartmentAsync(long departmentId);

        /// <summary>
        /// Get employees by job title
        /// </summary>
        Task<IEnumerable<EmployeeDto>> GetByJobTitleAsync(long jobTitleId);
    }
}
