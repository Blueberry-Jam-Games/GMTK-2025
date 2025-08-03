using BJ;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameManager gameManager;

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
}
