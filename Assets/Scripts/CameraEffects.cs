using UnityEngine;
using Unity.Cinemachine;

/// <summary>
/// Camera "juice" for Cinemachine 3.x: screen shake on every enemy kill,
/// temporary zoom-out when a BigMeteor spawns. Reacts only to GameEvents -
/// no references to GameManager, Player, or any enemy script.
///
/// SETUP (in the Editor, Cinemachine 3.x):
/// 1. Package Manager -> Add package by name -> com.unity.cinemachine
///    (3.0.0+ / 3.1.x). Remove any 2.x Cinemachine package first if present.
/// 2. Your scene camera needs a CinemachineBrain (Main Camera -> Add
///    Component -> Cinemachine Brain) and a CinemachineCamera GameObject
///    (GameObject -> Cinemachine -> Cinemachine Camera) tracking the player.
/// 3. Add a Cinemachine Impulse Listener to Main Camera.
/// 4. Add this component to any GameObject (a "GameFX" empty works well) -
///    it auto-adds the required CinemachineImpulseSource to itself.
/// 5. Drag your CinemachineCamera into the "Virtual Camera" field.
/// 6. Tune shakeForce / zoomedOutSize / zoomDuration to taste.
/// </summary>
[RequireComponent(typeof(CinemachineImpulseSource))]
public class CameraEffects : MonoBehaviour
{
    [SerializeField] private CinemachineCamera virtualCamera;
    [SerializeField] private float shakeForce = 0.5f;
    [SerializeField] private float zoomedOutSize = 8f;
    [SerializeField] private float zoomDuration = 3f;
    [SerializeField] private float zoomLerpSpeed = 2f;

    private CinemachineImpulseSource _impulseSource;
    private float _defaultSize;
    private float _targetSize;

    private void Awake()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        if (virtualCamera != null)
        {
            _defaultSize = virtualCamera.Lens.OrthographicSize;
            _targetSize = _defaultSize;
        }
    }

    private void OnEnable()
    {
        GameEvents.OnEnemyDestroyed += HandleEnemyDestroyed;
        GameEvents.OnBigMeteorSpawned += HandleBigMeteorSpawned;
    }

    private void OnDisable()
    {
        GameEvents.OnEnemyDestroyed -= HandleEnemyDestroyed;
        GameEvents.OnBigMeteorSpawned -= HandleBigMeteorSpawned;
    }

    private void Update()
    {
        if (virtualCamera == null) return;

        var lens = virtualCamera.Lens;
        lens.OrthographicSize = Mathf.Lerp(lens.OrthographicSize, _targetSize, zoomLerpSpeed * Time.deltaTime);
        virtualCamera.Lens = lens;
    }

    private void HandleEnemyDestroyed(Vector3 position)
    {
        _impulseSource.GenerateImpulse(shakeForce);
    }

    private void HandleBigMeteorSpawned()
    {
        _targetSize = zoomedOutSize;
        CancelInvoke(nameof(ResetZoom));
        Invoke(nameof(ResetZoom), zoomDuration);
    }

    private void ResetZoom()
    {
        _targetSize = _defaultSize;
    }
}