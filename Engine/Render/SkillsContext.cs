public struct SkillsContext
{
    public ISkill[] Skills;
    public SkillsContext(ISkill[] skills)
    {
        Skills = new ISkill[skills.Length];
        skills.CopyTo(Skills, 0);
    }
}