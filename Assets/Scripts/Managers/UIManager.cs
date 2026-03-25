using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Menus")]
    public GameObject pauseMenu;
    public GameObject deathScreen;
    public GameObject startMenu;
    public GameObject victoryScreen;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI victoryText;
    [SerializeField] private TextMeshProUGUI killCounterText;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        GameEvents.OnGameReset += HandleReset;
        GameEvents.OnGameStart += HandleGameStart;
        GameEvents.OnPause += HandlePause;
        GameEvents.OnPlayerDeath += HandleDeath;
        GameEvents.OnKillUpdated += UpdateKillUI;
        GameEvents.OnVictory += HandleVictory;

        GameEvents.CanPauseCheck += CanPause;
    }

    private void OnDisable()
    {
        GameEvents.OnGameReset -= HandleReset;
        GameEvents.OnGameStart -= HandleGameStart;
        GameEvents.OnPause -= HandlePause;
        GameEvents.OnPlayerDeath -= HandleDeath;
        GameEvents.OnKillUpdated -= UpdateKillUI;
        GameEvents.OnVictory -= HandleVictory;

        GameEvents.CanPauseCheck -= CanPause;
    }

    // ---------- EVENT HANDLERS ----------

    void HandleGameStart()
    {
        startMenu.SetActive(false);
        pauseMenu.SetActive(false);
        deathScreen.SetActive(false);
        victoryScreen.SetActive(false);
    }
    void HandleReset()
    {
        startMenu.SetActive(true);
        pauseMenu.SetActive(false);
        deathScreen.SetActive(false);
        victoryScreen.SetActive(false);
    }
    public void ShowStartMenu()
    {
        startMenu.SetActive(true);
    }
    void HandlePause(bool isPaused)
    {
        pauseMenu.SetActive(isPaused);
    }

    void HandleDeath()
    {
        deathScreen.SetActive(true);
    }

    void HandleVictory(int enemiesKilled)
    {
        victoryScreen.SetActive(true);
        victoryText.text = $"Victory!\nEnemies Defeated: {enemiesKilled}";
    }

    void UpdateKillUI(int kills)
    {
        if (killCounterText != null)
            killCounterText.text = "Kills: " + kills;
    }

    bool CanPause()
    {
        return !deathScreen.activeSelf && !startMenu.activeSelf;
    }
}