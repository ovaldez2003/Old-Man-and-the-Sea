using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FishingMinigameController : MonoBehaviour
{
    public static FishingMinigameController Instance { get; private set; }

    public PlayerController player;
    public GameObject qtePanel;
    public GameObject timingBarPanel;
    public GameObject rhythmPanel;
    public FishingCameraController cameraController;
    public float delayBeforeMinigame = 1f;
    public CatchDisplayController catchDisplay;
    public DialogueController dialogueController; 
    public EndGameController endGameController;  // Seconds to wait after the camera swings

    void Awake()
    {
        Instance = this;
    }

    public bool IsBusy{ get; private set; }

    public void StartRandomMinigame(Transform lookTarget, FishingInteraction spot)
    {

        if (IsBusy) return;

        List<GameObject> available = new List<GameObject>();
        if (qtePanel != null) available.Add(qtePanel);
        if (timingBarPanel != null) available.Add(timingBarPanel);
        if (rhythmPanel != null) available.Add(rhythmPanel);

        if (available.Count == 0)
        {
            Debug.LogWarning("No minigame panels assigned on FishingMinigameController.");
            return;
        }

        IsBusy = true;
        StartCoroutine(RunFishingSequence(available, lookTarget, spot));
    }

    IEnumerator RunFishingSequence(List<GameObject> available, Transform lookTarget, FishingInteraction spot)
    {
        player.SetControlsLocked(true);
        cameraController.ShowFishingView(lookTarget);

        yield return new WaitForSeconds(delayBeforeMinigame);

        GameObject chosenPanel = available[Random.Range(0, available.Count)];
        chosenPanel.SetActive(true);

        IFishingMinigame minigame = chosenPanel.GetComponent<IFishingMinigame>();
        minigame.StartGame(success => OnMinigameComplete(success, chosenPanel, spot));
    }

    void OnMinigameComplete(bool success, GameObject panel, FishingInteraction spot)
    {
        panel.SetActive(false);

        if (success)
        {
            CatchableItem item = CatchProgressTracker.Instance != null
                ? CatchProgressTracker.Instance.GetNextCatch()
                : null;

            if (item != null)
            {
                catchDisplay.Show(item, () =>
                {
                    dialogueController.StartDialogue(item.dialogueLines, () =>
                    {
                        if (CatchProgressTracker.Instance.IsGameComplete)
                            endGameController.Show();
                        else
                            FinishFishing(spot);
                    });
                });
            }
            else
            {
                FinishFishing(spot);
            }
        }
        else
        {
            FinishFishing(spot);
        }
    }

    void FinishFishing(FishingInteraction spot)
    {
        player.SetControlsLocked(false);
        cameraController.ReturnToBoatView();
        IsBusy = false;

        if (FishingSpotSpawner.Instance != null)
            FishingSpotSpawner.Instance.NotifySpotFinished(spot.gameObject);
    }
}