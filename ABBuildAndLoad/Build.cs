using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Build : Editor {


    [MenuItem("Assets/BuildFont &1",false,1)]
    public static void BuildFont()
    {
        Debug.Log("BuildFont");
    }

}
