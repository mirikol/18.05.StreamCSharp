public abstract class BodyPart : IDamageable
{
    public event Action HealthBelowZero;
    public event Action HasDamaged;

    public IArmor Armor { get; private set; }
    public bool HasArmor => Armor != null;

    public int Health { get; private set; }
    public int MaxHealth { get; private set; }

    public BodyPart(int health)
    {
        Health = health;
        MaxHealth = health;
    }

    public abstract Type GetFilteredType();

    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            Printer.Print($"{GetType().Name} disabled", ConsoleColor.DarkRed);
            HealthBelowZero?.Invoke();
            Health = 0;
        }

        HasDamaged?.Invoke();
        Console.WriteLine();
    }

    public void EquipArmor(IArmor armor)
    {
        if (GetFilteredType().IsAssignableFrom(armor.GetType()))
        {
            Armor = armor;
        }
        else
        {
            Logger.LogError($"Error to equip armor: {armor.GetType().Name} -> {GetType().Name}");
        }
    }

    public void DivestArmor()
    {
        Armor = null;
    }
}

public class Head : BodyPart
{
    public Head(int health) : base(health)
    {
    }

    public override Type GetFilteredType()
    {
        return typeof(IHeadEquipment);
    }
}

public class Arm : BodyPart
{
    public IWeapon Weapon { get; private set; }
    public bool HasWeapon => Weapon != null;

    public Arm(int health) : base(health)
    {
    }

    public void EquipWeapon(IWeapon weapon)
    {
        if (GetFilteredType().IsAssignableFrom(weapon.GetType()))
        {
            Weapon = weapon;
        }
        else
        {
            Logger.LogError($"Error to equip armor: {weapon.GetType().Name} -> {GetType().Name}");
        }
    }

    public void DivestWeapon()
    {
        Weapon = null;
    }

    public override Type GetFilteredType()
    {
        return typeof(IArmEquipment);
    }
}

public class Body : BodyPart
{
    public Body(int health) : base(health)
    {
    }

    public override Type GetFilteredType()
    {
        return typeof(IBodyEquipment);
    }
}

public class Leg : BodyPart
{
    public Leg(int health) : base(health)
    {
    }

    public override Type GetFilteredType()
    {
        return typeof(ILegEquipment);
    }
}