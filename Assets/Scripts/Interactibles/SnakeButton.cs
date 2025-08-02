using UnityEngine;

public class SnakeButton : MonoBehaviour
{
    public bool pressedLastUpdate = false;
    public bool pressedThisUpdate = false;
    public LevelManager levelManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Collider[] collide = Physics.OverlapCapsule(transform.position, transform.position + transform.rotation * new Vector3(0,0,0.1f), 0.1f);
        pressedThisUpdate = false;

        foreach (var item in collide)
        {
            if (item.gameObject.CompareTag("Head") || item.gameObject.CompareTag("Body") || item.gameObject.CompareTag("Tail"))
            {
                if (!pressedLastUpdate)
                {
                    levelManager.StartPressButton(this.name);
                    Debug.Log("hfihesyiufhyusdhfuihsdiufhisdhfisdhifuhsduifhisudhdfuisdhf");
                }
                pressedThisUpdate = true;
            }
        }
        if (pressedLastUpdate && !pressedThisUpdate)
        {
            Debug.Log("hfihesyiufhyusdhfuihsdiufhisdhfisdhifuhsduifhisudhdfuisdhf");
            levelManager.EndtPressButton(this.name);
            pressedLastUpdate = false;
        }
        if (pressedThisUpdate)
        {
            pressedLastUpdate = true;
        }
    }
}
