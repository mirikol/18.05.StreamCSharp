public class TurnController
{
    private List<Unit> _turnCycle = new List<Unit>();
    public IReadOnlyList<Unit> TurnCycle => _turnCycle;

    private int _turnIndex = 0;

    public TurnController(Arena arena, List<Unit> playerUnits, List<Unit> enemyUnits)
    {
        arena.UnitHasDied += DeleteFromTurnCycle;
        CreateTurnCycle(playerUnits, enemyUnits);
    }

    public void Turn(Unit attacker, List<Unit> defenders, bool playerTurn)
    {
        if (!attacker.IsAlive)
            return;

        Print(_turnIndex, playerTurn);

        List<string> selections = new List<string>();
        List<ICommand> commands = new List<ICommand>();

        foreach (var defender in defenders)
        {
            if (!defender.IsAlive)
            {
                continue;
            }
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
                    $"Weak: {50} damage (90%)",
                    $"Medium: {(int)(50 * 1.25f)} damage (75%)",
                    $"Strong: {(int)(50 * 2f)} damage (50%)"
                };

        commands = new List<ICommand>() {
                    new AttackCommand(attacker, defenders[enemyNumber - 1], 50, 90),
                    new AttackCommand(attacker, defenders[enemyNumber - 1], (int)(50 * 1.25f), 75),
                    new AttackCommand(attacker, defenders[enemyNumber - 1], (int)(50 * 2f), 50)
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

    private void Print(int turnIndex, bool playerTurn)
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

        for (int i = 0; i < _turnCycle.Count; i++)
        {
            if (!_turnCycle[i].IsAlive)
            {
                continue;
            }

            string unitName = _turnCycle[i].Model.Name;
            if (i == turnIndex)
            {
                unitName += " <<<";
            }

            Printer.Print(unitName, color);
        }

        Console.WriteLine();
    }

    private void CreateTurnCycle(List<Unit> playerUnits, List<Unit> enemyUnits)
    {
        List<(Unit, float)> tempResult = new List<(Unit, float)>();

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
        float minSpeed = sortedByInitiativeUnits.OrderByDescending(unit => unit.Model.Speed).Last().Model.Speed;

        foreach (Unit unit in sortedByInitiativeUnits)
        {
            float speed = unit.Model.Speed / minSpeed;

            while (speed > 0)
            {
                unitsSpeed.Add((unit, speed));
                speed -= (float)Math.Cbrt(unit.Model.Speed);
            }
        }

        unitsSpeed = unitsSpeed.OrderByDescending(x => x.Item2).ToList();
        List<List<(Unit, float)>> units = new List<List<(Unit, float)>>();

        foreach (Unit unit in sortedByInitiativeUnits)
        {
            List<(Unit, float)> unitList = new List<(Unit, float)>();

            tempResult.Add((unit, unit.Model.Speed / minSpeed));
            unitsSpeed.Remove((unit, unit.Model.Speed / minSpeed));

            foreach (var speedUnit in unitsSpeed)
            {
                if (speedUnit.Item1 == unit)
                {
                    unitList.Add(speedUnit);
                }
            }

            units.Add(unitList);
        }

        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].Count == 0)
            {
                continue;
            }

            int startIndex = 0;
            foreach (var unit in tempResult)
            {
                if (unit.Item1 == units[i][0].Item1)
                {
                    break;
                }
                startIndex++;
            }

            foreach (var speedUnit in units[i])
            {
                bool exit = false;

                for (int j = startIndex; j < tempResult.Count; j++)
                {
                    if (exit)
                    {
                        break;
                    }

                    if (speedUnit.Item2 > tempResult[j].Item2)
                    {
                        continue;
                    }
                    else
                    {
                        for (int k = j; k < tempResult.Count; k++)
                        {
                            if (speedUnit.Item2 < tempResult[k].Item2)
                            {
                                continue;
                            }
                            else
                            {
                                tempResult.Insert(Math.Max(k, 0), speedUnit);
                                exit = true;
                                break;
                            }
                        }

                        if (!exit)
                        {
                            tempResult.Add(speedUnit);
                            exit = true;
                        }
                    }
                }
            }
        }

        foreach (var unit in tempResult)
        {
            _turnCycle.Add(unit.Item1);
        }
    }

    private void DeleteFromTurnCycle(Unit unit)
    {
        _turnCycle.Remove(unit);
    }
}