using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject creditsUI;
    public GameManager gameManager;
    public bool paused = false;
    public bool credited = false;
    public GameObject gameManagerPrefab;
    [Header("position = key# (0,1,2...)")]
    public GameObject[] doorKeys;
    [Header("position = button# (0,1,2...)")]
    public GameObject[] buttons;
    [Header("Door Mapping (no key/button -> -1) (key -> key#) (button -> button#)")]
    public GameObject[] doors;
    public int[] keyOpensDoor;
    public int[] buttonOpensDoor;
    private Dictionary<String, (GameObject key, bool keyCollected)> doorKeyDict;
    private Dictionary<String, (GameObject door, String keyName, String buttonName, bool doorOpen)> doorDict;
    private Dictionary<String, (GameObject button, String doorName, bool buttonPressed)> buttonDict;

    private InputAction menu;

    void Start()
    {
        doorKeyDict = new Dictionary<string, (GameObject key, bool keyCollected)>();
        doorDict = new Dictionary<string, (GameObject door, string keyName, string buttonName, bool doorOpen)>();
        buttonDict = new Dictionary<string, (GameObject button, string doorName, bool buttonPressed)>();
        AssignMenus();
        AssignKeyDoorButton();
        if (GameManager.Instance == null)
        {
            Instantiate(gameManagerPrefab); // Autospawn if missing
        }
        gameManager = GameManager.Instance;

        menu = InputSystem.actions.FindAction("OpenMenu");
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


    public void CollectKey(String name)
    {
        doorKeyDict[name] = (doorKeyDict[name].key, true);
        doorKeyDict[name].key.SetActive(false);
    }
    public void StartPressButton(String name)
    {
        buttonDict[name] = (buttonDict[name].button, buttonDict[name].doorName, true);
        OpenDoor(buttonDict[name].doorName);
    }
    public void EndtPressButton(String name)
    {
        buttonDict[name] = (buttonDict[name].button, buttonDict[name].doorName, false);
        CloseDoor(buttonDict[name].doorName);
    }

    public void OpenDoor(String name)
    {
        // could have an AlwaysOpen hook to check if the door should stay open andnot fire these open/close triggers
        
        if (doorDict[name].keyName != "" && doorKeyDict[doorDict[name].keyName].keyCollected)
        {
            doorDict[name] = (doorDict[name].door, doorDict[name].keyName, doorDict[name].buttonName, true);
            doorDict[name].door.SetActive(false);
        }
        else if (doorDict[name].keyName == "" && buttonDict[doorDict[name].buttonName].buttonPressed)
        {
            doorDict[name].door.SetActive(false);
        }
    }

    public void CloseDoor(String name) {
        Debug.Log("hfihesyiufhyusdhfuihsdiufhisdhfisdhifuhsduifhisudhdfuisdhf");
        doorDict[name] = (doorDict[name].door, doorDict[name].keyName, doorDict[name].buttonName, false);
        doorDict[name].door.SetActive(true);
    }

    public void UnlockDoor(String name)
    {
        if (doorKeyDict[doorDict[name].keyName].keyCollected) {
            OpenDoor(name);
        }
    }

    private void AssignKeyDoorButton()
    {
        int keynum = 0;
        foreach (var dk in doorKeys)
        {
            Debug.Log(dk.name);
            Debug.Log(dk.tag);

            doorKeyDict.Add(dk.name, (dk, false));
            keynum++;
        }
        int buttonNum = 0;
        foreach (var b in buttons)
        {
            buttonDict.Add(b.name, (b, "", false));
            buttonNum++;
        }
        for (int i = 0; i < doors.Length; i++)
        {
            GameObject d = doors[i];
            int k = keyOpensDoor[i];
            int b = buttonOpensDoor[i];
            string kname = "";
            string bname = "";
            if (k > -1)
            {
                kname = doorKeys[k].name;
            }
            if (b > -1)
            {
                bname = buttons[b].name;
                buttonDict[bname] = (buttonDict[bname].button, d.name, buttonDict[bname].buttonPressed);
            }
            doorDict.Add(d.name, (d, kname, bname, false));
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