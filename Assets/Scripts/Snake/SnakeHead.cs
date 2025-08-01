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
    private float targetRotation = 0;

    public float distanceBetweenSegments = 0.1f;

    public List<Vector4> positions = new List<Vector4>();
    private List<Vector3> positionsXYZ = new List<Vector3>();

    public List<TorusTransform> body = new List<TorusTransform>();

    void Start()
    {
        body.Clear();
        body.AddRange(transform.parent.GetComponentsInChildren<TorusTransform>());
        body.RemoveAt(0);
    }

    void FixedUpdate()
    {
        if (transformation != null)
        {
            rotation %= 360;
            transformation.Rotation = rotation;
            transformation.MoveDirection(rotation, 0.05f);
        }

        if (Mathf.Abs((targetRotation - rotation) % 360) <= 5)
        {
            targetRotation = (float)Random.Range(0, 360);
        }

        if ((targetRotation - rotation) % 360 > 180)
        {
            rotation -= 2;
        }
        else if ((targetRotation - rotation) % 360 < 180)
        {
            rotation += 2;
        }

        updatePath();
    }

    private void updatePath()
    {
        positions.Add(transformation.GetPosition());
        positionsXYZ.Add(transformation.domain.ConvertToXYZ(transformation.GetPosition()));
        int queueIndex = positions.Count - 1;
        bool waitForValue = false;

        foreach (TorusTransform s in body)
        {
            bool success = false;
            for (int i = queueIndex; i > 0; i--)
            {
                if (Vector3.Distance(positionsXYZ[i], positionsXYZ[queueIndex]) >= distanceBetweenSegments)
                {
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

    void OnDrawGizmos()
    {
        foreach (Vector3 c in positionsXYZ)
        {
            Gizmos.DrawSphere(c, 0.01f);
        }
    }

}
