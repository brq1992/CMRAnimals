
using UnityEngine;
using UnityEngine.UI;

public class AnimalViewManager : MonoBehaviour
{
    public RawImage RawImage;

    public void Init(Texture2D image)
    {
        RawImage.texture = image;
    }

}
