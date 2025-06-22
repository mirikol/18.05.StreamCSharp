
public abstract class BodyPart : IDamageable
{
    public event Action HealthBelowZero;

    public IReadOnlyCollection<IEquipment> Equipment => _equipment;
    private List<IEquipment> _equipment;

    public int Health { get; private set; }
    public int MaxHealth { get; private set; }

    public BodyPart(int health)
    {
        Health = health;
        MaxHealth = health;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            Printer.Print($"{GetType().Name} disabled", ConsoleColor.DarkRed);
            HealthBelowZero?.Invoke();
        }

        Console.WriteLine();
    }

    public void Equip(IEquipment[] equipment)
    {
        foreach (var equipmentItem in equipment)
        {
            _equipment.Add(equipmentItem);
        }
    }

    public void Divest(IEquipment equipment)
    {
        _equipment.Remove(equipment);
    }
}

public class Head : BodyPart
{
    public Head(int health) : base(health)
    {
    }
}

public class Arm : BodyPart
{
    public Arm(int health) : base(health)
    {
    }
}

public class Body : BodyPart
{
    public Body(int health) : base(health)
    {
    }
}

public class Leg : BodyPart
{
    public Leg(int health) : base(health)
    {
    }
}