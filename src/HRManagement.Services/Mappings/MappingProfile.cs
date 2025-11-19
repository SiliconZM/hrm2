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
        }
    }
}
