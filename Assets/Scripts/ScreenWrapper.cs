using UnityEngine;
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
