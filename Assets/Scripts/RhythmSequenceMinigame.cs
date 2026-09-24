using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RhythmSequenceMinigame : MonoBehaviour, IFishingMinigame
{
    [System.Serializable]
    public struct KeySprite
    {
        public Key key;
        public Sprite sprite;
    }

    public Image promptImage;
    public KeySprite[] keySprites;      // One entry per key, set in the Inspector
    public int sequenceLength = 4;
    public float showTimePerKey = 0.5f;
    public float inputTimeLimit = 4f;

    private Key[] possibleKeys;
    private List<Key> sequence;
    private Action<bool> onComplete;

    void Awake()
    {
        possibleKeys = new Key[keySprites.Length];
        for (int i = 0; i < keySprites.Length; i++)
            possibleKeys[i] = keySprites[i].key;
    }

    public void StartGame(Action<bool> onComplete)
    {
        this.onComplete = onComplete;
        sequence = new List<Key>();
        for (int i = 0; i < sequenceLength; i++)
            sequence.Add(possibleKeys[UnityEngine.Random.Range(0, possibleKeys.Length)]);

        promptImage.gameObject.SetActive(false);
        StartCoroutine(RunSequence());
    }

    IEnumerator RunSequence()
    {
        yield return new WaitForSeconds(0.5f);

        foreach (Key k in sequence)
        {
            promptImage.gameObject.SetActive(true);
            promptImage.sprite = GetSpriteForKey(k);
            yield return new WaitForSeconds(showTimePerKey);
            promptImage.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.15f); // brief gap between prompts
        }

        int index = 0;
        float t = 0f;
        while (index < sequence.Count)
        {
            if (t > inputTimeLimit) { Finish(false); yield break; }

            foreach (Key k in possibleKeys)
            {
                if (Keyboard.current[k].wasPressedThisFrame)
                {
                    if (k == sequence[index]) index++;
                    else { Finish(false); yield break; }
                    break;
                }
            }
            t += Time.deltaTime;
            yield return null;
        }
        Finish(true);
    }

    Sprite GetSpriteForKey(Key key)
    {
        foreach (var ks in keySprites)
            if (ks.key == key) return ks.sprite;
        return null;
    }

    void Finish(bool success)
    {
        onComplete?.Invoke(success);
    }
}