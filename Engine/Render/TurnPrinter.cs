using Spectre.Console;

public class TurnPrinter : IPrinter
{
    private LiveDisplayContext _displayContext;
    private Layout _layout;

    public void Initialize(LiveDisplayContext context, Layout layout)
    {
        _displayContext = context;
        _layout = layout;
        Reset();
    }

    public void Reset()
    {
        var panel = new Panel("").Header("Turn order").BorderColor(Color.White).Expand();
        _layout["Battle"]["Turn"]["Misc"].Update(panel);
        _displayContext.Refresh();
    }

    public void Print(UnitsContext context)
    {
        var table = new Table().AddColumn("").Border(TableBorder.None);
        var panel = new Panel(table).Header("Turn order").BorderColor(Color.White).Expand();

        foreach (var unitTurn in context.Units)
        {
            string unitName = unitTurn.Unit.Model.Name;

            if (unitTurn.Order == context.UnitTurn.Order)
            {
                unitName = $"{unitName} <<<";
            }

            if (unitTurn.IsAlly)
            {
                if (unitTurn.Unit.IsAlive)
                {
                    unitName = $"[green]{unitName}[/]";
                }
                else
                {
                    unitName = $"[darkgreen]{unitName}[/]";
                }
            }
            else
            {
                if (unitTurn.Unit.IsAlive)
                {
                    unitName = $"[red]{unitName}[/]";
                }
                else
                {
                    unitName = $"[darkred]{unitName}[/]";
                }
            }

            table.AddRow(unitName);
        }

        _layout["Battle"]["Turn"]["Misc"].Update(panel);
        _displayContext.Refresh();
    }
}