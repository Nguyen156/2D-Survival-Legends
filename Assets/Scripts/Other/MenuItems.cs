using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


#if UNITY_EDITOR
using UnityEditor;

public class MenuItems
{
    [MenuItem("Tools/Organize Hierarchy")]
    static void OrganizeHierarchy()
    {
        new GameObject("--- ENVIRONMENT ---");
        new GameObject(" ");

        new GameObject("--- GAMEPLAY ---");
        new GameObject(" ");

        new GameObject("--- UI ---");
        new GameObject(" ");

        new GameObject("--- MANAGERS ---");
    }

    [MenuItem("Tools/Reload Scene")]
    static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

#endif