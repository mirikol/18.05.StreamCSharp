using Spectre.Console;
using System.ComponentModel;

public class GameRender
{
    private GameplayLogPrinter _gameplayLogPrinter;
    private UnitsPrinter _unitsPrinter;

    private Layout _layout;

    public GameRender(GameplayLogPrinter gameplayLogPrinter, UnitsPrinter unitsPrinter)
    {
        _gameplayLogPrinter = gameplayLogPrinter;
        _unitsPrinter = unitsPrinter;

        InitializeLayout();
        StartRender();
    }

    private void InitializeLayout()
    {
        _layout = new Layout("Root")
           .SplitColumns(
               new Layout("Battle").Ratio(3)
                   .SplitRows(
                       new Layout("Turn").Ratio(2)
                           .SplitColumns(
                               new Layout("Units").Ratio(2),
                               new Layout("Skill").Ratio(1),
                               new Layout("Misc").Ratio(1)
                           ),
                       new Layout("Output").Ratio(2)
                   ),
               new Layout("Info").Ratio(1)
                   .SplitRows(
                       new Layout("Equipment").Ratio(3),
                       new Layout("Stats").Ratio(1)
                   )
           );
        _layout["Battle"]["Turn"]["Misc"].Update(new Panel("").Expand().BorderColor(Color.Black));
    }

    private void StartRender()
    {
        AnsiConsole.Live(_layout)
            .Start(ctx =>
            {
                ctx.Refresh();
                _gameplayLogPrinter.Initialize(ctx, _layout);
                _unitsPrinter.Initialize(ctx, _layout);
                while (true) Thread.Sleep(1000);
            });
    }
}
