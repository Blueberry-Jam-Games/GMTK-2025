using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchManagers
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        GameObject GameManager = Resources.Load<GameObject>("GameManager");

        if (GameManager == null)
        {
            Debug.LogError("The configuaration prefab for the GameManager could not be found. Please ensure that one exists in the Assets/Resources directory of your project");
        }

        GameObject instance = Object.Instantiate(GameManager);
    }
}
