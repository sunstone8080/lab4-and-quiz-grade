/// <summary>
/// Interface Segregation: anything that can take damage implements just this
/// one small contract (Player, Meteor, BigMeteor, OrbitingEnemy) instead of a
/// bloated base class or a "God" enemy interface.
/// </summary>
public interface IDamageable
{
    void TakeDamage(int amount);
}
