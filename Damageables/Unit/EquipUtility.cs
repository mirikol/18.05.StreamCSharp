public static class EquipUtility
{
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