using Spectre.Console;

public class GameplayLogPrinter : IPrinter
{
    private LiveDisplayContext _displayContext;
    private Layout _layout;
    private Panel _outputPanel;
    private Table _outputTable;
    private int _rows;

    public void Initialize(LiveDisplayContext context, Layout layout)
    {
        _displayContext = context;
        _layout = layout;
        _rows = 0;

        _outputTable = new Table().AddColumn("Text").AddColumn("Time").Border(TableBorder.None);
        _outputPanel = new Panel(_outputTable).Header("Printer").BorderColor(Color.White).Expand();
        _layout["Battle"]["Output"].Update(_outputPanel);
    }

    public void Print(LogContext context)
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