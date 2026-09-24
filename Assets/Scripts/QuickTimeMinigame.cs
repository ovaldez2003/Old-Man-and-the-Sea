using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QuickTimeMinigame : MonoBehaviour, IFishingMinigame
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
    public float timePerKey = 0.8f;

    private Key[] sequence;
    private Action<bool> onComplete;

    public void StartGame(Action<bool> onComplete)
    {
        this.onComplete = onComplete;

        // Pick a random key for each slot and show it unpressed
        sequence = new Key[promptSlots.Length];
        for (int i = 0; i < promptSlots.Length; i++)
        {
            Key k = keySprites[UnityEngine.Random.Range(0, keySprites.Length)].key;
            sequence[i] = k;
            promptSlots[i].sprite = GetSprite(k, pressed: false);
        }

        StartCoroutine(RunSequence());
    }

    IEnumerator RunSequence()
    {
        for (int i = 0; i < sequence.Length; i++)
        {
            Key target = sequence[i];
            promptSlots[i].sprite = GetSprite(target, pressed: true);

            float t = 0f;
            bool hit = false;
            while (t < timePerKey)
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

            yield return null; // let this frame's key press expire before checking the next slot
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