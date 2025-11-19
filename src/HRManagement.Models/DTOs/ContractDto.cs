namespace HRManagement.Models.DTOs
{
    public class ContractDto
    {
        public long ContractId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        public string ContractType { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string Terms { get; set; }

        public string Status { get; set; }

        public DateTime? SignedDate { get; set; }
        public string SignedBy { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public bool IsExpired { get; set; }
    }

    public class CreateContractRequest
    {
        public long EmployeeId { get; set; }
        public string ContractType { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Terms { get; set; }
    }

    public class UpdateContractRequest
    {
        public string ContractType { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Terms { get; set; }
        public string Status { get; set; }
    }

    public class SignContractRequest
    {
        public DateTime SignedDate { get; set; }
        public string SignedBy { get; set; }
    }
}
