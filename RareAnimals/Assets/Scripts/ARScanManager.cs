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
    private Transform warnTrans;
    public override void InitView()
    {
        base.InitView();
        update = false;

        warnTrans = transform.Find("WarningBtn");
        Button warnBtn = warnTrans.GetChild(0).GetChild(0).GetComponent<Button>();
        warnBtn.onClick.AddListener(OnClickWarning);
        warnTrans.gameObject.SetActive(false);

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
        Core.NotificationEx.getSingleton().AddObserver<bool>(GlobelConst.FOUNDTARGET, Callback);
    }

    private void Callback(bool obj)
    {
        EnableLine(obj);
    }

    private void OnClickWarning()
    {

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
