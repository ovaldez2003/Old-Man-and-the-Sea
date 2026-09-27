using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pausePanel;
    public string optionsSceneName = "Options";
    public string tutorialSceneName = "Tutorial";

    private bool isPaused;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;

        Cursor.visible = isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void OnResumeButton()
    {
        TogglePause();
    }

    public void OnTutorialButton()
    {
        pausePanel.SetActive(false);
        MenuNavigation.onReturnFromTutorial = () => pausePanel.SetActive(true);
        SceneManager.LoadScene("Tutorial", LoadSceneMode.Additive);
    }

    public void OnOptionsButton()
    {
        pausePanel.SetActive(false);
        MenuNavigation.onReturnFromOptions = () => pausePanel.SetActive(true);
        SceneManager.LoadScene("Options", LoadSceneMode.Additive);
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}