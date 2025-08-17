public class Arena
{
    private List<Unit> _playerUnits = new List<Unit>();
    private List<Unit> _enemyUnits = new List<Unit>();

    private TurnController _turnController;

    private UnitModel[] _models = new UnitModel[3] {new UnitModel("Тимур", 1000, 1, 40, 10, 1),
        new UnitModel("Григорий", 50, 3, 10, 50, 10),
        new UnitModel("Михаил :)", 500, 2, 20, 1, 100),
        };

    public void Start()
    {
        var goldenHelmet = new Helmet(999, 0);
        var goldenVest = new Vest(999, 0);
        var goldenGlove = new Glove(999, 0);
        var goldenGreave = new Greave(999, 0);

        var commonSword = new Sword(0, 0, 100);
        var enemySword = new Sword(0, 0, 100);

        var player = new Unit(_models[0]);
        UnitUtility.EquipUnit(player, commonSword, BodyPartName.RightArm);
        //UnitUtility.EquipUnit(player, goldenHelmet, BodyPartName.Head);
        //UnitUtility.EquipUnit(player, goldenVest, BodyPartName.Body);
        //UnitUtility.EquipUnit(player, goldenGlove, BodyPartName.RightArm);
        //UnitUtility.EquipUnit(player, goldenGlove, BodyPartName.LeftArm);
        //UnitUtility.EquipUnit(player, goldenGreave, BodyPartName.RightLeg);
        //UnitUtility.EquipUnit(player, goldenGreave, BodyPartName.LeftLeg);

        var gregory = new Unit(_models[1]);
        UnitUtility.EquipUnit(gregory, enemySword, BodyPartName.RightArm);

        var michael = new Unit(_models[2]);
        UnitUtility.EquipUnit(michael, enemySword, BodyPartName.RightArm);

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