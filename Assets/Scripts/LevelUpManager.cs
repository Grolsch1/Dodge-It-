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
        PlayerController player = FindObjectOfType<PlayerController>();
        PlayerHealth health = FindObjectOfType<PlayerHealth>();
        
        switch (type)
        {
            case "Health":
                health.IncreaseMaxHealth(20);
                break;

            case "Damage":
                player.IncreaseDamage(5);
                break;

            case "Speed":
                player.IncreaseSpeed(1.5f);
                break;
                
            default:
                Debug.LogWarning("Unknown upgrade type: " + type);
                break;
        }
    }
}
