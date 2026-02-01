using Spectre.Console;

public class GameRender
{
    private GameplayLogPrinter _gameplayLogPrinter;

    private Layout _layout;

    private Table _outputTable;
    private Panel _outputPanel;

    private Table _unitsTable;
    private Panel _unitsPanel;

    public GameRender(GameplayLogPrinter gameplayLogPrinter)
    {
        _gameplayLogPrinter = gameplayLogPrinter;

        InitializeLayout();
        StartRender();
    }

    private void InitializeLayout()
    {
        _layout = new Layout("Root")
           .SplitColumns(
               new Layout("Battle").Ratio(4)
                   .SplitRows(
                       new Layout("Turn").Ratio(2)
                           .SplitColumns(
                               new Layout("Units").Ratio(1)
                                   .SplitColumns(
                                       new Layout("a1").Ratio(1)
                                           .SplitRows(
                                               new Layout("b1").Ratio(2),
                                               new Layout("b2").Ratio(2),
                                               new Layout("b3").Ratio(1),
                                               new Layout("b4").Ratio(1),
                                               new Layout("b5").Ratio(2),
                                               new Layout("b6").Ratio(2)
                                           ),
                                       new Layout("a2").Ratio(1)
                                           .SplitRows(
                                               new Layout("b1").Ratio(2),
                                               new Layout("b2").Ratio(2),
                                               new Layout("b3").Ratio(1),
                                               new Layout("b4").Ratio(1),
                                               new Layout("b5").Ratio(2),
                                               new Layout("b6").Ratio(2)
                                           ),
                                       new Layout("a3").Ratio(1)
                                           .SplitRows(
                                               new Layout("b1").Ratio(2),
                                               new Layout("b2").Ratio(2),
                                               new Layout("b3").Ratio(1),
                                               new Layout("b4").Ratio(1),
                                               new Layout("b5").Ratio(2),
                                               new Layout("b6").Ratio(2)
                                           )
                                   ),
                               new Layout("Skill").Ratio(1),
                               new Layout("Misc").Ratio(2)
                           ),
                       new Layout("Output").Ratio(2)
                   ),
               new Layout("Info").Ratio(2)
                   .SplitRows(
                       new Layout("Equipment").Ratio(3),
                       new Layout("Stats").Ratio(1)
                   )
           );
        _layout["Battle"]["Turn"]["Misc"].Update(new Panel("").Expand().BorderColor(Color.Black));

        _outputTable = new Table().AddColumn("Text").AddColumn("Time").Border(TableBorder.None);
        _outputPanel = new Panel(_outputTable).Header("Printer").BorderColor(Color.White).Expand();
        _layout["Battle"]["Output"].Update(_outputPanel);

        for (int i = 1; i <= 3; i++)
        {
            for (int j = 1; j <= 6; j++)
            {
                var textContent = $"Unit_{i}_{j}";
                if (j == 3 || j == 4)
                {
                    textContent = "";
                }

                Text text = new Text(textContent);
                Panel panel;

                if (j == 3 || j == 4)
                {
                    panel = new Panel(textContent).BorderColor(Color.Black);
                }
                else
                {
                    panel = new Panel(textContent);
                }

                _layout["Battle"]["Turn"]["Units"]["a" + i]["b" + j].Update(panel);
            }
        }

        _layout["Battle"]["Turn"]["Units"].Update(new Panel("Units").BorderColor(Color.White));
    }

    private void StartRender()
    {
        AnsiConsole.Live(_layout)
            .Start(ctx =>
            {
                ctx.Refresh();
                _gameplayLogPrinter.Initialize(ctx, _layout, _outputPanel, _outputTable);
                while (true) Thread.Sleep(1000);
            });
    }
}
