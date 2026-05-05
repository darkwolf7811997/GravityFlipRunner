using UnityEngine;

public class ObstacleSpawnerFollow : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float offsetX = 10f;

    private void Update()
    {
        if (cameraTarget == null) return;

        transform.position = new Vector3(
            cameraTarget.position.x + offsetX,
            transform.position.y,
            transform.position.z
        );
    }
}