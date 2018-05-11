using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARScanAnimalsConfig : ScriptableObject
{
    public List<AnimalContents> list = new List<AnimalContents>();
}

[System.Serializable]
public class AnimalContents
{
    public List<AnimalContent> list = new List<AnimalContent>();
    public string Key = "";
    public GameObject prefab;
    public Sprite AnimalName;
    public string[] ANames;
    public RawImage RawImage;
    public List<Texture2D> Texture2Ds = new List<Texture2D>();
}

[System.Serializable]
public class AnimalContent
{
    public Sprite Bg;
    public Sprite body;
    public Sprite font;
}
