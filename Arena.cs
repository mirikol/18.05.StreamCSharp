public class Arena
{
    private TurnController _turnController;

    public Arena()
    {
        var playerUnits = new List<Unit>();
        var enemyUnits = new List<Unit>();
        playerUnits.Add(UnitUtility.CreateUnit(SaveLoad<UnitSave>.Load("Player")));
        enemyUnits.Add(UnitUtility.CreateUnit(SaveLoad<UnitSave>.Load("Gregory")));
        enemyUnits.Add(UnitUtility.CreateUnit(SaveLoad<UnitSave>.Load("Michael")));

        _turnController = new TurnController(playerUnits, enemyUnits);
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
            Printer.Print("Player win", ConsoleColor.Green);
        }
        else if (battleState == BattleState.EnemyWins)
        {
            Printer.Print("Enemy win", ConsoleColor.DarkRed);
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