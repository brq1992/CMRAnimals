using System.Collections;
using UnityEngine;

public class GettingMacManager : MonoBehaviour 
{
    private string serverAddress = @"http://vmetis.org/ft/yishousaomiao/getLocalUDPInfo.php";
    private bool hasSet = false;
    private float currentTime = 0f;
    private float constTime = 5f;
    private UdpClient[] clients = new UdpClient[0];


    private string requestUrlAdd = @"http://www.vmetis.org/ft/yishousaomiao/getmacurl.php";

    public void InIt()
    {
        DontDestroyOnLoad(gameObject);
        //StartCoroutine(GetUdpAddress(serverAddress));
        StartCoroutine(GetMacRequestAdd(requestUrlAdd));
    }

    private IEnumerator GetMacRequestAdd(string s)
    {
        WWW www = new WWW(requestUrlAdd);
        yield return www;
        while (!www.isDone)
        {
            yield return null;
        }
        if (string.IsNullOrEmpty(www.error))
        {
            WebRequestUtility.WebGetMacRequest webAudio = new GameObject("GetMac").AddComponent<WebRequestUtility.WebGetMacRequest>();
            webAudio.OnSuccess += SeccessGetUrl;
            webAudio.OnFailed += FailedGetUrl;
            webAudio.StartGet(www.text);
        }
        else
        {
            Debug.LogError("返回的获取mac地址有误！");
        }
    }

    private void SeccessGetUrl(string mac)
    {
        PlayerPrefs.SetString("MacAddress", mac);
        Debug.Log(PlayerPrefs.GetString("MacAddress", mac));
        Destroy(gameObject);
    }

    private void FailedGetUrl(string msg)
    {
        PlayerPrefs.SetString("MacAddress", "");
        Debug.LogError(msg);
        Destroy(gameObject);
    }

    private IEnumerator GetUdpAddress(string url)
    {
        WWW www = new WWW(url);
        yield return www;
        while (!www.isDone)
        {
            yield return null;
        }
        if (string.IsNullOrEmpty(www.error))
        {
            string[] udpAddress = www.text.Split(';');
            if (udpAddress.Length.Equals(0))
            {
                PlayerPrefs.SetString("MacAddress", string.Empty);
            }
            else
            {
                clients = new UdpClient[udpAddress.Length];
                for (int i = 0; i < udpAddress.Length; i++)
                {
                    string[] ipandPort = udpAddress[i].Split(':');
                    UdpClient client = new GameObject("udpTools").AddComponent<UdpClient>();
                    clients[i] = client;
                    client.InitSocket(ipandPort[0], int.Parse(ipandPort[1]),CallBack);
                }
            }

        }
        else
        {
            PlayerPrefs.SetString("MacAddress", string.Empty);
        }
    }

    private void CallBack(string str)
    {
        string currentMac = GlobelConst.MacAddress;
        if (string.IsNullOrEmpty(currentMac) && !str.Contains("00:00:00:00"))
        {
            GlobelConst.MacAddress = str;
        }
    }

    private void Update()
    {
        if (!string.IsNullOrEmpty(GlobelConst.MacAddress)&&!hasSet)
        {
            PlayerPrefs.SetString("MacAddress", GlobelConst.MacAddress);
            hasSet = true;
        }
        if (hasSet)
        {
            for (int i = 0; i < clients.Length; i++)
            {
                Destroy(clients[i].gameObject);
            }
            Destroy(gameObject);
            return;
        }
        currentTime += Time.deltaTime;
        if (currentTime >= constTime)
        {
            for (int i = 0; i < clients.Length; i++)
            {
                clients[i].SocketSend("test");
            }
            currentTime = 0f;
        }
    }

}
