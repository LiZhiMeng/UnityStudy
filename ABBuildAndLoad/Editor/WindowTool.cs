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
            BuildTool.BuildFont();
        }
        if (GUILayout.Button("BuildSound"))
        {
            BuildTool.BuildSound();
        }
        if (GUILayout.Button("BuildIcon"))
        {
            BuildTool.BuildIcon();
        }
        if (GUILayout.Button("GenerateAtlas"))
        {
            BuildTool.GenerateAtlas();
        }
        if (GUILayout.Button("BuildAtlas"))
        {
            BuildTool.BuildAtlas();
        }
        if (GUILayout.Button("BuildModel"))
        {
            BuildTool.BuildModel();
        }
    }
}
