
public class NullSkill : ISkill
{
    public string Name => _name;
    public string Description => _description;

    public Unit Origin => throw new NotImplementedException();

    public IReadOnlyList<Unit> Targets => throw new NotImplementedException();

    private string _name;
    private string _description;

    public NullSkill(string name, string description)
    {
        _name = name;
        _description = description;
    }

    public void Initialize(Unit origin, IEnumerable<Unit> targets)
    {
        
    }

    public void Execute(GameplayLogPrinter printer)
    {
        printer.Print(new LogContext("Nothing to do"));
    }
}