using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RhythmSequenceMinigame : MonoBehaviour, IFishingMinigame
{
    [System.Serializable]
    public struct KeySpriteSet
    {
        public Key key;
        public Sprite unpressedSprite;
        public Sprite pressedSprite;
    }

    public KeySpriteSet[] keySprites;
    public Image[] promptSlots;
    public float showTimePerKey = 0.5f;
    public float gapBetweenKeys = 0.15f;
    public float timePerInput = 1.2f;

    public bool loopDemo = false;       // Check this on the tutorial page's copy
    public float pauseBetweenLoops = 1f;

    private Key[] sequence;
    private Action<bool> onComplete;
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

        sequence = new Key[promptSlots.Length];
        for (int i = 0; i < promptSlots.Length; i++)
        {
            sequence[i] = keySprites[UnityEngine.Random.Range(0, keySprites.Length)].key;
            promptSlots[i].enabled = false;
        }

        activeRoutine = StartCoroutine(RunSequence());
    }

    IEnumerator DemoLoop()
    {
        while (true)
        {
            Key[] demoSequence = new Key[promptSlots.Length];
            for (int i = 0; i < promptSlots.Length; i++)
            {
                demoSequence[i] = keySprites[UnityEngine.Random.Range(0, keySprites.Length)].key;
                promptSlots[i].enabled = false;
            }

            for (int i = 0; i < demoSequence.Length; i++)
            {
                promptSlots[i].sprite = GetSprite(demoSequence[i], pressed: true);
                promptSlots[i].enabled = true;
                yield return new WaitForSeconds(showTimePerKey);
                promptSlots[i].enabled = false;
                yield return new WaitForSeconds(gapBetweenKeys);
            }

            yield return new WaitForSeconds(pauseBetweenLoops);
        }
    }

    IEnumerator RunSequence()
    {
        // Show phase
        for (int i = 0; i < sequence.Length; i++)
        {
            promptSlots[i].sprite = GetSprite(sequence[i], pressed: true);
            promptSlots[i].enabled = true;
            yield return new WaitForSeconds(showTimePerKey);
            promptSlots[i].enabled = false;
            yield return new WaitForSeconds(gapBetweenKeys);
        }

        // Input phase
        for (int i = 0; i < sequence.Length; i++)
        {
            Key target = sequence[i];

            float t = 0f;
            bool hit = false;
            while (t < timePerInput)
            {
                if (Keyboard.current[target].wasPressedThisFrame) { hit = true; break; }
                t += Time.deltaTime;
                yield return null;
            }

            if (!hit)
            {
                Finish(false);
                yield break;
            }

            promptSlots[i].sprite = GetSprite(target, pressed: true);
            promptSlots[i].enabled = true;
            yield return null;
        }

        Finish(true);
    }

    Sprite GetSprite(Key key, bool pressed)
    {
        foreach (var ks in keySprites)
        {
            if (ks.key == key)
                return pressed ? ks.pressedSprite : ks.unpressedSprite;
        }
        return null;
    }

    void Finish(bool success)
    {
        onComplete?.Invoke(success);
    }
}