public class Sword : IWeapon, IArmEquipment
{
    public int Defense => _defense;
    public int Attack => _attack;
    public int BaseDamage => _baseDamage;

    private int _defense;
    private int _attack;
    private int _baseDamage;

    public Sword(int defense, int attack, int baseDamage)
    {
        _defense = defense;
        _attack = attack;
        _baseDamage = baseDamage;
    }
}