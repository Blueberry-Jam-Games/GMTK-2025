using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown("W"))
        {
            Vector3 position = transform.position;
            position += new Vector3(0, 0, 0.1f);
            transform.position = position;
        }
    }
}
