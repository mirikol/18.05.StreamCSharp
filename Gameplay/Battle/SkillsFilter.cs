public static class SkillsFilter
{
    public static void Exclude(ISkill[] skills, Type[] types)
    {
        List<ISkill> result = new List<ISkill>();
        foreach (ISkill skill in skills)
        {
            bool isExcluded = false;
            foreach (var type in types)
            {
                if (type.IsAssignableFrom(skill.GetType()))
                {
                    isExcluded = true;
                    break;
                }
            }

            if (!isExcluded)
            {
                result.Add(skill);
            }
        }

        skills = result.ToArray();
    }

    public static void Include(ISkill[] skills, Type[] types)
    {
        List<ISkill> result = new List<ISkill>();
        foreach (ISkill skill in skills)
        {
            bool isIncluded = false;
            foreach (var type in types)
            {
                if (type.IsAssignableFrom(skill.GetType()))
                {
                    isIncluded = true;
                    break;
                }
            }

            if (isIncluded)
            {
                result.Add(skill);
            }
        }

        skills = result.ToArray();
    }
}
