using UnityEngine;
using Unity.Cinemachine;

public class MenuTransition : MonoBehaviour
{
    public CinemachineCamera menuCamera;
    public CinemachineCamera playerCamera;
    public GameObject mainMenuUI;

    public void OnPlayButtonClicked()
    {
        menuCamera.Priority = 0;
        playerCamera.Priority = 10;

        if (mainMenuUI != null)
        mainMenuUI.SetActive(false);
    }
}
