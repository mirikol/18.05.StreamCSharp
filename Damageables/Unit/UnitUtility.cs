public static class UnitUtility
{
    private const float _attackModifier = 0.6f;
    private const float _miniamlBaseDamageModifier = 0.0f;

    public static int GetFlatDamage(int baseDamage, Unit attacker, Unit defender)
    {
        return (int)(baseDamage * (_miniamlBaseDamageModifier + (attacker.Attack * _attackModifier) / (float)((attacker.Attack * _attackModifier) + defender.Defense)));
    }

    public static void EquipUnit(Unit unit, IEquipment equipment, BodyPartName bodyPartName)
    {
        if (equipment is IWeapon weapon)
        {
            if (bodyPartName != BodyPartName.LeftArm && bodyPartName != BodyPartName.RightArm)
            {
                Logger.LogError("You should equip IWeapon on Arms only");
            }
            else
            {
                ((Arm)unit.BodyParts[bodyPartName]).EquipWeapon(weapon);
            }
        }
        else if (equipment is IArmor armor)
        {
            unit.BodyParts[bodyPartName].EquipArmor(armor);
        }
        else
        {
            Logger.LogError("You should use IWeapon or IArmor");
        }
    }
}