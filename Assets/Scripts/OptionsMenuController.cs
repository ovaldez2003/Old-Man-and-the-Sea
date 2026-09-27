using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class OptionsMenuController : MonoBehaviour
{
    public AudioMixer audioMixer;      // Your master mixer asset
    public Slider volumeSlider;

    void Start()
    {
        float saved = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        volumeSlider.value = saved;
        SetVolume(saved);
    }

    public void SetVolume(float value)
    {
        // Slider is linear 0-1, mixer expects decibels, so convert
        float dB = value > 0.0001f ? Mathf.Log10(value) * 20f : -80f;
        audioMixer.SetFloat("MasterVolume", dB);
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void OnReturnButton()
    {
        MenuNavigation.onReturnFromOptions?.Invoke();
        MenuNavigation.onReturnFromOptions = null;
        SceneManager.UnloadSceneAsync("Options");
    }
}