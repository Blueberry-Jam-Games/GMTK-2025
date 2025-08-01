using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        domain = GameObject.FindGameObjectWithTag("Torus").GetComponent<TorusDomain>();
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        
    }


    void EditorCode()
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
            NormalRotation();
        }
        else if (pMajorRotation != MajorRotation || pMinorRotation != MinorRotation || pRadius != Radius)
        {
            transform.position = domain.ConvertToXYZ(MajorRotation, MinorRotation, Radius);
            pMajorRotation = MajorRotation;
            pMinorRotation = MinorRotation;
            pRadius = Radius;
            pPosition = transform.position;
            NormalRotation();
        }
    }
#endif

    void Update()
    {
        //Seperation Of Editor Script code
#if UNITY_EDITOR
        EditorCode();
#endif
        //Regular code for update

    }

    public void MoveDirection(float direction, float distance)
    {
        direction *= Mathf.Deg2Rad;
        MajorRotation += (distance / ((2 * Mathf.PI) * (domain.majorRadius + domain.minorRadius * Mathf.Sin(MinorRotation)))) * Mathf.Sin(direction);
        MinorRotation += (distance / ((2 * Mathf.PI) * domain.minorRadius) * Mathf.Cos(direction));
    }

    void NormalRotation()
    {
        transform.LookAt(domain.GetLocalCenter(MajorRotation), Vector3.down);
    }
}
