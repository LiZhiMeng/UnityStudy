using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class BuildTool : Editor
{
    
    
    
    private static BuildTool instance;
    public static BuildTool Instance()
    {
        if(instance == null)
        {
            instance = new BuildTool();
            
        }
        return instance;
    }

    static string FontBuildInPath = "/PracticeAssets/Resources/font";
    static string SoundBuildInPath = "/PracticeAssets/Resources/sound";
    static string IconBuildPath = "/PracticeAssets/Resources/icon";
    static string AtlasBuildPath = "/PracticeAssets/Resources/atlas";
    public static string ABSUFFIX = "unity3d";
    public static List<string> _streamingAssetFileList = new List<string>();


    [MenuItem("Assets/EditorWindow &5",false)]
    public static  void OpenEditorWindow()
    {
        WindowTool tool = (WindowTool) EditorWindow.GetWindow(typeof(WindowTool));
        tool.Show();
    }


    [MenuItem("Assets/BuildFont", false)]
    public static  void BuildFont()
    {
        Debug.Log("Menu: BuildFont");

        string type = TConstant._font;
        ClearAB();
        List<string> suff = new List<string>();
        suff.Add("otf");
        suff.Add("ttf");
        SearchFileNeedBuild(Application.dataPath + FontBuildInPath, suff);
        BuildBundle(GetBundlePathFromType(type));   
        DeleteManifest();
        ClearAB();
        AssetDatabase.Refresh();
    }

    // [MenuItem("Assets/BuildSound &2", false)]
    public static  void BuildSound()
    {
        Debug.Log("Menu: Build Sound");
        ClearAB();

        string type = TConstant._sound;
        List<string> suffix = new List<string>();
        suffix.Add(".mp3");
        string buildPath = SoundBuildInPath;
        SearchFileNeedBuild(Application.dataPath + buildPath, suffix);
        BuildBundle(GetBundlePathFromType(type));
        DeleteManifest();
        ClearAB();
        AssetDatabase.Refresh();
    }

    // [MenuItem("Assets/BuildIcon &3",false)]
    public static  void BuildIcon()
    {
        Debug.Log("Menu: BuildIcon");
        ClearAB();

        string buildPath = Application.dataPath + IconBuildPath;
        List<string> suffix = new List<string>();
        suffix.Add(".png");
        SearchFileNeedBuild(buildPath, suffix);
        string type = TConstant._icon;
        BuildBundle(GetBundlePathFromType(type));
        DeleteManifest();
        ClearAB();
    }

    /// <summary>
    /// 生成图集选择界面
    /// </summary>
    // [MenuItem("Assets/GenerateAtlas &4",false)]
    public static  void GenerateAtlas()
    {
        AtlasWindow atlasWindow = EditorWindow.GetWindowWithRect <AtlasWindow>(new Rect(150f, 100f, 800f, 600f));
        atlasWindow.autoRepaintOnSceneChange = false;
        atlasWindow.Show();
    }

    public static  void BuildAtlas()
    {

        BuildBundleAtlas window = EditorWindow.GetWindowWithRect<BuildBundleAtlas>(new Rect(150f, 150f, 800, 600f));
        window.Show();
    }
    

    #region BuildModule

    /// <summary>
    /// 清理ab包
    /// </summary>
    public static void ClearAB()
    {
        string[] bundles = AssetDatabase.GetAllAssetBundleNames();
        for (int i = 0; i < bundles.Length; i++)
        {
            string[] paths = AssetDatabase.GetAssetPathsFromAssetBundle(bundles[i]);
            for (int j = 0; j < paths.Length; j++)
            {
                AssetImporter assetImporter = AssetImporter.GetAtPath(paths[j]);
                assetImporter.assetBundleName = "";
            }
        }
        AssetDatabase.Refresh();
    }



    /// <summary>
    /// 获取打包路径
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string GetBundlePathFromType(string type)
    {
        string bundlePath = $"{Application.streamingAssetsPath}/{type}";
        return bundlePath;
    }

    /// <summary>
    /// 打包
    /// </summary>
    public  static  void BuildBundle(string outputPath)
    {
        if (Directory.Exists(outputPath))
        {
            Directory.Delete(outputPath, true);
        }
        AssetDatabase.Refresh();
        Directory.CreateDirectory(outputPath);
        BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
    }


    /// <summary>
    /// 删除manifest
    /// </summary>
    public static  void DeleteManifest()
    {
        _streamingAssetFileList.Clear();
        GetList(Application.streamingAssetsPath);
        foreach (var curFile in _streamingAssetFileList)
        {
            if (File.Exists(curFile))
            {
                Debug.Log("file2:"+curFile);
                File.Delete(curFile);    
            }
            
        }
        Debug.Log("删除manifest完成");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        

    }

    /// <summary>
    /// 获取目录下所有文件
    /// </summary>
    /// <param name="path"></param>
    public static void GetList(string path)
    {
        string [] dirs = Directory.GetDirectories(path);
        for (int i = 0; i < dirs.Length; i++)
        {
            string [] files = Directory.GetFiles(dirs[i]);
            for (int j = 0; j < files.Length; j++)
            {
                if (files[j].Contains("manifest"))
                {
                    _streamingAssetFileList.Add(files[j]);
                }
            }
            if (Directory.GetDirectories(dirs[i]).Length > 0)
            {
                GetList(dirs[i]);
            }
        }
    }


    
    /// <summary>
    /// 删除指定路径
    /// </summary>
    /// <param name="path"></param>
    public static void DeleteOutPutDir(string path)
    {
        path = $"{Application.streamingAssetsPath}/{path}";
        if (Directory.Exists(path))
        {
            Directory.Delete(path,true);
        }
        Directory.CreateDirectory(path);
    }

    /// <summary>
    /// 查询需打包的字体文件
    /// </summary>
    public  static void SearchFileNeedBuild(string buildPath, List<string> suffix)
    {
        string[] filePaths = Directory.GetFiles(buildPath);
        for (int i = 0; i < filePaths.Length; i++)
        {
            if (filePaths[i] == buildPath) continue;
            string _filePath = filePaths[i].ToLower();
            foreach (string suf in suffix)
            {
                if (_filePath.Contains(suf))
                {
                    if (!_filePath.Contains("meta"))
                    {
                        string fileName = Path.GetFileName(_filePath);
                        
                        int idx = _filePath.IndexOf("assets/");
                        string impPath = _filePath.Substring(idx);
                        AssetImporter assetImporter = AssetImporter.GetAtPath(impPath);
                        assetImporter.assetBundleName = fileName;
                        Debug.Log("fileName");
                        assetImporter.assetBundleVariant = ABSUFFIX;
                    }
                }
            }
        }
    }
    #endregion

    
    public static string RemovePathPrefix(string path)
    {
        int idx = path.IndexOf("Assets/");
        return path.Substring(idx);
    }
    
    

}
