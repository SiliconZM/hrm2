namespace HRManagement.Models.DTOs
{
    public class TaxSlabDto
    {
        public long TaxSlabId { get; set; }
        public long TaxConfigurationId { get; set; }
        public decimal FromAmount { get; set; }
        public decimal ToAmount { get; set; }
        public decimal TaxRate { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateTaxSlabRequest
    {
        public long TaxConfigurationId { get; set; }
        public decimal FromAmount { get; set; }
        public decimal ToAmount { get; set; }
        public decimal TaxRate { get; set; }
        public int DisplayOrder { get; set; } = 0;
    }

    public class UpdateTaxSlabRequest
    {
        public decimal FromAmount { get; set; }
        public decimal ToAmount { get; set; }
        public decimal TaxRate { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
