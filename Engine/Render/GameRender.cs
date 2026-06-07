using Spectre.Console;

public class GameRender
{
    private GameplayLogPrinter _gameplayLogPrinter;
    private UnitsPrinter _unitsPrinter;
    private StatsPrinter _statsPrinter;
    private VitalsPrinter _vitalsPrinter;
    private SkillsPrinter _skillsPrinter;
    private SkillMenu _skillMenu;

    private Layout _layout;
    private bool _destroy = false;

    public GameRender(GameplayLogPrinter gameplayLogPrinter, UnitsPrinter unitsPrinter, StatsPrinter statsPrinter, VitalsPrinter vitalsPrinter, SkillsPrinter skillsPrinter, SkillMenu skillMenu)
    {
        _gameplayLogPrinter = gameplayLogPrinter;
        _unitsPrinter = unitsPrinter;
        _statsPrinter = statsPrinter;
        _vitalsPrinter = vitalsPrinter;
        _skillsPrinter = skillsPrinter;
        _skillMenu = skillMenu;

        Program.LevelHasFinished += StopRender;

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
                       new Layout("Vitals").Ratio(3),
                       new Layout("Stats").Ratio(1)
                   )
           );
        _layout["Battle"]["Turn"]["Misc"].Update(new Panel("Turn order").Expand());
    }

    private void StopRender(BattleState state)
    {
        _gameplayLogPrinter.PrintWinMessage(state);
        _unitsPrinter.ResetSelect();
        _statsPrinter.Reset();
        _vitalsPrinter.Reset();
        _skillsPrinter.Reset();

        _destroy = true;
    }

    private void StartRender()
    {
        AnsiConsole.Live(_layout)
            .Start(ctx =>
            {
                ctx.Refresh();
                _gameplayLogPrinter.Initialize(ctx, _layout);
                _unitsPrinter.Initialize(ctx, _layout);
                _statsPrinter.Initialize(ctx, _layout);
                _vitalsPrinter.Initialize(ctx, _layout);
                _skillsPrinter.Initialize(ctx, _layout);
                _skillMenu.Initialize(ctx, _layout);
                while (!_destroy) Thread.Sleep(1000);
            });
    }
}
