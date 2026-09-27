using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameController : MonoBehaviour
{
    public GameObject panelRoot;
    public string mainMenuSceneName = "MainMenu";

    public void Show()
    {
        panelRoot.SetActive(true);
    }

    public void OnReturnToMenuButton()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}