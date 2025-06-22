public class Unit : IDamageable
{
    public event Action HealthBelowZero;

    public bool IsAlive => _model.Health > 0;

    private const float _headHealthModifier = 0.0784f;
    private const float _bodyHealthModifier = 0.2941f;
    private const float _armHealthModifier = 0.1372f;
    private const float _legHealthModifier = 0.1764f;

    private UnitModel _model;
    public UnitModel Model => _model;

    private Dictionary<string, BodyPart> _bodyParts = new Dictionary<string, BodyPart>();
    public IReadOnlyDictionary<string, BodyPart> BodyParts => _bodyParts;

    public Unit(UnitModel model)
    {
        _model = model;

        BodyPart head = new Head((int)(_headHealthModifier * _model.Health));
        BodyPart body = new Body((int)(_bodyHealthModifier * _model.Health));
        BodyPart leftArm = new Arm((int)(_armHealthModifier * _model.Health));
        BodyPart rightArm = new Arm((int)(_armHealthModifier * _model.Health));
        BodyPart leftLeg = new Leg((int)(_legHealthModifier * _model.Health));
        BodyPart rightLeg = new Leg((int)(_legHealthModifier * _model.Health));

        //head.HealthBelowZero += () => _bodyParts.Remove(head);
        //body.HealthBelowZero += () => _bodyParts.Remove(body);
        //leftArm.HealthBelowZero += () => _bodyParts.Remove(leftArm);
        //rightArm.HealthBelowZero += () => _bodyParts.Remove(rightArm);
        //leftLeg.HealthBelowZero += () => _bodyParts.Remove(leftLeg);
        //rightLeg.HealthBelowZero += () => _bodyParts.Remove(rightLeg);

        _bodyParts = new Dictionary<string, BodyPart> { { "Head", head }, { "Body", body }, { "Left arm", leftArm }, { "Right arm", rightArm }, { "Left leg", leftLeg }, { "Right leg", rightLeg } };
    }

    public void Render()
    {
        foreach (var bodyPart in _bodyParts)
        {
            string status = bodyPart.Value.Health <= 0 ? "УНИЧТОЖЕНО" :
                          bodyPart.Value.Health < bodyPart.Value.MaxHealth * 0.6 ? bodyPart.Value.Health < bodyPart.Value.MaxHealth * 0.3 ? "КРИТ" : "ПОВРЕЖДЕНО" : "НОРМА";

            Console.ForegroundColor = GetStatusColor(status);
            Console.WriteLine($"{bodyPart.Key,-12}: [{bodyPart.Value.Health,2}/{bodyPart.Value.MaxHealth,2}] {GetHealthBar(bodyPart.Value)} {status}");
            Console.ResetColor();
        }

        string GetHealthBar(BodyPart bodyPart)
        {
            int barLength = 10;
            int filled = (int)Math.Round((double)bodyPart.Health / bodyPart.MaxHealth * barLength);
            filled = Math.Max(0, Math.Min(filled, barLength));

            return new string('█', filled) + new string('_', barLength - filled);
        }

        ConsoleColor GetStatusColor(string status)
        {
            return status switch
            {
                "УНИЧТОЖЕНО" => ConsoleColor.DarkRed,
                "ПОВРЕЖДЕНО" => ConsoleColor.Yellow,
                "КРИТ" => ConsoleColor.Red,
                _ => ConsoleColor.Green
            };
        }
    }

    public void TakeDamage(int damage)
    {
        Render();

        _model.Health -= damage;
        Printer.Print($"{_model.Name} health = {_model.Health}", ConsoleColor.Yellow);

        if (_model.Health <= 0)
        {
            Printer.Print($"{_model.Name} died", ConsoleColor.DarkRed);
            HealthBelowZero?.Invoke();
        }

        Console.WriteLine();
    }
}