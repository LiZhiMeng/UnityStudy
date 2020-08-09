using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class BuildBundleAtlas : EditorWindow
{
    private string inAtlasPath = "";
    private string inPngPath = "";
    private string outAtlasPath = "";
    private void OnGUI()
    {
        string [] dirs = Directory.GetDirectories(inAtlasPath);
        for (int i = 0; i < dirs.Length; i++)
        {
            string dirPath = dirs[i];
            string[] splitDirPath = dirPath.Split('\\');
            string atlasName = splitDirPath[splitDirPath.Length - 1];
            
            
            if (GUILayout.Button(atlasName))
            {
                Build(dirPath,atlasName);
            }
        }
        
    }

    private void OnEnable()
    {
        inAtlasPath = Application.dataPath + "/PracticeAssets/Resources/atlas";
        outAtlasPath = Application.streamingAssetsPath + "/atlas";
        outAtlasPath = BuildTool.RemovePathPrefix(outAtlasPath);
    }

    public void Build(string dirPath,string atlasName)
    {
        
        
        BuildTool.DeleteOutPutDir(TConstant._atlas);
        BuildTool.ClearAB();
        string prefabPath =  $"{inAtlasPath}/{atlasName}/{atlasName}.prefab";
        prefabPath = BuildTool.RemovePathPrefix(prefabPath);
        string pngPath = $"{inAtlasPath}/{atlasName}/{atlasName}.png";
        pngPath = BuildTool.RemovePathPrefix(pngPath);
        Debug.Log(BuildTool.RemovePathPrefix(prefabPath));
        AssetImporter importerPrefab = AssetImporter.GetAtPath( prefabPath );
        importerPrefab.assetBundleName =  atlasName;
        importerPrefab.assetBundleVariant =  BuildTool.ABSUFFIX;
        AssetImporter importerPng = AssetImporter.GetAtPath(pngPath);
        importerPng.assetBundleName =  atlasName;
        importerPng.assetBundleVariant = BuildTool.ABSUFFIX;
        BuildTool.BuildBundle(outAtlasPath);
        BuildTool.ClearAB();
        BuildTool.DeleteManifest();
        AssetDatabase.Refresh();
    }

}
