using AutoMapper;
using HRManagement.Models.DTOs;
using HRManagement.Models.Entities;

namespace HRManagement.Services.Mappings
{
    /// <summary>
    /// AutoMapper configuration profile for entity to DTO mappings
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Employee mappings
            CreateMap<Employee, EmployeeDto>()
                .ForMember(d => d.DepartmentName, o => o.MapFrom(s => s.Department != null ? s.Department.Name : null))
                .ForMember(d => d.JobTitleName, o => o.MapFrom(s => s.JobTitle != null ? s.JobTitle.TitleName : null))
                .ForMember(d => d.ReportingManagerName, o => o.MapFrom(s => s.ReportingManager != null ? s.ReportingManager.FullName : null));

            CreateMap<CreateEmployeeRequest, Employee>()
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.Ignore())
                .ForMember(d => d.CreatedBy, o => o.Ignore())
                .ForMember(d => d.UpdatedBy, o => o.Ignore());

            // Department mappings
            CreateMap<Department, DepartmentDto>()
                .ForMember(d => d.ParentDepartmentName, o => o.MapFrom(s => s.ParentDepartment != null ? s.ParentDepartment.Name : null))
                .ForMember(d => d.DepartmentHeadName, o => o.MapFrom(s => s.DepartmentHeadId.HasValue ? "Head" : null));

            // JobTitle mappings
            CreateMap<JobTitle, JobTitleDto>()
                .ForMember(d => d.ReportsToJobTitleName, o => o.MapFrom(s => s.ReportsToJobTitle != null ? s.ReportsToJobTitle.TitleName : null));

            // Organization mappings
            CreateMap<Organization, OrganizationDto>();

            // Leave Type mappings
            CreateMap<LeaveType, LeaveTypeDto>();
            CreateMap<CreateLeaveTypeRequest, LeaveType>()
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.Ignore());

            // Leave Request mappings
            CreateMap<LeaveRequest, LeaveRequestDto>()
                .ForMember(d => d.EmployeeName, o => o.MapFrom(s => s.Employee != null ? s.Employee.FullName : null))
                .ForMember(d => d.LeaveTypeName, o => o.MapFrom(s => s.LeaveType != null ? s.LeaveType.LeaveTypeName : null))
                .ForMember(d => d.ApprovedByName, o => o.MapFrom(s => s.ApprovedByEmployee != null ? s.ApprovedByEmployee.FullName : null));

            CreateMap<CreateLeaveRequestRequest, LeaveRequest>()
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.Ignore());

            // Leave Balance mappings
            CreateMap<LeaveBalance, LeaveBalanceDto>()
                .ForMember(d => d.EmployeeName, o => o.MapFrom(s => s.Employee != null ? s.Employee.FullName : null))
                .ForMember(d => d.LeaveTypeName, o => o.MapFrom(s => s.LeaveType != null ? s.LeaveType.LeaveTypeName : null))
                .ForMember(d => d.AvailableDays, o => o.MapFrom(s => s.TotalDays + s.CarryOverDays - s.UsedDays));

            CreateMap<CreateLeaveBalanceRequest, LeaveBalance>()
                .ForMember(d => d.UpdatedAt, o => o.Ignore());

            // Salary Structure mappings
            CreateMap<SalaryStructure, SalaryStructureDto>();
            CreateMap<CreateSalaryStructureRequest, SalaryStructure>()
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.Ignore())
                .ForMember(d => d.SalaryStructureId, o => o.Ignore());

            // Salary Component mappings
            CreateMap<SalaryComponent, SalaryComponentDto>();
            CreateMap<CreateSalaryComponentRequest, SalaryComponent>()
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.Ignore())
                .ForMember(d => d.SalaryComponentId, o => o.Ignore());

            // Employee Salary mappings
            CreateMap<EmployeeSalary, EmployeeSalaryDto>()
                .ForMember(d => d.EmployeeName, o => o.MapFrom(s => s.Employee != null ? s.Employee.FullName : null))
                .ForMember(d => d.SalaryStructureName, o => o.MapFrom(s => s.SalaryStructure != null ? s.SalaryStructure.StructureName : null));

            CreateMap<CreateEmployeeSalaryRequest, EmployeeSalary>()
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.Ignore())
                .ForMember(d => d.EmployeeSalaryId, o => o.Ignore());

            // Payroll mappings
            CreateMap<Payroll, PayrollDto>();
            CreateMap<CreatePayrollRequest, Payroll>()
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.Ignore())
                .ForMember(d => d.PayrollId, o => o.Ignore());

            // Payroll Detail mappings
            CreateMap<PayrollDetail, PayrollDetailDto>()
                .ForMember(d => d.EmployeeName, o => o.MapFrom(s => s.Employee != null ? s.Employee.FullName : null));

            CreateMap<CreatePayrollDetailRequest, PayrollDetail>()
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.Ignore())
                .ForMember(d => d.PayrollDetailId, o => o.Ignore());

            // Salary Slip mappings
            CreateMap<SalarySlip, SalarySlipDto>()
                .ForMember(d => d.EmployeeName, o => o.MapFrom(s => s.Employee != null ? s.Employee.FullName : null));

            CreateMap<SalarySlipComponent, SalarySlipComponentDto>();

            CreateMap<CreateSalarySlipRequest, SalarySlip>()
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.Ignore())
                .ForMember(d => d.SalarySlipId, o => o.Ignore());

            // Benefit Type mappings
            CreateMap<BenefitType, BenefitTypeDto>();
            CreateMap<CreateBenefitTypeRequest, BenefitType>()
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.Ignore())
                .ForMember(d => d.BenefitTypeId, o => o.Ignore());

            // Benefit Plan mappings
            CreateMap<BenefitPlan, BenefitPlanDto>()
                .ForMember(d => d.BenefitTypeName, o => o.MapFrom(s => s.BenefitType != null ? s.BenefitType.TypeName : null));
            CreateMap<CreateBenefitPlanRequest, BenefitPlan>()
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.Ignore())
                .ForMember(d => d.BenefitPlanId, o => o.Ignore());

            // Employee Benefit mappings
            CreateMap<EmployeeBenefit, EmployeeBenefitDto>()
                .ForMember(d => d.EmployeeName, o => o.MapFrom(s => s.Employee != null ? s.Employee.FullName : null))
                .ForMember(d => d.EmployeeCode, o => o.MapFrom(s => s.Employee != null ? s.Employee.EmployeeCode : null))
                .ForMember(d => d.BenefitTypeName, o => o.MapFrom(s => s.BenefitPlan != null && s.BenefitPlan.BenefitType != null ? s.BenefitPlan.BenefitType.TypeName : null))
                .ForMember(d => d.PlanName, o => o.MapFrom(s => s.BenefitPlan != null ? s.BenefitPlan.PlanName : null))
                .ForMember(d => d.PlanCode, o => o.MapFrom(s => s.BenefitPlan != null ? s.BenefitPlan.PlanCode : null))
                .ForMember(d => d.PlanEmployeeContribution, o => o.MapFrom(s => s.BenefitPlan != null ? s.BenefitPlan.EmployeeContribution : 0))
                .ForMember(d => d.PlanEmployerContribution, o => o.MapFrom(s => s.BenefitPlan != null ? s.BenefitPlan.EmployerContribution : 0));
            CreateMap<CreateEmployeeBenefitRequest, EmployeeBenefit>()
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.Ignore())
                .ForMember(d => d.EmployeeBenefitId, o => o.Ignore());

            // Benefit Deduction mappings
            CreateMap<BenefitDeduction, BenefitDeductionDto>()
                .ForMember(d => d.PlanName, o => o.MapFrom(s => s.BenefitPlan != null ? s.BenefitPlan.PlanName : null));
            CreateMap<CreateBenefitDeductionRequest, BenefitDeduction>()
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.Ignore())
                .ForMember(d => d.BenefitDeductionId, o => o.Ignore());

        }
    }
}
