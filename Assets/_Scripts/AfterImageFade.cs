using UnityEngine;

public class AfterImageFade : MonoBehaviour
{
    private SpriteRenderer sr;
    private float alpha;
    private float alphaDecay = 1f; //rate at which the alpha decays

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        alpha = sr.color.a;
    }

    void Update()
    {
        alpha -= alphaDecay * Time.deltaTime;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);  // Keep RGB values, update alpha
        if (alpha <= 0f)
        {
            Destroy(gameObject);
        }
    }

    // Add a public method to set alphaDecay
    public void SetAlphaDecay(float newAlphaDecay)
    {
        alphaDecay = newAlphaDecay;
    }
}
