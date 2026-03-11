using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI")]
    public GameObject pauseMenu;
    public GameObject deathScreen;
    public GameObject startMenu;

    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private TMPro.TextMeshProUGUI victoryText;
    [SerializeField] private TextMeshProUGUI killCounterText;

    bool isPaused = false;
    public bool isGamePlaying { get; private set; }
    private int enemiesKilled = 0;
    private int totalEnemies = 0;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {
        isGamePlaying = false;
        Time.timeScale = 0f;

        enemiesKilled = 0;
        totalEnemies = FindObjectsOfType<TurretEnemy>().Length;
        UpdateKillUI();
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
        startMenu.SetActive(false);
        isGamePlaying = true;
        Time.timeScale = 1f;
    }

    public void TogglePause()
    {
        if (deathScreen.activeSelf || startMenu.activeSelf)
            return;

        isPaused = !isPaused;

        pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void PlayerDied()
    {
        deathScreen.SetActive(true);
        Time.timeScale = 0f;
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
        UpdateKillUI();

        if (enemiesKilled >= totalEnemies)
        {
            Victory();
        }
    }

    void UpdateKillUI()
    {
        if (killCounterText != null)
            killCounterText.text = "Kills: " + enemiesKilled;
    }

    void Victory()
    {
        Time.timeScale = 0f;

        if (victoryScreen != null)
            victoryScreen.SetActive(true);

        if (victoryScreen != null)
            victoryText.text = $"Victory! \nEnemies Deafeated: {enemiesKilled}";
    }
}
