using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class DialogueController : MonoBehaviour
{
    public GameObject panelRoot;
    public TextMeshProUGUI dialogueText;

    private string[] lines;
    private int index;
    private Action onComplete;

    public void StartDialogue(string[] lines, Action onComplete)
    {
        if (lines == null || lines.Length == 0)
        {
            onComplete?.Invoke();
            return;
        }
        this.lines = lines;
        this.onComplete = onComplete;
        index = 0;

        panelRoot.SetActive(true);
        ShowLine();
    }

    void ShowLine()
    {
        dialogueText.text = lines[index];
    }

    void Update()
    {
        if (!panelRoot.activeSelf) return;

        bool pressed = (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
                    || (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame);

        if (!pressed) return;

        index++;
        if (index >= lines.Length)
        {
            panelRoot.SetActive(false);
            onComplete?.Invoke();
        }
        else
        {
            ShowLine();
        }
    }
}