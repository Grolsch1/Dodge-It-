using UnityEngine;
using UnityEngine.UI;  

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Image healthFill;
    [SerializeField] private PlayerHealth playerHealth;

    void Update()
    {
        if (playerHealth == null) return;

        float percent = (float)playerHealth.currentHealth / playerHealth.maxHealth;
        healthFill.fillAmount = Mathf.Lerp(healthFill.fillAmount, percent, Time.deltaTime * 10f);
    }
}
