using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Transform player;
    public float speedMultiplier = 0.3f;

    private float startX;
    private float spriteWidth;

    void Start()
    {
        startX = transform.position.x;
        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float movement = player.position.x * speedMultiplier;
        transform.position = new Vector3(startX + movement, transform.position.y, transform.position.z);

        // Loop infinito
        if (player.position.x > transform.position.x + spriteWidth)
        {
            startX += spriteWidth;
        }
    }
}