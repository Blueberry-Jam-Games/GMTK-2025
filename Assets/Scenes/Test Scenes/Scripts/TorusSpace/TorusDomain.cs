using System;
using UnityEngine;

public class TorusDomain : MonoBehaviour
{
    public float majorRadius;
    public float minorRadius;

    public Vector3 ConvertToXYZ(Vector3 rotations)
    {
        return ConvertToXYZ(rotations.x, rotations.y, rotations.z);
    }

    // Mx = Major rotation
    // my = minor Rotation
    // lz = Local Rotation
    public Vector3 ConvertToXYZ(float Mx, float my, float r)
    {
        float x = Mathf.Cos(Mx) * (majorRadius + r * Mathf.Sin(my)) + transform.position.x;
        float y = r * Mathf.Cos(my) + transform.position.y;
        float z = Mathf.Sin(Mx) * (majorRadius + r * Mathf.Sin(my)) + transform.position.z;
        return new Vector3(x, y, z);
    }

    public Vector3 ConvertFromXYZ(Vector3 position)
    {
        Vector3 flatPosition = new Vector3(position.x, 0, position.z);
        float majorRotation = Mathf.Deg2Rad * Vector3.SignedAngle(flatPosition, Vector3.right, Vector3.up);

        float minorRotation = Mathf.Deg2Rad * Vector3.SignedAngle(Vector3.up, position - GetLocalCenter(majorRotation), Vector3.Cross(Vector3.up, position));
        float radius = minorRadius;

        return new Vector3(majorRotation, minorRotation, radius);
    }

    public float GetRadius()
    {
        return minorRadius;
    }

    public Vector3 GetLocalCenter(float Mx)
    {
        float x = Mathf.Cos(Mx) * majorRadius;
        float y = 0;
        float z = Mathf.Sin(Mx) * majorRadius;

        return new Vector3(x, y, z);
    }
}
