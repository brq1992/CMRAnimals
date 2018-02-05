package com.codestalkers.plugin;

import java.util.List;

import com.unity3d.player.UnityPlayerActivity;

import android.content.Context;
import android.net.wifi.ScanResult;
import android.net.wifi.WifiInfo;
import android.net.wifi.WifiManager;
import android.os.Bundle;

import java.util.List;

public class Main extends UnityPlayerActivity {
    public static Context mContext;
    
    @Override
    protected void onCreate(Bundle bundle)
    {
        super.onCreate(bundle);
        mContext = this;
    }
    
    public static String getConnectedWifiMacAddress() {
        String connectedWifiMacAddress = null;
        WifiManager wifiManager = (WifiManager) mContext.getSystemService(Context.WIFI_SERVICE);
        List<ScanResult> wifiList;

        if (wifiManager != null) {
            wifiList = wifiManager.getScanResults();
            WifiInfo info = wifiManager.getConnectionInfo();
            if (wifiList != null && info != null) {
                for (int i = 0; i < wifiList.size(); i++) {
                    ScanResult result = wifiList.get(i);
                    if (info.getBSSID().equals(result.BSSID)) {
                        connectedWifiMacAddress = result.BSSID;
                    }
                }
            }
        }
        return connectedWifiMacAddress;
    }
}
