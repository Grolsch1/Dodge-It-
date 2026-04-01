using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private Image healthFill;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI speedText;

    [Header("XP")]
    [SerializeField] private Image xpFill;
    [SerializeField] private TextMeshProUGUI levelText;
    private float targetXP;

    void Update()
    {
        xpFill.fillAmount = Mathf.Lerp(xpFill.fillAmount, targetXP, Time.deltaTime * 10f);
    } 

    public void UpdateHealth(int current, int max)
    {
        float percent = (float)current / max;

        healthFill.fillAmount = percent;
        healthText.text = current + " / " + max;
    }

    public void UpdateXP(int currentXP, int requiredXP, int level)
    {
        targetXP = (float)currentXP / requiredXP;
        levelText.text = "LVL " + level;
    }
    public void UpdateStats(PlayerStats stats)
    {
        damageText.text = "DAMAGE: " + stats.damage;
        speedText.text = "SPEED: " + stats.moveSpeed;
    }
}
