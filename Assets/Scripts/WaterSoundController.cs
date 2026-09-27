using UnityEngine;

public class WaterSoundController : MonoBehaviour
{
    public AudioClip waterLoop;
    [Range(0f, 1f)] public float maxVolume = 0.6f;
    public float fadeSpeed = 2f;        // How quickly volume ramps up/down
    public float speedForMaxVolume = 5f; // Speed at which volume caps out

    private Rigidbody rb;
    private AudioSource source;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        source = gameObject.AddComponent<AudioSource>();
        source.clip = waterLoop;
        source.loop = true;
        source.volume = 0f;
        source.playOnAwake = false;
        source.Play(); // Always playing, just silent when volume is 0
    }

    void Update()
    {
        float speed = rb.linearVelocity.magnitude;
        float targetVolume = Mathf.Clamp01(speed / speedForMaxVolume) * maxVolume;
        source.volume = Mathf.MoveTowards(source.volume, targetVolume, fadeSpeed * Time.deltaTime);
    }
}