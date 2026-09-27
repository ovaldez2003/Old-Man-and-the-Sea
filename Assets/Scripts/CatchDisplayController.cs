using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class CatchDisplayController : MonoBehaviour
{
    public GameObject panelRoot;        // The whole reveal panel, start disabled
    public Transform modelAnchor;       // Empty transform the model spawns at and spins around
    public TextMeshProUGUI catchText;
    public float rotationSpeed = 40f;
    public float tiltAngle = 15f;

    private GameObject spawnedModel;
    private Action onContinue;

    public void Show(CatchableItem item, Action onContinue)
    {
        this.onContinue = onContinue;

        catchText.text = "You caught a " + item.itemName + "!";

        if (spawnedModel != null) Destroy(spawnedModel);
        spawnedModel = Instantiate(item.modelPrefab, modelAnchor.position, Quaternion.identity, modelAnchor);
        spawnedModel.transform.localRotation = Quaternion.Euler(tiltAngle, 0f, 0f);

        panelRoot.SetActive(true);
    }

    void Update()
    {
        if (!panelRoot.activeSelf) return;

        if (spawnedModel != null)
            spawnedModel.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

        bool pressed = (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
                    || (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame);

        if (pressed)
        {
            panelRoot.SetActive(false);
            if (spawnedModel != null) Destroy(spawnedModel);
            onContinue?.Invoke();
        }
    }
}