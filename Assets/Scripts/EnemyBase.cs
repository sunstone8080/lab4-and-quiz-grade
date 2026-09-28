using UnityEngine;


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

    //Subclasses define their own movement pattern.
    protected abstract void Move();
    //Hook for subclass-specific side effects when killed by damage
    protected virtual void OnDestroyedByDamage() { }
    //Toggle for big and small meteors 
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
