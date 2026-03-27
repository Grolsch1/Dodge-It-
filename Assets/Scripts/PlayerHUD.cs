using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using Unity.VisualScripting;

public class PlayerHUD : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private Image healthFill;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI speedText;

    public void UpdateHealth(int current, int max)
    {
        float percent = (float)current / max;

        healthFill.fillAmount = percent;
        healthText.text = current + " / " + max;
    }

    public void UpdateStats(float damage, float fireRate, float speed)
{
    damageText.text = "DMG: " + damage.ToString("F1");
    speedText.text = "SPD: " + speed.ToString("F1");
}
}
