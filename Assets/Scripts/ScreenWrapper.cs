using UnityEngine;

/// <summary>
/// Single Responsibility: only knows how to wrap a transform's position
/// around screen bounds. Extracted out of Player so it's reusable (e.g. on
/// an enemy that should wrap instead of despawn) and so Player.cs isn't doing
/// movement, shooting AND bounds-checking all at once.
/// </summary>
public class ScreenWrapper : MonoBehaviour
{
    [SerializeField] private float horizontalLimit = 10f;
    [SerializeField] private float verticalLimit = 6f;

    public void WrapIfNeeded()
    {
        Vector3 pos = transform.position;

        if (pos.x > horizontalLimit || pos.x <= -horizontalLimit)
        {
            pos.x *= -1f;
        }
        if (pos.y > verticalLimit || pos.y <= -verticalLimit)
        {
            pos.y *= -1f;
        }

        transform.position = pos;
    }
}
