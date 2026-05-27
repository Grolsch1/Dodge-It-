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
        GameEvents.OnGameReset?.Invoke();

        AudioManager.instance.PlayMusic("TitleMusic");
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
        AudioManager.instance.PlayMusic("GameMusic");
    }

    public void TogglePause()
    {
        if (GameEvents.CanPauseCheck != null && !GameEvents.CanPauseCheck())
            return;

        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;
        GameEvents.OnPause?.Invoke(isPaused);

        if (isPaused)
        {
            AudioManager.instance.PlayMusic("PauseMusic");
        }
        else
        {
            AudioManager.instance.PlayMusic("GameMusic");
        }
    }

    public void PlayerDied()
    {
        Time.timeScale = 0f;
        AudioManager.instance.PlayMusic("DeathMusic");
        GameEvents.OnPlayerDeath?.Invoke();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        AudioManager.instance.PlayMusic("TitleMusic");
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