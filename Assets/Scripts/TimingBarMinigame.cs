using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimingBarMinigame : MonoBehaviour, IFishingMinigame
{
    public RectTransform marker;
    public RectTransform targetZone;
    public RectTransform barBounds;

    public int hitsNeeded = 3;
    public int missesAllowed = 2;
    public float markerSpeed = 300f;
    public float minTargetWidth = 60f;
    public float maxTargetWidth = 100f;

    public bool loopDemo = false;
    public float pauseBetweenLoops = 1f;

    private Action<bool> onComplete;
    private int hits, misses;
    private float direction = 1f;
    private Coroutine activeRoutine;

    void OnEnable()
    {
        if (loopDemo)
            activeRoutine = StartCoroutine(DemoLoop());
    }

    void OnDisable()
    {
        if (activeRoutine != null)
            StopCoroutine(activeRoutine);
    }

    public void StartGame(Action<bool> onComplete)
    {
        this.onComplete = onComplete;
        hits = 0;
        misses = 0;
        direction = 1f;

        marker.anchoredPosition = new Vector2(barBounds.rect.xMin, 0f);
        RandomizeTargetZone();

        activeRoutine = StartCoroutine(RunLoop());
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
                    RandomizeTargetZone();
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

    IEnumerator DemoLoop()
    {
        while (true)
        {
            marker.anchoredPosition = new Vector2(barBounds.rect.xMin, 0f);
            direction = 1f;
            RandomizeTargetZone();

            int simulatedHits = 0;
            // Sweep back and forth until the marker "happens" to pass through the zone a few times
            while (simulatedHits < hitsNeeded)
            {
                float x = marker.anchoredPosition.x + direction * markerSpeed * Time.deltaTime;
                if (x > barBounds.rect.xMax) { x = barBounds.rect.xMax; direction = -1f; }
                if (x < barBounds.rect.xMin) { x = barBounds.rect.xMin; direction = 1f; }
                marker.anchoredPosition = new Vector2(x, 0f);

                bool inZone = x >= targetZone.anchoredPosition.x - targetZone.rect.width / 2f
                           && x <= targetZone.anchoredPosition.x + targetZone.rect.width / 2f;

                // Simulate a "press" automatically once per pass through the zone's center
                if (inZone && Mathf.Abs(x - targetZone.anchoredPosition.x) < 4f)
                {
                    simulatedHits++;
                    if (simulatedHits < hitsNeeded)
                        RandomizeTargetZone();
                }

                yield return null;
            }

            yield return new WaitForSeconds(pauseBetweenLoops);
        }
    }

    void Finish(bool success)
    {
        onComplete?.Invoke(success);
    }
}