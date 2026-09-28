using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour, IInputReader
{
    private PlayerControls _controls;
    private bool _firePressedThisFrame;

    public Vector2 MoveInput { get; private set; }
    public bool FirePressedThisFrame => _firePressedThisFrame;

    private void Awake()
    {
        _controls = new PlayerControls();
        _controls.Gameplay.Fire.performed += _ => _firePressedThisFrame = true;


    }

    private void OnEnable() => _controls.Enable();
    private void OnDisable() => _controls.Disable();

    private void Update()
    {
        MoveInput = _controls.Gameplay.Move.ReadValue<Vector2>();
    }

    private void LateUpdate()
    {
        _firePressedThisFrame = false;



    }
}
