using UnityEngine;

public class SpeedZone : MonoBehaviour
{
    [Header("Efecto de velocidad")]
    [SerializeField] private float speedAmount = 1f;

    private bool used = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (used) return;

        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();

            if (player != null)
            {
                if (speedAmount > 0)
                {
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.speedZoneSound);
                    VibrationManager.Vibrate();
                }
                else if (speedAmount < 0)
                {
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.slowZoneSound);
                    VibrationManager.Vibrate();
                }

                player.AddZoneSpeedModifier(speedAmount);
                used = true;
            }
        }
    }
}