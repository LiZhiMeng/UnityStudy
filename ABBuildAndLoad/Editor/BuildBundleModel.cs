using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LitJson;
using UnityEditor;
using UnityEngine;

public static class BuildBundleModel
{
    public static List<string> fileList = new List<string>();

    public static void MarkAB(string modelPathDir)
    {
        Debug.Log("dir:" + modelPathDir);
        //先拿到源
        string[] dirs = Directory.GetDirectories(modelPathDir);
        foreach (var dir in dirs)
        {
            MarkByOne(dir);
        }
    }

    public static void MarkByOne(string path)
    {
        //取得文件路径
        List<string> fileList = new List<string>();
        TCommon.GetList(path, ref fileList);
        string pngPath = "";
        string matPath = "";
        string fbxPath = "";
        string prefabPath = "";
        foreach (var cur_path in fileList)
        {
            if (cur_path.Contains(".meta")) continue;
            if (cur_path.ToLower().Contains(".fbx"))
            {
                fbxPath = cur_path;
            }

            if (cur_path.ToLower().Contains(".mat"))
            {
                matPath = cur_path;
            }

            if (cur_path.Contains(".prefab"))
            {
                prefabPath = cur_path;
            }
        }

        string rootName = path.Substring(path.LastIndexOf('\\') + 1);
        DoMatAndPng(rootName, matPath);
        DoFbx(rootName, fbxPath);
    }

    public static void DoMatAndPng(string rootName, string path)
    {
        string assetPath = TCommon.RemovePathPrefix(path);
        Material m = AssetDatabase.LoadAssetAtPath<Material>(assetPath);
        MatData data = MatTool.MatToJson(m);
        string jsonData = JsonMapper.ToJson(data);
        StringBuilder sb = new StringBuilder();
        JsonWriter write = new JsonWriter(sb);
        JsonMapper.ToJson(data, write);
        int idx = path.LastIndexOf('\\');
        string paraPath = path.Substring(0, idx);
        Debug.Log("paraPath:" + paraPath);
        paraPath = paraPath + "\\mat.json";
        File.WriteAllText(paraPath, sb.ToString());
        //对mat_json文件标记ab
        AssetImporter importer = AssetImporter.GetAtPath(TCommon.RemovePathPrefix(paraPath));
        importer.assetBundleName = $"{rootName}_mat";
        importer.assetBundleVariant = TCommon.ABSUFFIX;

        AssetImporter importerTex = AssetImporter.GetAtPath(data.attributes[0].texAttri.Item3.bundleDir);
        importerTex.assetBundleName = $"{rootName}_png";
        importerTex.assetBundleVariant = TCommon.ABSUFFIX;
    }

    public static void DoFbx(string rootName, string fbxPath)
    {
        GameObject fbxGo = AssetDatabase.LoadAssetAtPath<GameObject>(TCommon.RemovePathPrefix(fbxPath));
        GameObject instanGo = GameObject.Instantiate(fbxGo);
        string prefabPath = fbxPath.Substring(0, fbxPath.LastIndexOf('\\'));
        prefabPath = $"{prefabPath}\\{rootName}.prefab";
        prefabPath = prefabPath.Replace("\\", "/");
        Object newPrefab = PrefabUtility.CreateEmptyPrefab(TCommon.RemovePathPrefix(prefabPath));
        GameObject finalPrefab = PrefabUtility.ReplacePrefab(instanGo, newPrefab);
        string finalPath = AssetDatabase.GetAssetPath(finalPrefab);
        AssetImporter importer = AssetImporter.GetAtPath(finalPath);
        importer.assetBundleName = $"{rootName}_base";
        importer.assetBundleVariant = TCommon.ABSUFFIX;
    }
}