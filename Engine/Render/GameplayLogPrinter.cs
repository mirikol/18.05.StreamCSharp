using Spectre.Console;

public class GameplayLogPrinter : IPrinter
{
    private LiveDisplayContext _displayContext;
    private Layout _layout;
    private Panel _outputPanel;
    private Table _outputTable;
    private int _rows;

    public void Initialize(LiveDisplayContext context, Layout layout, Panel outputPanel, Table outputTable)
    {
        _displayContext = context;
        _layout = layout;
        _outputPanel = outputPanel;
        _outputTable = outputTable;
        _rows = 0;
    }

    public void Print(PrinterContext context)
    {
        context.Text = context.Text.Replace("[", "[[").Replace("]", "]]");

        _rows++;
        if (_rows > 22)
        {
            _outputTable.RemoveRow(0);
        }

        _outputTable.AddRow(context.Text, DateTime.Now.ToString());

        _layout["Battle"]["Output"].Update(_outputPanel);
        _displayContext.Refresh();
    }
}

public class UnitsPrinter : IPrinter
{
    public void Print(PrinterContext context)
    {
        throw new NotImplementedException();
    }
}