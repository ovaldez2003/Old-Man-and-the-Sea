using UnityEngine;

public class CatchProgressTracker : MonoBehaviour
{
    public static CatchProgressTracker Instance { get; private set; }

    public CatchableItem[] orderedCatches;   // In story order
    public CatchableItem fallbackCatch;      // Shown once the list is exhausted, optional

    private int nextIndex = 0;

    void Awake()
    {
        Instance = this;
    }

    public bool HasNext => nextIndex < orderedCatches.Length;

    public CatchableItem GetNextCatch()
    {
        if (HasNext)
        {
            CatchableItem item = orderedCatches[nextIndex];
            nextIndex++;
            return item;
        }
        return fallbackCatch; // may be null, handled by caller
    }
}