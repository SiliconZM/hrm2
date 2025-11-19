using HRManagement.Models.DTOs;

namespace HRManagement.Services.Interfaces
{
    public interface IPayrollService
    {
        #region Salary Structure Management
        Task<PagedResponse<SalaryStructureDto>> GetAllSalaryStructuresAsync(long organizationId, int pageNumber = 1, int pageSize = 10);
        Task<SalaryStructureDto> GetSalaryStructureByIdAsync(long salaryStructureId);
        Task<long> CreateSalaryStructureAsync(CreateSalaryStructureRequest request);
        Task UpdateSalaryStructureAsync(long salaryStructureId, UpdateSalaryStructureRequest request);
        Task DeleteSalaryStructureAsync(long salaryStructureId);
        #endregion

        #region Salary Component Management
        Task<List<SalaryComponentDto>> GetComponentsByStructureAsync(long salaryStructureId);
        Task<SalaryComponentDto> GetSalaryComponentByIdAsync(long componentId);
        Task<long> CreateSalaryComponentAsync(CreateSalaryComponentRequest request);
        Task UpdateSalaryComponentAsync(long componentId, UpdateSalaryComponentRequest request);
        Task DeleteSalaryComponentAsync(long componentId);
        #endregion

        #region Employee Salary Management
        Task<PagedResponse<EmployeeSalaryDto>> GetAllEmployeeSalariesAsync(long organizationId, int pageNumber = 1, int pageSize = 10);
        Task<EmployeeSalaryDto> GetEmployeeActiveSalaryAsync(long employeeId);
        Task<List<EmployeeSalaryDto>> GetEmployeeSalaryHistoryAsync(long employeeId);
        Task<long> CreateEmployeeSalaryAsync(CreateEmployeeSalaryRequest request);
        Task UpdateEmployeeSalaryAsync(long employeeSalaryId, UpdateEmployeeSalaryRequest request);
        Task DeactivateEmployeeSalaryAsync(long employeeSalaryId);
        Task<decimal> CalculateGrossSalaryAsync(long employeeSalaryId);
        Task<decimal> CalculateNetSalaryAsync(long employeeSalaryId);
        #endregion

        #region Payroll Management
        Task<PagedResponse<PayrollDto>> GetAllPayrollsAsync(long organizationId, int pageNumber = 1, int pageSize = 10);
        Task<PayrollDto> GetPayrollByIdAsync(long payrollId);
        Task<long> CreatePayrollAsync(CreatePayrollRequest request);
        Task UpdatePayrollAsync(long payrollId, UpdatePayrollRequest request);
        Task ProcessPayrollAsync(long payrollId);
        Task MarkPayrollAsPaidAsync(long payrollId, DateTime paidDate);
        Task DeletePayrollAsync(long payrollId);
        #endregion

        #region Payroll Detail Management
        Task<List<PayrollDetailDto>> GetPayrollDetailsAsync(long payrollId);
        Task<PayrollDetailDto> GetPayrollDetailByIdAsync(long payrollDetailId);
        Task<long> CreatePayrollDetailAsync(CreatePayrollDetailRequest request);
        Task UpdatePayrollDetailAsync(long payrollDetailId, PayrollDetailDto request);
        Task ApprovePayrollDetailAsync(long payrollDetailId);
        Task<decimal> CalculatePayrollDetailSalaryAsync(long payrollDetailId);
        #endregion

        #region Salary Slip Management
        Task<SalarySlipDto> GetSalarySlipByIdAsync(long salarySlipId);
        Task<List<SalarySlipDto>> GetSalarySlipsByEmployeeAsync(long employeeId, int year);
        Task<long> GenerateSalarySlipAsync(CreateSalarySlipRequest request);
        Task UpdateSalarySlipAsync(long salarySlipId, SalarySlipDto request);
        Task MarkSalarySlipAsApprovedAsync(long salarySlipId);
        Task MarkSalarySlipAsSentAsync(long salarySlipId);
        Task MarkSalarySlipAsPaidAsync(long salarySlipId, DateTime paidDate);
        Task<byte[]> GenerateSalarySlipPdfAsync(long salarySlipId);
        #endregion
    }
}
