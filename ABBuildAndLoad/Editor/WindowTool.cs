using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WindowTool : EditorWindow
{


    static void Init()
    {


    }

    private void OnGUI()
    {
        
        if (GUILayout.Button("BuildFont"))
        {
            Build.BuildFont();
        }
        if (GUILayout.Button("BuildSound"))
        {
            Build.BuildSound();
        }
        if (GUILayout.Button("BuildIcon"))
        {
            Build.BuildIcon();
        }
        if (GUILayout.Button("GenerateAtlas"))
        {
            Build.GenerateAtlas();
        }

    }

}
