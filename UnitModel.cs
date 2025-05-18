public struct UnitModel
{
    public string Name { get; private set; }

    public int Health { get; set; }
    public int Initiative { get; private set; }
    public int Speed { get; private set; }
    public int Attack { get; private set; }
    public int Defense { get; private set; }

    public UnitModel(string name, int health, int initiative, int speed)
    {
        Name = name;
        Health = health;
        Initiative = initiative;
        Speed = speed;
    }

    public UnitModel(string name, int health, int initiative, int speed, int attack, int defense)
    {
        Name = name;
        Health = health;
        Initiative = initiative;
        Speed = speed;
        Attack = attack;
        Defense = defense;
    }
}