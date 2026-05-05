using UnityEngine;

public class DestroyOffscreen : MonoBehaviour
{
    [SerializeField] private float leftLimit = -20f;

    private void Update()
    {
        if (transform.position.x < leftLimit)
        {
            Destroy(gameObject);
        }
    }
}