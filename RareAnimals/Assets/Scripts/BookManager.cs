using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class BookManager : BaseManager
{
    public Transform MainRoot;
    public static BookState bookState;
    public override void InitView()
    {
        base.InitView();
        MainRoot = transform.Find("MainBook");
        int count = MainRoot.childCount;
        for(int i=0; i< count; i++)
        {
            Transform trans = MainRoot.GetChild(i);
            if(trans.name.Contains("Book"))
            {
                Button btnSouth = trans.GetComponent<Button>();
                btnSouth.onClick.AddListener(SouthClick);
            }
        }

        //Transform retTrans = transform.Find("RtBtn");
        //Button retBtn = retTrans.GetComponent<Button>();
        //retBtn.onClick.AddListener(ClickReturn);

        bookState = BookState.Main;
    }

    private void SouthClick()
    {
        string btnName = EventSystem.current.currentSelectedGameObject.name;
        GameObject prefab = Resources.Load("Prefabs/BookView") as GameObject;
        RareAnimalsBookConfig conf = Resources.Load("NewConfig") as RareAnimalsBookConfig;
        BookPare book= new BookPare();
        for (int i = 0; i < conf.South.Count; i++)
        {
            if (conf.South[i].Key.Contains(btnName))
            {
                book = conf.South[i];
                break;
            }
        }
        GameObject obj = Instantiate(prefab);
        obj.name = "BookView";
        obj.transform.SetParent(transform);
        obj.transform.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        obj.transform.GetComponent<RectTransform>().sizeDelta = Vector3.zero;
        obj.transform.GetComponent<RectTransform>().localScale = Vector3.one;
        BookContentManager manager = obj.GetComponent<BookContentManager>();
        manager.InitBookContent(book);
        bookState = BookState.Other;
    }

    public void ClickReturn()
    {
        if(bookState == BookState.Main)
        {
            return;
        }
        Transform root = transform.Find("BookView");
        if (root)
        {
            Destroy(root.gameObject);
        }
        bookState = BookState.Main;

    }

    public enum BookState
    {
        Other,
        Main
    }
}
