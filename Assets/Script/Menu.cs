using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    [SerializeField] private GameObject tutorialPage;

    private void Start()
    {
        tutorialPage.SetActive(false);
    }

    public void OpenTutorial()
    {
        tutorialPage.SetActive(true);
    }

    public void CloseTutorial()
    {
        tutorialPage.SetActive(false);
    }

    public void StartPlay()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
