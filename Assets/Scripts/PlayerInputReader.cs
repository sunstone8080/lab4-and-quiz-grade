using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Concrete IInputReader backed by Unity's new Input System.
///
/// SETUP (one-time, in the Editor):
/// 1. Install the "Input System" package (Window > Package Manager).
/// 2. In Project Settings > Player > Active Input Handling, set it to
///    "Input System Package (New)" (or "Both" if you still need the old one elsewhere).
/// 3. Create an Input Actions asset, e.g. Assets/Input/PlayerControls.inputactions,
///    with an action map called "Gameplay" containing:
///      - "Move"  (Value, Vector2) with a 2D Vector composite bound to WASD /
///        arrow keys, and optionally a gamepad left stick binding.
///      - "Fire"  (Button) bound to Space (and optionally gamepad South button).
/// 4. In the asset's importer settings, tick "Generate C# Class" so Unity
///    creates a `PlayerControls` class (matches the name used below).
/// 5. Add this component to the Player GameObject alongside Player.cs.
/// </summary>
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
        // Cleared after every other Update this frame has had a chance to read it.
        _firePressedThisFrame = false;
    }
}
