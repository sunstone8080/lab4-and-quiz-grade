using UnityEngine;

public interface IInputReader
{
    Vector2 MoveInput { get; }
    bool FirePressedThisFrame { get; }
}
