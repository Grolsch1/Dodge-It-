using UnityEngine;

public class LevelUpManager : MonoBehaviour
{
    public static LevelUpManager instance;

    [Header("UI")]
    public GameObject levelUpPanel;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        PlayerXP.instance.onLevelUp += OpenLevelUp;
    }

    void OpenLevelUp()
    {
        Time.timeScale = 0f;
        levelUpPanel.SetActive(true);
    }

    public void ChooseUpgrade(string upgradeType)
    {
        ApplyUpgrade(upgradeType);
        levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    void ApplyUpgrade(string type)
{
    PlayerStats stats = FindObjectOfType<PlayerStats>();

    switch (type)
    {
        case "Health":
            stats.IncreaseHealth(20);
            break;

        case "Damage":
            stats.IncreaseDamage(5);
            break;

        case "Speed":
            stats.IncreaseSpeed(1.5f);
            break;
    }
}
}
