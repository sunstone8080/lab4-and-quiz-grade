using UnityEngine;

/// <summary>
/// Dependency Inversion: Player depends on this abstraction, not on the new
/// Input System (or the old Input Manager) directly. Swapping input backends,
/// adding rebindable controls, or unit-testing Player never requires touching
/// Player.cs.
/// </summary>
public interface IInputReader
{
    Vector2 MoveInput { get; }
    bool FirePressedThisFrame { get; }
}
