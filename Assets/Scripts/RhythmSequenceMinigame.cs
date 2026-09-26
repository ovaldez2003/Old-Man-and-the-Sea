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

    public KeySpriteSet[] keySprites;   // One entry per possible key (W, A, S, D, Q, E)
    public Image[] promptSlots;         // 5 fixed slots, left to right
    public float showTimePerKey = 0.5f;
    public float gapBetweenKeys = 0.15f;
    public float timePerInput = 1.2f;

    private Key[] sequence;
    private Action<bool> onComplete;

    public void StartGame(Action<bool> onComplete)
    {
        this.onComplete = onComplete;

        sequence = new Key[promptSlots.Length];
        for (int i = 0; i < promptSlots.Length; i++)
        {
            sequence[i] = keySprites[UnityEngine.Random.Range(0, keySprites.Length)].key;
            promptSlots[i].enabled = false; // start hidden, nothing visible yet
        }

        StartCoroutine(RunSequence());
    }

    IEnumerator RunSequence()
    {
        // Show phase: reveal one key at a time, then hide it before the next appears
        for (int i = 0; i < sequence.Length; i++)
        {
            promptSlots[i].sprite = GetSprite(sequence[i], pressed: true);
            promptSlots[i].enabled = true;
            yield return new WaitForSeconds(showTimePerKey);

            promptSlots[i].enabled = false;
            yield return new WaitForSeconds(gapBetweenKeys);
        }

        // Input phase: all slots stay hidden until the player earns them
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
            promptSlots[i].enabled = true; // reveal as confirmation, stays visible
            yield return null; // avoid double-counting the same keypress on a repeated key
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