public class Arena
{
    public event Action<Unit> UnitHasDied;

    private List<Unit> _playerUnits = new List<Unit>();
    private List<Unit> _enemyUnits = new List<Unit>();

    private List<Unit> _diedUnits = new List<Unit>();

    private TurnController _turnController;

    private UnitModel[] _models = new UnitModel[3] {new UnitModel("Тимур", 10000, 1, 40, 500, 0),
        new UnitModel("Григорий", 50, 3, 10, 150, 0),
        new UnitModel("Михаил :)", 5000, 2, 20, 1000, 0),
        };

    public void Start()
    {
        _playerUnits.Add(new Unit(_models[0]));
        _enemyUnits.Add(new Unit(_models[1]));
        _enemyUnits.Add(new Unit(_models[2]));

        _turnController = new TurnController(this, _playerUnits, _enemyUnits);

        SubscribeToDeleteOnDeath(_playerUnits, _diedUnits);
        SubscribeToDeleteOnDeath(_enemyUnits, _diedUnits);

        while (true)
        {
            foreach (var unit in _turnController.TurnCycle)
            {
                if (!IsBattle())
                {
                    WinLoseMessage(_playerUnits);
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

            UpdateDiedUnits(_playerUnits, _diedUnits);
            UpdateDiedUnits(_enemyUnits, _diedUnits);
            _diedUnits.Clear();
        }
    }

    private void WinLoseMessage(List<Unit> playerUnits)
    {
        if (TryGetAlive(playerUnits, out Unit alivePlayerUnit))
        {
            Printer.Print("Player win", ConsoleColor.Green);
        }
        else
        {
            Printer.Print("Enemy win", ConsoleColor.DarkRed);
        }
    }

    private void UpdateDiedUnits(List<Unit> aliveUnits, List<Unit> diedUnits)
    {
        foreach (var unit in diedUnits)
        {
            aliveUnits.Remove(unit);
            UnitHasDied?.Invoke(unit);
        }
    }

    private void SubscribeToDeleteOnDeath(List<Unit> units, List<Unit> unitsToDelete)
    {
        foreach (var unit in units)
            unit.HasDied += () => unitsToDelete.Add(unit);
    }

    private bool TryGetAlive(List<Unit> units, out Unit aliveUnit)
    {
        aliveUnit = units.Find(unit => unit.IsAlive);
        return aliveUnit != null;
    }

    private bool IsBattle()
    {
        return
            TryGetAlive(_playerUnits, out Unit alivePlayerUnit)
            && TryGetAlive(_enemyUnits, out Unit aliveEnemyUnit);
    }
}