using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARScanManager : BaseManager
{
    private Transform lineTrans;
    public bool update;
    private int topPos;
    private int bottomPos;
    private float currentPos;
    public int speed = 5;
    public override void InitView()
    {
        base.InitView();
        update = false;
        lineTrans = transform.Find("ScanLine");
        Image image = lineTrans.GetComponent<Image>();
        int center =(int) (image.GetComponent<RectTransform>().sizeDelta.y / 2) ;
        int halfScreen = Screen.height / 2;
        topPos = halfScreen + center;
        currentPos  = topPos;
        bottomPos = -topPos;
        RectTransform rect = lineTrans.GetComponent<RectTransform>();
        Vector3 vector3 = new Vector3(rect.localPosition.x, topPos, rect.localPosition.z);
        rect.localPosition = vector3;
        EnableLine(true);
    }

    private void Scaning()
    {
        Vector3 pos = lineTrans.localPosition;
        pos.y -= speed;
        if(pos.y<= bottomPos)
        {
            pos.y = topPos;
        }
        lineTrans.localPosition = pos;
        currentPos = pos.y;

    }

    private void EnableLine(bool b)
    {
        update = b;
    }

    private void Update()
    {
        if(update)
        {
            Scaning();
        }
    }


}
