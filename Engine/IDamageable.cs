public interface IDamageable
{
    public event Action HealthBelowZero;
    public void TakeDamage(int damage);
}