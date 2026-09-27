using UnityEngine;

public class AmbientMusicController : MonoBehaviour
{
    public static AmbientMusicController Instance { get; private set; }

    public AudioClip ambientTrack;
    [Range(0f, 1f)] public float volume = 0.25f;

    private AudioSource source;

    void Awake()
    {
        // If one already exists (came back from a scene reload), don't make a duplicate
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        source = gameObject.AddComponent<AudioSource>();
        source.clip = ambientTrack;
        source.loop = true;
        source.volume = volume;
        source.playOnAwake = false;
        source.Play();
    }
}