using Spectre.Console;

public class BattleProcessor
{
    private UnitsPrinter _unitsPrinter;
    private GameplayLogPrinter _gameplayLogPrinter;
    private TurnPrinter _turnPrinter;
    private StatsPrinter _statsPrinter;
    private VitalsPrinter _vitalsPrinter;
    private SkillsPrinter _skillsPrinter;
    private SkillMenu _skillMenu;

    public BattleProcessor(UnitsPrinter unitsPrinter, GameplayLogPrinter gameplayLogPrinter, TurnPrinter turnPrinter, StatsPrinter statsPrinter, VitalsPrinter vitalsPrinter, SkillsPrinter skillsPrinter, SkillMenu skillMenu)
    {
        _unitsPrinter = unitsPrinter;
        _gameplayLogPrinter = gameplayLogPrinter;
        _turnPrinter = turnPrinter;
        _statsPrinter = statsPrinter;
        _vitalsPrinter = vitalsPrinter;
        _skillsPrinter = skillsPrinter;
        _skillMenu = skillMenu;
    }

    public void Battle(UnitTurn[] turnCycle, UnitTurn[] enemies, UnitTurn[] allies, UnitTurn attackerTurn, Action onComplete)
    {
        _vitalsPrinter.Reset();
        _statsPrinter.Reset();
        _turnPrinter.Print(turnCycle, attackerTurn);

        if (attackerTurn.IsAlly)
        {
            ConsoleKeyInfo keyInfo;
            _unitsPrinter.ResetSelect();
            if (FindUnitByName(turnCycle, _unitsPrinter.GetSelectedUnitName(), out Unit firstTurnUnit))
            {
                _statsPrinter.Print(new StatsContext(firstTurnUnit));
                _vitalsPrinter.Print(new VitalsContext(firstTurnUnit));
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

                if (FindUnitByName(turnCycle, _unitsPrinter.GetSelectedUnitName(), out Unit selectedUnit))
                {
                    if (selectedUnit == attackerTurn.Unit)
                    {
                        SkillsFilter.Include(attackerTurn.Unit.Skills, new Type[] { typeof(ISelfSkill) });
                    }

                    if (TryTurn(attackerTurn.Unit, selectedUnit))
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
            // Get Select Index and select unit
        }

        else
        {
            new AttackCommand(_gameplayLogPrinter, attackerTurn.Unit, allies.First(unit => unit.Unit.IsAlive).Unit, BodyPartName.Head, UnitUtility.GetFlatDamage(attackerTurn.Unit.BaseDamage, attackerTurn.Unit, allies.First(unit => unit.Unit.IsAlive).Unit), 90).Execute();
        }


        //Unit enemy;
        //if (attackerTurn.IsAlly)
        //{
        //    enemy = SelectEnemy(enemies, true);
        //}
        //else
        //{
        //    enemy = SelectEnemy(allies, false);
        //}

        //BodyPartName bodyPart = SelectBodyPart(attackerTurn.IsAlly);
        //int attackIndex = SelectAttack(attackerTurn.Unit, enemy, bodyPart, attackerTurn.IsAlly);

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

    private bool TryTurn(Unit attackerUnit, Unit selectedUnit)
    {
        bool skillHasCompleted = false;
        do
        {
            ConsoleKeyInfo keyInfo;
            _skillsPrinter.Print(new SkillsContext(attackerUnit.Skills));

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

            } while (keyInfo.Key != ConsoleKey.Enter);

            ISkill skill = new NullSkill("Skip", "");
            if (attackerUnit.Skills.Length > 0)
            {
                skill = _skillsPrinter.GetSkillFromSelected();
            }
            skill.Initialize(_skillMenu, attackerUnit, new List<Unit>() { selectedUnit });
            skillHasCompleted = skill.TryExecute(_gameplayLogPrinter);
        } while (!skillHasCompleted);

        _skillsPrinter.Reset();
        return true;
        //new AttackCommand(_gameplayLogPrinter, attackerUnit, selectedUnit, BodyPartName.Head, UnitUtility.GetFlatDamage(attackerUnit.BaseDamage, attackerUnit, selectedUnit), 100).Execute();
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
        if (FindUnitByName(turnCycle, _unitsPrinter.GetSelectedUnitName(), out Unit unit))
        {
            _statsPrinter.Print(new StatsContext(unit));
            _vitalsPrinter.Print(new VitalsContext(unit));
        }
        else
        {
            _statsPrinter.Reset();
            _vitalsPrinter.Reset();
        }
    }

    private bool FindUnitByName(UnitTurn[] unitTurns, string name, out Unit unit)
    {
        if (string.IsNullOrEmpty(name))
        {
            unit = null;
            return false;
        }

        foreach (var unitTurn in unitTurns)
        {
            if (unitTurn.Unit.Model.Name == name)
            {
                unit = unitTurn.Unit;
                return true;
            }
        }

        unit = null;
        return false;
    }

    private Unit SelectEnemy(UnitTurn[] defenders, bool playerTurn)
    {
        defenders = Array.FindAll(defenders, defender => defender.Unit.IsAlive);

        List<CommandBinding> bindings = new List<CommandBinding>();
        foreach (var defender in defenders)
        {
            CommandBinding binding = new CommandBinding(defender.Unit.Model.Name, new NullCommand());
            bindings.Add(binding);
        }
        int selectedEnemyIndex = GetMenuChoice("Select enemy", bindings, playerTurn);
        var enemy = defenders[selectedEnemyIndex];

        return enemy.Unit;
    }

    private BodyPartName SelectBodyPart(bool playerTurn)
    {
        List<CommandBinding> bindings = new List<CommandBinding>();
        var bodyPartNames = Enum.GetNames(typeof(BodyPartName));
        foreach (var bodyPartName in bodyPartNames)
        {
            CommandBinding binding = new CommandBinding(bodyPartName, new NullCommand());
            bindings.Add(binding);
        }
        int selectedBodyPartIndex = GetMenuChoice("Select body part", bindings, playerTurn);
        BodyPartName bodyPart = (BodyPartName)Enum.Parse(typeof(BodyPartName), bodyPartNames[selectedBodyPartIndex]);

        return bodyPart;
    }

    private int SelectAttack(Unit attacker, Unit defender, BodyPartName bodyPart, bool playerTurn)
    {
        List<CommandBinding> bindings =
        [
            new CommandBinding($"Weak: {UnitUtility.GetFlatDamage(attacker.BaseDamage, attacker, defender)} damage (90%)", new AttackCommand(_gameplayLogPrinter, attacker, defender, bodyPart, UnitUtility.GetFlatDamage(attacker.BaseDamage, attacker, defender), 90)),
            new CommandBinding($"Medium: {UnitUtility.GetFlatDamage((int)(attacker.BaseDamage * 1.25f), attacker, defender)} damage (75%)", new AttackCommand(_gameplayLogPrinter, attacker, defender, bodyPart, UnitUtility.GetFlatDamage((int)(attacker.BaseDamage * 1.25f), attacker, defender), 75)),
            new CommandBinding($"Strong: {UnitUtility.GetFlatDamage((int)(attacker.BaseDamage * 2f), attacker, defender)} damage (50%)", new AttackCommand(_gameplayLogPrinter, attacker, defender, bodyPart, UnitUtility.GetFlatDamage((int)(attacker.BaseDamage * 2f), attacker, defender), 50)),
        ];
        int selectedAttackIndex = GetMenuChoice("Select attack", bindings, playerTurn);

        return selectedAttackIndex;
    }

    private int GetMenuChoice(string menuName, List<CommandBinding> bindings, bool playerTurn)
    {
        var menu = new Menu(menuName, bindings.ToArray());

        int selectIndex;
        if (playerTurn)
        {
            menu.Show();
            selectIndex = menu.GetInput();
        }
        else
        {
            Random random = new Random();
            selectIndex = random.Next(1, menu.BindingsCount + 1);
        }
        menu.Select(selectIndex);

        return selectIndex - 1;
    }
}