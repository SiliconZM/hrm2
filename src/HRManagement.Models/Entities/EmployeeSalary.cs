namespace HRManagement.Models.Entities
{
    /// <summary>
    /// Represents an employee's assigned salary structure with custom overrides
    /// </summary>
    public class EmployeeSalary
    {
        public long EmployeeSalaryId { get; set; }
        public long EmployeeId { get; set; }
        public long SalaryStructureId { get; set; }

        /// <summary>
        /// Effective date of this salary assignment
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// End date if salary was changed (null = current)
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Override basic salary if different from structure
        /// </summary>
        public decimal? OverrideBasicSalary { get; set; }

        /// <summary>
        /// Calculated gross salary (sum of all earnings)
        /// </summary>
        public decimal GrossSalary { get; set; }

        /// <summary>
        /// Calculated net salary (gross - deductions - tax)
        /// </summary>
        public decimal NetSalary { get; set; }

        /// <summary>
        /// Is this the current active salary
        /// </summary>
        public bool IsActive { get; set; } = true;

        public string? Remarks { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Employee? Employee { get; set; }
        public virtual SalaryStructure? SalaryStructure { get; set; }
    }
}
