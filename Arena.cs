using System;
using System.Linq;

public class Arena
{
    private List<Unit> _playerUnits = new List<Unit>();
    private List<Unit> _enemyUnits = new List<Unit>();

    private List<Unit> _turnCycle = new List<Unit>();
    private List<Unit> _diedUnits = new List<Unit>();

    private UnitModel[] _models = new UnitModel[3] {new UnitModel("Тимур", 10000, 3, 1),
        new UnitModel("Григорий", 50, 2, 8),
        new UnitModel("Михаил :)", 5000, 1, 5),
        };

    private int _turnIndex = 0;

    public void Start()
    {
        _playerUnits.Add(new Unit(_models[0]));
        _enemyUnits.Add(new Unit(_models[1]));
        _enemyUnits.Add(new Unit(_models[2]));

        _turnCycle = CreateTurnCycle(_playerUnits, _enemyUnits);

        SubscribeToDeleteOnDeath(_playerUnits, _diedUnits);
        SubscribeToDeleteOnDeath(_enemyUnits, _diedUnits);

        while (IsBattle())
        {
            foreach (var unit in _turnCycle)
            {
                if (_playerUnits.Contains(unit))
                {
                    Turn(unit, _diedUnits, _enemyUnits, true);
                }
                else
                {
                    Turn(unit, _diedUnits, _playerUnits, false);
                }
            }

            UpdateDiedUnits(_playerUnits, _diedUnits, _turnCycle);
            UpdateDiedUnits(_enemyUnits, _diedUnits, _turnCycle);
            _diedUnits.Clear();
        }

        WinLoseMessage(_playerUnits);
    }

    private void PrintTurn(List<Unit> turnCycle, int turnIndex, bool playerTurn)
    {
        ConsoleColor color;

        if (playerTurn)
        {
            color = ConsoleColor.Green;
            Printer.Print("[YOUR TURN]", color);
        }
        else
        {
            color = ConsoleColor.DarkCyan;
            Printer.Print("[ENEMY TURN]", color);
        }

        for (int i = 0; i < turnCycle.Count; i++)
        {
            string unitName = turnCycle[i].Model.Name;
            if (i == turnIndex)
            {
                unitName += " <<<";
            }

            Printer.Print(unitName, color);
        }

        Console.WriteLine();
    }

    private List<Unit> CreateTurnCycle(List<Unit> playerUnits, List<Unit> enemyUnits)
    {
        List<Unit> result = new List<Unit>();

        List<Unit> sortedByInitiativeUnits = new List<Unit>();
        List<(Unit, float)> unitsSpeed = new List<(Unit, float)>();

        foreach (Unit unit in playerUnits)
        {
            sortedByInitiativeUnits.Add(unit);
        }
        foreach (Unit unit in enemyUnits)
        {
            sortedByInitiativeUnits.Add(unit);
        }

        sortedByInitiativeUnits = sortedByInitiativeUnits.OrderByDescending(unit => unit.Model.Initiative).ToList();

        foreach (Unit unit in sortedByInitiativeUnits)
        {
            float speed = unit.Model.Speed;

            while (speed > 0)
            {
                unitsSpeed.Add((unit, speed));
                speed -= (float)Math.Sqrt(unit.Model.Speed);
            }
        }

        unitsSpeed = unitsSpeed.OrderByDescending(x => x.Item2).ToList();

        foreach (Unit unit in sortedByInitiativeUnits)
        {
            if (!result.Contains(unit))
            {
                result.Add(unit);
                unitsSpeed.Remove((unit, unit.Model.Speed));
            }
            while (unitsSpeed.Count > 0 && unitsSpeed[0].Item1 == unit)
            {
                result.Add(unit);
                unitsSpeed.RemoveAt(0);
            }
        }

        foreach (var unitPair in unitsSpeed)
        {
            int firstUnitIndex = 0;

            foreach (var unit in result)
            {
                if (unit == unitPair.Item1)
                {
                    break;
                }
                firstUnitIndex++;
            }

            for (int i = firstUnitIndex; i < result.Count; i++)
            {
                if (unitPair.Item2 > result[i])
            }
        }

        return result;
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

    private void Turn(Unit attacker, List<Unit> diedAttackers, List<Unit> defenders, bool playerTurn)
    {
        if (diedAttackers.Contains(attacker))
            return;

        PrintTurn(_turnCycle, _turnIndex, playerTurn);

        List<string> selections = new List<string>();
        List<ICommand> commands = new List<ICommand>();

        foreach (var defender in defenders)
        {
            selections.Add(defender.Model.Name);
            commands.Add(new NullCommand());
        }

        var selectEnemy = Menu.Create("Select enemy", selections.ToArray(), commands.ToArray());
        int enemyNumber;
        if (playerTurn)
        {
            selectEnemy.Show();
            enemyNumber = selectEnemy.GetInput();
        }
        else
        {
            Random random = new Random();
            enemyNumber = random.Next(1, defenders.Count() + 1);
        }

        selectEnemy.Select(enemyNumber);

        selections = new List<string> {
                    "Weak: 250 damage (90%)",
                    "Medium: 1500 damage (75%)",
                    "Strong: 2500 damage (50%)"
                };

        commands = new List<ICommand>() {
                    new AttackCommand(attacker, defenders[enemyNumber - 1], 250, 90),
                    new AttackCommand(attacker, defenders[enemyNumber - 1], 1500, 75),
                    new AttackCommand(attacker, defenders[enemyNumber - 1], 2500, 50)
                };

        var selectAttack = Menu.Create("Select attack", selections.ToArray(), commands.ToArray());


        if (playerTurn)
        {
            selectAttack.Show();
            selectAttack.Select(selectAttack.GetInput());
        }
        else
        {
            Random random = new Random();
            selectAttack.Select(random.Next(1, selections.Count + 1));
        }

        _turnIndex++;
        if (_turnIndex >= _turnCycle.Count)
        {
            _turnIndex = 0;
        }
    }

    private void UpdateDiedUnits(List<Unit> aliveUnits, List<Unit> diedUnits, List<Unit> turnCycle)
    {
        foreach (var unit in diedUnits)
        {
            aliveUnits.Remove(unit);
            turnCycle.Remove(unit);
        }
    }

    private void SubscribeToDeleteOnDeath(List<Unit> units, List<Unit> unitsToDelete)
    {
        foreach (var unit in units)
            unit.HasDied += () => unitsToDelete.Add(unit);
    }

    private bool TryGetAlive(List<Unit> units, out Unit aliveUnit)
    {
        aliveUnit = units.Find(unit => unit.Model.Health > 0);
        return aliveUnit != null;
    }

    private bool IsBattle()
    {
        return
            TryGetAlive(_playerUnits, out Unit alivePlayerUnit)
            && TryGetAlive(_enemyUnits, out Unit aliveEnemyUnit);
    }
}