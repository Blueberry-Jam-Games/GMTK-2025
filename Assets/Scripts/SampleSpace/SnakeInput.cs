using UnityEngine;
using UnityEngine.InputSystem;

public class SnakeMovement : MonoBehaviour
{
    private SnakeControls controls;
    private Vector3 moveDirection;

    private void Awake()
    {
        controls = new SnakeControls();

        controls.Movement.MoveForward.performed += ctx => MoveForward();
        controls.Movement.TurnLeft.performed += ctx => TurnLeft();
        controls.Movement.TurnRight.performed += ctx => TurnRight();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void MoveForward()
    {
        // Move forward in current direction
        transform.position += transform.forward;
    }

    void TurnLeft()
    {
        // Turn left 90 degrees
        transform.Rotate(0, -90, 0);
    }

    void TurnRight()
    {
        // Turn right 90 degrees
        transform.Rotate(0, 90, 0);
    }
}
