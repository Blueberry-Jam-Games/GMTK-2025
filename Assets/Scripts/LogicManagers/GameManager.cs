using System.Collections.Generic;
using BJ;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonGameObject<GameManager>
{
    public int level;
    private string[] allLevels = { "Main Menu", "Level 1", "Level 2", "Level 3", "Thank You" };

    public SceneTransitioner transitioner;

    // Start is called before the first frame update
    void Start()
    {
        level = 0;
        transitioner = FindFirstObjectByType<SceneTransitioner>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // fully loads the scene again
    public void Restart()
    {
        transitioner.LoadNewScene(SceneManager.GetActiveScene().name, this);
    }

    public void NextLevel()
    {
        level++;
        transitioner.LoadNewScene(allLevels[level], this);
    }

    public void MainMenuLoad()
    {
        level = 0;
        transitioner.LoadNewScene("MainMenu", this);
    }

    public void FinishedTransition()
    {
        
    }
}