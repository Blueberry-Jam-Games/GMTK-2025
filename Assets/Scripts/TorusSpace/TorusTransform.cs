using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class TorusTransform : MonoBehaviour
{
    public TorusDomain domain;
    public float MajorRotation;
    public float MinorRotation;
    public float Radius;

    public float Rotation;

    private float pMajorRotation;
    private float pMinorRotation;
    private float pRadius;
    private Vector3 pPosition;
    private float pRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

#if UNITY_EDITOR

    void EditorCode()
    {
        if (domain != null)
        {
            if (pPosition != transform.position)
            {
                Vector3 rotations = domain.ConvertFromXYZ(transform.position);
                MajorRotation = rotations.x;
                MinorRotation = rotations.y;
                Radius = rotations.z;

                transform.position = domain.ConvertToXYZ(MajorRotation, MinorRotation, Radius);

                pMajorRotation = MajorRotation;
                pMinorRotation = MinorRotation;
                pRadius = Radius;
                pPosition = transform.position;
                pRotation = Rotation;
                NormalRotation();
            }
            else if (pMajorRotation != MajorRotation || pMinorRotation != MinorRotation || pRadius != Radius)
            {
                transform.position = domain.ConvertToXYZ(MajorRotation, MinorRotation, Radius);
                pMajorRotation = MajorRotation;
                pMinorRotation = MinorRotation;
                pRadius = Radius;
                pPosition = transform.position;
                pRotation = Rotation;
                NormalRotation();
            }
            else if (pRotation != Rotation)
            {
                NormalRotation();
                pRotation = Rotation;
            }
        }
    }
#endif

    void Update()
    {
        //Seperation Of Editor Script code
#if UNITY_EDITOR
        if (!EditorApplication.isPlaying)
        {
            EditorCode();
        }
#endif
        //Regular code for update

    }

    public void MoveDirection(float direction, float distance)
    {
        direction *= Mathf.Deg2Rad;
        MajorRotation += (distance / ((2 * Mathf.PI) * (domain.majorRadius + domain.minorRadius * Mathf.Sin(MinorRotation)))) * Mathf.Cos(direction);
        MinorRotation += (distance / ((2 * Mathf.PI) * domain.minorRadius) * Mathf.Sin(direction));
        transform.position = domain.ConvertToXYZ(MajorRotation, MinorRotation, Radius);
        NormalRotation();
    }

    public void NormalRotation()
    {
        Vector3 flatposition = new Vector3(transform.position.x, 0, transform.position.z);
        transform.LookAt(domain.GetLocalCenter(MajorRotation));

        Quaternion newRotation = transform.rotation;

        Quaternion flip;

        if (flatposition.magnitude >= domain.majorRadius)
        {
            flip = Quaternion.Euler(180, 0, Rotation + 180);
        }
        else
        {
            flip = Quaternion.Euler(180, 0, Rotation);
        }

        newRotation *= flip;

        transform.rotation = newRotation;
    }

    public Vector4 GetPosition()
    {
        return new Vector4(MajorRotation, MinorRotation, Radius, Rotation);
    }

    public void SetPosition(Vector4 newTransform)
    {
        MajorRotation = newTransform.x;
        MinorRotation = newTransform.y;
        Radius = newTransform.z;
        Rotation = newTransform.w;

        transform.position = domain.ConvertToXYZ(newTransform);
        NormalRotation();
    }
}
