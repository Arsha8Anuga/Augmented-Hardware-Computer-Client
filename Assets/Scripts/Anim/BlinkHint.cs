using UnityEngine;

public class BlinkHint : MonoBehaviour
{
    public float speed = 2f;
    public float minAlpha = 0.05f;
    public float maxAlpha = 0.25f;

    Renderer rend;
    MaterialPropertyBlock block;
    Color baseColor;
    Color emissionColor;
    float timeOffset; // tambah ini

    void Awake()
    {
        rend = GetComponent<Renderer>();
        block = new MaterialPropertyBlock();
        baseColor = rend.sharedMaterial.GetColor("_BaseColor");
        emissionColor = rend.sharedMaterial.GetColor("_EmissionColor"); // tambah ini

        timeOffset = Random.Range(0f, 100f);
        speed = Random.Range(0.5f, 1.2f);
    }

        void Update()
    {
        float t = Mathf.SmoothStep(0f, 1f, (Mathf.Sin((Time.time + timeOffset) * speed) + 1f) * 0.5f);

        float alpha = Mathf.Lerp(minAlpha, maxAlpha, t);

        Color c = baseColor;
        float intensity = Mathf.Lerp(0.2f, 0.5f, t);
        c.r *= intensity;
        c.g *= intensity;
        c.b *= intensity;
        c.a = alpha;

        // glow
        float emissionIntensity = Mathf.Lerp(0f, 1.5f, t);
        Color emission = emissionColor * emissionIntensity;

        rend.GetPropertyBlock(block);
        block.SetColor("_BaseColor", c);
        block.SetColor("_EmissionColor", emission);
        rend.SetPropertyBlock(block);
    }
}