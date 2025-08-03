using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SetSelectedElement : MonoBehaviour
{
    public Button firstSelected;
    GameObject eventSystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnEnable()
    {
        eventSystem = GameObject.Find("EventSystem");
        eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(firstSelected.gameObject);
    }
}
