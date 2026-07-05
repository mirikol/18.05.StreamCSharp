using Spectre.Console;

public class BattleProcessor
{
    private UnitsPrinter _unitsPrinter;
    private GameplayLogPrinter _gameplayLogPrinter;
    private StatsPrinter _statsPrinter;
    private VitalsPrinter _vitalsPrinter;
    private SkillsPrinter _skillsPrinter;
    private SkillMenu _skillMenu;

    public BattleProcessor(UnitsPrinter unitsPrinter, GameplayLogPrinter gameplayLogPrinter, StatsPrinter statsPrinter, VitalsPrinter vitalsPrinter, SkillsPrinter skillsPrinter, SkillMenu skillMenu)
    {
        _unitsPrinter = unitsPrinter;
        _gameplayLogPrinter = gameplayLogPrinter;
        _statsPrinter = statsPrinter;
        _vitalsPrinter = vitalsPrinter;
        _skillsPrinter = skillsPrinter;
        _skillMenu = skillMenu;
    }

    public void Battle(UnitTurn[] turnCycle, UnitTurn[] enemies, UnitTurn[] allies, UnitTurn attackerTurn, Action onComplete)
    {
        _vitalsPrinter.Reset();
        _statsPrinter.Reset();
        _unitsPrinter.Print(new UnitsContext(turnCycle, turnCycle.Select(x => x.Unit.Placement).ToArray(), attackerTurn));

        if (attackerTurn.IsAlly)
        {
            ConsoleKeyInfo keyInfo;
            _unitsPrinter.ResetSelect();
            if (FindUnitByName(turnCycle, _unitsPrinter.GetSelectedUnitName(), out UnitTurn firstTurnUnit))
            {
                _statsPrinter.Print(new StatsContext(firstTurnUnit.Unit));
                _vitalsPrinter.Print(new VitalsContext(firstTurnUnit.Unit));
            }

            bool successfulSelect = false;
            while (!successfulSelect)
            {
                do
                {
                    keyInfo = Console.ReadKey(true);

                    ProcessKey(keyInfo);
                    UpdatePlayerInfo(turnCycle);

                } while (keyInfo.Key != ConsoleKey.Enter);

                if (FindUnitByName(turnCycle, _unitsPrinter.GetSelectedUnitName(), out UnitTurn selectedUnit))
                {
                    ISkill[] skills = attackerTurn.Unit.Skills;

                    if (selectedUnit.Unit == attackerTurn.Unit)
                    {
                        skills = SkillsFilter.Include(attackerTurn.Unit.Skills, new Type[] { typeof(ISelfSkill) });
                    }
                    else if (attackerTurn.IsAlly && selectedUnit.IsAlly)
                    {
                        skills = SkillsFilter.Include(attackerTurn.Unit.Skills, new Type[] { typeof(IAllySkill) });
                    }
                    else
                    {
                        skills = SkillsFilter.Exclude(attackerTurn.Unit.Skills, new Type[] { typeof(ISelfSkill) });
                    }

                    if (TryTurn(skills, attackerTurn.Unit, selectedUnit.Unit))
                    {
                        successfulSelect = true;
                    }
                    else
                    {
                        _skillsPrinter.Reset();
                    }

                }
                else
                {
                    _gameplayLogPrinter.Print(new LogContext("Нужно выбрать юнита для взаимодействия с ним.", ConsoleColor.Red));
                }
            }
        }

        else
        {
            new AttackCommand(_gameplayLogPrinter, attackerTurn.Unit, allies.First(unit => unit.Unit.IsAlive).Unit, BodyPartName.Head, UnitUtility.GetFlatDamage(attackerTurn.Unit.BaseDamage, attackerTurn.Unit, allies.First(unit => unit.Unit.IsAlive).Unit), 90).Execute();
        }

        for (int i = 0; i < turnCycle.Length; i++)
        {
            if (turnCycle[i].Order == attackerTurn.Order)
            {
                turnCycle[i] = new UnitTurn(attackerTurn.Unit, attackerTurn.IsAlly, false, i);
                break;
            }
        }

        onComplete();
    }

    private bool TryTurn(ISkill[] skills, Unit attackerUnit, Unit selectedUnit)
    {
        bool skillHasCompleted = false;
        do
        {
            ConsoleKeyInfo keyInfo;
            _skillsPrinter.Print(new SkillsContext(skills));

            do
            {
                keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.W)
                {
                    _skillsPrinter.SelectUp();
                }
                if (keyInfo.Key == ConsoleKey.S)
                {
                    _skillsPrinter.SelectDown();
                }
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    _gameplayLogPrinter.Print(new LogContext($"Ход от юнита {attackerUnit.Model.Name} к юниту {selectedUnit.Model.Name} был прерван"));
                    return false;
                }

            } while (keyInfo.Key != ConsoleKey.Enter || skills.Length == 0);

            ISkill skill = _skillsPrinter.GetSkillFromSelected();
            skill.Initialize(_skillMenu, attackerUnit, new List<Unit>() { selectedUnit });
            skillHasCompleted = skill.TryExecute(_gameplayLogPrinter);

        } while (!skillHasCompleted);

        _skillsPrinter.Reset();
        return true;
    }

    private void ProcessKey(ConsoleKeyInfo keyInfo)
    {
        if (keyInfo.Key == ConsoleKey.D)
        {
            _unitsPrinter.SelectRight();
        }
        if (keyInfo.Key == ConsoleKey.A)
        {
            _unitsPrinter.SelectLeft();
        }
        if (keyInfo.Key == ConsoleKey.W)
        {
            _unitsPrinter.SelectUp();
        }
        if (keyInfo.Key == ConsoleKey.S)
        {
            _unitsPrinter.SelectDown();
        }
        if (keyInfo.Key == ConsoleKey.RightArrow)
        {
            _vitalsPrinter.SwiitchPrintMode();
        }
    }

    private void UpdatePlayerInfo(UnitTurn[] turnCycle)
    {
        if (FindUnitByName(turnCycle, _unitsPrinter.GetSelectedUnitName(), out UnitTurn foundUnitTurn))
        {
            _statsPrinter.Print(new StatsContext(foundUnitTurn.Unit));
            _vitalsPrinter.Print(new VitalsContext(foundUnitTurn.Unit));
        }
        else
        {
            _statsPrinter.Reset();
            _vitalsPrinter.Reset();
        }
    }

    private bool FindUnitByName(UnitTurn[] unitTurns, string name, out UnitTurn foundUnitTurn)
    {
        if (string.IsNullOrEmpty(name))
        {
            foundUnitTurn = UnitTurn.NULL;
            return false;
        }

        foreach (var unitTurn in unitTurns)
        {
            if (unitTurn.Unit.Model.Name == name)
            {
                foundUnitTurn = unitTurn;
                return true;
            }
        }

        foundUnitTurn = UnitTurn.NULL;
        return false;
    }
}