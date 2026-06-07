[JsonInterfaceConverter]
public interface ISkill
{
    public SkillMenu Menu { get; }
    public string Name { get; }
    public string Description { get; }
    public Unit Origin { get; }
    public IReadOnlyList<Unit> Targets { get; }
    public void Initialize(SkillMenu menu, Unit origin, IEnumerable<Unit> targets);
    public bool TryExecute(GameplayLogPrinter printer);
}

public interface ISelfSkill
{
}

public interface IAttackSkill
{
}