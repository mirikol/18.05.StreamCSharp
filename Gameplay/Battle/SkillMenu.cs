using Spectre.Console;

public class SkillMenu
{
    private string _title;
    private string[] _selections;
    private int _selectedIndex;

    private LiveDisplayContext _displayContext;
    private Layout _layout;

    public void Initialize(LiveDisplayContext displayContext, Layout layout)
    {
        _displayContext = displayContext;
        _layout = layout;
    }

    public void Update(string title, string[] selections)
    {
        _title = title;
        _selections = new string[selections.Length];
        selections.CopyTo(_selections, 0);
        _selectedIndex = 0;
    }

    public bool TryGetChoice(out int selectedIndex)
    {
        Print();
        ConsoleKeyInfo keyInfo;

        do
        {
            keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.W)
            {
                SelectUp();
            }
            if (keyInfo.Key == ConsoleKey.S)
            {
                SelectDown();
            }
        } while (keyInfo.Key != ConsoleKey.Enter && keyInfo.Key != ConsoleKey.Escape);

        if (keyInfo.Key == ConsoleKey.Escape)
        {
            selectedIndex = -1;
            return false;
        }

        selectedIndex = _selectedIndex;
        return true;
    }

    private void Print()
    {
        Table table = new Table().AddColumn("").AddColumn("").Border(TableBorder.None);

        for (int i = 0; i < _selections.Length; i++)
        {
            if (i == _selectedIndex)
            {
                table.AddRow(_selections[i], "<<<");
            }
            else
            {
                table.AddRow(_selections[i], "");
            }
        }

        Panel panel = new Panel(table).Header(_title).Expand();
        _layout["Battle"]["Turn"]["Skill"].Update(panel);
        _displayContext.Refresh();
    }

    private void SelectUp()
    {
        _selectedIndex = Math.Max(_selectedIndex - 1, 0);
        Print();
    }

    private void SelectDown()
    {
        _selectedIndex = Math.Min(_selectedIndex + 1, _selections.Length - 1);
        Print();
    }
}