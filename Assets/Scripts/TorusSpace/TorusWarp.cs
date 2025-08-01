using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class TorusWarp : MonoBehaviour
{
    public TorusTransform torus;

    [SerializeField]
    public float savedRadius;
    [SerializeField]
    public float savedRotation;

    public Mesh mesh;
    private Mesh altered;
    Vector3[] originalVerticies;

    void Start()
    {
        // Instantiate a new mesh to prevent editing the shared mesh
        MeshFilter filter = GetComponent<MeshFilter>();

        altered = Instantiate(mesh); // Create a unique copy
        filter.mesh = altered; // Assign it to this object only

        if (Application.isPlaying)
        {
            UpdateVerticies();
        }
    }

    void Update()
    {
#if UNITY_EDITOR
            UpdateVerticies();
#endif
    }

    void UpdateVerticies()
    {
        savedRotation = torus.Rotation;
        savedRadius = torus.Radius;

        Quaternion reverseRotation = Quaternion.AngleAxis(-savedRotation, Vector3.forward);
        Quaternion correctRotation = Quaternion.AngleAxis(savedRotation, Vector3.forward);

        Vector3[] vertices = mesh.vertices;
        Vector3[] finalVertices = altered.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = vertices[i];
            vertex = reverseRotation * vertex;

            vertex.y += vertex.y / (savedRadius / 100) * vertex.z;

            vertex = correctRotation * vertex;

            finalVertices[i] = vertex;
        }

        altered.vertices = finalVertices;
        EditorUtility.SetDirty(this);
    }
}
