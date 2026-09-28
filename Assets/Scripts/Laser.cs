using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float despawnY = 11f;

    private void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);

        if (transform.position.y > despawnY)
        {
            Destroy(gameObject);
        }
    }
}
