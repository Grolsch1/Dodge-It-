using UnityEngine;

public class ClickIndicator : MonoBehaviour
{
    public float lifetime = 0.5f;
    private float timer;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        float scale = Mathf.Lerp(1f, 0f, timer /  lifetime);
        transform.localScale = Vector3.one * scale;

        Color c = sr.color;
        c.a = scale;
        sr.color = c;

        if (timer >= lifetime) 
            Destroy(gameObject);
        
    }
}
//need to fix the code so that the Indicator click disappears