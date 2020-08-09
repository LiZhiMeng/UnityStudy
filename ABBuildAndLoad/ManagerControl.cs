using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ManagerControl : MonoBehaviour  {

    // Use this for initialization
    void Start ()
    {
        LoadInit();
        LoadUIText();
        LoadSound();
        LoadIcon();
        LoadAtlas();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private GameObject _canvasGo;
    void LoadInit()
    {
        _canvasGo = GameObject.Find("Canvas");
        if (_canvasGo == null)
        {
            Debug.LogError("CanvasGo is null");
        }
        
    }

    void LoadUIText()
    {
        Font _font = Load.Instance.LoadFont("fzzdhjw.ttf");
        Text text = GameObject.Find("Text").GetComponent<Text>();
        text.font = _font;
        text.text = "字体加载";
    }

    void LoadSound()
    {
        AudioClip _clip = Load.Instance.LoadClip("mishu_1.mp3");
        AudioSource source = _canvasGo.GetComponent<AudioSource>();
        if (source == null)
        {
            source = _canvasGo.AddComponent<AudioSource>();
        }
        source.clip = _clip;
        source.loop = true;
        source.Play();
    }

    void LoadIcon()
    {
        Texture2D texture = Load.Instance.LoadIcon("bg_cjzclb2.png");
        Sprite _sprite = Sprite.Create(texture, new Rect(0,0, texture.width, texture.height) , Vector2.zero);
        GameObject canvas = GameObject.Find("Canvas");
        GameObject textureNode = new GameObject("Image");
        textureNode.transform.SetParent(canvas.transform, true);
        Image image = textureNode.AddComponent<Image>();
        image.sprite = _sprite;
        textureNode.GetComponent<RectTransform>().localPosition = Vector3.zero;
        image.SetNativeSize();
    }
    
    void LoadAtlas()
    {
        GameObject atlasGo = GameObject.Find("AtlasGo");
        if (atlasGo != null)
        {
            GameObject.DestroyImmediate(atlasGo);
        }
        atlasGo = new GameObject("AtlasGo");
        atlasGo.SetParent(_canvasGo.transform);
        Image img = atlasGo.AddComponent<Image>();
        Sprite sp = Load.Instance.LoadAtlas("activity_g6", "bg_zcm2");
        
        img.sprite = sp;
        img.SetNativeSize();
    }
}
