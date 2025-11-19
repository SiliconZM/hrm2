using HRManagement.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Data.Seeders
{
    public class PayrollSeeder
    {
        public static async Task SeedPayrollDataAsync(HRContext context)
        {
            try
            {
                // Check if data already exists
                if (await context.SalaryStructures.AnyAsync())
                {
                    return; // Data already seeded
                }

                // Get sample organization (assumed to exist from phase 1)
                var organization = await context.Organizations.FirstOrDefaultAsync(o => o.OrganizationId == 1);
                if (organization == null)
                {
                    // Create a sample organization if none exists
                    organization = new Organization
                    {
                        Name = "ABC Corporation Zambia",
                        IndustryType = "Manufacturing",
                        CountryCode = "ZM",
                        City = "Lusaka",
                        Description = "Manufacturing company operating in Zambia",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    context.Organizations.Add(organization);
                    await context.SaveChangesAsync();
                }

                // Create Salary Structure (Zambian Standard)
                var salaryStructure = new SalaryStructure
                {
                    OrganizationId = organization.OrganizationId,
                    StructureName = "Standard Zambian Salary Structure 2024",
                    Description = "Standard salary structure for Zambian private sector employees",
                    BasicSalary = 5000m,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                context.SalaryStructures.Add(salaryStructure);
                await context.SaveChangesAsync();

                // Create Salary Components (Zambian Context)
                var components = new List<SalaryComponent>
                {
                    // Earnings
                    new SalaryComponent
                    {
                        SalaryStructureId = salaryStructure.SalaryStructureId,
                        ComponentName = "Basic Salary",
                        ComponentType = "Earning",
                        Amount = 5000m,
                        IsPercentageBased = false,
                        IsTaxable = true,
                        IsActive = true,
                        DisplayOrder = 1,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new SalaryComponent
                    {
                        SalaryStructureId = salaryStructure.SalaryStructureId,
                        ComponentName = "House Allowance",
                        ComponentType = "Earning",
                        Amount = 1500m,
                        IsPercentageBased = false,
                        IsTaxable = true,
                        IsActive = true,
                        DisplayOrder = 2,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new SalaryComponent
                    {
                        SalaryStructureId = salaryStructure.SalaryStructureId,
                        ComponentName = "Transport Allowance",
                        ComponentType = "Earning",
                        Amount = 500m,
                        IsPercentageBased = false,
                        IsTaxable = true,
                        IsActive = true,
                        DisplayOrder = 3,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new SalaryComponent
                    {
                        SalaryStructureId = salaryStructure.SalaryStructureId,
                        ComponentName = "Meal Allowance",
                        ComponentType = "Earning",
                        Amount = 300m,
                        IsPercentageBased = false,
                        IsTaxable = false,
                        IsActive = true,
                        DisplayOrder = 4,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },

                    // Deductions
                    new SalaryComponent
                    {
                        SalaryStructureId = salaryStructure.SalaryStructureId,
                        ComponentName = "PAYE Tax",
                        ComponentType = "Deduction",
                        Percentage = 15m,
                        IsPercentageBased = true,
                        IsTaxable = false,
                        IsActive = true,
                        DisplayOrder = 5,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new SalaryComponent
                    {
                        SalaryStructureId = salaryStructure.SalaryStructureId,
                        ComponentName = "NAPSA Contribution",
                        ComponentType = "Deduction",
                        Percentage = 5m,
                        IsPercentageBased = true,
                        IsTaxable = false,
                        IsActive = true,
                        DisplayOrder = 6,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new SalaryComponent
                    {
                        SalaryStructureId = salaryStructure.SalaryStructureId,
                        ComponentName = "Work Injury Benefits",
                        ComponentType = "Deduction",
                        Amount = 50m,
                        IsPercentageBased = false,
                        IsTaxable = false,
                        IsActive = true,
                        DisplayOrder = 7,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                };

                context.SalaryComponents.AddRange(components);
                await context.SaveChangesAsync();

                // Create sample employees (assuming they exist from phase 1)
                var employees = await context.Employees
                    .Where(e => e.OrganizationId == organization.OrganizationId)
                    .Take(5)
                    .ToListAsync();

                // If no employees exist, create sample ones
                if (!employees.Any())
                {
                    employees = new List<Employee>
                    {
                        new Employee
                        {
                            OrganizationId = organization.OrganizationId,
                            EmployeeCode = "EMP001",
                            FirstName = "John",
                            LastName = "Banda",
                            EmailOfficial = "john.banda@abc.co.zm",
                            EmailPersonal = "john.banda@example.com",
                            PhonePrimary = "+260 964 123456",
                            HireDate = new DateTime(2022, 1, 15),
                            EmploymentStatus = "Active",
                            EmploymentType = "Full-time",
                            CurrentAddress = "Lusaka, Zambia",
                            IsActive = true,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        },
                        new Employee
                        {
                            OrganizationId = organization.OrganizationId,
                            EmployeeCode = "EMP002",
                            FirstName = "Grace",
                            LastName = "Mwale",
                            EmailOfficial = "grace.mwale@abc.co.zm",
                            EmailPersonal = "grace.mwale@example.com",
                            PhonePrimary = "+260 974 234567",
                            HireDate = new DateTime(2022, 3, 20),
                            EmploymentStatus = "Active",
                            EmploymentType = "Full-time",
                            CurrentAddress = "Lusaka, Zambia",
                            IsActive = true,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        },
                        new Employee
                        {
                            OrganizationId = organization.OrganizationId,
                            EmployeeCode = "EMP003",
                            FirstName = "Mwila",
                            LastName = "Chulu",
                            EmailOfficial = "mwila.chulu@abc.co.zm",
                            EmailPersonal = "mwila.chulu@example.com",
                            PhonePrimary = "+260 965 345678",
                            HireDate = new DateTime(2023, 6, 10),
                            EmploymentStatus = "Active",
                            EmploymentType = "Full-time",
                            CurrentAddress = "Ndola, Zambia",
                            IsActive = true,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        },
                        new Employee
                        {
                            OrganizationId = organization.OrganizationId,
                            EmployeeCode = "EMP004",
                            FirstName = "Lungile",
                            LastName = "Phiri",
                            EmailOfficial = "lungile.phiri@abc.co.zm",
                            EmailPersonal = "lungile.phiri@example.com",
                            PhonePrimary = "+260 975 456789",
                            HireDate = new DateTime(2023, 2, 14),
                            EmploymentStatus = "Active",
                            EmploymentType = "Full-time",
                            CurrentAddress = "Kitwe, Zambia",
                            IsActive = true,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        },
                        new Employee
                        {
                            OrganizationId = organization.OrganizationId,
                            EmployeeCode = "EMP005",
                            FirstName = "Nathalie",
                            LastName = "Kabonde",
                            EmailOfficial = "nathalie.kabonde@abc.co.zm",
                            EmailPersonal = "nathalie.kabonde@example.com",
                            PhonePrimary = "+260 966 567890",
                            HireDate = new DateTime(2023, 9, 01),
                            EmploymentStatus = "Active",
                            EmploymentType = "Full-time",
                            CurrentAddress = "Livingstone, Zambia",
                            IsActive = true,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        }
                    };

                    context.Employees.AddRange(employees);
                    await context.SaveChangesAsync();
                }

                // Assign salary structure to employees
                foreach (var employee in employees)
                {
                    var employeeSalary = new EmployeeSalary
                    {
                        EmployeeId = employee.EmployeeId,
                        SalaryStructureId = salaryStructure.SalaryStructureId,
                        EffectiveDate = DateTime.UtcNow.AddDays(-30),
                        EndDate = null,
                        OverrideBasicSalary = null,
                        GrossSalary = 7800m, // Basic + House + Transport + Meal
                        NetSalary = 6318m, // After PAYE, NAPSA, WIB
                        IsActive = true,
                        Remarks = "Standard salary assignment for 2024",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    context.EmployeeSalaries.Add(employeeSalary);
                }

                await context.SaveChangesAsync();

                // Create sample Payroll Run (November 2024)
                var payroll = new Payroll
                {
                    OrganizationId = organization.OrganizationId,
                    PayrollName = "November 2024 Payroll",
                    PayrollFrequency = "Monthly",
                    StartDate = new DateTime(2024, 11, 1),
                    EndDate = new DateTime(2024, 11, 30),
                    Status = "Draft",
                    TotalGrossSalary = 0,
                    TotalDeductions = 0,
                    TotalTax = 0,
                    TotalNetSalary = 0,
                    EmployeeCount = 0,
                    Remarks = "November salary run",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                context.Payrolls.Add(payroll);
                await context.SaveChangesAsync();

                // Create Payroll Details for each employee
                var employeeSalaries = await context.EmployeeSalaries
                    .Where(es => es.IsActive && es.EmployeeId == employees.First().EmployeeId ||
                                 es.EmployeeId == employees[1].EmployeeId ||
                                 es.EmployeeId == employees[2].EmployeeId ||
                                 es.EmployeeId == employees[3].EmployeeId ||
                                 es.EmployeeId == employees[4].EmployeeId)
                    .ToListAsync();

                decimal totalGross = 0;
                decimal totalDeductions = 0;
                decimal totalTax = 0;
                decimal totalNet = 0;

                foreach (var empSalary in employeeSalaries)
                {
                    var grossSalary = 7800m; // 5000 + 1500 + 500 + 300 (no tax on meal)
                    var payeeTax = 1170m; // 15% of 7800 (actually PAYE on taxable income)
                    var napsaDeduction = 390m; // 5% of 7800
                    var wibDeduction = 50m;
                    var totalDeduction = payeeTax + napsaDeduction + wibDeduction;
                    var netSalary = grossSalary - totalDeduction;

                    var payrollDetail = new PayrollDetail
                    {
                        PayrollId = payroll.PayrollId,
                        EmployeeId = empSalary.EmployeeId,
                        TotalEarnings = grossSalary,
                        TotalDeductions = totalDeduction,
                        TotalTax = payeeTax,
                        GrossSalary = grossSalary,
                        NetSalary = netSalary,
                        WorkingDays = 22,
                        DaysWorked = 22,
                        LeavesDays = 0,
                        Status = "Draft",
                        Remarks = "November salary",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    context.PayrollDetails.Add(payrollDetail);

                    totalGross += grossSalary;
                    totalDeductions += totalDeduction;
                    totalTax += payeeTax;
                    totalNet += netSalary;
                }

                await context.SaveChangesAsync();

                // Update Payroll totals
                payroll.TotalGrossSalary = totalGross;
                payroll.TotalDeductions = totalDeductions;
                payroll.TotalTax = totalTax;
                payroll.TotalNetSalary = totalNet;
                payroll.EmployeeCount = employeeSalaries.Count;

                context.Payrolls.Update(payroll);
                await context.SaveChangesAsync();

                // Create sample Salary Slips for the first two employees
                var payrollDetails = await context.PayrollDetails
                    .Where(pd => pd.PayrollId == payroll.PayrollId)
                    .Include(pd => pd.Employee)
                    .ToListAsync();

                foreach (var detail in payrollDetails.Take(2))
                {
                    var slip = new SalarySlip
                    {
                        PayrollDetailId = detail.PayrollDetailId,
                        EmployeeId = detail.EmployeeId,
                        SlipNumber = $"SS-{payroll.PayrollId}-{detail.EmployeeId}-20241101",
                        Period = "November 2024",
                        GrossSalary = detail.GrossSalary,
                        TotalDeductions = detail.TotalDeductions,
                        IncomeTax = detail.TotalTax,
                        NetPayable = detail.NetSalary,
                        Status = "Generated",
                        SalaryCreditedDate = null,
                        Remarks = "Auto-generated salary slip",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    context.SalarySlips.Add(slip);
                }

                await context.SaveChangesAsync();

                // Add components to the first generated slip
                var firstSlip = await context.SalarySlips
                    .Where(ss => ss.Status == "Generated")
                    .FirstOrDefaultAsync();

                if (firstSlip != null)
                {
                    var slipComponents = new List<SalarySlipComponent>
                    {
                        new SalarySlipComponent
                        {
                            SalarySlipId = firstSlip.SalarySlipId,
                            ComponentName = "Basic Salary",
                            ComponentType = "Earning",
                            Amount = 5000m,
                            DisplayOrder = 1
                        },
                        new SalarySlipComponent
                        {
                            SalarySlipId = firstSlip.SalarySlipId,
                            ComponentName = "House Allowance",
                            ComponentType = "Earning",
                            Amount = 1500m,
                            DisplayOrder = 2
                        },
                        new SalarySlipComponent
                        {
                            SalarySlipId = firstSlip.SalarySlipId,
                            ComponentName = "Transport Allowance",
                            ComponentType = "Earning",
                            Amount = 500m,
                            DisplayOrder = 3
                        },
                        new SalarySlipComponent
                        {
                            SalarySlipId = firstSlip.SalarySlipId,
                            ComponentName = "Meal Allowance",
                            ComponentType = "Earning",
                            Amount = 300m,
                            DisplayOrder = 4
                        },
                        new SalarySlipComponent
                        {
                            SalarySlipId = firstSlip.SalarySlipId,
                            ComponentName = "PAYE Tax",
                            ComponentType = "Deduction",
                            Amount = 1170m,
                            DisplayOrder = 5
                        },
                        new SalarySlipComponent
                        {
                            SalarySlipId = firstSlip.SalarySlipId,
                            ComponentName = "NAPSA Contribution",
                            ComponentType = "Deduction",
                            Amount = 390m,
                            DisplayOrder = 6
                        },
                        new SalarySlipComponent
                        {
                            SalarySlipId = firstSlip.SalarySlipId,
                            ComponentName = "Work Injury Benefits",
                            ComponentType = "Deduction",
                            Amount = 50m,
                            DisplayOrder = 7
                        }
                    };

                    context.SalarySlipComponents.AddRange(slipComponents);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error seeding payroll data", ex);
            }
        }
    }
}
