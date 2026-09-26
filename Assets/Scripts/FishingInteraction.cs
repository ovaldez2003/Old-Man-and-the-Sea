using UnityEngine;
using UnityEngine.InputSystem;

public class FishingInteraction : MonoBehaviour
{
    public GameObject fishPromptText;   // Drag FishPromptText here
    public GameObject fishPromptImage;
    public FishingMinigameController minigameController;

    private bool playerInRange;

    void Awake()
    {
    if (minigameController == null)
        minigameController = FishingMinigameController.Instance;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            fishPromptText.SetActive(true);
            fishPromptImage.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            fishPromptText.SetActive(false);
            fishPromptImage.SetActive(false);
        }
    }

    void Update()
    {
        bool busy = minigameController != null && minigameController.IsBusy;

        fishPromptImage.SetActive(playerInRange && !busy);
        fishPromptText.SetActive(playerInRange && !busy);

        if (playerInRange && !busy && Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
        {
            StartFishing();
        }
    }

    void StartFishing()
    {
        minigameController.StartRandomMinigame(transform);
        // Add your fishing logic here
    }
}