using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Coordinates spawning and win/lose state. No longer reaches into other
/// objects via GameObject.Find/GetComponent (that coupling was the original
/// script's biggest SOLID violation) - it just reacts to GameEvents.
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject meteorPrefab;
    [SerializeField] private GameObject bigMeteorPrefab;
    [SerializeField] private GameObject orbitingEnemyPrefab;

    [SerializeField] private float meteorSpawnInterval = 2f;
    [SerializeField] private int meteorsPerBigMeteor = 5;
    [SerializeField] private float orbitingEnemySpawnInterval = 6f;

    private bool _gameOver;
    private int _meteorCount;

    private void OnEnable()
    {
        GameEvents.OnMeteorDestroyed += HandleMeteorDestroyed;
        GameEvents.OnPlayerDied += HandlePlayerDied;
    }

    private void OnDisable()
    {
        GameEvents.OnMeteorDestroyed -= HandleMeteorDestroyed;
        GameEvents.OnPlayerDied -= HandlePlayerDied;
    }

    private void Start()
    {
        GameObject player = Instantiate(playerPrefab, transform.position, Quaternion.identity);

        // The player is created at runtime, so the virtual camera can't be
        // pointed at it in the Editor ahead of time - wire it up here instead.
        var virtualCamera = FindFirstObjectByType<CinemachineCamera>();
        if (virtualCamera != null)
        {
            virtualCamera.Follow = player.transform;
        }

        InvokeRepeating(nameof(SpawnMeteor), 1f, meteorSpawnInterval);
        InvokeRepeating(nameof(SpawnOrbitingEnemy), 4f, orbitingEnemySpawnInterval);
    }

    private void Update()
    {
        if (_gameOver && Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("Week5Lab");
        }
    }

    private void HandleMeteorDestroyed()
    {
        _meteorCount++;
        if (_meteorCount >= meteorsPerBigMeteor)
        {
            _meteorCount = 0;
            SpawnBigMeteor();
        }
    }

    private void HandlePlayerDied()
    {
        _gameOver = true;
        CancelInvoke();
    }

    private void SpawnMeteor()
    {
        Instantiate(meteorPrefab, new Vector3(Random.Range(-8, 8), 7.5f, 0), Quaternion.identity);
    }

    private void SpawnBigMeteor()
    {
        Instantiate(bigMeteorPrefab, new Vector3(Random.Range(-8, 8), 7.5f, 0), Quaternion.identity);
        GameEvents.BigMeteorSpawned();
    }

    private void SpawnOrbitingEnemy()
    {
        if (orbitingEnemyPrefab == null) return;
        Instantiate(orbitingEnemyPrefab, new Vector3(Random.Range(-8, 8), 7.5f, 0), Quaternion.identity);
    }
}