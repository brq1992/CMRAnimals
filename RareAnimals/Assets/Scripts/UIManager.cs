using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public Transform ARScan;
    public Transform AudioNav;
    public Transform AnimalBooks;

	void Start () {

        if (!ARScan)
        {
            ARScan = transform.GetChild(0).Find("ARScan");
        }
        if (!AudioNav)
        {
            AudioNav = transform.GetChild(0).Find("AudioNav");
        }
        if (!AnimalBooks)
        {
            AnimalBooks = transform.GetChild(0).Find("AnimalBooks");
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
