using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private int value = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        CoinManager.Instance.AddCoins(value);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.coinSound);
        Destroy(gameObject);
    }
}