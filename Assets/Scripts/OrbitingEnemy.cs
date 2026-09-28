using UnityEngine;

public class OrbitingEnemy : EnemyBase
{
    [SerializeField] private float orbitRadius = 3f;
    [SerializeField] private float angularSpeedDegPerSec = 60f;
    [SerializeField] private float baseMoveSpeed = 3f;
    [SerializeField] private float turnSpeed = 8f;
    [SerializeField] private float nearDistance = 2f;   
    [SerializeField] private float farDistance = 8f;   
    [SerializeField] private float maxSpeedMultiplier = 2.5f;
    [SerializeField] private float avoidanceRadius = 1.5f;
    [SerializeField] private float avoidanceStrength = 2f;
    [SerializeField] private float _angle;
    [SerializeField] private Transform _player;

    private void Awake()
    {
        hitPoints = 2;
        _angle = Random.Range(0f, 360f); 

    }

    private void Start()
    {
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) _player = playerObj.transform;
    }

    protected override void Move()
    {
        if (_player == null) return;

        //push the orbit angle and compute the point on the circle thats being followed.
        _angle += angularSpeedDegPerSec * Time.deltaTime;
        float rad = _angle * Mathf.Deg2Rad;
        Vector3 orbitOffset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * orbitRadius;
        Vector3 orbitTarget = _player.position + orbitOffset;

        Vector3 toOrbitTarget = orbitTarget - transform.position;
        Vector3 toPlayer = _player.position - transform.position;

        //Speed up when far from the player
        float speedMultiplier = Mathf.Lerp(
            1f,
            maxSpeedMultiplier,
            Mathf.InverseLerp(nearDistance * nearDistance, farDistance * farDistance, toPlayer.sqrMagnitude));

        Vector3 avoidance = ComputeAvoidance();
        Vector3 desiredMove = toOrbitTarget.normalized * (baseMoveSpeed * speedMultiplier) + avoidance * avoidanceStrength;

        transform.position = Vector3.MoveTowards(
            transform.position,
            transform.position + desiredMove,
            desiredMove.magnitude * Time.deltaTime);

        FacePlayer(toPlayer);
    }

    private void FacePlayer(Vector3 toPlayer)
    {
        if (toPlayer.sqrMagnitude < 0.0001f) return;

        //Angle of the direction vector
        float targetAngleDeg = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngleDeg);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    private Vector3 ComputeAvoidance()
    {
        Vector3 push = Vector3.zero;
        OrbitingEnemy[] others = FindObjectsOfType<OrbitingEnemy>();
        foreach (var other in others)
        {
            if (other == this)
            {
                continue;


            }


            Vector3 away = transform.position - other.transform.position;
            float sqrDist = away.sqrMagnitude;
            if (sqrDist < avoidanceRadius * avoidanceRadius && sqrDist > 0.0001f)
            {
                push += away.normalized / Mathf.Sqrt(sqrDist);

            }
        }
        return push;
    }
}
