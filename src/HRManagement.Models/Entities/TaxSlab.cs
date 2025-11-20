namespace HRManagement.Models.Entities
{
    /// <summary>
    /// Represents a tax slab/bracket for progressive taxation
    /// Example: Income from 0-50,000 taxed at 10%, 50,001-100,000 at 15%, etc.
    /// </summary>
    public class TaxSlab
    {
        public long TaxSlabId { get; set; }
        public long TaxConfigurationId { get; set; }

        /// <summary>
        /// Lower limit of income for this slab
        /// </summary>
        public decimal FromAmount { get; set; }

        /// <summary>
        /// Upper limit of income for this slab
        /// </summary>
        public decimal ToAmount { get; set; }

        /// <summary>
        /// Tax rate as percentage for this slab
        /// </summary>
        public decimal TaxRate { get; set; }

        /// <summary>
        /// Display order for the slab
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Whether this slab is active
        /// </summary>
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public TaxConfiguration TaxConfiguration { get; set; } = null!;
    }
}
