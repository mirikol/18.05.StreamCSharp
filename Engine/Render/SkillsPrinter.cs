using Spectre.Console;

public class SkillsPrinter : IPrinter
{
    private LiveDisplayContext _displayContext;
    private Layout _layout;
    private int _selectedSkillIndex = 0;
    private SkillsContext _context;

    public void Initialize(LiveDisplayContext context, Layout layout)
    {
        _displayContext = context;
        _layout = layout;
        Reset();
    }

    public void Reset()
    {
        _selectedSkillIndex = 0;
        Text text = new Text("Select the unit.", Color.Gray);
        Panel panel = new Panel(text).Header("Skills").Expand();
        _layout["Battle"]["Turn"]["Skill"].Update(panel);
        _displayContext.Refresh();
    }

    public ISkill GetSkillFromSelected()
    {
        return _context.Skills[_selectedSkillIndex];
    }

    public void Print(SkillsContext context)
    {
        _context = context;
        Table table = new Table().AddColumn("").AddColumn("").Border(TableBorder.None);

        for (int i = 0; i < context.Skills.Length; i++)
        {
            if (i == _selectedSkillIndex)
            {
                table.AddRow(context.Skills[i].Name, "<<<");
            }
            else
            {
                table.AddRow(context.Skills[i].Name, "");
            }
        }

        Panel panel = new Panel(table).Header("Skills").Expand();
        _layout["Battle"]["Turn"]["Skill"].Update(panel);
        _displayContext.Refresh();
    }

    public void SelectUp()
    {
        _selectedSkillIndex = Math.Max(_selectedSkillIndex - 1, 0);
        Print(_context);
    }

    public void SelectDown()
    {
        _selectedSkillIndex = Math.Min(_selectedSkillIndex + 1, _context.Skills.Length - 1);
        Print(_context);
    }
}