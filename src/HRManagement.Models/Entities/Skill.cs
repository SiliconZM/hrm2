namespace HRManagement.Models.Entities
{
    /// <summary>
    /// Represents a skill in the organization
    /// </summary>
    public class Skill
    {
        public long SkillId { get; set; }
        public long OrganizationId { get; set; }

        public string SkillName { get; set; } = string.Empty;
        public string? Description { get; set; }

        /// <summary>
        /// Category: Technical, Soft, Management, etc.
        /// </summary>
        public string? Category { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Organization? Organization { get; set; }
        public virtual ICollection<EmployeeSkill> EmployeeSkills { get; set; } = new List<EmployeeSkill>();
    }
}
