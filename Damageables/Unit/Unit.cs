public class Unit
{
    public event Action HealthBelowZero;

    public bool IsAlive
    {
        get
        {
            return !(_bodyParts[BodyPartName.Head].Health <= 0
                || _bodyParts[BodyPartName.Body].Health <= 0
                || (_bodyParts[BodyPartName.LeftArm].Health <= 0 && _bodyParts[BodyPartName.RightArm].Health <= 0)
                || (_bodyParts[BodyPartName.LeftLeg].Health <= 0 && _bodyParts[BodyPartName.RightLeg].Health <= 0)
                );
        }
    }

    private const float _headHealthModifier = 0.0784f;
    private const float _bodyHealthModifier = 0.2941f;
    private const float _armHealthModifier = 0.1372f;
    private const float _legHealthModifier = 0.1764f;

    private const string _destroyedBodyPartStatus = "УНИЧТОЖЕНО";
    private const string _criticalBodyPartStatus = "КРИТ";
    private const string _hurtBodyPartStatus = "ПОВРЕЖДЕНО";
    private const string _normalBodyPartStatus = "НОРМА";

    private const char _healthBarFillChar = '█';

    private UnitModel _model;
    public UnitModel Model => _model;

    private Dictionary<BodyPartName, BodyPart> _bodyParts = new Dictionary<BodyPartName, BodyPart>();
    public IReadOnlyDictionary<BodyPartName, BodyPart> BodyParts => _bodyParts;

    public int BaseDamage
    {
        get
        {
            int baseDamage = 0;

            var leftArm = (Arm)BodyParts[BodyPartName.LeftArm];
            var rightArm = (Arm)BodyParts[BodyPartName.RightArm];

            if (leftArm.Health > 0 && leftArm.HasWeapon)
            {
                baseDamage += leftArm.Weapon.BaseDamage;
            }
            if (rightArm.Health > 0 && rightArm.HasWeapon)
            {
                baseDamage += rightArm.Weapon.BaseDamage;
            }

            return baseDamage;
        }
    }

    public int Attack
    {
        get
        {
            int attack = Model.Attack;
            foreach (var bodyPart in BodyParts.Values)
            {
                if (bodyPart.Health <= 0)
                {
                    continue;
                }

                if (bodyPart.HasArmor)
                {
                    attack += bodyPart.Armor.Attack;
                }
                if (bodyPart is Arm arm && arm.HasWeapon)
                {
                    attack += arm.Weapon.Attack;
                }
            }

            return attack;
        }
    }

    public int Defense
    {
        get
        {
            int defense = Model.Defense;
            foreach (var bodyPart in BodyParts.Values)
            {
                if (bodyPart.Health <= 0)
                {
                    continue;
                }

                if (bodyPart.HasArmor)
                {
                    defense += bodyPart.Armor.Defense;
                }
                if (bodyPart is Arm arm && arm.HasWeapon)
                {
                    defense += arm.Weapon.Defense;
                }
            }

            return defense;
        }
    }

    public Unit(UnitModel model)
    {
        _model = model;

        BodyPart head = new Head((int)(_headHealthModifier * _model.Health));
        BodyPart body = new Body((int)(_bodyHealthModifier * _model.Health));
        BodyPart leftArm = new Arm((int)(_armHealthModifier * _model.Health));
        BodyPart rightArm = new Arm((int)(_armHealthModifier * _model.Health));
        BodyPart leftLeg = new Leg((int)(_legHealthModifier * _model.Health));
        BodyPart rightLeg = new Leg((int)(_legHealthModifier * _model.Health));

        head.HasDamaged += () => UpdateDamagedStatus(BodyPartName.Head);
        body.HasDamaged += () => UpdateDamagedStatus(BodyPartName.Body);
        leftArm.HasDamaged += () => UpdateDamagedStatus(BodyPartName.LeftArm);
        rightArm.HasDamaged += () => UpdateDamagedStatus(BodyPartName.RightArm);
        leftLeg.HasDamaged += () => UpdateDamagedStatus(BodyPartName.LeftLeg);
        rightLeg.HasDamaged += () => UpdateDamagedStatus(BodyPartName.RightLeg);

        head.HealthBelowZero += () => UpdateAliveStatus();
        body.HealthBelowZero += () => UpdateAliveStatus();
        leftArm.HealthBelowZero += () => UpdateAliveStatus();
        rightArm.HealthBelowZero += () => UpdateAliveStatus();
        leftLeg.HealthBelowZero += () => UpdateAliveStatus();
        rightLeg.HealthBelowZero += () => UpdateAliveStatus();

        _bodyParts = new Dictionary<BodyPartName, BodyPart> { { BodyPartName.Head, head }, { BodyPartName.Body, body }, { BodyPartName.LeftArm, leftArm }, { BodyPartName.RightArm, rightArm }, { BodyPartName.LeftLeg, leftLeg }, { BodyPartName.RightLeg, rightLeg } };
    }

    private void UpdateDamagedStatus(BodyPartName bodyPart)
    {
        Printer.Print($"{_model.Name} {bodyPart.ToString()} health = {_bodyParts[bodyPart].Health}", ConsoleColor.Yellow);
        Render();
    }

    private void UpdateAliveStatus()
    {
        if (!IsAlive)
        {
            Printer.Print($"{_model.Name} died", ConsoleColor.DarkRed);
            HealthBelowZero?.Invoke();
        }

        Console.WriteLine();
    }

    public void Render()
    {
        foreach (var bodyPart in _bodyParts)
        {
            string status = bodyPart.Value.Health <= 0 ? _destroyedBodyPartStatus :
                          bodyPart.Value.Health < bodyPart.Value.MaxHealth * 0.6 ? bodyPart.Value.Health < bodyPart.Value.MaxHealth * 0.3 ? _criticalBodyPartStatus : _hurtBodyPartStatus : _normalBodyPartStatus;

            Console.ForegroundColor = GetStatusColor(status);
            Console.WriteLine($"{bodyPart.Key,-12}: [{bodyPart.Value.Health,2}/{bodyPart.Value.MaxHealth,2}] {GetHealthBar(bodyPart.Value)} {status}");
            Console.ResetColor();
        }

        string GetHealthBar(BodyPart bodyPart)
        {
            int barLength = 10;
            int filled = (int)Math.Round((double)bodyPart.Health / bodyPart.MaxHealth * barLength);
            filled = Math.Max(0, Math.Min(filled, barLength));

            return new string(_healthBarFillChar, filled) + new string('_', barLength - filled);
        }

        ConsoleColor GetStatusColor(string status)
        {
            return status switch
            {
                _destroyedBodyPartStatus => ConsoleColor.DarkRed,
                _criticalBodyPartStatus => ConsoleColor.Red,
                _hurtBodyPartStatus => ConsoleColor.Yellow,
                _ => ConsoleColor.Green
            };
        }
    }
}