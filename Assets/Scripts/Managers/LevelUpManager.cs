using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelUpManager : MonoBehaviour
{
    public static LevelUpManager instance;

    [Header("UI")]
    public GameObject levelUpPanel;
    public Button[] upgradeButtons;

    [Header("Upgrades")]
    public List<UpgradeOption> allUpgrades;

    private List<UpgradeOption> currentChoices = new List<UpgradeOption>();

    public CanvasGroup canvasGroup;

    void Awake()
    {
        instance = this;
    }
    System.Collections.IEnumerator 
    Start()
    {
        yield return null; // wait 1 frame

        PlayerXP.instance.onLevelUp += OpenLevelUp;
    }

    void OnEnable()
    {
        if (PlayerXP.instance != null)
            PlayerXP.instance.onLevelUp += OpenLevelUp;
    }

    void OnDisable()
    {
        if (PlayerXP.instance != null)
            PlayerXP.instance.onLevelUp -= OpenLevelUp;
    }

    void OpenLevelUp()
    {

        Time.timeScale = 0f;

        EventSystem.current.SetSelectedGameObject(null);
        levelUpPanel.SetActive(true);

        GenerateUpgradeOptions();

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        StartCoroutine(EnableUIAfterDelay());
    }

    IEnumerator EnableUIAfterDelay()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        while (Input.GetMouseButton(0))
            yield return null;

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    void GenerateUpgradeOptions()
    {
        currentChoices.Clear();

        // Add the upgrades in a fixed order
        for (int i = 0; i < 3; i++)
        {
            currentChoices.Add(allUpgrades[i]);

        }

        // Set button listeners
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            int choiceIndex = i;

            upgradeButtons[i].onClick.RemoveAllListeners();
            upgradeButtons[i].onClick.AddListener(() => ChooseUpgrade(choiceIndex));
        }
    }

    void ChooseUpgrade(int index)
    {
        ApplyUpgrade(currentChoices[index]);

        levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    void ApplyUpgrade(UpgradeOption upgrade)
    {
        PlayerStats stats = FindObjectOfType<PlayerStats>();
        switch (upgrade.type)
        {
            case UpgradeType.Health:
                stats.IncreaseHealth(50);
                break;

            case UpgradeType.Damage:
                stats.IncreaseDamage(5);
                break;

            case UpgradeType.Speed:
                stats.IncreaseSpeed(2f);
                break;
        }
    }
}
