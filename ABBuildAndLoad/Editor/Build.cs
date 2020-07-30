using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Build : Editor
{

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
    static string SoundBuildInPath = "/Resources/sound";
    static string IconBuildPath = "/Resources/icon";
    public static string ABSUFFIX = "unity3d";
     enum AssetType
    {
        font,
        sound,
        icon,
    }
    static Dictionary<AssetType, string> TypeDic = new Dictionary<AssetType, string>();


    [MenuItem("Assets/EditorWindow &1",false)]
    public static void OpenEditorWindow()
    {
        WindowTool tool = (WindowTool) EditorWindow.GetWindow(typeof(WindowTool));
        tool.Show();
    }


    [MenuItem("Assets/BuildFont", false)]
    public static void BuildFont()
    {
        Debug.Log("Menu: BuildFont");
        InitTypeDic();
        string type = TypeDic[AssetType.font];
        ClearAB();
        List<string> suff = new List<string>();
        suff.Add("otf");
        suff.Add("ttf");
        SearchFileNeedBuild(Application.dataPath + FontBuildInPath, suff);
        BuildBundle(GetBundlePathFromType(type));   
        DeleteManifest(type);
        ClearAB();
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/BuildSound &2", false)]
    public static void BuildSound()
    {
        Debug.Log("Menu: Build Sound");
        ClearAB();
        InitTypeDic();
        string type = TypeDic[AssetType.sound];
        List<string> suffix = new List<string>();
        suffix.Add(".mp3");
        string buildPath = SoundBuildInPath;
        SearchFileNeedBuild(Application.dataPath + buildPath, suffix);
        BuildBundle(GetBundlePathFromType(type));
        DeleteManifest(type);
        ClearAB();
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/BuildIcon &3",false)]
    public static void BuildIcon()
    {
        Debug.Log("Menu: BuildIcon");
        ClearAB();
        InitTypeDic();
        string buildPath = Application.dataPath + IconBuildPath;
        List<string> suffix = new List<string>();
        suffix.Add(".png");
        SearchFileNeedBuild(buildPath, suffix);
        string type = TypeDic[AssetType.icon];
        BuildBundle(GetBundlePathFromType(type));
        DeleteManifest(type);
        ClearAB();
    }

    /// <summary>
    /// 生成图集选择界面
    /// </summary>
    [MenuItem("Assets/GenerateAtlas &4",false)]
    public static void GenerateAtlas()
    {
        AtlasWindow atlasWindow = EditorWindow.GetWindowWithRect <AtlasWindow>(new Rect(150f, 100f, 800f, 600f));
        Debug.Log("sho");
        atlasWindow.Show();
    }


    #region BuildModule

    /// <summary>
    /// 清理ab包
    /// </summary>
    static void ClearAB()
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
    /// 初始化类型字典
    /// </summary>
    static void InitTypeDic()
    {
        TypeDic.Clear();
        TypeDic.Add(AssetType.font, "font");
        TypeDic.Add(AssetType.sound, "sound");
        TypeDic.Add(AssetType.icon, "icon");
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
    static void DeleteManifest(string type)
    {
        string[] files = Directory.GetFiles($"{Application.streamingAssetsPath}/{type}");
        foreach (string _file in files)
        {
            if (_file.Contains("manifest")) { File.Delete(_file); }
        }
    }

    /// <summary>
    /// 查询需打包的字体文件
    /// </summary>
    static void SearchFileNeedBuild(string buildPath, List<string> suffix)
    {
        string[] filePaths = Directory.GetFiles(buildPath);
        for (int i = 0; i < filePaths.Length; i++)
        {
            if (filePaths[i] == buildPath) continue;
            string _filePath = filePaths[i];
            foreach (string suf in suffix)
            {
                if (_filePath.Contains(suf))
                {
                    if (!_filePath.Contains("meta"))
                    {
                        string fileName = Path.GetFileName(_filePath);
                        int idx = _filePath.IndexOf("Assets");
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

    public override void OnInspectorGUI()
    {
        GUILayout.Label("aaaa");
    }

}
