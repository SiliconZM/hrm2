namespace HRManagement.Models.Entities
{
    /// <summary>
    /// Represents individual salary components (Basic, HRA, TA, DA, etc.)
    /// Used to build up the total salary in a salary structure
    /// </summary>
    public class SalaryComponent
    {
        public long SalaryComponentId { get; set; }
        public long SalaryStructureId { get; set; }

        /// <summary>
        /// Component name (e.g., "HRA", "TA", "DA", "Medical Allowance")
        /// </summary>
        public string ComponentName { get; set; } = string.Empty;

        /// <summary>
        /// Component type: Earning, Deduction, Tax
        /// </summary>
        public string ComponentType { get; set; } = "Earning"; // Earning, Deduction, Tax

        /// <summary>
        /// Amount for this component
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Percentage of basic salary (for percentage-based components)
        /// </summary>
        public decimal? Percentage { get; set; }

        /// <summary>
        /// Is this component taxable
        /// </summary>
        public bool IsTaxable { get; set; } = false;

        /// <summary>
        /// Is this component based on percentage or fixed amount
        /// </summary>
        public bool IsPercentageBased { get; set; } = false;

        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual SalaryStructure? SalaryStructure { get; set; }
    }
}
