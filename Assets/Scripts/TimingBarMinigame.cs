using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimingBarMinigame : MonoBehaviour, IFishingMinigame
{
    public RectTransform marker;       // Moving indicator
    public RectTransform targetZone;   // Highlighted hit area
    public RectTransform barBounds;    // Full width of the bar

    public int hitsNeeded = 3;
    public int missesAllowed = 2;
    public float markerSpeed = 300f;   // Pixels per second
    public float minTargetWidth = 60f;
    public float maxTargetWidth = 100f;

    private Action<bool> onComplete;
    private int hits, misses;
    private float direction = 1f;

    public void StartGame(Action<bool> onComplete)
    {
        this.onComplete = onComplete;
        hits = 0;
        misses = 0;
        direction = 1f;

        marker.anchoredPosition = new Vector2(barBounds.rect.xMin, 0f);
        RandomizeTargetZone();

        StartCoroutine(RunLoop());
    }

    void RandomizeTargetZone()
    {
        float width = UnityEngine.Random.Range(minTargetWidth, maxTargetWidth);
        float halfBar = barBounds.rect.width / 2f;
        float x = UnityEngine.Random.Range(-halfBar + width / 2f, halfBar - width / 2f);

        targetZone.sizeDelta = new Vector2(width, targetZone.sizeDelta.y);
        targetZone.anchoredPosition = new Vector2(x, 0f);
    }

    IEnumerator RunLoop()
    {
        while (true)
        {
            float x = marker.anchoredPosition.x + direction * markerSpeed * Time.deltaTime;
            if (x > barBounds.rect.xMax) { x = barBounds.rect.xMax; direction = -1f; }
            if (x < barBounds.rect.xMin) { x = barBounds.rect.xMin; direction = 1f; }
            marker.anchoredPosition = new Vector2(x, 0f);

            if (Keyboard.current.fKey.wasPressedThisFrame)
            {
                bool inZone = x >= targetZone.anchoredPosition.x - targetZone.rect.width / 2f
                           && x <= targetZone.anchoredPosition.x + targetZone.rect.width / 2f;

                if (inZone)
                {
                    hits++;
                    if (hits >= hitsNeeded) { Finish(true); yield break; }
                    RandomizeTargetZone(); // move the zone so it's not trivial to repeat
                }
                else
                {
                    misses++;
                    if (misses >= missesAllowed) { Finish(false); yield break; }
                }
            }

            yield return null;
        }
    }

    void Finish(bool success)
    {
        onComplete?.Invoke(success);
    }
}