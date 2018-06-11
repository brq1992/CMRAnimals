﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARScanAnimalsConfig : ScriptableObject
{
    public List<AnimalContents> list = new List<AnimalContents>();
}

[System.Serializable]
public class AnimalContents
{
    public string Key = "";
    public GameObject prefab;
    public Sprite AnimalName;
    public Vector3 AnimalScale;
    public Vector3 AnimalPos;
    public Quaternion AnimalRot;
    public string[] ANames;
    public List<Texture2D> Texture2Ds = new List<Texture2D>();
}
