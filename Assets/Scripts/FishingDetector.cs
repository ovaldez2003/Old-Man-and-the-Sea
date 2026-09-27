using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class FishingDetector : MonoBehaviour
{
    public FishingMinigameController minigameController;

    private List<FishingInteraction> nearbySpots = new List<FishingInteraction>();
    private FishingInteraction closest;

    void OnTriggerEnter(Collider other)
    {
        FishingInteraction spot = other.GetComponent<FishingInteraction>();
        if (spot != null && !nearbySpots.Contains(spot))
            nearbySpots.Add(spot);
    }

    void OnTriggerExit(Collider other)
    {
        FishingInteraction spot = other.GetComponent<FishingInteraction>();
        if (spot != null)
            nearbySpots.Remove(spot);
    }

    void Update()
    {
        bool busy = minigameController != null && minigameController.IsBusy;

        // Find the nearest in-range spot, if any
        closest = null;
        float bestDist = float.MaxValue;
        foreach (var spot in nearbySpots)
        {
            if (spot == null) continue;
            float d = Vector3.Distance(transform.position, spot.transform.position);
            if (d < bestDist) { bestDist = d; closest = spot; }
        }

        bool shouldShow = closest != null && !busy;

        if (FishingPromptUI.Instance != null)
            FishingPromptUI.Instance.SetVisible(shouldShow);

        if (shouldShow && Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
        {
            closest.StartFishing();
        }
    }
}