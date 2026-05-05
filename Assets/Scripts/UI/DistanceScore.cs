using UnityEngine;
using TMPro;

public class DistanceScore : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;

    [Header("UI")]
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI bestDistanceText;

    [Header("Configuracion")]
    public string bestScoreKey = "BestDistance";
    public bool contarSoloSiEstaVivo = true;

    [Header("Debug")]
    public float currentDistance;
    public float bestDistance;

    private float startX;
    private PlayerHealth playerHealth;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("DistanceScore: No se asigno el Player.");
            enabled = false;
            return;
        }

        startX = player.position.x;

        playerHealth = player.GetComponent<PlayerHealth>();

        bestDistance = PlayerPrefs.GetFloat(bestScoreKey, 0f);
        UpdateBestUI();
        UpdateDistanceUI(0f);
    }

    void Update()
    {
        if (player == null) return;

        if (contarSoloSiEstaVivo && playerHealth != null && playerHealth.isDead)
        {
            return;
        }

        currentDistance = Mathf.Max(0f, player.position.x - startX);

        UpdateDistanceUI(currentDistance);

        if (currentDistance > bestDistance)
        {
            bestDistance = currentDistance;
            PlayerPrefs.SetFloat(bestScoreKey, bestDistance);
            PlayerPrefs.Save();
            UpdateBestUI();
        }
    }

    void UpdateDistanceUI(float distance)
    {
        if (distanceText != null)
        {
            distanceText.text = "Distancia: " + Mathf.FloorToInt(distance) + " m";
        }
    }

    void UpdateBestUI()
    {
        if (bestDistanceText != null)
        {
            bestDistanceText.text = "Record: " + Mathf.FloorToInt(bestDistance) + " m";
        }
    }

    public float GetCurrentDistance()
    {
        return currentDistance;
    }

    public float GetBestDistance()
    {
        return bestDistance;
    }

    public void ResetRunDistance()
    {
        if (player != null)
        {
            startX = player.position.x;
            currentDistance = 0f;
            UpdateDistanceUI(currentDistance);
        }
    }

    public void DeleteBestDistance()
    {
        PlayerPrefs.DeleteKey(bestScoreKey);
        bestDistance = 0f;
        UpdateBestUI();
    }
}