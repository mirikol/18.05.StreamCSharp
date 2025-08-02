public static class UnitUntility
{
    private const float _attackModifier = 0.6f;

    public static int GetFlatDamage(int baseDamage, Unit attacker, Unit defender)
    {
        return (int)(baseDamage * (1 + (attacker.Attack * _attackModifier) / (float)((attacker.Attack * _attackModifier) + defender.Defense)));
    }
}