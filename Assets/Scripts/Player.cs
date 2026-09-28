using UnityEngine;

[RequireComponent(typeof(ScreenWrapper))]
public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private float speed = 6f;
    [SerializeField] private float shotCooldown = 1f;

    private IInputReader _input;
    private ScreenWrapper _wrapper;
    private bool _canShoot = true;

    private void Awake()
    {
        _input = GetComponent<IInputReader>();
        _wrapper = GetComponent<ScreenWrapper>();

        if (_input == null)
        {
            Debug.LogError("Player requires a component implementing IInputReader " +
                            "(add PlayerInputReader to this GameObject).");
        }
    }

    private void Update()
    {
        Move();
        Shoot();
    }

    private void Move()
    {
        Vector2 move = _input.MoveInput;
        transform.Translate(new Vector3(move.x, move.y, 0f) * Time.deltaTime * speed);
        _wrapper.WrapIfNeeded();
    }

    private void Shoot()
    {
        if (_input.FirePressedThisFrame && _canShoot)
        {
            Instantiate(laserPrefab, transform.position + Vector3.up, Quaternion.identity);
            _canShoot = false;
            Invoke(nameof(ResetCooldown), shotCooldown);
        }
    }

    private void ResetCooldown() => _canShoot = true;

    public void TakeDamage(int amount)
    {
        GameEvents.PlayerDied();
        Destroy(gameObject);
    }
}
