using UnityEngine;
using UnityEngine.UI;

public class DamageUI : MonoBehaviour
{ 
    [Header("Flash Settings")]
    [SerializeField] private Image flashImage;
    [SerializeField, Range(0f, 1f)] private float maxAlpha = 0.5f;
    [SerializeField] private float flashDuration = 0.3f;

    private float flashTimer;
    private float currentAlpha;

    void Update()
    {
        if (flashTimer > 0)
        {
            flashTimer -= Time.deltaTime;

            // Smooth fade
            float targetAlpha = maxAlpha * (flashTimer / flashDuration);
            currentAlpha = Mathf.Lerp(currentAlpha, targetAlpha, Time.deltaTime * 10f);

            Color c = flashImage.color;
            c.a = currentAlpha;
            flashImage.color = c;
        }
        else if (currentAlpha > 0)
        {
            currentAlpha = Mathf.Lerp(currentAlpha, 0, Time.deltaTime * 10f);
            Color c = flashImage.color;
            c.a = currentAlpha;
            flashImage.color = c;
        }
    }

    public void TriggerFlash()
    {
        flashTimer = flashDuration;
    }
}
