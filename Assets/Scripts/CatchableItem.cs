using UnityEngine;

[CreateAssetMenu(fileName = "NewCatch", menuName = "Fishing/Catchable Item")]
public class CatchableItem : ScriptableObject
{
    public string itemName;
    public GameObject modelPrefab;      // The 3D model to display
    [TextArea(2, 4)]
    public string[] dialogueLines;      // Lines shown after the reveal
}