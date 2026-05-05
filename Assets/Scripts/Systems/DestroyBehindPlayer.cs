using UnityEngine;

public class DestroyBehindPlayer : MonoBehaviour
{
    [SerializeField] private float distanceBehindPlayer = 25f;

    private Transform player;

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    private void Update()
    {
        if (player == null) return;

        if (transform.position.x < player.position.x - distanceBehindPlayer)
        {
            Destroy(gameObject);
        }
    }
}