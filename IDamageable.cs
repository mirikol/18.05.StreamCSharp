public interface IDamageable
{
    public event Action HasDied;
    public void TakeDamage(int damage);
}