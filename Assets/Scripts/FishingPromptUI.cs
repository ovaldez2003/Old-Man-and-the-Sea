using UnityEngine;

public class FishingPromptUI : MonoBehaviour
{
    public static FishingPromptUI Instance { get; private set; }

    public GameObject fishPromptText;
    public GameObject fishPromptImage;

    void Awake()
    {
        Instance = this;
    }

    public void SetVisible(bool visible)
    {
        fishPromptText.SetActive(visible);
        fishPromptImage.SetActive(visible);
    }
}