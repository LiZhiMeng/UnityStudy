using System.Collections;
using System.Collections.Generic;
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
        string fontPath = $"{Application.streamingAssetsPath}/font/{fontName}{TConstant.ABSUFFIX_AFTER}";
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
        string abName = clipName + TConstant.ABSUFFIX_AFTER;
        string assetPath = $"{Application.streamingAssetsPath}/sound/{abName}";
        AssetBundle ab = AssetBundle.LoadFromFile(assetPath);
        AudioClip clip =  ab.LoadAsset<AudioClip>(clipName.Substring(0, clipName.IndexOf(".")));
        return clip;
    }
    
    public Texture2D LoadIcon(string iconName )
    {
        Debug.Log("LoadIcon");
        string _iconFullName = iconName + TConstant.ABSUFFIX_AFTER;
        string path =$"{Application.streamingAssetsPath}/icon/{_iconFullName}"; 
        AssetBundle ab = AssetBundle.LoadFromFile(path);
        iconName =  iconName.Substring(0, iconName.IndexOf("."));
        return ab.LoadAsset<Texture2D>(iconName);
    }
    
    
    public Sprite LoadAtlas(string atlasName, string spriteName )
    {
        Debug.Log("LoadAtlas");
        string abPath = $"{Application.streamingAssetsPath}/atlas/{atlasName}{TConstant.ABSUFFIX_AFTER}";
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
        Debug.LogError(data.rect);
        Sprite sprite = Sprite.Create(tx2D,data.rect,data.pivot,
                    100f,0,SpriteMeshType.Tight,Vector4.zero);
        return sprite;
    }


}
