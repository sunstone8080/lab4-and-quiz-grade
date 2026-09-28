using UnityEngine;
using Unity.Cinemachine;

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
        if (virtualCamera == null)
        {
            return;
        }
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