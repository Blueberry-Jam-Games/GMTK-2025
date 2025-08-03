using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class SnakeHead : MonoBehaviour
{
    public TorusTransform transformation;

    public float rotation = 0;
    //private float targetRotation = 0;

    public float distanceBetweenSegments = 0.1f;

    public List<Vector4> positions = new List<Vector4>();
    private List<Vector3> positionsXYZ = new List<Vector3>();

    public List<TorusTransform> body = new List<TorusTransform>();
    public GameObject bodyPrefab;

    public LevelManager levelManager;
    public float turnRadiusNerf = 2f;
    public float speedFactor = 15f;

    public bool OnButton = false;

    private InputAction Forward;
    private InputAction Turn;

    public bool OnButtonNow = false;

    void Start()
    {
        body.Clear();
        body.AddRange(transform.parent.GetComponentsInChildren<TorusTransform>(true));
        body.RemoveAt(0);
        Forward = InputSystem.actions.FindAction("MoveForward");
        Turn = InputSystem.actions.FindAction("Turn");
    }

    void FixedUpdate()
    {
        if (transformation != null)
        {
            float forwardValue = Forward.ReadValue<float>();
            if (forwardValue != 0)
            {
                float turnValue = Turn.ReadValue<float>();
                transformation.Rotate(turnValue / turnRadiusNerf, forwardValue);
                transformation.MoveDirection(transformation.Rotation, forwardValue / speedFactor);
            }
        }

        DetectTaggedItems();

        // if (Mathf.Abs((targetRotation - rotation) % 360) <= 5)
        // {
        //     targetRotation = (float)Random.Range(0, 360);
        // }

        // if ((targetRotation - rotation) % 360 > 180)
        // {
        //     rotation -= 2;
        // }
        // else if ((targetRotation - rotation) % 360 < 180)
        // {
        //     rotation += 2;
        // }

        updatePath();
    }

    private void updatePath()
    {
        if (transformation.hasMoved())
        {
            positions.Add(transformation.GetPosition());
            positionsXYZ.Add(transformation.domain.ConvertToXYZ(transformation.GetPosition()));   
        }
        int queueIndex = positions.Count - 1;
        bool waitForValue = false;

        foreach (TorusTransform s in body)
        {
            bool success = false;
            for (int i = queueIndex; i > 0; i--)
            {
                if (Vector3.Distance(positionsXYZ[i], positionsXYZ[queueIndex]) >= distanceBetweenSegments)
                {
                    if (!s.gameObject.activeSelf)
                    {
                        s.gameObject.SetActive(true);
                    }
                    s.SetPosition(positions[i]);
                    queueIndex = i;
                    success = true;
                    break;
                }
            }

            if (!success)
            {
                waitForValue = true;
            }
        }

        if (queueIndex > 0 && !waitForValue)
        {
            positions.RemoveRange(0, queueIndex);
            positionsXYZ.RemoveRange(0, queueIndex);
        }
    }

    private void DetectTaggedItems()
    {
        Collider[] collide = Physics.OverlapSphere(transform.position + transform.rotation * new Vector3(-0.2f, 0f, 0.06f), 0.1f);
        OnButtonNow = false;
        foreach (var item in collide)
        {
            //Debug.Log(item.gameObject.tag + " from head");
            if (item.gameObject.tag == "Apple")
            {
                body.Insert(1, Instantiate(bodyPrefab.GetComponent<TorusTransform>()));
                item.gameObject.SetActive(false);
            }
            else if (item.gameObject.tag == "Key")
            {
                levelManager.CollectKey(item.gameObject.name);
            }
            else if (item.gameObject.tag == "Door")
            {
                levelManager.OpenDoor(item.gameObject.name);
            }
            else if (item.gameObject.tag == "Button")
            {
                if (!OnButton)
                {
                    levelManager.StartPressButton(item.gameObject.name);
                    OnButtonNow = true;
                }
            }
            else if (item.gameObject.tag == "Portal")
            {
                item.gameObject.GetComponent<Portal>().UsePortal();
            }
            else if (item.gameObject.tag == "Tail") {
                levelManager.CallForNextLevel();
            }
        }
        if (!OnButtonNow && OnButton) {
            OnButton = false;
        }
    }

    public bool GetOnButton()
    {
        return OnButton;
    }

    void OnDrawGizmos()
    {
        foreach (Vector3 c in positionsXYZ)
        {
            Gizmos.DrawSphere(c, 0.01f);
        }
        //Gizmos.DrawSphere(transform.position + transform.rotation * new Vector3(-0.2f, 0f, 0.06f), 0.1f);
    }
}
