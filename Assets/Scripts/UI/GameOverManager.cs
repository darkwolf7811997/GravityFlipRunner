using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI finalDistanceText;
    [SerializeField] private TextMeshProUGUI bestDistanceText;
    [SerializeField] private TextMeshProUGUI runCoinsText;
    [SerializeField] private TextMeshProUGUI totalCoinsText;

    [Header("Referencias")]
    [SerializeField] private DistanceScore distanceScore;
    [SerializeField] private CoinManager coinManager;

    public void ShowGameOver()
    {
        gameOverUI.SetActive(true);

        if (distanceScore != null)
        {
            finalDistanceText.text = "Distancia: " + Mathf.FloorToInt(distanceScore.GetCurrentDistance()) + " m";
            bestDistanceText.text = "Record: " + Mathf.FloorToInt(distanceScore.GetBestDistance()) + " m";
        }

        if (coinManager != null)
        {
            runCoinsText.text = "Monedas partida: " + coinManager.RunCoins;
            totalCoinsText.text = "Monedas totales: " + coinManager.TotalCoins;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void ReviveWithAd()
    {
        Debug.Log("Aquí irá el anuncio recompensado para revivir.");
    }
}