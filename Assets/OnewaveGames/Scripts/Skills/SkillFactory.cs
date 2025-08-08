public static class SkillFactory
{
    public static Skill CreateSkillBySkillTag(ESkillTag eSkillTag)
    {
        switch (eSkillTag)
        {
            case ESkillTag.Skill_SelfTarget:
            default:
            {
                return new Skill_SelfTarget();
            }
            case ESkillTag.Skill_Grab:
            {
                return new Skill_Grab();
            }
        }
    }
}