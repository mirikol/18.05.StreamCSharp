[JsonInterfaceConverter]
public interface ISkill
{
    public string Name { get; }
    public string Description { get; }
    public Unit Origin { get; }
    public IReadOnlyList<Unit> Targets { get; }
    public void Initialize(Unit origin, IEnumerable<Unit> targets);
    public void Execute(GameplayLogPrinter printer);
}