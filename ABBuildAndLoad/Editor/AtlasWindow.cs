using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class AtlasWindow : EditorWindow
{


    string textureDic;
    private void OnEnable()
    {
        //textureDic = Application.dataPath + "/Resources/texture";
    }

    //private string atlasStreamingAssetPath = Application.streamingAssetsPath + "/" + PublicEnum.ATLAS;
    private void OnGUI()
    {


        //string [] atlasDic = Directory.GetDirectories(textureDic);

        //AssetDatabase.Refresh();

        //GUILayout.BeginVertical();
        //Debug.Log(atlasDic.Length);
        //foreach(string dic in atlasDic)
        //{
        //    Debug.Log(dic);
        //    if (GUILayout.Button(dic))//点击了按钮，则打图集
        //    {
        //        //查找需要打的图集
        //        string [] filse =  Directory.GetFiles(dic);

        //    }
        //}

        //GUILayout.EndVertical();

        
        Debug.Log("aa");
        //GUILayout.BeginVertical();
        GUILayout.Button("aa");
        GUILayout.Button("bb");

        //GUILayout.EndVertical();






        //foreach (KeyValuePair<string, string> dic in namePath)
        //{
        //    if (GUILayout.Button(dic.Key))
        //    {
        //        //点击图集按钮时，对图集打包

        //        string AtlasSize = "1024";
        //        string commond = @" --sheet {0} --data {1} --format unity-texture2d --trim-mode None --pack-mode Best --algorithm Polygon --max-size " + AtlasSize + @" --size-constraints POT --disable-rotation --scale 1 {2}";

        //        ////创建目录
        //        //string dicPath = atlasStreamingAssetPath + "/" + dic.Key;
        //        //if (Directory.Exists(dicPath))
        //        //{
        //        //    Directory.Delete(dicPath, true);
        //        //}
        //        //AssetDatabase.Refresh();
        //        //Directory.CreateDirectory(dicPath);

        //        //string pngPath = $"{dicPath}/{dic.Key}.png";
        //        //string dataPath = $"{dicPath}/{dic.Key}.txt";
        //        //StringBuilder atlasPaths = new StringBuilder();
        //        //string arg = string.Format(commond,)
        //        //System.Diagnostics.Process process = new System.Diagnostics.Process();
        //        //process.StartInfo.Arguments = arg;
        //        //process.StartInfo.UseShellExecute = false;
        //        //process.StartInfo.FileName = @"TexturePacker.exe";

        //        //process.StartInfo.RedirectStandardOutput = true;
        //        //process.StartInfo.RedirectStandardError = true;
        //        //process.Start();
        //        //process.WaitForExit();
        //        //process.Dispose();
        //    }
        //}
    }


}
