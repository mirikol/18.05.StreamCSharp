
public class SkipSkill : ISkill, ISelfSkill
{
    public SkillMenu Menu => _menu;
    public string Name => _name;
    public string Description => _description;

    public Unit Origin => throw new NotImplementedException();

    public IReadOnlyList<Unit> Targets => throw new NotImplementedException();

    private SkillMenu _menu;
    private string _name;
    private string _description;

    public SkipSkill(string name, string description)
    {
        _name = name;
        _description = description;
    }

    public void Initialize(SkillMenu menu, Unit origin, IEnumerable<Unit> targets)
    {
        _menu = menu;
    }

    public bool TryExecute(GameplayLogPrinter printer)
    {
        _menu.Update("Skip", new string[] {"Хотите пропустить ход?"});
        if (!_menu.TryGetChoice(out int selectedIndex))
        {
            return false;
        }

        printer.Print(new LogContext("Пропускаем ход"));
        return true;
    }
}