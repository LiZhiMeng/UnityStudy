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


    private string suff = ".unity3d";

        //.ttf.{ABSUFFIX}

    /// <summary>
    /// 加载字体
    /// </summary>
    /// <param name="fontName"></param>
    /// <returns></returns>
    public Font LoadFont(string fontName)
    {
        string fontPath = $"{Application.streamingAssetsPath}/font/{fontName}{suff}";
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
        string abName = clipName + suff;
        string assetPath = $"{Application.streamingAssetsPath}/sound/{abName}";
        AssetBundle ab = AssetBundle.LoadFromFile(assetPath);
        AudioClip clip =  ab.LoadAsset<AudioClip>(clipName.Substring(0, clipName.IndexOf(".")));
        return clip;
    }
    
    public Texture2D LoadIcon(string iconName )
    {
        string _iconFullName = iconName + suff;
        string path =$"{Application.streamingAssetsPath}/icon/{_iconFullName}"; 
        AssetBundle ab = AssetBundle.LoadFromFile(path);
        iconName =  iconName.Substring(0, iconName.IndexOf("."));
        return ab.LoadAsset<Texture2D>(iconName);
    }


}
