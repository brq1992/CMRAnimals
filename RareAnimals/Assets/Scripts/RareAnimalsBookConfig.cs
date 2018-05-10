using System.Collections.Generic;
using UnityEngine;

public class RareAnimalsBookConfig : ScriptableObject
{
    public List<BookPare> South = new List<BookPare>();
}

[System.Serializable]
public class BookPare
{
    public List<TextSprite> Middle = new List<TextSprite>();
    public Sprite Title;
    public string Key = "";
}

[System.Serializable]
public class TextSprite
{
    public Sprite Sprite;
    public TextAsset Asset;
}
