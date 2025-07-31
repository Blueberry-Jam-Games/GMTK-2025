using UnityEngine;

public class MovementTest : MonoBehaviour
{
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
            transformation.MoveDirection(transformation.Rotation, 0.05f);
        }
    }
}
