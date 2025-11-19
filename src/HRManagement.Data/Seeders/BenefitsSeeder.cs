using HRManagement.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Data.Seeders
{
    /// <summary>
    /// Seeder for Benefits Management test data
    /// </summary>
    public class BenefitsSeeder
    {
        public static async Task SeedBenefitsDataAsync(HRContext context)
        {
            try
            {
                // Check if benefits data already exists
                if (await context.BenefitTypes.AnyAsync() && await context.BenefitPlans.AnyAsync())
                {
                    Console.WriteLine("Benefits data already seeded. Skipping.");
                    return;
                }

                Console.WriteLine("Seeding Benefits data...");

                // Disable foreign key constraints for seeding
                await context.Database.ExecuteSqlRawAsync("PRAGMA foreign_keys = OFF");

                // Get organization
                var organization = await context.Organizations.FirstOrDefaultAsync();
                if (organization == null)
                {
                    Console.WriteLine("No organization found. Skipping benefits seeding.");
                    await context.Database.ExecuteSqlRawAsync("PRAGMA foreign_keys = ON");
                    return;
                }

                // Create Benefit Types
                var healthInsuranceType = new BenefitType
                {
                    OrganizationId = organization.OrganizationId,
                    TypeName = "Health Insurance",
                    Description = "Medical, dental, and vision coverage",
                    Category = "Health",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var retirementType = new BenefitType
                {
                    OrganizationId = organization.OrganizationId,
                    TypeName = "Retirement Benefits",
                    Description = "Pension and retirement savings plans",
                    Category = "Retirement",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var lifeInsuranceType = new BenefitType
                {
                    OrganizationId = organization.OrganizationId,
                    TypeName = "Life Insurance",
                    Description = "Term and permanent life insurance coverage",
                    Category = "Insurance",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var wellnessType = new BenefitType
                {
                    OrganizationId = organization.OrganizationId,
                    TypeName = "Wellness Programs",
                    Description = "Fitness, mental health, and wellness benefits",
                    Category = "Wellness",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                context.BenefitTypes.AddRange(healthInsuranceType, retirementType, lifeInsuranceType, wellnessType);
                await context.SaveChangesAsync();

                // Refresh to get IDs
                healthInsuranceType = await context.BenefitTypes.FirstAsync(bt => bt.TypeName == "Health Insurance");
                retirementType = await context.BenefitTypes.FirstAsync(bt => bt.TypeName == "Retirement Benefits");
                lifeInsuranceType = await context.BenefitTypes.FirstAsync(bt => bt.TypeName == "Life Insurance");
                wellnessType = await context.BenefitTypes.FirstAsync(bt => bt.TypeName == "Wellness Programs");

                // Create Benefit Plans
                var goldHealthPlan = new BenefitPlan
                {
                    BenefitTypeId = healthInsuranceType.BenefitTypeId,
                    OrganizationId = organization.OrganizationId,
                    PlanName = "Gold Health Plan",
                    Description = "Comprehensive health coverage with low deductibles",
                    PlanCode = "HEALTH-GOLD-001",
                    EmployeeContribution = 250.00m,
                    EmployerContribution = 500.00m,
                    ContributionFrequency = "Monthly",
                    EffectiveDate = DateTime.UtcNow.AddMonths(-6),
                    EndDate = null,
                    CoverageAmount = 50000.00m,
                    CoverageDetails = "Medical, Dental, Vision coverage",
                    IsActive = true,
                    IsDefaultPlan = true,
                    DisplayOrder = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var silverHealthPlan = new BenefitPlan
                {
                    BenefitTypeId = healthInsuranceType.BenefitTypeId,
                    OrganizationId = organization.OrganizationId,
                    PlanName = "Silver Health Plan",
                    Description = "Standard health coverage with moderate deductibles",
                    PlanCode = "HEALTH-SILVER-001",
                    EmployeeContribution = 150.00m,
                    EmployerContribution = 300.00m,
                    ContributionFrequency = "Monthly",
                    EffectiveDate = DateTime.UtcNow.AddMonths(-6),
                    EndDate = null,
                    CoverageAmount = 30000.00m,
                    CoverageDetails = "Medical, Dental coverage",
                    IsActive = true,
                    IsDefaultPlan = false,
                    DisplayOrder = 2,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var retirementPlan = new BenefitPlan
                {
                    BenefitTypeId = retirementType.BenefitTypeId,
                    OrganizationId = organization.OrganizationId,
                    PlanName = "401(k) Retirement Plan",
                    Description = "Tax-deferred retirement savings with employer match",
                    PlanCode = "RET-401K-001",
                    EmployeeContribution = 300.00m,
                    EmployerContribution = 300.00m,
                    ContributionFrequency = "Monthly",
                    EffectiveDate = DateTime.UtcNow.AddMonths(-12),
                    EndDate = null,
                    CoverageAmount = 500000.00m,
                    CoverageDetails = "Up to 100% match on first 3% of salary",
                    IsActive = true,
                    IsDefaultPlan = true,
                    DisplayOrder = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var lifeInsurancePlan = new BenefitPlan
                {
                    BenefitTypeId = lifeInsuranceType.BenefitTypeId,
                    OrganizationId = organization.OrganizationId,
                    PlanName = "Term Life Insurance",
                    Description = "2x annual salary in life insurance coverage",
                    PlanCode = "LIFE-TERM-001",
                    EmployeeContribution = 25.00m,
                    EmployerContribution = 75.00m,
                    ContributionFrequency = "Monthly",
                    EffectiveDate = DateTime.UtcNow.AddMonths(-6),
                    EndDate = null,
                    CoverageAmount = 300000.00m,
                    CoverageDetails = "2x annual salary death benefit",
                    IsActive = true,
                    IsDefaultPlan = true,
                    DisplayOrder = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var fitnessPlan = new BenefitPlan
                {
                    BenefitTypeId = wellnessType.BenefitTypeId,
                    OrganizationId = organization.OrganizationId,
                    PlanName = "Gym & Fitness Membership",
                    Description = "Free gym membership and fitness classes",
                    PlanCode = "WELLNESS-GYM-001",
                    EmployeeContribution = 0.00m,
                    EmployerContribution = 75.00m,
                    ContributionFrequency = "Monthly",
                    EffectiveDate = DateTime.UtcNow.AddMonths(-3),
                    EndDate = null,
                    CoverageAmount = 0.00m,
                    CoverageDetails = "Access to 500+ gyms nationwide",
                    IsActive = true,
                    IsDefaultPlan = true,
                    DisplayOrder = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                context.BenefitPlans.AddRange(goldHealthPlan, silverHealthPlan, retirementPlan, lifeInsurancePlan, fitnessPlan);
                await context.SaveChangesAsync();

                // Get first 3 employees for enrollment
                var employees = await context.Employees
                    .Where(e => e.IsActive)
                    .Take(3)
                    .ToListAsync();

                // Create sample employee enrollments
                foreach (var employee in employees)
                {
                    var goldHealthEnrollment = new EmployeeBenefit
                    {
                        EmployeeId = employee.EmployeeId,
                        BenefitPlanId = goldHealthPlan.BenefitPlanId,
                        EnrolledDate = DateTime.UtcNow.AddMonths(-6),
                        EnrollmentStatus = "Active",
                        BeneficiaryInfo = $"{employee.FirstName} {employee.LastName}",
                        UsedAmount = 0.00m,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    var retirementEnrollment = new EmployeeBenefit
                    {
                        EmployeeId = employee.EmployeeId,
                        BenefitPlanId = retirementPlan.BenefitPlanId,
                        EnrolledDate = DateTime.UtcNow.AddMonths(-12),
                        EnrollmentStatus = "Active",
                        UsedAmount = 0.00m,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    context.EmployeeBenefits.AddRange(goldHealthEnrollment, retirementEnrollment);
                }
                await context.SaveChangesAsync();

                // Create benefit deductions (how they appear in payroll)
                goldHealthPlan = await context.BenefitPlans.FirstAsync(bp => bp.PlanCode == "HEALTH-GOLD-001");
                retirementPlan = await context.BenefitPlans.FirstAsync(bp => bp.PlanCode == "RET-401K-001");

                var healthDeduction = new BenefitDeduction
                {
                    BenefitPlanId = goldHealthPlan.BenefitPlanId,
                    DeductionName = "Health Insurance Premium",
                    DeductionCode = "HIP",
                    DeductionType = "Employee",
                    Amount = 250.00m,
                    Percentage = 0.00m,
                    IsPercentageBased = false,
                    IsFixed = true,
                    IsTaxable = false,
                    IsTaxDeductible = false,
                    DeductionFrequency = "Monthly",
                    IsActive = true,
                    DisplayOrder = 1,
                    EffectiveDate = DateTime.UtcNow.AddMonths(-6),
                    EndDate = null,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var retirementDeduction = new BenefitDeduction
                {
                    BenefitPlanId = retirementPlan.BenefitPlanId,
                    DeductionName = "401(k) Contribution",
                    DeductionCode = "401K",
                    DeductionType = "Employee",
                    Amount = 0.00m,
                    Percentage = 3.00m,
                    IsPercentageBased = true,
                    IsFixed = false,
                    IsTaxable = false,
                    IsTaxDeductible = true,
                    DeductionFrequency = "Monthly",
                    IsActive = true,
                    DisplayOrder = 2,
                    EffectiveDate = DateTime.UtcNow.AddMonths(-12),
                    EndDate = null,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                context.BenefitDeductions.AddRange(healthDeduction, retirementDeduction);
                await context.SaveChangesAsync();

                // Re-enable foreign key constraints
                await context.Database.ExecuteSqlRawAsync("PRAGMA foreign_keys = ON");

                Console.WriteLine("Benefits data seeded successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding benefits data: {ex.Message}");
                throw;
            }
        }
    }
}
