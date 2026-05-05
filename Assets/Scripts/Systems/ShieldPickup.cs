using UnityEngine;

public class ShieldPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.ActivarEscudo();
            }
            AudioManager.Instance.PlaySFX(AudioManager.Instance.shieldPickupSound);
            Destroy(gameObject);
        }
    }
}