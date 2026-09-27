using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialMenuController : MonoBehaviour
{
    public void OnReturnButton()
    {
        MenuNavigation.onReturnFromTutorial?.Invoke();
        MenuNavigation.onReturnFromTutorial = null;
        SceneManager.UnloadSceneAsync("Tutorial");
    }
}
