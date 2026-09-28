using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject meteorPrefab;
    [SerializeField] private GameObject bigMeteorPrefab;
    [SerializeField] private GameObject orbitingEnemyPrefab;
    [SerializeField] private float meteorSpawnInterval = 2f;
    [SerializeField] private int meteorsPerBigMeteor = 5;
    [SerializeField] private float orbitingEnemySpawnInterval = 6f;
    [SerializeField] private bool _gameOver;
    [SerializeField] private int _meteorCount;

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

        //Finds player at runtime because its instantiated
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