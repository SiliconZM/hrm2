using Microsoft.EntityFrameworkCore;
using HRManagement.Models.Entities;

namespace HRManagement.Data
{
    public class HRContext : DbContext
    {
        public HRContext(DbContextOptions<HRContext> options) : base(options)
        {
        }

        // DbSets for core entities
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmploymentHistory> EmploymentHistories { get; set; }

        // DbSets for Leave Management module
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<LeaveBalance> LeaveBalances { get; set; }
        public DbSet<Attendance> Attendances { get; set; }

        // DbSets for Recruitment module
        public DbSet<JobPosting> JobPostings { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Application> Applications { get; set; }

        // DbSets for Performance Management module
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<Goal> Goals { get; set; }

        // DbSets for Skills & Competencies module
        public DbSet<Skill> Skills { get; set; }
        public DbSet<EmployeeSkill> EmployeeSkills { get; set; }

        // DbSets for Contracts module
        public DbSet<Contract> Contracts { get; set; }

        // DbSets for Payroll & Compensation module
        public DbSet<SalaryStructure> SalaryStructures { get; set; }
        public DbSet<SalaryComponent> SalaryComponents { get; set; }
        public DbSet<EmployeeSalary> EmployeeSalaries { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<PayrollDetail> PayrollDetails { get; set; }
        public DbSet<SalarySlip> SalarySlips { get; set; }
        public DbSet<SalarySlipComponent> SalarySlipComponents { get; set; }

        // DbSets for Benefits Management module
        public DbSet<BenefitType> BenefitTypes { get; set; }
        public DbSet<BenefitPlan> BenefitPlans { get; set; }
        public DbSet<EmployeeBenefit> EmployeeBenefits { get; set; }
        public DbSet<BenefitDeduction> BenefitDeductions { get; set; }
        // DbSets for Tax Management module
        public DbSet<TaxConfiguration> TaxConfigurations { get; set; }
        public DbSet<TaxSlab> TaxSlabs { get; set; }

        // Future modules will add more DbSets here
        // - Training, etc.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Organization
            modelBuilder.Entity<Organization>()
                .HasKey(o => o.OrganizationId);
            modelBuilder.Entity<Organization>()
                .HasIndex(o => o.Name)
                .IsUnique();

            // Department
            modelBuilder.Entity<Department>()
                .HasKey(d => d.DepartmentId);
            modelBuilder.Entity<Department>()
                .HasOne(d => d.Organization)
                .WithMany(o => o.Departments)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Department>()
                .HasOne(d => d.ParentDepartment)
                .WithMany(d => d.SubDepartments)
                .HasForeignKey(d => d.ParentDepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Department>()
                .HasIndex(d => new { d.OrganizationId, d.Name })
                .IsUnique();

            // JobTitle
            modelBuilder.Entity<JobTitle>()
                .HasKey(j => j.JobTitleId);
            modelBuilder.Entity<JobTitle>()
                .HasOne(j => j.Organization)
                .WithMany(o => o.JobTitles)
                .HasForeignKey(j => j.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<JobTitle>()
                .HasOne(j => j.ReportsToJobTitle)
                .WithMany(j => j.SubordinateJobTitles)
                .HasForeignKey(j => j.ReportsToJobTitleId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<JobTitle>()
                .HasIndex(j => new { j.OrganizationId, j.TitleName })
                .IsUnique();

            // Employee
            modelBuilder.Entity<Employee>()
                .HasKey(e => e.EmployeeId);
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Organization)
                .WithMany(o => o.Employees)
                .HasForeignKey(e => e.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.JobTitle)
                .WithMany(j => j.Employees)
                .HasForeignKey(e => e.JobTitleId)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.ReportingManager)
                .WithMany(e => e.DirectReports)
                .HasForeignKey(e => e.ReportingManagerId)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Employee>()
                .HasIndex(e => new { e.OrganizationId, e.EmployeeCode })
                .IsUnique();
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.EmailOfficial)
                .IsUnique();

            // EmploymentHistory
            modelBuilder.Entity<EmploymentHistory>()
                .HasKey(eh => eh.EmploymentHistoryId);
            modelBuilder.Entity<EmploymentHistory>()
                .HasOne(eh => eh.Employee)
                .WithMany(e => e.EmploymentHistories)
                .HasForeignKey(eh => eh.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<EmploymentHistory>()
                .HasOne(eh => eh.JobTitle)
                .WithMany()
                .HasForeignKey(eh => eh.JobTitleId)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<EmploymentHistory>()
                .HasOne(eh => eh.Department)
                .WithMany()
                .HasForeignKey(eh => eh.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull);

            // LeaveType
            modelBuilder.Entity<LeaveType>()
                .HasKey(lt => lt.LeaveTypeId);
            modelBuilder.Entity<LeaveType>()
                .HasOne(lt => lt.Organization)
                .WithMany()
                .HasForeignKey(lt => lt.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<LeaveType>()
                .HasIndex(lt => new { lt.OrganizationId, lt.LeaveTypeName })
                .IsUnique();

            // LeaveBalance
            modelBuilder.Entity<LeaveBalance>()
                .HasKey(lb => lb.LeaveBalanceId);
            modelBuilder.Entity<LeaveBalance>()
                .HasOne(lb => lb.Employee)
                .WithMany()
                .HasForeignKey(lb => lb.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<LeaveBalance>()
                .HasOne(lb => lb.LeaveType)
                .WithMany(lt => lt.LeaveBalances)
                .HasForeignKey(lb => lb.LeaveTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<LeaveBalance>()
                .HasIndex(lb => new { lb.EmployeeId, lb.LeaveTypeId, lb.FinancialYear })
                .IsUnique();

            // LeaveRequest
            modelBuilder.Entity<LeaveRequest>()
                .HasKey(lr => lr.LeaveRequestId);
            modelBuilder.Entity<LeaveRequest>()
                .HasOne(lr => lr.Employee)
                .WithMany()
                .HasForeignKey(lr => lr.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<LeaveRequest>()
                .HasOne(lr => lr.LeaveType)
                .WithMany(lt => lt.LeaveRequests)
                .HasForeignKey(lr => lr.LeaveTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<LeaveRequest>()
                .HasOne(lr => lr.ApprovedByEmployee)
                .WithMany()
                .HasForeignKey(lr => lr.ApprovedBy)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<LeaveRequest>()
                .HasIndex(lr => new { lr.EmployeeId, lr.StartDate, lr.EndDate });

            // Attendance
            modelBuilder.Entity<Attendance>()
                .HasKey(a => a.AttendanceId);
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Employee)
                .WithMany()
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Attendance>()
                .HasIndex(a => new { a.EmployeeId, a.AttendanceDate })
                .IsUnique();

            // JobPosting
            modelBuilder.Entity<JobPosting>()
                .HasKey(jp => jp.JobPostingId);
            modelBuilder.Entity<JobPosting>()
                .HasOne(jp => jp.Organization)
                .WithMany()
                .HasForeignKey(jp => jp.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<JobPosting>()
                .HasOne(jp => jp.Department)
                .WithMany()
                .HasForeignKey(jp => jp.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<JobPosting>()
                .HasIndex(jp => new { jp.OrganizationId, jp.Status });

            // Candidate
            modelBuilder.Entity<Candidate>()
                .HasKey(c => c.CandidateId);
            modelBuilder.Entity<Candidate>()
                .HasOne(c => c.Organization)
                .WithMany()
                .HasForeignKey(c => c.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Candidate>()
                .HasIndex(c => c.Email)
                .IsUnique();

            // Application
            modelBuilder.Entity<Application>()
                .HasKey(a => a.ApplicationId);
            modelBuilder.Entity<Application>()
                .HasOne(a => a.Candidate)
                .WithMany(c => c.Applications)
                .HasForeignKey(a => a.CandidateId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Application>()
                .HasOne(a => a.JobPosting)
                .WithMany(jp => jp.Applications)
                .HasForeignKey(a => a.JobPostingId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Application>()
                .HasOne(a => a.AssignedTo)
                .WithMany()
                .HasForeignKey(a => a.AssignedToId)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Application>()
                .HasIndex(a => new { a.CandidateId, a.JobPostingId })
                .IsUnique();

            // Evaluation
            modelBuilder.Entity<Evaluation>()
                .HasKey(e => e.EvaluationId);
            modelBuilder.Entity<Evaluation>()
                .HasOne(e => e.Employee)
                .WithMany()
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Evaluation>()
                .HasOne(e => e.Evaluator)
                .WithMany()
                .HasForeignKey(e => e.EvaluatorId)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Evaluation>()
                .HasIndex(e => new { e.EmployeeId, e.EvaluationType });

            // Goal
            modelBuilder.Entity<Goal>()
                .HasKey(g => g.GoalId);
            modelBuilder.Entity<Goal>()
                .HasOne(g => g.Employee)
                .WithMany()
                .HasForeignKey(g => g.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Goal>()
                .HasOne(g => g.Owner)
                .WithMany()
                .HasForeignKey(g => g.OwnerId)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Goal>()
                .HasOne(g => g.Evaluation)
                .WithMany(e => e.Goals)
                .HasForeignKey(g => g.EvaluationId)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Goal>()
                .HasIndex(g => new { g.EmployeeId, g.Status });

            // Skill
            modelBuilder.Entity<Skill>()
                .HasKey(s => s.SkillId);
            modelBuilder.Entity<Skill>()
                .HasOne(s => s.Organization)
                .WithMany()
                .HasForeignKey(s => s.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Skill>()
                .HasIndex(s => new { s.OrganizationId, s.SkillName })
                .IsUnique();

            // EmployeeSkill
            modelBuilder.Entity<EmployeeSkill>()
                .HasKey(es => es.EmployeeSkillId);
            modelBuilder.Entity<EmployeeSkill>()
                .HasOne(es => es.Employee)
                .WithMany()
                .HasForeignKey(es => es.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<EmployeeSkill>()
                .HasOne(es => es.Skill)
                .WithMany(s => s.EmployeeSkills)
                .HasForeignKey(es => es.SkillId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<EmployeeSkill>()
                .HasIndex(es => new { es.EmployeeId, es.SkillId })
                .IsUnique();

            // Contract
            modelBuilder.Entity<Contract>()
                .HasKey(c => c.ContractId);
            modelBuilder.Entity<Contract>()
                .HasOne(c => c.Employee)
                .WithMany()
                .HasForeignKey(c => c.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Contract>()
                .HasIndex(c => new { c.EmployeeId, c.ContractType });

            // SalaryStructure
            modelBuilder.Entity<SalaryStructure>()
                .HasKey(ss => ss.SalaryStructureId);
            modelBuilder.Entity<SalaryStructure>()
                .HasOne(ss => ss.Organization)
                .WithMany()
                .HasForeignKey(ss => ss.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SalaryStructure>()
                .HasIndex(ss => new { ss.OrganizationId, ss.StructureName })
                .IsUnique();

            // SalaryComponent
            modelBuilder.Entity<SalaryComponent>()
                .HasKey(sc => sc.SalaryComponentId);
            modelBuilder.Entity<SalaryComponent>()
                .HasOne(sc => sc.SalaryStructure)
                .WithMany(ss => ss.SalaryComponents)
                .HasForeignKey(sc => sc.SalaryStructureId)
                .OnDelete(DeleteBehavior.Cascade);

            // EmployeeSalary
            modelBuilder.Entity<EmployeeSalary>()
                .HasKey(es => es.EmployeeSalaryId);
            modelBuilder.Entity<EmployeeSalary>()
                .HasOne(es => es.Employee)
                .WithMany()
                .HasForeignKey(es => es.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<EmployeeSalary>()
                .HasOne(es => es.SalaryStructure)
                .WithMany(ss => ss.EmployeeSalaries)
                .HasForeignKey(es => es.SalaryStructureId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<EmployeeSalary>()
                .HasIndex(es => new { es.EmployeeId, es.IsActive });

            // Payroll
            modelBuilder.Entity<Payroll>()
                .HasKey(p => p.PayrollId);
            modelBuilder.Entity<Payroll>()
                .HasOne(p => p.Organization)
                .WithMany()
                .HasForeignKey(p => p.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Payroll>()
                .HasIndex(p => new { p.OrganizationId, p.Status });

            // PayrollDetail
            modelBuilder.Entity<PayrollDetail>()
                .HasKey(pd => pd.PayrollDetailId);
            modelBuilder.Entity<PayrollDetail>()
                .HasOne(pd => pd.Payroll)
                .WithMany(p => p.PayrollDetails)
                .HasForeignKey(pd => pd.PayrollId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<PayrollDetail>()
                .HasOne(pd => pd.Employee)
                .WithMany()
                .HasForeignKey(pd => pd.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PayrollDetail>()
                .HasIndex(pd => new { pd.PayrollId, pd.EmployeeId })
                .IsUnique();

            // SalarySlip
            modelBuilder.Entity<SalarySlip>()
                .HasKey(ss => ss.SalarySlipId);
            modelBuilder.Entity<SalarySlip>()
                .HasOne(ss => ss.PayrollDetail)
                .WithMany(pd => pd.SalarySlips)
                .HasForeignKey(ss => ss.PayrollDetailId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<SalarySlip>()
                .HasOne(ss => ss.Employee)
                .WithMany()
                .HasForeignKey(ss => ss.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SalarySlip>()
                .HasIndex(ss => ss.SlipNumber)
                .IsUnique();

            // SalarySlipComponent
            modelBuilder.Entity<SalarySlipComponent>()
                .HasKey(ssc => ssc.SalarySlipComponentId);
            modelBuilder.Entity<SalarySlipComponent>()
                .HasOne(ssc => ssc.SalarySlip)
                .WithMany(ss => ss.SalarySlipComponents)
                .HasForeignKey(ssc => ssc.SalarySlipId)
                .OnDelete(DeleteBehavior.Cascade);

            // BenefitType
            modelBuilder.Entity<BenefitType>()
                .HasKey(bt => bt.BenefitTypeId);
            modelBuilder.Entity<BenefitType>()
                .HasOne(bt => bt.Organization)
                .WithMany()
                .HasForeignKey(bt => bt.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<BenefitType>()
                .HasIndex(bt => new { bt.OrganizationId, bt.TypeName })
                .IsUnique();

            // BenefitPlan
            modelBuilder.Entity<BenefitPlan>()
                .HasKey(bp => bp.BenefitPlanId);
            modelBuilder.Entity<BenefitPlan>()
                .HasOne(bp => bp.BenefitType)
                .WithMany(bt => bt.BenefitPlans)
                .HasForeignKey(bp => bp.BenefitTypeId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<BenefitPlan>()
                .HasOne(bp => bp.Organization)
                .WithMany()
                .HasForeignKey(bp => bp.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<BenefitPlan>()
                .HasIndex(bp => bp.PlanCode)
                .IsUnique();

            // EmployeeBenefit
            modelBuilder.Entity<EmployeeBenefit>()
                .HasKey(eb => eb.EmployeeBenefitId);
            modelBuilder.Entity<EmployeeBenefit>()
                .HasOne(eb => eb.Employee)
                .WithMany()
                .HasForeignKey(eb => eb.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<EmployeeBenefit>()
                .HasOne(eb => eb.BenefitPlan)
                .WithMany(bp => bp.EmployeeBenefits)
                .HasForeignKey(eb => eb.BenefitPlanId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<EmployeeBenefit>()
                .HasIndex(eb => new { eb.EmployeeId, eb.BenefitPlanId })
                .IsUnique();

            // BenefitDeduction
            modelBuilder.Entity<BenefitDeduction>()
                .HasKey(bd => bd.BenefitDeductionId);
            modelBuilder.Entity<BenefitDeduction>()
                .HasOne(bd => bd.BenefitPlan)
                .WithMany(bp => bp.BenefitDeductions)
                .HasForeignKey(bd => bd.BenefitPlanId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<BenefitDeduction>()
                .HasIndex(bd => bd.DeductionCode)
                .IsUnique();
            // TaxConfiguration
            modelBuilder.Entity<TaxConfiguration>()
                .HasKey(tc => tc.TaxConfigurationId);
            modelBuilder.Entity<TaxConfiguration>()
                .HasOne(tc => tc.Organization)
                .WithMany()
                .HasForeignKey(tc => tc.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TaxConfiguration>()
                .HasIndex(tc => new { tc.OrganizationId, tc.FinancialYear, tc.ConfigurationName })
                .IsUnique();

            // TaxSlab
            modelBuilder.Entity<TaxSlab>()
                .HasKey(ts => ts.TaxSlabId);
            modelBuilder.Entity<TaxSlab>()
                .HasOne(ts => ts.TaxConfiguration)
                .WithMany(tc => tc.TaxSlabs)
                .HasForeignKey(ts => ts.TaxConfigurationId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<TaxSlab>()
                .HasIndex(ts => new { ts.TaxConfigurationId, ts.FromAmount, ts.ToAmount })
                .IsUnique();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Add audit tracking
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is Entity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((dynamic)entry.Entity).CreatedAt = DateTime.UtcNow;
                    ((dynamic)entry.Entity).UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    ((dynamic)entry.Entity).UpdatedAt = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Base class for audit tracking
    /// </summary>
    public abstract class Entity
    {
    }
}
