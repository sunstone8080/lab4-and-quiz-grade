using UnityEngine;

public class Meteor : EnemyBase
{
    [SerializeField] private float fallSpeed = 2f;

    protected override void Move()
    {
        transform.Translate(Vector3.down * Time.deltaTime * fallSpeed);
    }

    protected override void OnDestroyedByDamage()
    {
        // Only small meteors count toward spawning the next BigMeteor.
        GameEvents.MeteorDestroyed();
    }
}
