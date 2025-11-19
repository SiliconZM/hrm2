namespace HRManagement.Models.DTOs
{
    public class SkillDto
    {
        public long SkillId { get; set; }
        public long OrganizationId { get; set; }

        public string SkillName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateSkillRequest
    {
        public long OrganizationId { get; set; }
        public string SkillName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
    }

    public class UpdateSkillRequest
    {
        public string SkillName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool IsActive { get; set; }
    }

    public class EmployeeSkillDto
    {
        public long EmployeeSkillId { get; set; }
        public long EmployeeId { get; set; }
        public long SkillId { get; set; }

        public string EmployeeName { get; set; }
        public string SkillName { get; set; }

        public string ProficiencyLevel { get; set; }
        public int? YearsOfExperience { get; set; }
        public bool IsPrimarySkill { get; set; }
        public string Notes { get; set; }

        public DateTime LastUpdated { get; set; }
    }

    public class CreateEmployeeSkillRequest
    {
        public long EmployeeId { get; set; }
        public long SkillId { get; set; }
        public string ProficiencyLevel { get; set; } = "Beginner";
        public int? YearsOfExperience { get; set; }
        public bool IsPrimarySkill { get; set; }
        public string Notes { get; set; }
    }

    public class UpdateEmployeeSkillRequest
    {
        public string ProficiencyLevel { get; set; }
        public int? YearsOfExperience { get; set; }
        public bool IsPrimarySkill { get; set; }
        public string Notes { get; set; }
    }
}
