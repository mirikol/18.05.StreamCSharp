public class TurnController
{
    private const int initiativeToSpeed = 10000;

    public IReadOnlyCollection<UnitTurn> TurnCycle => _turnCycle;
    public bool HasPlayerUnit => _allies.Any(turn => turn.Unit.IsAlive);
    public bool HasEnemyUnit => _enemies.Any(turn => turn.Unit.IsAlive);

    private UnitTurn[] _turnCycle;
    private UnitTurn[] _allies => Array.FindAll(_turnCycle, turn => turn.IsAlly);
    private UnitTurn[] _enemies => Array.FindAll(_turnCycle, turn => !turn.IsAlly);

    private BattleProcessor _battleProcessor;

    public TurnController(TurnPrinter printer, IReadOnlyCollection<Unit> allyUnits, IReadOnlyCollection<Unit> enemyUnits)
    {
        _battleProcessor = new BattleProcessor(printer);

        CreateTurnCycle(allyUnits, enemyUnits);
    }

    public UnitTurn GetNextTurn()
    {
        int startIndex = 0;
        for (int i = 0; i < _turnCycle.Length; i++)
        {
            if (_turnCycle[i].IsAllowed)
            {
                return _turnCycle[i];
            }
        }

        BeginTurnCycle();
        return GetNextTurn();
    }

    public void BeginTurnCycle()
    {
        for (int i = 0; i < _turnCycle.Length; i++)
        {
            _turnCycle[i] = new UnitTurn(_turnCycle[i].Unit, _turnCycle[i].IsAlly, true, i);
        }
    }

    public void Turn(UnitTurn attackerTurn)
    {
        _battleProcessor.Battle(_turnCycle, _enemies, _allies, attackerTurn, UpdateTurnCycle);
    }

    private void CreateTurnCycle(IReadOnlyCollection<Unit> allyUnits, IReadOnlyCollection<Unit> enemyUnits)
    {
        var allUnits = allyUnits.Concat(enemyUnits).ToList();
        var turnOrder = CalculateTurnOrder(allUnits);

        _turnCycle = new UnitTurn[turnOrder.Count];
        for (int i = 0; i < turnOrder.Count; i++)
        {
            bool isAlly = allyUnits.Contains(turnOrder[i].Unit);
            _turnCycle[i] = new UnitTurn(turnOrder[i].Unit, isAlly, true, i);
        }
    }

    private void UpdateTurnCycle()
    {
        var activeUnits = _turnCycle.Select(t => t.Unit).Distinct().ToList();
        var turnOrder = CalculateTurnOrder(activeUnits);

        var tempTurnCycle = new UnitTurn[turnOrder.Count];
        for (int i = 0; i < turnOrder.Count; i++)
        {
            bool isAlly = _allies.Any(x => x.Unit == turnOrder[i].Unit);
            tempTurnCycle[i] = new UnitTurn(turnOrder[i].Unit, isAlly, true, i);
        }

        ApplyTurnRestrictions(tempTurnCycle, activeUnits);
        _turnCycle = tempTurnCycle;
    }

    private List<UnitTurn> CalculateTurnOrder(List<Unit> units)
    {
        var sortedUnits = units.OrderByDescending(unit => unit.Initiative).ToList();
        float minSpeed = sortedUnits.Min(unit => unit.Speed);

        var speedUnits = new List<(Unit Unit, float Speed)>();
        foreach (var unit in units)
        {
            float speed = unit.Initiative * initiativeToSpeed;
            speedUnits.Add((unit, speed));
        }

        foreach (var unit in sortedUnits)
        {
            float speed = unit.Speed / minSpeed;
            while (speed > minSpeed)
            {
                speedUnits.Add((unit, speed));
                speed -= (float)Math.Cbrt(unit.Speed);
            }
        }

        return speedUnits
            .OrderByDescending(x => x.Speed)
            .Select((x, i) => new UnitTurn(x.Unit, false, true, i))
            .ToList();
    }

    private void ApplyTurnRestrictions(UnitTurn[] newTurnCycle, List<Unit> activeUnits)
    {
        foreach (var unit in activeUnits)
        {
            int restrictedTurns = _turnCycle.Count(t => !t.IsAllowed && t.Unit == unit);
            var unitTurns = newTurnCycle.Where(t => t.Unit == unit).Take(restrictedTurns);

            foreach (var turn in unitTurns)
            {
                int index = Array.IndexOf(newTurnCycle, turn);
                newTurnCycle[index] = new UnitTurn(turn.Unit, turn.IsAlly, false, index);
            }
        }
    }
}