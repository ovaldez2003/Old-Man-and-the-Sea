using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialPageController : MonoBehaviour
{
    public GameObject[] pages;   // Drag each page panel in, in order
    public GameObject nextButton;
    public GameObject backButton;

    private int currentPage = 0;

    void Start()
    {
        ShowPage(0);
    }

    void ShowPage(int index)
    {
        for (int i = 0; i < pages.Length; i++)
            pages[i].SetActive(i == index);

        currentPage = index;
    }

    public void OnNextButton()
    {
        if (currentPage < pages.Length - 1)
        {
            ShowPage(currentPage + 1);
        }
        else
        {
            MenuNavigation.onReturnFromTutorial?.Invoke();
            MenuNavigation.onReturnFromTutorial = null;
            SceneManager.UnloadSceneAsync("Tutorial");
        }
    }

    public void OnBackButton()
    {
        if (currentPage > 0)
            ShowPage(currentPage - 1);

        else
        {
            MenuNavigation.onReturnFromTutorial?.Invoke();
            MenuNavigation.onReturnFromTutorial = null;
            SceneManager.UnloadSceneAsync("Tutorial");
        }
    }
}