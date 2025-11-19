namespace HRManagement.Models.Entities
{
    /// <summary>
    /// Represents an employment contract or agreement
    /// </summary>
    public class Contract
    {
        public long ContractId { get; set; }
        public long EmployeeId { get; set; }

        /// <summary>
        /// Type: Employment Contract, NDA, Offer Letter, Severance, etc.
        /// </summary>
        public string ContractType { get; set; } = string.Empty;

        public string? FileName { get; set; }
        public string? FileUrl { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string? Terms { get; set; }

        /// <summary>
        /// Status: Draft, Signed, Expired, Renewed, Terminated
        /// </summary>
        public string Status { get; set; } = "Draft";

        public DateTime? SignedDate { get; set; }
        public string? SignedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Employee? Employee { get; set; }

        // Computed property
        public bool IsExpired => EndDate.HasValue && EndDate.Value < DateTime.Today;
    }
}
