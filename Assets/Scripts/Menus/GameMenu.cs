using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private string optionsSceneName;
    [SerializeField] private string tutorialSceneName;

    public CinemachineCamera menuCamera;
    public CinemachineCamera playerCamera;
    public GameObject mainMenuUI;
    public GameObject quitButton;

    void Start()
    {
        #if UNITY_WEBGL
            if (quitButton != null) quitButton.SetActive(false);
        #endif
    }

    public void Play()
    {
        menuCamera.Priority = 0;
        playerCamera.Priority = 10;

        if (mainMenuUI != null)
        mainMenuUI.SetActive(false);
    }

    public void Options()
    {
            mainMenuUI.SetActive(false);
            MenuNavigation.onReturnFromOptions = () => mainMenuUI.SetActive(true);
            SceneManager.LoadScene(optionsSceneName, LoadSceneMode.Additive);
    } 

    public void Tutorial()
    {
        mainMenuUI.SetActive(false);
        MenuNavigation.onReturnFromTutorial = () => mainMenuUI.SetActive(true);
        SceneManager.LoadScene(tutorialSceneName, LoadSceneMode.Additive);
    }

    public void Quit()
    {
        #if UNITY_WEBGL
            Debug.Log("Quit is disabled in WebGL builds.");
        #else
            Application.Quit();
        #endif
    }
}
