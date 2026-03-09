using UnityEngine;
using UnityEngine.UIElements;

public class ClickIndicator : MonoBehaviour
{
    public float lifetime = 0.6f;
    private float timer = 0f;
    private SpriteRenderer clickIndicator;

    void Start()
    {
        clickIndicator = GetComponentInChildren<SpriteRenderer>();
        Destroy(gameObject, 0.5f);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        float time = timer / lifetime;

        float scale = Mathf.Lerp(1f, 0f, timer / lifetime);
        transform.localScale = Vector3.one * scale;

        Color color = clickIndicator.color;
        Color c = color;
        c.a = scale;
        clickIndicator.color = c;
    }

}
//Can look to replace update with animations/particle effects for the indicator to make game less reliant on code