using UnityEngine;

/// <summary>
/// Shared enemy behaviour: takes damage, despawns off-screen, destroys the
/// player on contact, and reports its own death through GameEvents.
///
/// Open/Closed: new enemy types (see OrbitingEnemy) are added by subclassing
/// and overriding Move()/hooks, never by editing this file or Meteor/BigMeteor.
/// Liskov Substitution: Meteor, BigMeteor and OrbitingEnemy are all fully
/// interchangeable wherever an EnemyBase (or IDamageable) is expected.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public abstract class EnemyBase : MonoBehaviour, IDamageable
{
    [SerializeField] protected int hitPoints = 1;
    [SerializeField] protected float despawnY = -11f;

    protected virtual void Update()
    {
        Move();

        if (transform.position.y < despawnY)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>Subclasses define their own movement pattern.</summary>
    protected abstract void Move();

    /// <summary>Hook for subclass-specific side effects when killed by damage (e.g. Meteor counting toward BigMeteor spawn).</summary>
    protected virtual void OnDestroyedByDamage() { }

    /// <summary>BigMeteor overrides this to false: it survives hitting the player.</summary>
    protected virtual bool DestroySelfOnPlayerHit => true;

    public virtual void TakeDamage(int amount)
    {
        hitPoints -= amount;
        if (hitPoints <= 0)
        {
            GameEvents.EnemyDestroyed(transform.position);
            OnDestroyedByDamage();
            Destroy(gameObject);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D whatIHit)
    {
        if (whatIHit.CompareTag("Laser"))
        {
            Destroy(whatIHit.gameObject);
            TakeDamage(1);
        }
        else if (whatIHit.CompareTag("Player") && whatIHit.TryGetComponent<IDamageable>(out var player))
        {
            player.TakeDamage(int.MaxValue);
            if (DestroySelfOnPlayerHit)
            {
                Destroy(gameObject);
            }
        }
    }
}
