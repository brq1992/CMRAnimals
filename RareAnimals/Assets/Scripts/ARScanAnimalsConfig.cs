using System.Collections.Generic;
using UnityEngine;

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
    //public string name = "";
    public string[] ANames;
    //public AnimalCamera[] AnimationTrans;
}

[System.Serializable]
public class AnimalContent
{
    public Sprite Bg;
    public Sprite body;
    public Sprite font;
}

[System.Serializable]
public class AnimalCamera
{
    public string AName;
    public Vector3 LocalPos;
    public Vector3 LocalRot;
}
