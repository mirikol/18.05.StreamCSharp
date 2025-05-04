
public interface IDamageable
{
    public event Action HasDied;
    public int Health { get; }
    public void TakeDamage(int damage);
}
