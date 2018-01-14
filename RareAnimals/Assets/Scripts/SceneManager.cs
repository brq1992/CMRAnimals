using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public MenuItem menuItem;
    // Use this for initialization
    void Awake ()
    {
        menuItem = MenuItem.ARScan;
    }

    public void ChangeMenuItem(MenuItem item)
    {
        switch(item)
        {
            case MenuItem.ARScan:
                {
                    menuItem = MenuItem.ARScan;
                    break;
                }
            case MenuItem.AudioNav:
                {
                    menuItem = MenuItem.AudioNav;
                    break;
                }
            case MenuItem.AnimalsBook:
                {
                    menuItem = MenuItem.AnimalsBook;
                    break;
                }
            default:
                {
                    Debug.LogError("MenuItem is unmodified! MenuItem: " + item.ToString());
                    break;
                }

        }
          
    }
	
}
