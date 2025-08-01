using System;
using System.Collections.Generic;
using BJ;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonGameObject<GameManager>
{
    public int level;
    private string[] allLevels = { "Main Menu","Level 1", "Level 2", "Level 3", "Thank You" };

    // Start is called before the first frame update
    void Start()
    {
        level = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // fully loads the scene again
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        level++;
        SceneManager.LoadScene(allLevels[level]);
    }

    public void MainMenuLoad()
    {
        level = 0;
        SceneManager.LoadScene("Main Menu");
    }
}