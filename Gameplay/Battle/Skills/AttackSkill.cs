public class AttackSkill : ISkill
{
    private readonly string[] _bodyPartNames = Enum.GetNames(typeof(BodyPartName));
    public SkillMenu Menu => _menu;
    public Unit Origin => _origin;
    public IReadOnlyList<Unit> Targets => _targets;
    public string Name => _name;
    public string Description => _description;

    private SkillMenu _menu;
    private Unit _origin;
    private List<Unit> _targets;
    private string _name;
    private string _description;

    public AttackSkill(string name, string description)
    {
        _name = name;
        _description = description;
    }

    public void Initialize(SkillMenu menu, Unit origin, IEnumerable<Unit> targets)
    {
        _menu = menu;
        _origin = origin;
        _targets = new List<Unit>(targets);
    }

    public bool TryExecute(GameplayLogPrinter printer)
    {
        _menu.Update("Attack", _bodyPartNames);
        if (!_menu.TryGetChoice(out int selectedIndex))
        {
            printer.Print(new LogContext("Умение было прервано."));
            return false;
        }    
        
        BodyPartName selectedBodyPart = (BodyPartName)Enum.Parse(typeof(BodyPartName), _bodyPartNames[selectedIndex]);
        Random random = new Random();
        var damage = UnitUtility.GetFlatDamage(_origin.BaseDamage, _origin, _targets[0]);
        LogContext context = new LogContext($"{_origin.Model.Name} атаковал {_targets[0].Model.Name} с уроном {damage} в часть тела: {selectedBodyPart.ToString()}", ConsoleColor.DarkRed);
        printer.Print(context);

        if (0.7 > random.NextDouble())
        {
            _targets[0].BodyParts[selectedBodyPart].TakeDamage(damage);
        }
        else
        {
            context.Text = "Промазал\n";
            context.ForegroundColor = ConsoleColor.DarkRed;
            printer.Print(context);
        }

        return true;
    }
}