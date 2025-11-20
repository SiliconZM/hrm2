namespace HRManagement.Models.DTOs
{
    public class TaxConfigurationDto
    {
        public long TaxConfigurationId { get; set; }
        public long OrganizationId { get; set; }
        public string ConfigurationName { get; set; } = string.Empty;
        public int FinancialYear { get; set; }
        public string Country { get; set; } = string.Empty;
        public string? Region { get; set; }
        public decimal StandardTaxRate { get; set; }
        public decimal MinimumTaxableIncome { get; set; }
        public decimal MonthlyTaxExemption { get; set; }
        public bool UseProgressiveTax { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<TaxSlabDto> TaxSlabs { get; set; } = new();
    }

    public class CreateTaxConfigurationRequest
    {
        public long OrganizationId { get; set; }
        public string ConfigurationName { get; set; } = string.Empty;
        public int FinancialYear { get; set; }
        public string Country { get; set; } = string.Empty;
        public string? Region { get; set; }
        public decimal StandardTaxRate { get; set; } = 15;
        public decimal MinimumTaxableIncome { get; set; } = 0;
        public decimal MonthlyTaxExemption { get; set; } = 0;
        public bool UseProgressiveTax { get; set; } = true;
    }

    public class UpdateTaxConfigurationRequest
    {
        public string ConfigurationName { get; set; } = string.Empty;
        public int FinancialYear { get; set; }
        public string Country { get; set; } = string.Empty;
        public string? Region { get; set; }
        public decimal StandardTaxRate { get; set; }
        public decimal MinimumTaxableIncome { get; set; }
        public decimal MonthlyTaxExemption { get; set; }
        public bool UseProgressiveTax { get; set; }
        public bool IsActive { get; set; }
    }
}
