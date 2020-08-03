using System.Collections;
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

    string textureDic;
    private string[] atlasDic;
    private List<name_dic> list_dic = new List<name_dic>();
    private string atlasResPath;
    private void OnEnable()
    {
        atlasResPath= Application.dataPath + "/Resources/atlas";
        textureDic = Application.dataPath + "/Resources/texture";
        atlasDic = Directory.GetDirectories(textureDic);
        list_dic.Clear();
        foreach (var VARIABLE in atlasDic)
        {
            string [] split = VARIABLE.Split('\\');
            string atlasName = split[split.Length - 1];
            name_dic dic = new name_dic();
            dic._atlasDic = VARIABLE;
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
            }
        }
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
        string dataPath = $"{dicPath}/{dic._atlasName}.txt";
        StringBuilder atlasPaths = new StringBuilder();

        string arg = string.Format(commond, pngPath, dataPath,atlasPaths);
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        process.StartInfo.Arguments = arg;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.FileName = @"TexturePacker.exe";

        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.Start();
        process.WaitForExit();
        process.Dispose();
    }
    
}
