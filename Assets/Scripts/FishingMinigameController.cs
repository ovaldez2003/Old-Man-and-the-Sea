using UnityEngine;
using System.Collections.Generic;

public class FishingMinigameController : MonoBehaviour
{
    public PlayerController player;
    public GameObject qtePanel;
    public GameObject timingBarPanel;
    public GameObject rhythmPanel;

    public void StartRandomMinigame()
    {
        List<GameObject> available = new List<GameObject>();
        if (qtePanel != null) available.Add(qtePanel);
        if (timingBarPanel != null) available.Add(timingBarPanel);
        if (rhythmPanel != null) available.Add(rhythmPanel);

        if (available.Count == 0)
        {
            Debug.LogWarning("No minigame panels assigned on FishingMinigameController.");
            return;
        }

        player.SetControlsLocked(true);

        GameObject chosenPanel = available[Random.Range(0, available.Count)];
        chosenPanel.SetActive(true);

        IFishingMinigame minigame = chosenPanel.GetComponent<IFishingMinigame>();
        minigame.StartGame(success => OnMinigameComplete(success, chosenPanel));
    }

    void OnMinigameComplete(bool success, GameObject panel)
    {
        Debug.Log(success ? "Caught a fish!" : "The fish escaped.");
        panel.SetActive(false);
        player.SetControlsLocked(false);
    }
}