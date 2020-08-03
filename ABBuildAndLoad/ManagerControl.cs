using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ManagerControl : MonoBehaviour  {

    // Use this for initialization
    void Start () {

        //LoadUIText();
        //LoadSound();
        LoadIcon();
    }
	
	// Update is called once per frame
	void Update () {
		
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
        GameObject camera = GameObject.Find("Main Camera");
        AudioSource source = camera.GetComponent<AudioSource>();
        if (source == null)
        {
            source = camera.AddComponent<AudioSource>();
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
}
