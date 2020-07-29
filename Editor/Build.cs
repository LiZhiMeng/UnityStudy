 using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Build : Editor {

    //private Build instance;
    //public Build Instance
    //{
    //    get
    //    {
    //        if(instance == null)
    //        {
    //            instance = new Build();
    //        }
    //        return instance;
    //    }
    //}

    static string FontBuildInPath = "/Resources/font";
    public static string ABSUFFIX = "unity3d";
    enum AssetType
    {
        font,
    }
    static Dictionary<AssetType, string> TypeDic = new Dictionary<AssetType, string>();


    [MenuItem("Assets/BuildFont &1",false,1)]
    public static void BuildFont()
    {
        Debug.Log("BuildFont");
        InitTypeDic();
        string type = TypeDic[AssetType.font];
        ClearAB();
        SearchFileNeedBuild();
        BuildBundle(GetBundlePathFromType(type));
        DeleteManifest(type);
        ClearAB();
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 清理ab包
    /// </summary>
    static void ClearAB()
    {
        string [] bundles =  AssetDatabase.GetAllAssetBundleNames();
        for(int i = 0; i < bundles.Length; i++)
        {
            string [] paths = AssetDatabase.GetAssetPathsFromAssetBundle(bundles[i]);
            for(int j = 0; j < paths.Length; j++)
            {
                AssetImporter assetImporter =  AssetImporter.GetAtPath(paths[j]);
                assetImporter.assetBundleName = "";
            }
        }
    }
    /// <summary>
    /// 初始化类型字典
    /// </summary>
    static void InitTypeDic()
    {
        TypeDic.Add(AssetType.font, "font");
    }

    /// <summary>
    /// 获取打包路径
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    static string GetBundlePathFromType(string type)
    {
        string bundlePath = $"{Application.streamingAssetsPath}/{type}";
        return bundlePath;
    }

    /// <summary>
    /// 打包
    /// </summary>
    static void BuildBundle(string outputPath)
    {
        if(Directory.Exists(outputPath))
        {
            Directory.Delete(outputPath,true);
        }
        Directory.CreateDirectory(outputPath);
        BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.ChunkBasedCompression,  EditorUserBuildSettings.activeBuildTarget );
    }

    /// <summary>
    /// 删除manifest
    /// </summary>
    static void DeleteManifest(string type)
    {
        string[] files = Directory.GetFiles($"{Application.streamingAssetsPath}/{type}");
        foreach(string _file in files)
        {
            if (_file.Contains("manifest")) 
            {
                File.Delete(_file);
            }
        }
    }

    /// <summary>
    /// 查询需打包的文件
    /// </summary>
    static void SearchFileNeedBuild()
    {
        string [] filePaths =  Directory.GetFiles(Application.dataPath + FontBuildInPath);
        for(int i = 0; i < filePaths.Length; i++)
        {
            string _filePath = filePaths[i];
            if (_filePath.ToLower() .Contains("ttf") || _filePath.ToLower() .Contains("otf"))
            {
                if (!_filePath.Contains("meta"))
                {
                    string fileName =  Path.GetFileName(_filePath);
                    int idx =  _filePath.IndexOf("Assets");
                    string impPath = _filePath.Substring(idx); 
                    AssetImporter assetImporter =  AssetImporter.GetAtPath(impPath);
                    assetImporter.assetBundleName = fileName;
                    assetImporter.assetBundleVariant = ABSUFFIX;
                }
            }
        }
    }
}
