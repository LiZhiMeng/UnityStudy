﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class AtlasWindow : EditorWindow
{
    
    public class name_dic
    {
        public string _atlasName;
        public string _atlasDic;
    }

    private string _textureDic;
    private string[] _atlasDic;
    private List<name_dic> list_dic = new List<name_dic>();
    private string atlasResPath;
    private void OnEnable()
    {
        atlasResPath= Application.dataPath + "/PracticeAssets/Resources/atlas";
        _textureDic = Application.dataPath + "/PracticeAssets/Resources/texture";
        _atlasDic = Directory.GetDirectories(_textureDic);
        list_dic.Clear();
        foreach (var variable in _atlasDic)
        {
            string [] split = variable.Split('\\');
            string atlasName = split[split.Length - 1];
            name_dic dic = new name_dic();
            dic._atlasDic = variable;
            dic._atlasName = atlasName;
            list_dic.Add(dic);
        }
    }

    //private string atlasStreamingAssetPath = Application.streamingAssetsPath + "/" + PublicEnum.ATLAS;
    private void OnGUI()
    {
        
        foreach (name_dic dic in list_dic)
        {
            if (GUILayout.Button(dic._atlasName))//点击了按钮，则打图集
            {
                Debug.Log(dic._atlasName);
                Debug.Log(dic._atlasDic);
                //查找需要打的图集
                string path = dic._atlasDic;

                ProcessTexturePacker(dic);
                AnalySheet(dic._atlasName);
            }
        }
    }

    /// <summary>
    /// 解析图集
    /// </summary>
    void AnalySheet(string _atlasName)
    {
        string atlasPath = $"{atlasResPath}/{_atlasName}";
        string outPngPath = $"{atlasResPath}/{_atlasName}/{_atlasName}.png";
        string outTxtPath = $"{atlasResPath}/{_atlasName}/{_atlasName}.txt";
        string outTextAssetPath = $"{atlasResPath}/{_atlasName}/{_atlasName}.asset";
        string outPrefabPath = $"{atlasResPath}/{_atlasName}/{_atlasName}.prefab";

        if (Directory.Exists(outPrefabPath))
        {
            Directory.Delete(outPrefabPath,true);
        }
        AssetDatabase.Refresh();

        GameObject prefab = new GameObject();
        Object o = PrefabUtility.CreateEmptyPrefab(RemovePathPrefix(outPrefabPath));
        TUIAtlas tAtlas = prefab.AddComponent<TUIAtlas>();
        TUIAtlasSheet tSheetList = ScriptableObject.CreateInstance<TUIAtlasSheet>();
        AssetDatabase.CreateAsset(tSheetList, RemovePathPrefix( outTextAssetPath));


        TextAsset txtAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(RemovePathPrefix( outTxtPath));
        Debug.Log("Txt:" + txtAsset);
        string txtString = txtAsset.ToString() ;
        


        //foreach (var item in collection)
        //{
        //    TUiSpriteData data = new TUiSpriteData();
        //    data.border = new Vector4(2, 3, 4, 5);
        //    tSheetList.uispriteList.Add(data);
        //}
        
        
        
        tAtlas.sheet = tSheetList;
        
        Texture2D tx2d = AssetDatabase.LoadAssetAtPath<Texture2D>(RemovePathPrefix(outPngPath));
        tAtlas.Texture = tx2d;
        PrefabUtility.ReplacePrefab(prefab, o);
        
    }

    public string RemovePathPrefix(string path)
    {
        int idx = path.IndexOf("Assets/");
        return path.Substring(idx);
    }
    
    /// <summary>
    /// 运行texturePacker打包图集
    /// </summary>
    void ProcessTexturePacker(name_dic dic)
    {
        string AtlasSize = "1024";
        string commond = @" --sheet {0} --data {1} --format unity-texture2d --trim-mode None --pack-mode Best --algorithm Polygon --max-size " + AtlasSize + @" --size-constraints POT --disable-rotation --scale 1 {2}";

        // D:\Program Files\TexturePacker 2.4.5\TexturePacker\bin>TexturePacker.exe C:\Users\user\Desktop\temp\SE_0053\3_0001.png --max-size 4096 --trim --allow-free-size --format sparrow --shape-padding 0 --bor
        // der-padding 0 --disable-rotation --algorithm MaxRects --opt RGBA4444 --scale 1 --sheet C:\Users\user\Desktop\temp\SE_0053\out_put.png --data C:\Users\user\Desktop\temp\SE_0053\out_put.xml
            
        //创建目录
        string dicPath = atlasResPath + "/" + dic._atlasName;
        if (Directory.Exists(dicPath))
        {
            Directory.Delete(dicPath, true);
        }
        
        Directory.CreateDirectory(dicPath);
        AssetDatabase.Refresh();
        string pngPath = $"{dicPath}/{dic._atlasName}.png";
        int _idx = pngPath.IndexOf("Assets/");
        pngPath = pngPath.Substring(_idx);
        string dataPath = $"{dicPath}/{dic._atlasName}.txt"; 
        dataPath = dataPath.Substring(_idx);

        StringBuilder atlasPaths = new StringBuilder();
        string inPath = $"{_textureDic}/{dic._atlasName}";
        if (!Directory.Exists(inPath))
        {
            return;
        }

        string[] paths = Directory.GetFiles(inPath);
        foreach (var VARIABLE in paths)
        {
            if(VARIABLE.Contains(".meta")) continue;
            int idx =  VARIABLE.IndexOf("Assets/");
            string _assetsPath = VARIABLE.Substring( idx);
            atlasPaths.Append(_assetsPath);
            atlasPaths.Append(" ");
        }
        string arg = string.Format(commond, pngPath, dataPath, atlasPaths.ToString());
        EditorUtility.DisplayProgressBar("message","texturePacker",0.1f); 
        Debug.Log("arg:"+arg);
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        process.StartInfo.Arguments = arg;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.FileName = @"TexturePacker.exe";
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.Start();
        process.WaitForExit();
        process.Dispose();
        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
    }
}
