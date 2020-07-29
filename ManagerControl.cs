using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ManagerControl : MonoBehaviour {

    public string ABSUFFIX = "unity3d";
    // Use this for initialization
    void Start () {
        LoadUIText();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void LoadUIText()
    {
        Font _font = Load.Instance.LoadFont($"fzzdhjw.ttf.{ABSUFFIX}");
        
        Text text = GameObject.Find("Text").GetComponent<Text>();
        text.font = _font;
        text.text = "字体加载";
    }
}
