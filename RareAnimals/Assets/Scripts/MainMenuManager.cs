using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {

    private Image ar;
    private Image audi;
    private Image book;

    public delegate void OnClickButton();
    public event OnClickButton OnClickAREvent;
    public event OnClickButton OnClickAudioEvent;
    public event OnClickButton OnClickBookEvent;


    public void Init()
    {
        Transform arTs = transform.Find("AR");
        Transform audioTs = transform.Find("Audio");
        Transform bookTs = transform.Find("Book");


        Button arBt = arTs.GetComponent<Button>();
        arBt.onClick.AddListener(OnClickARBtn);

        Button audioBt = audioTs.GetComponent<Button>();
        audioBt.onClick.AddListener(OnClickAudioBtn);

        Button bookBt = bookTs.GetComponent<Button>();
        bookBt.onClick.AddListener(OnClickBookBtn);

        ar = arTs.GetChild(0).GetComponent<Image>();
        audi = audioTs.GetChild(0).GetComponent<Image>();
        book = bookTs.GetChild(0).GetComponent<Image>();
    }


    public void SetActiveMenu(MenuItem item)
    {
        switch(item)
        {
            case MenuItem.ARScan:
                {
                    ar.enabled = true;
                    audi.enabled = false;
                    book.enabled = false;
                    break;
                }
            case MenuItem.AudioNav:
                {
                    ar.enabled = false;
                    audi.enabled = true;
                    book.enabled = false;
                    break;
                }
            case MenuItem.AnimalsBook:
                {
                    ar.enabled = false;
                    audi.enabled = false;
                    book.enabled = true;
                    break;
                }
            default:
                {
                    ar.enabled = false;
                    audi.enabled = false;
                    book.enabled = false;
                    Debug.LogError("MenuItem is wrong! item is " + item.ToString());
                    break;
                }
        }
    }

#region Button click
    public void OnClickARBtn()
    {
        if(null!=OnClickAREvent)
        {
            OnClickAREvent();
        }
    }

    public void OnClickAudioBtn()
    {
        if (null != OnClickAREvent)
        {
            OnClickAudioEvent();
        }
    }

    public void OnClickBookBtn()
    {
        if (null != OnClickAREvent)
        {
            OnClickBookEvent();
        }
    }

#endregion



}
