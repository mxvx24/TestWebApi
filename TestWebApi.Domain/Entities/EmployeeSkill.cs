namespace TestWebApi.Domain.Entities
{
    /// <summary>
    /// The employee skill.
    /// </summary>
    public class EmployeeSkill
    {
        /// <summary>
        /// Gets or sets the skill id.
        /// </summary>
        public int SkillId { get; set; }

        /// <summary>
        /// Gets or sets the employee id.
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the years of experience.
        /// </summary>
        public int YearsOfExperience { get; set; } = 0;

        /// <summary>
        /// Gets or sets the skill.
        /// </summary>
        public Skill Skill { get; set; }

        /// <summary>
        /// Gets or sets the employee.
        /// </summary>
        public Employee Employee { get; set; }
    }
}
