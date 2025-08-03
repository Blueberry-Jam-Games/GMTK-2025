using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class TorusTransform : MonoBehaviour
{
    [SerializeField]
    public TorusDomain domain;
    [SerializeField]
    public float MajorRotation;
    [SerializeField]
    public float MinorRotation;
    [SerializeField]
    public float Radius;
    [SerializeField]
    public float Rotation;

    private float pMajorRotation;
    private float pMinorRotation;
    private float pRadius;
    private Vector3 pPosition;
    private float pRotation;

    bool moved = false;

    //public Vector3 offset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        domain = FindFirstObjectByType<TorusDomain>();
    }

#if UNITY_EDITOR

    void EditorCode()
    {
        moved = false;
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
                moved = true;
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
                moved = true;
            }
            else if (pRotation != Rotation)
            {
                NormalRotation();
                pRotation = Rotation;
                moved = true;
            }

            if (moved)
            {
                EditorUtility.SetDirty(this);
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
        float newMajorRotation = (distance / ((2 * Mathf.PI) * (domain.majorRadius + domain.minorRadius * Mathf.Sin(MinorRotation)))) * Mathf.Cos(direction) + MajorRotation;
        float newMinorRotation = (distance / ((2 * Mathf.PI) * domain.minorRadius) * Mathf.Sin(direction)) + MinorRotation;
        Vector3 newPosition = domain.ConvertToXYZ(newMajorRotation, newMinorRotation, Radius);

        Collider[] collide = Physics.OverlapSphere(newPosition + transform.rotation * new Vector3(-0.2f, 0f, 0.06f), 0.01f);

        if (collide.Length == 0)
        {
            transform.position = newPosition;
            MajorRotation = newMajorRotation;
            MinorRotation = newMinorRotation;
            moved = true;
        }
        else
        {
            foreach (Collider c in collide)
            {
                Debug.Log(c.name);
            }
        }
        NormalRotation();
    }

    public void Rotate(float inRotation, float rotationModifier)
    {
        float checkRotation = (inRotation * 5f) * rotationModifier;

        Quaternion newRotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, checkRotation));

        Collider[] collide = Physics.OverlapSphere(transform.position + newRotation * new Vector3(-0.2f, 0f, 0.06f), 0.01f);
        if (collide.Length == 0)
        {
            Rotation += (inRotation * 5f) * rotationModifier;
            moved = true;
        }
        else
        {

        }
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

    public bool hasMoved()
    {
        return moved;
    }

    // public void OnDrawGizmos()
    // {
    //     Gizmos.color = new Color(0.0f, 1f, 0.0f);

    //     float distance = 0.01f;
    //     float direction = 0;
    //     float newMajorRotation = (distance / ((2 * Mathf.PI) * (domain.majorRadius + domain.minorRadius * Mathf.Sin(MinorRotation)))) * Mathf.Cos(direction) + MajorRotation;
    //     float newMinorRotation = (distance / ((2 * Mathf.PI) * domain.minorRadius) * Mathf.Sin(direction)) + MinorRotation;
    //     Vector3 newPosition = domain.ConvertToXYZ(newMajorRotation, newMinorRotation, Radius);
    //     newPosition += transform.rotation * offset;
    //     Gizmos.DrawSphere(newPosition, 0.09f);
    // }
}
