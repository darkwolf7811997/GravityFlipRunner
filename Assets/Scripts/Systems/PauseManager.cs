using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;

    public void PauseGame()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.pauseSound);
    }

    public void ResumeGame()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.resumeSound);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}