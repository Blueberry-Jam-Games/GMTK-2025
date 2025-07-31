using UnityEngine;

public class MovementTest : MonoBehaviour
{
    public float direction = 0;
    public TorusTransform transformation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transformation != null)
        {
            transformation.MoveDirection(direction, 0.05f);
        }
    }
}
