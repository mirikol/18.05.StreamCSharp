public class Arena
{
    private List<Unit> _playerUnits = new List<Unit>();
    private List<Unit> _enemyUnits = new List<Unit>();

    private TurnController _turnController;

    private UnitModel[] _models = new UnitModel[3] {SaveLoad<UnitModel>.Load("Timur"),
        SaveLoad<UnitModel>.Load("Gregory"),
        SaveLoad<UnitModel>.Load("Michael"),
        };

    public void Start()
    {
        var player = new Unit(_models[0]);
        UnitUtility.EquipUnit(player, SaveLoad<Sword>.Load("Golden sword"), BodyPartName.RightArm);
        UnitUtility.EquipUnit(player, SaveLoad<Sword>.Load("Golden sword"), BodyPartName.LeftArm);
        SaveLoad<Unit>.Save(player, "Player");

        var gregory = new Unit(_models[1]);
        UnitUtility.EquipUnit(gregory, SaveLoad<Sword>.Load("Common sword"), BodyPartName.RightArm);

        var michael = new Unit(_models[2]);
        UnitUtility.EquipUnit(michael, SaveLoad<Sword>.Load("Common sword"), BodyPartName.RightArm);

        _playerUnits.Add(player);
        _enemyUnits.Add(gregory);
        _enemyUnits.Add(michael);

        _turnController = new TurnController(_playerUnits, _enemyUnits);

        SubscribeToDeleteOnDeath(_playerUnits);
        SubscribeToDeleteOnDeath(_enemyUnits);

        while (true)
        {
            foreach (var unit in _turnController.TurnCycle)
            {
                var battleState = GetBattleState();

                if (battleState != BattleState.Battle)
                {
                    WinLoseMessage(_playerUnits, battleState);
                    return;
                }

                if (_playerUnits.Contains(unit))
                {
                    _turnController.Turn(unit, _enemyUnits, true);
                }
                else
                {
                    _turnController.Turn(unit, _playerUnits, false);
                }
            }
        }
    }

    private void WinLoseMessage(List<Unit> playerUnits, BattleState battleState)
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

    private void UpdateDiedUnits(List<Unit> aliveUnits, Unit diedUnit)
    {
        aliveUnits.Remove(diedUnit);
    }

    private void SubscribeToDeleteOnDeath(List<Unit> units)
    {
        foreach (var unit in units)
            unit.HealthBelowZero += () =>
            {
                UpdateDiedUnits(_playerUnits, unit);
                UpdateDiedUnits(_enemyUnits, unit);
            };
    }

    private BattleState GetBattleState()
    {
        bool hasAlivePlayerUnits = _playerUnits.Count > 0;
        bool hasAliveEnemyUnits = _enemyUnits.Count > 0;

        if (hasAlivePlayerUnits && hasAliveEnemyUnits)
        {
            return BattleState.Battle;
        }
        else if (hasAlivePlayerUnits)
        {
            return BattleState.PlayerWins;
        }
        else
        {
            return BattleState.EnemyWins;
        }
    }
}