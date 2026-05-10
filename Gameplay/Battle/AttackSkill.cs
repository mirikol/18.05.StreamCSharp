public class AttackSkill : ISkill
{
    public Unit Origin => _origin;
    public IReadOnlyList<Unit> Targets => _targets;
    public string Name => _name;
    public string Description => _description;

    private Unit _origin;
    private List<Unit> _targets;
    private string _name;
    private string _description;

    public AttackSkill(string name, string description)
    {
        _name = name;
        _description = description;
    }

    public void Initialize(Unit origin, IEnumerable<Unit> targets)
    {
        _origin = origin;
        _targets = new List<Unit>(targets);
    }

    public void Execute(GameplayLogPrinter printer)
    {
        Random random = new Random();
        var damage = UnitUtility.GetFlatDamage(_origin.BaseDamage, _origin, _targets[0]);
        LogContext context = new LogContext($"{_origin.Model.Name} атаковал {_targets[0].Model.Name} с уроном {damage}.", ConsoleColor.DarkRed);
        printer.Print(context);

        if (0.7 > random.NextDouble())
        {
            _targets[0].BodyParts[BodyPartName.Body].TakeDamage(damage);
        }
        else
        {
            context.Text = "Промазал\n";
            context.ForegroundColor = ConsoleColor.DarkRed;
            printer.Print(context);
        }
    }
}