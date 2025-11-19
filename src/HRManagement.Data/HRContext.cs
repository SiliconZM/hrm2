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

        // Future modules will add more DbSets here
        // - Recruitment (JobPosting, Candidate, Application)
        // - Performance (Evaluation, Goal)
        // - Payroll (PayrollRun, PaySlip, Salary)
        // - Benefits, Contracts, Skills, Training, etc.

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
