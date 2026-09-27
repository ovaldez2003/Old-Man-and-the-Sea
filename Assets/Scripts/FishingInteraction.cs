using UnityEngine;

public class FishingInteraction : MonoBehaviour
{
    public FishingMinigameController minigameController;

    void Awake()
    {
        if (minigameController == null)
            minigameController = FishingMinigameController.Instance;
    }

    public void StartFishing()
    {
        minigameController.StartRandomMinigame(transform, this);
    }
}