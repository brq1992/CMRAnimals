using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
    private BaseManager baseManager;
    private SceneManager SceneManager;
    private Transform ContentRoot;   

	void Start ()
    {
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
        if (SceneManager.menuItem == MenuItem.ARScan)
        {
            //todo:make arscan active
            OnClickAR();
        }
    }


# region btn function
    public void OnClickAR()
    {
        GetViewManager("Prefabs/ARScan");
        Debug.Log("you click AR");
    }

    public void OnClickNav()
    {
        GetViewManager("Prefabs/AudioNav");
        Debug.Log("you click NAV");
    }

    public void OnClickBook()
    {
        GetViewManager("Prefabs/AnimalBooks");
        Debug.Log("you click Book");
    }
#endregion

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
