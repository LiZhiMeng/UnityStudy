using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class Load {


    private static Load instace;
    public static Load Instance
    {
        get
        {
            if(instace == null)
            {
                instace = new Load();
            }
            return instace;
        }
    }

    //.ttf.{ABSUFFIX}

    /// <summary>
    /// 加载字体
    /// </summary>
    /// <param name="fontName"></param>
    /// <returns></returns>
    public Font LoadFont(string fontName)
    {
        Debug.Log("LoadFont");
        string fontPath = $"{Application.streamingAssetsPath}/font/{fontName}{TCommon.ABSUFFIX_AFTER}";
        AssetBundle ab =  AssetBundle.LoadFromFile(fontPath);
        string _fontUperName = fontName.Substring(0, fontName.IndexOf(".")).ToUpper();
        Font font = ab.LoadAsset<Font>(_fontUperName) as Font;
        return font;
    }
    
    /// <summary>
    /// 加载声音
    /// </summary>
    /// <param name="clipName"></param>
    /// <returns></returns>
    public AudioClip LoadClip(string clipName)
    {
        Debug.Log("LoadClip");
        string abName = clipName + TCommon.ABSUFFIX_AFTER;
        string assetPath = $"{Application.streamingAssetsPath}/sound/{abName}";
        AssetBundle ab = AssetBundle.LoadFromFile(assetPath);
        AudioClip clip =  ab.LoadAsset<AudioClip>(clipName.Substring(0, clipName.IndexOf(".")));
        return clip;
    }
    
    public Texture2D LoadIcon(string iconName )
    {
        Debug.Log("LoadIcon");
        string _iconFullName = iconName + TCommon.ABSUFFIX_AFTER;
        string path =$"{Application.streamingAssetsPath}/icon/{_iconFullName}"; 
        AssetBundle ab = AssetBundle.LoadFromFile(path);
        iconName =  iconName.Substring(0, iconName.IndexOf("."));
        return ab.LoadAsset<Texture2D>(iconName);
    }
    
    
    public Sprite LoadAtlas(string atlasName, string spriteName )
    {
        Debug.Log("LoadAtlas");
        string abPath = $"{Application.streamingAssetsPath}/atlas/{atlasName}{TCommon.ABSUFFIX_AFTER}";
        AssetBundle ab = AssetBundle.LoadFromFile(abPath);
        GameObject atlasPrefab = ab.LoadAsset<GameObject>(atlasName);
        atlasPrefab.SetParent(GameObject.Find("Canvas").transform);
        TUIAtlas tuiAtlas = atlasPrefab.GetComponent<TUIAtlas>();
        TUIAtlasSheet sheet = tuiAtlas.sheet;
        TUiSpriteData data = new TUiSpriteData();
        for (int i = 0; i < sheet.uispriteList.Count; i++)
        {
            if (sheet.uispriteList[i].spriteName == spriteName)
            {
                data = sheet.uispriteList[i];
                break;
            }
        }
        Texture2D tx2D = ab.LoadAsset(atlasName) as Texture2D;
        //使用图集和精灵信息，加载出精灵
        Sprite sprite = Sprite.Create(tx2D,data.rect,data.pivot, 100f,0,SpriteMeshType.Tight,Vector4.zero);
        return sprite;
    }

    public GameObject LoadModel(string modelName)
    {
        //加载图片
        string pngPath = $"{Application.streamingAssetsPath}/model/{modelName}_png{TCommon.ABSUFFIX_AFTER}";
        string fbxPath = $"{Application.streamingAssetsPath}/model/{modelName}_base{TCommon.ABSUFFIX_AFTER}";
        AssetBundle ab_png = AssetBundle.LoadFromFile(TCommon.RemovePathPrefix(pngPath));
        Texture2D png = ab_png.LoadAsset<Texture2D>("city02_beauty");
        
        //加载材质球json
        string jsonPath = $"{Application.streamingAssetsPath}/model/{modelName}_mat{TCommon.ABSUFFIX_AFTER}";
        AssetBundle ab_json = AssetBundle.LoadFromFile(jsonPath);
        TextAsset textAsset = ab_json.LoadAsset<TextAsset>("mat");
        MatData matData = new MatData();
        matData = JsonMapper.ToObject<MatData>(textAsset.text);
        Shader shader = new Shader();
        
        shader = Shader.Find(matData.shader);
        Material mat = new Material( shader);
        mat.mainTexture = png;
        Debug.Log("aa:"+matData.shader);

        //加载fbx
        AssetBundle fbx_ab = AssetBundle.LoadFromFile(TCommon.RemovePathPrefix(fbxPath));
        GameObject prefab = fbx_ab.LoadAsset<GameObject>(modelName);
        GameObject model = GameObject.Instantiate(prefab);
        MeshRenderer render = model.GetComponent<MeshRenderer>();
        render.material = mat;
        return model;

    }
}
