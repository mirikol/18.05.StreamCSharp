public class Helmet : IArmor, IHeadEquipment
{
    public int Defense => _defense;
    public int Attack => _attack;

    private int _defense;
    private int _attack;

    public Helmet(int defense, int attack)
    {
        _defense = defense;
        _attack = attack;
    }
}