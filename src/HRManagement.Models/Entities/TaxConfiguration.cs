namespace HRManagement.Models.Entities
{
    /// <summary>
    /// Represents tax configuration for an organization (e.g., location-based tax rules, rates)
    /// </summary>
    public class TaxConfiguration
    {
        public long TaxConfigurationId { get; set; }
        public long OrganizationId { get; set; }

        /// <summary>
        /// Name of the tax configuration (e.g., "Zambia-2024", "Standard-Tax")
        /// </summary>
        public string ConfigurationName { get; set; } = string.Empty;

        /// <summary>
        /// Tax year/period this configuration applies to
        /// </summary>
        public int FinancialYear { get; set; }

        /// <summary>
        /// Country/Region for which this tax rule applies
        /// </summary>
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// State/Province (if applicable)
        /// </summary>
        public string? Region { get; set; }

        /// <summary>
        /// Standard/default tax rate as percentage (fallback for non-slab based)
        /// </summary>
        public decimal StandardTaxRate { get; set; } = 15; // Default 15%

        /// <summary>
        /// Minimum annual salary subject to income tax
        /// </summary>
        public decimal MinimumTaxableIncome { get; set; } = 0;

        /// <summary>
        /// Tax exemption amount per month
        /// </summary>
        public decimal MonthlyTaxExemption { get; set; } = 0;

        /// <summary>
        /// Whether this configuration uses progressive tax slabs
        /// </summary>
        public bool UseProgressiveTax { get; set; } = true;

        /// <summary>
        /// Whether this configuration is active
        /// </summary>
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Organization Organization { get; set; } = null!;
        public ICollection<TaxSlab> TaxSlabs { get; set; } = new List<TaxSlab>();
    }
}
