using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

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
    [SerializeField] private TextMeshProUGUI waveText;

    [Header("Cutscene")]
    public GameObject cutscenePanel;
    public UnityEngine.UI.Image cutsceneImage;
    private bool canSkipCutscene = false;
    [SerializeField] private float skipDelay = 0.75f; // tweak this

    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if (cutscenePanel.activeSelf && canSkipCutscene && Input.anyKeyDown)
        {
            GameEvents.OnHideCutscene?.Invoke();
        }
    }

    private void OnEnable()
    {
        GameEvents.OnGameReset += HandleReset;
        GameEvents.OnGameStart += HandleGameStart;
        GameEvents.OnPause += HandlePause;
        GameEvents.OnPlayerDeath += HandleDeath;
        GameEvents.OnKillUpdated += UpdateKillUI;
        GameEvents.OnVictory += HandleVictory;
        GameEvents.OnWaveUpdated += UpdateWaveUI;

        GameEvents.CanPauseCheck += CanPause;
        GameEvents.OnShowCutscene += ShowCutscene;
        GameEvents.OnHideCutscene += HideCutscene;
    }

    private void OnDisable()
    {
        GameEvents.OnGameReset -= HandleReset;
        GameEvents.OnGameStart -= HandleGameStart;
        GameEvents.OnPause -= HandlePause;
        GameEvents.OnPlayerDeath -= HandleDeath;
        GameEvents.OnKillUpdated -= UpdateKillUI;
        GameEvents.OnVictory -= HandleVictory;
        GameEvents.OnWaveUpdated -= UpdateWaveUI;

        GameEvents.CanPauseCheck -= CanPause;
        GameEvents.OnShowCutscene -= ShowCutscene;
        GameEvents.OnHideCutscene -= HideCutscene;
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
        return !deathScreen.activeSelf
            && !startMenu.activeSelf
            && !cutscenePanel.activeSelf;
    }

    void UpdateWaveUI(int wave)
    {
        waveText.text = "Wave: " + wave;
    }

    void ShowCutscene(Sprite image)
    {
        cutscenePanel.SetActive(true);
        cutsceneImage.sprite = image;

        Time.timeScale = 0f;

        canSkipCutscene = false;
        StartCoroutine(EnableSkipAfterDelay());
    }

    void HideCutscene()
    {
        cutscenePanel.SetActive(false);
        Time.timeScale = 1f;
    }
    System.Collections.IEnumerator EnableSkipAfterDelay()
    {
        yield return new WaitForSecondsRealtime(skipDelay);
        canSkipCutscene = true;
    }
}