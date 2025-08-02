using UnityEditor;
using UnityEditor.Actions;
using UnityEngine;

[ExecuteAlways]
public class SetInitPosition : MonoBehaviour
{
#if UNITY_EDITOR
    public SnakeHead head;
    public bool tail = false;
    void Update()
    {
        if (!Application.isPlaying)
        {
            if (head == null)
            {
                head = this.transform.parent.GetComponentInChildren<SnakeHead>();
            }

            if (head.transformation.hasMoved())
            {
                if (tail)
                {
                    transform.position = head.transform.position - (head.transform.rotation * new Vector3(-0.13f, 0, 0));
                }
                else
                {
                    transform.position = head.transform.position;
                }
                
                transform.rotation = head.transform.rotation;
                EditorUtility.SetDirty(this);
            }
        }
    }
#endif
}
