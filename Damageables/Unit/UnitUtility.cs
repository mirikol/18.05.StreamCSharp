public static class UnitUtility
{
    private const float _attackModifier = 0.6f;
    private const float _miniamlBaseDamageModifier = 0.0f;

    public static int GetFlatDamage(int baseDamage, Unit attacker, Unit defender)
    {
        return (int)(baseDamage * (_miniamlBaseDamageModifier + (attacker.Attack * _attackModifier) / (float)((attacker.Attack * _attackModifier) + defender.Defense)));
    }

    public static Unit CreateUnit(UnitSave unitSave)
    {
        Unit unit = new Unit(unitSave.UnitModel);

        foreach (var weapon in unitSave.Weapons)
        {
            EquipUtility.EquipUnit(unit, weapon.Weapon, weapon.BodyPart);
        }
        foreach (var armor in unitSave.Armors)
        {
            EquipUtility.EquipUnit(unit, armor.Armor, armor.BodyPart);
        }

        return unit;
    }
}