using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    LevelManager manager;
    public Button firstSelected;
    GameObject eventSystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        manager = FindFirstObjectByType<LevelManager>();
        eventSystem = GameObject.Find("EventSystem");
    }

    void OnEnable()
    {
        eventSystem = GameObject.Find("EventSystem");
        eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(firstSelected.gameObject);
    }

    public void Quit()
    {

    }

    public void Reset()
    {

    }

    public void Resume()
    {
        manager.TogglePauseMenu();
    }
}
