using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class LevelUpManager : MonoBehaviour
{
    public static LevelUpManager instance;

    [Header("UI")]
    public GameObject levelUpPanel;
    public Button[] upgradeButtons;
    public TextMeshProUGUI[] buttonTexts;

    [Header("Upgrades")]
    public List<UpgradeOption> allUpgrades;

    private List<UpgradeOption> currentChoices = new List<UpgradeOption>();

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
        Debug.Log("Opening Level Up Panel");

        Time.timeScale = 0f;
        levelUpPanel.SetActive(true);

        GenerateRandomUpgrades();
    }

    void GenerateRandomUpgrades()
    {
        currentChoices.Clear();

        List<UpgradeOption> tempList = new List<UpgradeOption>(allUpgrades);

        for (int i = 0; i < 3; i++)
        {
            int index = Random.Range(0, tempList.Count);
            currentChoices.Add(tempList[index]);
            tempList.RemoveAt(index); // prevents duplicates
        }

        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            int choiceIndex = i;

            buttonTexts[i].text = currentChoices[i].displayName;

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
