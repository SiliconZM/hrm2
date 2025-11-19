namespace HRManagement.Models.Entities
{
    /// <summary>
    /// Bridge entity for employee-skill relationship
    /// Tracks skills and proficiency levels for employees
    /// </summary>
    public class EmployeeSkill
    {
        public long EmployeeSkillId { get; set; }
        public long EmployeeId { get; set; }
        public long SkillId { get; set; }

        /// <summary>
        /// Proficiency level: Beginner, Intermediate, Advanced, Expert
        /// </summary>
        public string ProficiencyLevel { get; set; } = "Beginner";

        /// <summary>
        /// Years of experience with this skill
        /// </summary>
        public int? YearsOfExperience { get; set; }

        /// <summary>
        /// Whether this is a primary skill for the role
        /// </summary>
        public bool IsPrimarySkill { get; set; }

        public string? Notes { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Employee? Employee { get; set; }
        public virtual Skill? Skill { get; set; }
    }
}
