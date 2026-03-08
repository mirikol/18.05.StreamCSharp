public class Arena
{
    private GameplayLogPrinter _gameplayLogPrinter;
    private UnitsPrinter _unitsPrinter;

    private TurnController _turnController;
    private ArenaModel _model;

    public Arena(GameplayLogPrinter gameplayLogPrinter, UnitsPrinter unitsPrinter, ArenaModel model)
    {
        _gameplayLogPrinter = gameplayLogPrinter;
        _unitsPrinter = unitsPrinter;

        _model = model;
        _turnController = new TurnController(_gameplayLogPrinter, _unitsPrinter, new TurnPrinter(_gameplayLogPrinter, _unitsPrinter), _model.PlayerUnits, _model.EnemyUnits);
    }

    public void Start()
    {
        while (true)
        {
            var nextTurn = _turnController.GetNextTurn();

            var battleState = GetBattleState();
            if (battleState != BattleState.Battle)
            {
                WinLoseMessage(battleState);
                return;
            }

            _turnController.Turn(nextTurn);
        }
    }

    private void WinLoseMessage(BattleState battleState)
    {
        if (battleState == BattleState.PlayerWins)
        {
            _gameplayLogPrinter.Print(new LogContext("Player win", ConsoleColor.Green));
        }
        else if (battleState == BattleState.EnemyWins)
        {
            _gameplayLogPrinter.Print(new LogContext("Enemy win", ConsoleColor.DarkRed));
        }
    }

    private BattleState GetBattleState()
    {
        if (_turnController.HasPlayerUnit && _turnController.HasEnemyUnit)
        {
            return BattleState.Battle;
        }
        else if (_turnController.HasPlayerUnit)
        {
            return BattleState.PlayerWins;
        }
        else
        {
            return BattleState.EnemyWins;
        }
    }
}