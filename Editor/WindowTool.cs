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
        GUILayout.BeginVertical();
        GUILayout.Label("打包工具");
        //第一行
        GUILayout.BeginHorizontal();
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
        GUILayout.EndHorizontal();

        //第二行
        GUILayout.BeginHorizontal();


        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }

}
