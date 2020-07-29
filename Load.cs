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



    /// <summary>
    /// 加载字体
    /// </summary>
    /// <param name="fontName"></param>
    /// <returns></returns>
    public Font LoadFont(string fontName)
    {
        string fontPath = $"{Application.streamingAssetsPath}/font/{fontName}";
        AssetBundle ab =  AssetBundle.LoadFromFile(fontPath);
        string _fontUperName = fontName.Substring(0, fontName.IndexOf(".")).ToUpper();
        Font font = ab.LoadAsset<Font>(_fontUperName) as Font;
        return font;
    }


}
