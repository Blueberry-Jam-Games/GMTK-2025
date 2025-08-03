using Unity.VisualScripting;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject sisterPortal;
    public SnakeHead snakeHead;
    public bool thisPortalActive = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UsePortal()
    {
        Vector4 orig = sisterPortal.GetComponent<Portal>().GetMyV4();
        snakeHead.transformation.SetPosition(new Vector4(orig.x,orig.y,orig.z,orig.w+90));
    }

    public Vector4 GetMyV4()
    {
        return transform.parent.gameObject.GetComponent<TorusTransform>().GetPosition();
    }
}
