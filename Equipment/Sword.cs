public class Sword : IWeapon, IArmEquipment
{
    public string Name => _name;
    public int Defense => _defense;
    public int Attack => _attack;
    public int Speed => _speed;
    public int Initiative => _initiative;
    public int BaseDamage => _baseDamage;

    private string _name;
    private int _defense;
    private int _attack;
    private int _speed;
    private int _initiative;
    private int _baseDamage;

    public Sword(string name, int defense, int attack, int speed, int initiative, int baseDamage)
    {
        _name = name;
        _defense = defense;
        _attack = attack;
        _speed = speed;
        _initiative = initiative;
        _baseDamage = baseDamage;
    }
}