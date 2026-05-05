using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    [Header("SFX Clips")]
    public AudioClip coinSound;
    public AudioClip shieldPickupSound;
    public AudioClip shieldBreakSound;
    public AudioClip deathSound;
    public AudioClip speedZoneSound;
    public AudioClip slowZoneSound;
    public AudioClip pauseSound;
    public AudioClip resumeSound;

    [Header("Music Playlists")]
    [SerializeField] private AudioClip[] menuMusicPlaylist;
    [SerializeField] private AudioClip[] gameplayMusicPlaylist;

    private AudioClip[] currentPlaylist;
    private AudioClip lastSong;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "MainMenu")
        {
            SetPlaylist(menuMusicPlaylist);
        }
        else
        {
            SetPlaylist(gameplayMusicPlaylist);
        }
    }
    private void Update()
    {
        if (!musicSource.isPlaying && currentPlaylist != null && currentPlaylist.Length > 0)
        {
            PlayRandomMusic();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        sfxSource.PlayOneShot(clip);
    }

    private void PlayRandomMusic()
    {
        if (currentPlaylist == null || currentPlaylist.Length == 0) return;

        AudioClip selectedSong;

        do
        {
            int randomIndex = Random.Range(0, currentPlaylist.Length);
            selectedSong = currentPlaylist[randomIndex];
        }
        while (selectedSong == lastSong && currentPlaylist.Length > 1);

        lastSong = selectedSong;

        musicSource.clip = selectedSong;
        musicSource.loop = false;
        musicSource.Play();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            SetPlaylist(menuMusicPlaylist);
        }
        else
        {
            SetPlaylist(gameplayMusicPlaylist);
        }
    }

    private void SetPlaylist(AudioClip[] newPlaylist)
    {
        if (currentPlaylist == newPlaylist) return;

        currentPlaylist = newPlaylist;
        lastSong = null;

        musicSource.Stop();
        PlayRandomMusic();
    }
}