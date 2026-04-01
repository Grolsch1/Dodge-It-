using UnityEngine;
using TMPro;
using System.Collections;

public class WavePopupUI : MonoBehaviour
{
    public static WavePopupUI instance;

    [SerializeField] private GameObject popupObject;
    [SerializeField] private TextMeshProUGUI popupText;

    [SerializeField] private float displayTime = 3f;

    private void Awake()
    {
        instance = this;
    }

    public void ShowWave(int waveNumber, bool isBoss)
    {
        StopAllCoroutines();

        if (isBoss)
        {
            popupText.text = "BOSS WAVE";
            popupText.color = Color.red;
            popupText.fontSize = 100;
        }
        else
        {
            popupText.text = "WAVE\n" + waveNumber;
            popupText.color = Color.whiteSmoke;
            popupText.fontSize = 80;
        }

        StartCoroutine(ShowRoutine());
    }

    private IEnumerator ShowRoutine()
    {
        popupObject.SetActive(true);

        yield return new WaitForSeconds(displayTime);

        popupObject.SetActive(false);
    }
}