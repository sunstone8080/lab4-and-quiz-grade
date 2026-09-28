using UnityEngine;

public class BigMeteor : EnemyBase
{
    [SerializeField] private float fallSpeed = 0.5f;

    private void Awake()
    {
        hitPoints = 5;
    }

    protected override void Move()
    {
        transform.Translate(Vector3.down * Time.deltaTime * fallSpeed);
    }

    // Matches original behaviour: hitting the player destroys the player,
    // but the BigMeteor itself keeps going.
    protected override bool DestroySelfOnPlayerHit => false;
}
