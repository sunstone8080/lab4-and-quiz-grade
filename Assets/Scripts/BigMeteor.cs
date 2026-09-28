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

    protected override bool DestroySelfOnPlayerHit => false;
}
