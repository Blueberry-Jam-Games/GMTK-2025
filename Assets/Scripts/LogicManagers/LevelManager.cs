using System;
using UnityEngine;
using UnityEngine.SceneManagement;



public class LevelManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject creditsUI;
    public GameManager gameManager;
    public bool paused = false;
    public bool credited = false;
    public GameObject gameManagerPrefab;

    void Start()
    {
        AssignMenus();
        if (GameManager.Instance == null)
        {
            Instantiate(gameManagerPrefab); // Autospawn if missing
        }
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
    
    private void AssignMenus()
    {
        Canvas[] canvases = FindObjectsByType<Canvas>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var canvas in canvases)
        {
            switch (canvas.name)
            {
                case "PauseCanvas":
                    pauseMenuUI = canvas.transform.parent.gameObject;
                    break;
                case "CreditsCanvas":
                    creditsUI = canvas.transform.parent.gameObject;
                    break;
                case "MainMenuCanvas":
                    print("MainMenuCanvas");
                    break;
                default:
                    print("No more canvases please its workday now");
                    break;
            }
        }
    }

    public void TogglePauseMenu()
    {
        paused = !paused;
        pauseMenuUI.SetActive(paused);
    }

    public void ToggleCredits()
    {
        credited = !credited;
        creditsUI.SetActive(credited);
    }
}