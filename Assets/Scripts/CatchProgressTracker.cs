using UnityEngine;
using System.Collections.Generic;

public class CatchProgressTracker : MonoBehaviour
{
    public static CatchProgressTracker Instance { get; private set; }

    public CatchableItem[] orderedCatches;
    public CatchableItem fallbackCatch;

    private int nextIndex = 0;
    public List<CatchableItem> CaughtHistory { get; private set; } = new List<CatchableItem>();

    void Awake()
    {
        Instance = this;
    }

    public bool HasNext => nextIndex < orderedCatches.Length;
    public bool IsGameComplete => nextIndex >= orderedCatches.Length;

    public CatchableItem GetNextCatch()
    {
        if (HasNext)
        {
            CatchableItem item = orderedCatches[nextIndex];
            nextIndex++;
            CaughtHistory.Add(item);
            return item;
        }
        return fallbackCatch;
    }
}