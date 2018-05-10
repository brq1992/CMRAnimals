
using UnityEngine;
using UnityEngine.UI;

public class AnimalViewManager : MonoBehaviour
{
    public Image bg;
    public Image font;
    public Image animal;
    public void Init(AnimalContent result)
    {
        bg.sprite = result.Bg;
        font.sprite = result.font;
        animal.sprite = result.body;
    }
}
