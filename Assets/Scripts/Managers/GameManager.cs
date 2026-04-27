using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isGamePlaying { get; private set; }
    private bool isPaused = false;

    private int enemiesKilled = 0;
    [SerializeField] public Sprite startCutsceneSprite;
    [SerializeField] public Sprite midCutsceneSprite;
    [SerializeField] public Sprite endCutsceneSprite;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        isGamePlaying = false;
        Time.timeScale = 0f;

        enemiesKilled = 0;

        GameEvents.OnKillUpdated?.Invoke(enemiesKilled);
        GameEvents.OnGameReset?.Invoke(); // tells UI to show start menu
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void StartGame()
    {
        isGamePlaying = true;
        Time.timeScale = 1f;

        GameEvents.OnGameStart?.Invoke();
        GameEvents.OnShowCutscene?.Invoke(startCutsceneSprite);
        AudioManager.instance.PlayMusic("Music");
    }

    public void TogglePause()
    {
        if (GameEvents.CanPauseCheck != null && !GameEvents.CanPauseCheck())
            return;

        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;
        GameEvents.OnPause?.Invoke(isPaused);
    }

    public void PlayerDied()
    {
        Time.timeScale = 0f;
        GameEvents.OnPlayerDeath?.Invoke();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void AddKill()
    {
        enemiesKilled++;

        GameEvents.OnKillUpdated?.Invoke(enemiesKilled);
    }

    private void Victory()
    {
        Time.timeScale = 0f;
        GameEvents.OnShowCutscene?.Invoke(endCutsceneSprite);
        GameEvents.OnVictory?.Invoke(enemiesKilled);
    }

    public void TriggerVictory()
    {
        Victory();
    }

    public void OnBossKilled()
    {
        Victory();
    }

    public void OnMiniBossKilled()
    {
        Time.timeScale = 0f;
        GameEvents.OnShowCutscene?.Invoke(midCutsceneSprite);
    }
}