using UnityEngine;
using UnityEngine.InputSystem;

public class FishingInteraction : MonoBehaviour
{
    public GameObject fishPromptText;   // Drag FishPromptText here
    public GameObject fishPromptImage;
    public FishingMinigameController minigameController;

    private bool playerInRange;

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
        if (playerInRange && Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
        {
            StartFishing();
        }
    }

    void StartFishing()
    {
        minigameController.StartRandomMinigame();
        // Add your fishing logic here
    }
}