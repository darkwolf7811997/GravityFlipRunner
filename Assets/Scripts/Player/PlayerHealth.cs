using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Estado")]
    public bool tieneEscudo = false;
    public bool esInvulnerable = false;

    [Header("Configuración")]
    public float tiempoInvulnerable = 1.5f;

    [Header("Layers")]
    public string playerLayerName = "Player";
    public string obstacleLayerName = "Obstacle";

    private bool estaVivo = true;
    private SpriteRenderer sr;
    private int playerLayer;
    private int obstacleLayer;
    public bool isDead = false;

    private Animator animator;

    private void Awake()
    {
        // Ahora el SpriteRenderer está en Player > Visual,
        // por eso lo buscamos también en los hijos.
        sr = GetComponentInChildren<SpriteRenderer>();

        playerLayer = LayerMask.NameToLayer(playerLayerName);
        obstacleLayer = LayerMask.NameToLayer(obstacleLayerName);
        animator = GetComponentInChildren<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!estaVivo) return;

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            RecibirGolpe();
            // Destruir SOLO si sigue siendo obstáculo (doble seguridad)
            if (collision.gameObject.layer == obstacleLayer)
            {
                Destroy(collision.gameObject);
            }
        }
        
    }

    public void RecibirGolpe()
    {
        if (!estaVivo || esInvulnerable) return;

        if (externalInvulnerable) return;

        if (tieneEscudo)
        {
            PerderEscudo();
        }
        else
        {
            Morir();
        }
    }

    private void PerderEscudo()
    {
        tieneEscudo = false;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.shieldBreakSound);
        }

        VibrationManager.Vibrate();

        StartCoroutine(Invulnerabilidad());
    }

    private IEnumerator Invulnerabilidad()
    {
        esInvulnerable = true;

        if (playerLayer != -1 && obstacleLayer != -1)
        {
            Physics2D.IgnoreLayerCollision(playerLayer, obstacleLayer, true);
        }

        float tiempoParpadeo = 0.1f;
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < tiempoInvulnerable)
        {
            if (sr != null)
            {
                sr.enabled = !sr.enabled;
            }

            yield return new WaitForSeconds(tiempoParpadeo);
            tiempoTranscurrido += tiempoParpadeo;
        }

        if (sr != null)
        {
            sr.enabled = true;
        }

        if (playerLayer != -1 && obstacleLayer != -1)
        {
            Physics2D.IgnoreLayerCollision(playerLayer, obstacleLayer, false);
        }

        esInvulnerable = false;
    }

    private void Morir()
    {
        if (isDead) return;

        isDead = true;
        estaVivo = false;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.deathSound);
        }

        VibrationManager.Vibrate();

        // Dispara animación de muerte
        if (animator != null)
        {
            animator.Play("Cat_Death");
        }
        

        // Espera antes de Game Over
        StartCoroutine(MuerteConDelay());
    }

    public void ActivarEscudo()
    {
        tieneEscudo = true;
    }

    public bool EstaVivo()
    {
        return estaVivo;
    }

    private IEnumerator MuerteConDelay()
    {
        yield return new WaitForSeconds(1f); // ajustable

        GameManager.Instance.GameOver();
    }


    //PARA QUE NO MUERA CON OBSTACULOS
    private bool externalInvulnerable;

    public void SetExternalInvulnerable(bool value)
    {
        externalInvulnerable = value;
    }

    public void ActivateInvulnerability(float duration)
    {
        StartCoroutine(InvulnerabilidadTemporal(duration));
    }

    private IEnumerator InvulnerabilidadTemporal(float duration)
    {
        esInvulnerable = true;

        if (playerLayer != -1 && obstacleLayer != -1)
        {
            Physics2D.IgnoreLayerCollision(playerLayer, obstacleLayer, true);
        }

        float tiempoParpadeo = 0.1f;
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duration)
        {
            if (sr != null)
            {
                sr.enabled = !sr.enabled;
            }

            yield return new WaitForSeconds(tiempoParpadeo);
            tiempoTranscurrido += tiempoParpadeo;
        }

        if (sr != null)
        {
            sr.enabled = true;
        }

        if (playerLayer != -1 && obstacleLayer != -1)
        {
            Physics2D.IgnoreLayerCollision(playerLayer, obstacleLayer, false);
        }

        esInvulnerable = false;
    }
}