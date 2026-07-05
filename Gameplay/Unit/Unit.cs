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

    private UnitModel _model;
    public UnitModel Model => _model;

    private Dictionary<BodyPartName, BodyPart> _bodyParts = new Dictionary<BodyPartName, BodyPart>();
    public IReadOnlyDictionary<BodyPartName, BodyPart> BodyParts => _bodyParts;

    private int[] _placement;
    public int[] Placement => _placement;

    private List<ISkill> _skills;
    public ISkill[] Skills => _skills.ToArray();

    private List<ISkill> _filteredSkills;
    public ISkill[] FilteredSkills => _filteredSkills.ToArray();

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

    public int Speed
    {
        get
        {
            int speed = Model.Speed;
            foreach (var bodyPart in BodyParts.Values)
            {
                if (bodyPart.Health <= 0)
                {
                    continue;
                }

                if (bodyPart.HasArmor)
                {
                    speed += bodyPart.Armor.Speed;
                }
                if (bodyPart is Arm arm && arm.HasWeapon)
                {
                    speed += arm.Weapon.Speed;
                }
            }

            return speed;
        }
    }

    public int Initiative
    {
        get
        {
            int initiative = Model.Initiative;
            foreach (var bodyPart in BodyParts.Values)
            {
                if (bodyPart.Health <= 0)
                {
                    continue;
                }

                if (bodyPart.HasArmor)
                {
                    initiative += bodyPart.Armor.Initiative;
                }
                if (bodyPart is Arm arm && arm.HasWeapon)
                {
                    initiative += arm.Weapon.Initiative;
                }
            }

            return initiative;
        }
    }

    public Unit(UnitModel model, int[] placement, List<ISkill> skills)
    {
        _placement = new int[placement.Length];
        placement.CopyTo(_placement, 0);

        _skills = new List<ISkill>(skills);

        _model = model;

        BodyPart head = new Head((int)(_headHealthModifier * _model.Health));
        BodyPart body = new Body((int)(_bodyHealthModifier * _model.Health));
        BodyPart leftArm = new Arm((int)(_armHealthModifier * _model.Health));
        BodyPart rightArm = new Arm((int)(_armHealthModifier * _model.Health));
        BodyPart leftLeg = new Leg((int)(_legHealthModifier * _model.Health));
        BodyPart rightLeg = new Leg((int)(_legHealthModifier * _model.Health));

        head.HealthBelowZero += () => UpdateAliveStatus();
        body.HealthBelowZero += () => UpdateAliveStatus();
        leftArm.HealthBelowZero += () => UpdateAliveStatus();
        rightArm.HealthBelowZero += () => UpdateAliveStatus();
        leftLeg.HealthBelowZero += () => UpdateAliveStatus();
        rightLeg.HealthBelowZero += () => UpdateAliveStatus();

        _bodyParts = new Dictionary<BodyPartName, BodyPart> { { BodyPartName.Head, head }, { BodyPartName.Body, body }, { BodyPartName.LeftArm, leftArm }, { BodyPartName.RightArm, rightArm }, { BodyPartName.LeftLeg, leftLeg }, { BodyPartName.RightLeg, rightLeg } };
    }

    private void UpdateAliveStatus()
    {
        if (!IsAlive)
        {
            HealthBelowZero?.Invoke();
        }
    }
}