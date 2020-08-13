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
    
    private void OnGUI()
    {
        foreach (name_dic dic in list_dic)
        {
            if (GUILayout.Button(dic._atlasName))//点击了按钮，则打图集
            {
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
        Object o = PrefabUtility.CreateEmptyPrefab(BuildTool.RemovePathPrefix(outPrefabPath));
        TUIAtlas tAtlas = prefab.AddComponent<TUIAtlas>();
        TUIAtlasSheet tSheetList = ScriptableObject.CreateInstance<TUIAtlasSheet>();
        AssetDatabase.CreateAsset(tSheetList, BuildTool.RemovePathPrefix( outTextAssetPath));


        TextAsset txtAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(BuildTool.RemovePathPrefix( outTxtPath));
        Debug.Log("Txt:" + txtAsset);
        string txtString = txtAsset.ToString() ;
        string[] split = txtString.Split('\n');
        foreach (var _curSplit in split)
        {
            if (_curSplit.Contains(";"))
            {
                string [] _dataSplit = _curSplit.Split(';');
                
                TUiSpriteData data = new TUiSpriteData();
                data.border = Vector4.zero;
                data.spriteName = _dataSplit[0];
                float x = 0;
                float.TryParse(_dataSplit[1],out x);
                float y = 0;
                float.TryParse(_dataSplit[2],out y);
                float width = 0;
                float.TryParse(_dataSplit[3],out width);
                float height = 0;
                float.TryParse(_dataSplit[4],out height);
                
                data.rect = new Rect(x,y,width,height);
                data.pivot = new Vector2(0.5f,0.5f);
                tSheetList.uispriteList.Add(data);
            }
        }
        
        SpriteMetaData[] metaData = new SpriteMetaData[tSheetList.uispriteList.Count];
        for (int i = 0; i < metaData.Length; i++)
        {
            metaData[i].pivot = tSheetList.uispriteList[i].pivot;
            metaData[i].border = tSheetList.uispriteList[i].border;
            metaData[i].rect = tSheetList.uispriteList[i].rect;
            metaData[i].name = tSheetList.uispriteList[i].spriteName;
        }
        
        TextureImporter textureImporter = TextureImporter.GetAtPath(BuildTool.RemovePathPrefix(outPngPath)) as TextureImporter;
        textureImporter.spritesheet = metaData;
        textureImporter.textureType = TextureImporterType.Sprite;
        textureImporter.spriteImportMode = SpriteImportMode.Multiple;
        textureImporter.mipmapEnabled = false;
        textureImporter.SaveAndReimport();

        tAtlas.sheet = tSheetList;
        
        
        
        
        Texture2D tx2d = AssetDatabase.LoadAssetAtPath<Texture2D>(BuildTool. RemovePathPrefix(outPngPath));
        tAtlas.Texture = tx2d;
        PrefabUtility.ReplacePrefab(prefab, o);
        
    }


    
    /// <summary>
    /// 运行texturePacker打包图集
    /// </summary>
    void ProcessTexturePacker(name_dic dic)
    {

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
        string AtlasSize = "1024";
        //核心代码：  传入四个参数， 依次是commond命令载体， 打成整图后的图片路径， 生成的txt文件位置，   需要被打成图集的图片们（依次用空格间隔开）
      string commond = @" --sheet {0} --data {1} --format unity-texture2d --trim-mode None --pack-mode Best --algorithm Polygon --max-size " + AtlasSize + @" --size-constraints POT --disable-rotation --scale 1 {2}";
        string arg = string.Format(commond, pngPath, dataPath, atlasPaths.ToString());
        EditorUtility.DisplayProgressBar("message","texturePacker",0.1f);
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
