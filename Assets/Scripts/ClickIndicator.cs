using UnityEngine;

public class ClickIndicator : MonoBehaviour
{
    public float lifetime = 0.6f;
    private float timer;
    private SpriteRenderer clickIndicator;

    void Start()
    {
        clickIndicator = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        float scale = Mathf.Lerp(1f, 0f, timer /  lifetime);
        transform.localScale = Vector3.one * scale;

        Color c = clickIndicator.color;
        c.a = scale;
        clickIndicator.color = c;

        if (timer >= lifetime) 
            Destroy(gameObject);
        
    }
}
//need to fix the code so that the Indicator click disappears