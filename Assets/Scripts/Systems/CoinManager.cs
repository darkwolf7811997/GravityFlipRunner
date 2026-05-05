using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    private const string TotalCoinsKey = "TOTAL_COINS";

    [Header("HUD")]
    [SerializeField] private TMP_Text runCoinsText;
    [SerializeField] private TMP_Text totalCoinsText;

    private int runCoins;
    private int totalCoins;

    public int RunCoins => runCoins;
    public int TotalCoins => totalCoins;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        totalCoins = PlayerPrefs.GetInt(TotalCoinsKey, 0);
        runCoins = 0;
        UpdateUI();
    }

    public void AddCoins(int amount)
    {
        runCoins += amount;
        totalCoins += amount;

        PlayerPrefs.SetInt(TotalCoinsKey, totalCoins);
        PlayerPrefs.Save();

        UpdateUI();
    }

    public void ResetRunCoins()
    {
        runCoins = 0;
        UpdateUI();
    }

    public int GetTotalCoins()
    {
        return totalCoins;
    }

    private void UpdateUI()
    {
        if (runCoinsText != null)
            runCoinsText.text = "Monedas: " + runCoins;

        if (totalCoinsText != null)
            totalCoinsText.text = "Total: " + totalCoins;
    }
}