using UnityEngine;
using System.Runtime.InteropServices;
using System;


public class Android : MonoBehaviour
{
    public static Android instance = null;
#if UNITY_ANDROID
    public static AndroidJavaObject jo;
    public static AndroidJavaClass jc;
#endif
    void Awake()
    {
#if UNITY_ANDROID
        //获取安卓类，用于掉用安卓的接口
        jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
#endif
    }

#if UNITY_ANDROID
    public static string GetWifiMacAddress()
    {
        return jc.CallStatic<string>("");
        //jo.Call("openPdf");
    }
#elif UNITY_IPHONE
    [DllImport("__Internal")]
    private static extern void getWifiMacAddress();
    public static void GetWifiMacAddress()
    {
        return getWifiMacAddress();
    }
#else
    public static void GetWifiMacAddress(string path)
    {
        return "";
    }
#endif
}

