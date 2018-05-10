using UnityEngine;
using UnityEngine.UI;

public class AnimalBookItemSetting : MonoBehaviour
{
    public Text Content;
    public Image Photo;
    public Transform NameOne;
    public Transform NameTwo;
    public Transform NameThree;

    public void Init(string content,Sprite sprite)
    {
        Photo.sprite = sprite;
        string[] str = AppUtility.CommonMethod.GetThreePartContent(content);
        Content.text = string.Format("\u3000\u3000{0}", str[2]);
        Text one = NameOne.Find("Text").GetComponent<Text>();
        Text pinyin = NameOne.Find("Pinyin").GetComponent<Text>();
        Text two = NameTwo.Find("Text").GetComponent<Text>();
        Text pinyinOne = NameTwo.Find("Pinyin").GetComponent<Text>();
        Text three = NameThree.Find("Text").GetComponent<Text>();
        Text pinyinTwo = NameThree.Find("Pinyin").GetComponent<Text>();

        NameOne.gameObject.SetActive(false);
        pinyin.gameObject.SetActive(false);
        NameTwo.gameObject.SetActive(false);
        pinyinOne.gameObject.SetActive(false); 
        NameThree.gameObject.SetActive(false);
        pinyinTwo.gameObject.SetActive(false);

        for (int i=0;i<str[0].Length;i++)
        {
            if(i==0)
            {
                one.text = str[0][i].ToString();
                NameOne.gameObject.SetActive(true);
                if(i==str[0].Length-1)
                {
                    pinyin.text = str[1];
                    pinyin.gameObject.SetActive(true);
                }
            }
            if(i==1)
            {
                two.text = str[0][i].ToString();
                NameTwo.gameObject.SetActive(true);
               
                if (i==str[0].Length-1)
                {
                    pinyinOne.text = str[1];
                    pinyinOne.gameObject.SetActive(true);
                }
            }
            if(i==2)
            {
                three.text = str[0][i].ToString();
                NameThree.gameObject.SetActive(true);
                if (i == str[0].Length - 1)
                {
                    pinyinTwo.text = str[1];
                    pinyinTwo.gameObject.SetActive(true);
                }
            }
        }
    }
}
