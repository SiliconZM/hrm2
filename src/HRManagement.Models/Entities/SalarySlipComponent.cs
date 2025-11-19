namespace HRManagement.Models.Entities
{
    /// <summary>
    /// Breakdown of individual salary components on a salary slip
    /// </summary>
    public class SalarySlipComponent
    {
        public long SalarySlipComponentId { get; set; }
        public long SalarySlipId { get; set; }

        /// <summary>
        /// Component name (e.g., "Basic Salary", "HRA", "TA", "PF Deduction")
        /// </summary>
        public string ComponentName { get; set; } = string.Empty;

        /// <summary>
        /// Component type: Earning, Deduction, Tax
        /// </summary>
        public string ComponentType { get; set; } = "Earning";

        /// <summary>
        /// Amount for this component
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Display order in salary slip
        /// </summary>
        public int DisplayOrder { get; set; }

        // Navigation properties
        public virtual SalarySlip? SalarySlip { get; set; }
    }
}
