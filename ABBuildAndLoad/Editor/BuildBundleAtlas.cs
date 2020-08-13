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
                Debug.Log($"Build{atlasName}");
                EditorUtility.DisplayProgressBar("Building",$"Building:{TCommon._atlas}",0.5f);
                string prefabPath = $"{dirPath}/{atlasName}.prefab";
                TCommon.GetNewAssetVersion( prefabPath );
                TCommon.GetOldAssetVersion(TCommon._atlas);
                if (TCommon.IsNeedPak(prefabPath))
                {
                    Build(dirPath,atlasName);
                }
                TCommon.SaveAssetVersion(TCommon._atlas);
                AssetDatabase.Refresh();
                EditorUtility.ClearProgressBar();
            }
        }
    }
    

    private void OnEnable()
    {
        inAtlasPath = Application.dataPath + "/PracticeAssets/Resources/atlas";
        outAtlasPath = Application.streamingAssetsPath + "/atlas";
        outAtlasPath = TCommon.RemovePathPrefix(outAtlasPath);
    }

    public void Build(string dirPath,string atlasName)
    {
        TCommon.DeleteOutPutDir(TCommon._atlas);
        BuildTool.ClearAB();
        string prefabPath =  $"{inAtlasPath}/{atlasName}/{atlasName}.prefab";
        prefabPath = TCommon.RemovePathPrefix(prefabPath);
        string pngPath = $"{inAtlasPath}/{atlasName}/{atlasName}.png";
        pngPath = TCommon.RemovePathPrefix(pngPath);
        Debug.Log(TCommon.RemovePathPrefix(prefabPath));
        AssetImporter importerPrefab = AssetImporter.GetAtPath( prefabPath );
        importerPrefab.assetBundleName =  atlasName;
        importerPrefab.assetBundleVariant =  TCommon.ABSUFFIX;
        AssetImporter importerPng = AssetImporter.GetAtPath(pngPath);
        importerPng.assetBundleName =  atlasName;
        importerPng.assetBundleVariant = TCommon.ABSUFFIX;
        BuildTool.BuildBundle(outAtlasPath);
        BuildTool.ClearAB();
        BuildTool.DeleteManifest();
        AssetDatabase.Refresh();
    }

}
