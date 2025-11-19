using HRManagement.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Data.Seeders
{
    /// <summary>
    /// Seeder for Employee test data
    /// </summary>
    public class EmployeeSeeder
    {
        public static async Task SeedEmployeeDataAsync(HRContext context)
        {
            try
            {
                // Check if employees already exist
                if (await context.Employees.AnyAsync())
                {
                    Console.WriteLine("Employee data already seeded. Skipping.");
                    return;
                }

                Console.WriteLine("Seeding Employee data...");

                // Get organization (should exist from initial setup)
                var organization = await context.Organizations.FirstOrDefaultAsync();
                if (organization == null)
                {
                    // Create default organization if it doesn't exist
                    organization = new Organization
                    {
                        Name = "ABC Corporation Zambia",
                        IndustryType = "Manufacturing",
                        CountryCode = "ZM",
                        City = "Lusaka",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    context.Organizations.Add(organization);
                    await context.SaveChangesAsync();
                }

                // Get or create department
                var department = await context.Departments.FirstOrDefaultAsync(d => d.OrganizationId == organization.OrganizationId);
                if (department == null)
                {
                    department = new Department
                    {
                        OrganizationId = organization.OrganizationId,
                        Name = "Operations",
                        Description = "Operations Department",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    context.Departments.Add(department);
                    await context.SaveChangesAsync();
                }

                // Get or create job title
                var jobTitle = await context.JobTitles.FirstOrDefaultAsync(jt => jt.OrganizationId == organization.OrganizationId);
                if (jobTitle == null)
                {
                    jobTitle = new JobTitle
                    {
                        OrganizationId = organization.OrganizationId,
                        TitleName = "Staff",
                        Description = "General Staff Position",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    context.JobTitles.Add(jobTitle);
                    await context.SaveChangesAsync();
                }

                // Create test employees
                var employees = new List<Employee>
                {
                    new Employee
                    {
                        OrganizationId = organization.OrganizationId,
                        DepartmentId = department.DepartmentId,
                        JobTitleId = jobTitle.JobTitleId,
                        EmployeeCode = "EMP001",
                        FirstName = "John",
                        LastName = "Banda",
                        EmailOfficial = "john.banda@abc.co.zm",
                        EmailPersonal = "john.banda@email.com",
                        PhonePrimary = "+260123456789",
                        DateOfBirth = new DateTime(1985, 05, 15),
                        Gender = 'M',
                        Nationality = "Zambian",
                        HireDate = new DateTime(2022, 01, 15),
                        EmploymentType = "Full-Time",
                        EmploymentStatus = "Active",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Employee
                    {
                        OrganizationId = organization.OrganizationId,
                        DepartmentId = department.DepartmentId,
                        JobTitleId = jobTitle.JobTitleId,
                        EmployeeCode = "EMP002",
                        FirstName = "Grace",
                        LastName = "Mwale",
                        EmailOfficial = "grace.mwale@abc.co.zm",
                        EmailPersonal = "grace.mwale@email.com",
                        PhonePrimary = "+260198765432",
                        DateOfBirth = new DateTime(1988, 08, 22),
                        Gender = 'F',
                        Nationality = "Zambian",
                        HireDate = new DateTime(2022, 03, 20),
                        EmploymentType = "Full-Time",
                        EmploymentStatus = "Active",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Employee
                    {
                        OrganizationId = organization.OrganizationId,
                        DepartmentId = department.DepartmentId,
                        JobTitleId = jobTitle.JobTitleId,
                        EmployeeCode = "EMP003",
                        FirstName = "Mwila",
                        LastName = "Chulu",
                        EmailOfficial = "mwila.chulu@abc.co.zm",
                        EmailPersonal = "mwila.chulu@email.com",
                        PhonePrimary = "+260165432100",
                        DateOfBirth = new DateTime(1990, 12, 03),
                        Gender = 'M',
                        Nationality = "Zambian",
                        HireDate = new DateTime(2023, 06, 10),
                        EmploymentType = "Full-Time",
                        EmploymentStatus = "Active",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Employee
                    {
                        OrganizationId = organization.OrganizationId,
                        DepartmentId = department.DepartmentId,
                        JobTitleId = jobTitle.JobTitleId,
                        EmployeeCode = "EMP004",
                        FirstName = "Lungile",
                        LastName = "Phiri",
                        EmailOfficial = "lungile.phiri@abc.co.zm",
                        EmailPersonal = "lungile.phiri@email.com",
                        PhonePrimary = "+260176543210",
                        DateOfBirth = new DateTime(1987, 03, 18),
                        Gender = 'M',
                        Nationality = "Zambian",
                        HireDate = new DateTime(2023, 02, 14),
                        EmploymentType = "Full-Time",
                        EmploymentStatus = "Active",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Employee
                    {
                        OrganizationId = organization.OrganizationId,
                        DepartmentId = department.DepartmentId,
                        JobTitleId = jobTitle.JobTitleId,
                        EmployeeCode = "EMP005",
                        FirstName = "Nathalie",
                        LastName = "Kabonde",
                        EmailOfficial = "nathalie.kabonde@abc.co.zm",
                        EmailPersonal = "nathalie.kabonde@email.com",
                        PhonePrimary = "+260187654321",
                        DateOfBirth = new DateTime(1992, 07, 28),
                        Gender = 'F',
                        Nationality = "Zambian",
                        HireDate = new DateTime(2023, 09, 01),
                        EmploymentType = "Full-Time",
                        EmploymentStatus = "Active",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                };

                context.Employees.AddRange(employees);
                await context.SaveChangesAsync();

                Console.WriteLine($"Successfully seeded {employees.Count} employees");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding employee data: {ex.Message}");
                throw;
            }
        }
    }
}
