using System;
using System.Collections;
using LitJson;
using UnityEngine;

namespace WebRequestUtility 
{
    public class WebAudio : MonoBehaviour
    {
        public event Action<string> OnSuccess;
        public event Action<string> OnFailed;

        public void StartGet(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                StartCoroutine(GetAudioJson(url));
            }
            else
            {
                Debug.Log("url is null");
            }
        }

        private IEnumerator GetAudioJson(string url)
        {
            WWW www = new WWW(url);
            yield return www;
            while (!www.isDone)
            {
                yield return null;
            }
            if (string.IsNullOrEmpty(www.error))
            {
                if (OnSuccess != null)
                {
                    OnSuccess(www.text);
                }
            }
            else
            {
                if (OnFailed != null)
                {
                    OnFailed(www.error);
                }
            }
            www.Dispose();
            Destroy(gameObject);

        }
    }

    public class WebDownload:MonoBehaviour
    {
        public event Action<AudioClip> OnSuccess;
        public event Action<string> OnFailed;

        public void StartDownload(string url)
        {
            //Debug.Log("downloadMp3: " + url);
            if (!string.IsNullOrEmpty(url))
            {
                StartCoroutine(DownloadAudio(url));
            }
            else
            {
                Debug.Log("url is null");
            }
        }

        private IEnumerator DownloadAudio(string url)
        {
            WWW www = new WWW(url);
            yield return www;
            while (!www.isDone)
            {
                yield return null;
            }
            if (string.IsNullOrEmpty(www.error))
            {
                if (OnSuccess != null)
                {
                    OnSuccess(www.GetAudioClip());
                }
            }
            else
            {
                if (OnFailed != null)
                {
                    OnFailed(www.error);
                }
            }
            www.Dispose();
            Destroy(gameObject);
        }

        public void StopDownload()
        {
            Destroy(gameObject);
        }
    }

    public class WebGetMacRequest:MonoBehaviour
    {
        public event Action<string> OnSuccess;
        public event Action<string> OnFailed;

        public void StartGet(string url)
        {
            DontDestroyOnLoad(gameObject);
            if (!string.IsNullOrEmpty(url))
            {
                StartCoroutine(GetMacJson(url));
            }
            else
            {
                Debug.Log("url is null");
            }
        }

        private IEnumerator GetMacJson(string url)
        {

            WWW www = new WWW(url);
            yield return www;
            while (!www.isDone)
            {
                yield return null;
            }
            if (string.IsNullOrEmpty(www.error))
            {
                try
                {
                    string jsonStr = System.Text.Encoding.UTF8.GetString(www.bytes, 3, www.bytes.Length - 3);
                    jsonStr = jsonStr.Replace("phone mac", "mac");
                    Debug.Log(jsonStr);
                    RequestMacJsonModel data = JsonUtility.FromJson<RequestMacJsonModel>(jsonStr);
                    if (data.code.Equals(0))
                    {
                        if (OnSuccess != null)
                        {
                            OnSuccess(data.mac);
                        }
                    }
                    else
                    {
                        if (OnFailed != null)
                        {
                            OnFailed(data.status);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
                if (OnFailed != null)
                {
                    OnFailed("");
                }
            }
            else
            {
                if (OnFailed != null)
                {
                    OnFailed(www.error);
                }
            }
            www.Dispose();
            Destroy(gameObject);
        }

        [Serializable]
        public class RequestMacJsonModel
        {
            public int code;
            public string status;
            public string mac;
        }
    }
}
