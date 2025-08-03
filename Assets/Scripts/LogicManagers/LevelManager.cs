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
        TimelinePlayer timeline = GameObject.FindWithTag("EndCamera").GetComponent<TimelinePlayer>();
        timeline.PlayTimeline();
        SnakeHead s = GameObject.FindWithTag("Head").GetComponent<SnakeHead>();
        s.autopilot = true;
        gameManager.NextLevelDelayed(5);
    }

    public void CallForMainMenu()
    {
        gameManager.MainMenuLoad();
    }

    public void Restart()
    {
        gameManager.Restart();
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
        Debug.Log(doorDict[name]);
        if (doorDict[name].keyName != "" && doorKeyDict[doorDict[name].keyName].keyCollected)
        {
            DoorOpenStuff(name, true);
            doorDict[name] = (doorDict[name].door, doorDict[name].keyName, doorDict[name].buttonName, true);
        }
        else if (doorDict[name].keyName == "" && buttonDict[doorDict[name].buttonName].buttonPressed)
        {
            DoorOpenStuff(name, false);
        }
    }

    public void CloseDoor(String name)
    {
        Debug.Log("nfidfvhjkdkfhjkdhfkjghkjfdghkjhdfkgjhjdfghkjdfhkgjhkjdfghdkfgdhfgj");
        DoorCloseStuff(name);
        doorDict[name] = (doorDict[name].door, doorDict[name].keyName, doorDict[name].buttonName, false);
    }

    public void UnlockDoor(String name)
    {
        if (doorKeyDict[doorDict[name].keyName].keyCollected)
        {
            OpenDoor(name);
        }
    }

    private void DoorOpenStuff(String name, bool isKeyDoor)
    {
        if (!doorDict[name].doorOpen) {
            GameObject left = doorDict[name].door.transform.Find("door_left").gameObject;
            GameObject right = doorDict[name].door.transform.Find("door_right").gameObject;
            if (isKeyDoor)
            {
                GameObject wood = doorDict[name].door.transform.Find("keyDoor").gameObject;
                GameObject warlock = doorDict[name].door.transform.Find("keyDoor_lock").gameObject;
                // lock disappears\
                wood.SetActive(false);
                warlock.SetActive(false);
            }
            // doors swing open
            Vector4 posL = left.GetComponent<TorusTransform>().GetPosition();
            left.GetComponent<TorusTransform>().SetPosition(new Vector4(posL.x,posL.y,posL.z,posL.w-90));
            Vector4 posR = right.GetComponent<TorusTransform>().GetPosition();
            right.GetComponent<TorusTransform>().SetPosition(new Vector4(posR.x,posR.y,posR.z,posR.w+90));
        }
    }

    private void DoorCloseStuff(String name)
    {
        GameObject left = doorDict[name].door.transform.Find("door_left").gameObject;
        Vector4 posL = left.GetComponent<TorusTransform>().GetPosition();
        left.GetComponent<TorusTransform>().SetPosition(new Vector4(posL.x,posL.y,posL.z,posL.w+90));
        GameObject right = doorDict[name].door.transform.Find("door_right").gameObject;
        Vector4 posR = right.GetComponent<TorusTransform>().GetPosition();
        right.GetComponent<TorusTransform>().SetPosition(new Vector4(posR.x,posR.y,posR.z,posR.w-90));
        // only for button doors so just swing shut
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