using UnityEngine;

public class GlowPulse : MonoBehaviour
{
    public Color baseColor = new Color(0.3f, 0.8f, 1f);
    public float pulseSpeed = 2f;
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;

    private Material mat;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    void Start()
    {
        mat = GetComponent<Renderer>().material; // instance, won't affect other objects using the same material
    }

    void Update()
    {
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
        mat.SetColor(EmissionColor, baseColor * intensity);
    }
}