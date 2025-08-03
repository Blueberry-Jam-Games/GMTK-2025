using BJ;
using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject CreditsUI;
    public GameObject TitleUI;

    void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void CallForNextLevel()
    {
        gameManager.NextLevel();
    }

    public void CallForMainMenu()
    {
        gameManager.MainMenuLoad();
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void OpenCredits()
    {
        TitleUI.SetActive(false);
        CreditsUI.SetActive(true);
    }

    public void CloseCredits()
    {
        TitleUI.SetActive(true);
        CreditsUI.SetActive(false);
    }
}
