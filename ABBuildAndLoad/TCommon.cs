using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using LitJson;
using UnityEngine;

public class TCommon : MonoBehaviour
{
    public static string _font = "font";
    public static string _sound = "sound";
    public static string _atlas = "atlas";
    public static string _icon = "icon";
    public static string _ui = "ui";
    public static string _effect = "effect";
    public static string _scene = "scene";
    public static string _model = "model";
    public static string ABSUFFIX = "unity3d";
    public static string ABSUFFIX_AFTER = ".unity3d";

    public static string FontBuildInPath = "/PracticeAssets/Resources/font";
    public static string SoundBuildInPath = "/PracticeAssets/Resources/sound";
    public static string IconBuildPath = "/PracticeAssets/Resources/icon";
    public static string AtlasBuildPath = "/PracticeAssets/Resources/atlas";
    public static string ModelBuildPath = "/PracticeAssets/Resources/model";

    public static Dictionary<string, string> oldVersion = new Dictionary<string, string>();
    public static Dictionary<string, string> newVersion = new Dictionary<string, string>();

    public static List<string> _streamingAssetFileList = new List<string>();

    /// <summary>
    /// 绝对路径转相对路径
    /// </summary>
    public static string GetAssetPath(string _path)
    {
        int idx = _path.IndexOf("Assets/");
        _path = _path.Substring(idx);
        return _path;
    }

    /// <summary>
    /// 拿到新资源的版本
    /// </summary>
    public static void GetNewAssetVersion(string path)
    {
        newVersion.Clear();
        string md5Data = TCommon.ComputeMD5(path);
        newVersion.Add(GetAssetPath(path), md5Data);
    }

    /// <summary>
    /// 拿到旧资源的版本
    /// </summary>
    public static void GetOldAssetVersion(string type)
    {
        string version_path = $"{Application.dataPath}/Practice/UnityStudy/ABBuildAndLoad/version/{type}.json";
        if (!File.Exists(version_path)) return;
        oldVersion.Clear();
        try
        {
            oldVersion = JsonMapper.ToObject<Dictionary<string, string>>(File.ReadAllText(version_path));
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    /// <summary>
    /// 打包前判断，是否需要打包// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static bool IsNeedPak(string path)
    {
        bool needPak = true;

        if (oldVersion == null)
        {
            return true;
        }

        if (!oldVersion.ContainsKey(GetAssetPath(path)))
        {
            return true;
        }

        if (!newVersion.ContainsKey(GetAssetPath(path)))
        {
            Debug.LogError(path + ":newVersion error");
            return true;
        }

        if (oldVersion[GetAssetPath(path)] != newVersion[GetAssetPath(path)])
        {
            return true;
        }

        return false;
    }


    /// <summary>
    /// 保存资源版本文件
    /// </summary>
    /// <param name="type"></param>
    public static void SaveAssetVersion(string type)
    {
        StringBuilder builder = new StringBuilder();
        JsonWriter write = new JsonWriter(builder);
        write.PrettyPrint = true;
        JsonMapper.ToJson(newVersion, write);
        File.WriteAllText($"{Application.dataPath}/Practice/UnityStudy/ABBuildAndLoad/version/{type}.json",
            builder.ToString());
    }

    /// <summary>
    /// string路径的md5转换
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string ComputeMD5(string path)
    {
        StringBuilder sb = new StringBuilder();
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] data = File.ReadAllBytes(path);
        byte[] md5Data = md5.ComputeHash(data);
        for (int i = 0; i < md5Data.Length; i++)
        {
            sb.Append(md5Data[i].ToString("x2"));
        }

        return sb.ToString();
    }

    /// <summary>
    /// 通过路径全部文件
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static List<string> GetFileListByPath(string path)
    {
        fileList.Clear();
        dicList.Clear();
        dicList.Add(path);
        GetRecursionDics(path);
        for (int i = 0; i < dicList.Count; i++)
        {
            string[] files = Directory.GetFiles(dicList[i]);
            foreach (var cur_file in files)
            {
                if (!cur_file.Contains("meta"))
                {
                    fileList.Add(cur_file);
                }
            }
        }

        return fileList;
    }

    public static List<string> fileList = new List<string>();
    public static List<string> dicList = new List<string>();

    /// <summary>
    /// 获取全部文件// </summary>
    /// <param name="path"></param>
    public static void GetRecursionDics(string path)
    {
        string[] dirs = Directory.GetDirectories(path);
        foreach (var dirPath in dirs)
        {
            if (!dicList.Contains(dirPath))
            {
                dicList.Add(dirPath);
            }

            GetRecursionDics(dirPath);
        }
    }


    public static string RemovePathPrefix(string path)
    {
        int idx = path.IndexOf("Assets/");
        return path.Substring(idx);
    }


    #region BuildModule




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
    /// 获取目录下所有的manifest文件
    /// </summary>
    /// <param name="path"></param>
    public static void GetList(string path)
    {
        string[] dirs = Directory.GetDirectories(path);
        for (int i = 0; i < dirs.Length; i++)
        {
            string[] files = Directory.GetFiles(dirs[i]);
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
    /// 获取目录下所有文件
    /// </summary>
    /// <param name="path"></param>
    public static void GetList(string path, ref List<string> fileLists)
    {
        string[] dirs = Directory.GetDirectories(path);
        for (int i = 0; i < dirs.Length; i++)
        {
            string[] files = Directory.GetFiles(dirs[i]);
            for (int j = 0; j < files.Length; j++)
            {
                if (!files[j].Contains("manifest") && !_streamingAssetFileList.Contains(files[j]))
                {
                    fileLists.Add(files[j]);
                }
            }

            if (Directory.GetDirectories(dirs[i]).Length > 0)
            {
                GetList(dirs[i],ref fileLists);
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
            Directory.Delete(path, true);
        }

        Directory.CreateDirectory(path);
    }



    #endregion
}