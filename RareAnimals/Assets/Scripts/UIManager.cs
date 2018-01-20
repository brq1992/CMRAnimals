using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
    private BaseManager baseManager;
    private SceneManager SceneManager;
    private Transform ContentRoot;
    private MainMenuManager MainMenuManager;
    public static MenuItem MenuItem;

    void Start ()
    {
        MenuItem = MenuItem.ARScan;
        SceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManager>();
        if(null==SceneManager)
        {
            Debug.LogError("SceneManager can't be find!");
        }
        ContentRoot = transform.Find("Canvas/Content");
        if(null == ContentRoot)
        {
            Debug.LogError("ContentRoot can't be find!");
        }

        MainMenuManager = transform.GetChild(0).Find("MainMenu").GetComponent<MainMenuManager>();
        MainMenuManager.Init();
        MainMenuManager.OnClickAREvent += OnClickAR;
        MainMenuManager.OnClickAudioEvent += OnClickAudio;
        MainMenuManager.OnClickBookEvent += OnClickBook;
        OnClickAR();
    }
    
    private void OnClickAR()
    {
        MenuItem = MenuItem.ARScan;
        MainMenuManager.SetActiveMenu(MenuItem);
        GetViewManager("Prefabs/ARScan");
    }

    private void OnClickAudio()
    {
        MenuItem = MenuItem.AudioNav;
        MainMenuManager.SetActiveMenu(MenuItem);
        GetViewManager("Prefabs/AudioNav");
    }

    private void OnClickBook()
    {
        MenuItem = MenuItem.AnimalsBook;
        MainMenuManager.SetActiveMenu(MenuItem);
        GetViewManager("Prefabs/AnimalBooks");
    }

    private void GetViewManager(string path)
    {
        if(null != baseManager)
        {
            baseManager.DestroyView();
            baseManager = null;
        }
        GameObject prefab = Resources.Load(path) as GameObject;
        GameObject view = Instantiate(prefab, ContentRoot);
        baseManager = view.GetComponent<BaseManager>();
        if(null != baseManager)
        {
            baseManager.InitView();
        }
        else
        {
            Debug.LogError("baseManager is null!");
        }
    }

}

public enum MenuItem
{
    ARScan,
    AudioNav,
    AnimalsBook
}
