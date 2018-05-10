using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AudioNavManager : BaseManager
{
    private AudioSource audioSource;
    private string getServer = @"http://www.vmetis.org/ft/yishousaomiao/getserverurl.php";
    private string currentMac = string.Empty;
    public string webAddress = string.Empty;
    public string path = "";
    Button btnPlay;
    Button btnPause;
    private bool playing;

    private float constTime = 2f;
    private float currentTime = 0f;
    private string mp3Address = "";
    private int delay = 1;
    private bool isPlaying;
    private SpectrumAnalyzer analyzer;
    private AudioClip defaultClip;
	private AudioClip lastClip;
	private Queue<AudioClip> clipQueues;
    private AudioSourceStatus sourceStatus;

    #region wwwcase

    private void StartGetMp3Address(string url)
    {
        Debug.Log("开始获取MP3地址！");
        currentMac = PlayerPrefs.GetString("MacAddress");
        if (string.IsNullOrEmpty(currentMac))
        {
            currentMac = "123";
        }
        path = string.Format("{0}{1}", url, currentMac);
        //Debug.Log("getMp3: " + path);
        WebRequestUtility.WebAudio webAudio = new GameObject("audiourlget").AddComponent<WebRequestUtility.WebAudio>();
        webAudio.OnSuccess += SeccessGetUrl;
        webAudio.OnFailed += FailedGetUrl;
        webAudio.StartGet(path);
    }

    private IEnumerator GetServerAddress()
    {
        WWW www = new WWW(getServer);
        yield return www;
        while (!www.isDone)
        {
            yield return null;
        }
        if (string.IsNullOrEmpty(www.error))
        {
            webAddress = www.text;
            StartGetMp3Address(webAddress);
        }
        else
        {
            Debug.Log("GetServerAddress.error: " + www.error);
        }
        www.Dispose();
    }

    private void SeccessGetUrl(string json)
    {
        Debug.Log("开始判定是否下载！"+json);
        string[] ar = json.Split('=');
        string finalUrl = ar[ar.Length - 1];
        if (!string.IsNullOrEmpty(finalUrl) && finalUrl != mp3Address)
        {
            GameObject obj = GameObject.Find("webtool");
            if (obj)
            {
                obj.GetComponent<WebRequestUtility.WebDownload>().StopDownload();
            }
            WebRequestUtility.WebDownload webRequestUtility = new GameObject("mp3Get").AddComponent<WebRequestUtility.WebDownload>();
            webRequestUtility.OnSuccess += SuccessCallback;
            webRequestUtility.OnFailed += FailedCallback;
            webRequestUtility.StartDownload(finalUrl);
            mp3Address = finalUrl;
        }
        else
        {
            Debug.LogError("无法下载MP3");
        }
    }

    public void FailedGetUrl(string json)
    {
        PlayDefaultAudio();
        Debug.LogError("get MP3 URL failed " + json);
    }

    private void FailedCallback(string obj)
    {
        PlayDefaultAudio();
        Debug.LogError("download audio error: " + obj);
    }

    private void PlayDefaultAudio()
    {
        if (sourceStatus.Equals(AudioSourceStatus.PlayBack))
        {
            return;
        }
        if (!defaultClip)
        {
            defaultClip = Resources.Load("DefaultAudio") as AudioClip;
        }
        if (audioSource.clip == null || audioSource.clip != defaultClip)
        {
            audioSource.loop = true;
            analyzer.isBuilding = false;
            audioSource.Stop();
            audioSource.clip = defaultClip;
            if (isPlaying)
            {
                audioSource.PlayDelayed(delay);
                analyzer.isBuilding = true;
            }
            sourceStatus = AudioSourceStatus.PlayBack;
        }
    }
		
	public void SuccessCallback(AudioClip clip)
    {
		clipQueues.Enqueue (clip);
    }

	private void PlayAudioSource(AudioClip clip,bool isLoop = false)
	{
		if (audioSource.clip != clip)
		{
			audioSource.loop = isLoop;
			analyzer.isBuilding = false;
			audioSource.Stop();
			audioSource.clip = clip;
			if(isPlaying)
			{
				StopCoroutine(CountAudioFinish(0f));
				audioSource.PlayDelayed(delay);
				StartCoroutine (CountAudioFinish (clip.length + delay));
				analyzer.isBuilding = true;
                sourceStatus = AudioSourceStatus.PlayAudio;
                btnPause.gameObject.SetActive(true);
                btnPlay.gameObject.SetActive(false);
			}
		}
	}

	private IEnumerator CountAudioFinish(float lenth)
	{
		yield return new WaitForSeconds (lenth); 
        sourceStatus = AudioSourceStatus.FinishAudio;
	    lastClip = clipQueues.Dequeue();
        btnPause.gameObject.SetActive(false);
        btnPlay.gameObject.SetActive(true);
	}

#endregion

    public override void InitView()
    {
		clipQueues = new Queue<AudioClip> ();
        sourceStatus = AudioSourceStatus.Wait;
        playing = true;
        base.InitView();
        audioSource = transform.Find("AudioSource").GetComponent<AudioSource>();
        btnPlay = transform.Find("Play").GetComponent<Button>();
        btnPlay.onClick.AddListener(PlayClick);

        btnPause = transform.Find("Pause").GetComponent<Button>();
        btnPause.onClick.AddListener(PauseClick);
        btnPlay.gameObject.SetActive(false);
        isPlaying = true;

        analyzer = transform.Find("AudioWave").GetComponent<SpectrumAnalyzer>();
        defaultClip = Resources.Load("entrance") as AudioClip;
    }

    #region region UI
    private void PlayClick()
    {
        switch (sourceStatus)
        {
            case AudioSourceStatus.PauseAudio:
                {
                    audioSource.PlayDelayed(0.5f);
                    analyzer.isBuilding = true;
                    sourceStatus = AudioSourceStatus.PlayAudio;
                    btnPlay.gameObject.SetActive(false);
                    btnPause.gameObject.SetActive(true);
                    playing = true;
                    break;
                }
            case AudioSourceStatus.PlayBack:
                {
                    PlayAudioSource(lastClip);
                    break;
                }
            default:
            {
                Debug.Log(sourceStatus);
                break;
            }
        }
    }


    public void PauseClick()
    {
        switch (sourceStatus)
        {
            case AudioSourceStatus.PlayAudio:
                {
                    analyzer.isBuilding = false;
                    audioSource.Pause();
                    btnPause.gameObject.SetActive(false);
                    btnPlay.gameObject.SetActive(true);
                    playing = false;
                    break;
                }
            default:
                {
                    Debug.Log(sourceStatus);
                    break;
                }
        }
        
    }
    #endregion

    private void Update()
    {
        if (!playing)
        {
            return;
        }
        CheckAudioSourceStatus();
        GetNextAudio();
    }

    private void CheckAudioSourceStatus()
    {
        if (clipQueues.Count > 0)
        {
            if (sourceStatus.Equals(AudioSourceStatus.Wait) || sourceStatus.Equals(AudioSourceStatus.FinishAudio) || sourceStatus.Equals(AudioSourceStatus.PlayBack))
            {
                PlayAudioSource(clipQueues.Peek());
            }
        }
        else if (sourceStatus.Equals(AudioSourceStatus.Wait))
        {
            PlayDefaultAudio();
        }
        else
        {
            Debug.LogError("CheckAudioSourceStatus! AudioState: " + sourceStatus);
        }
    }

    private void GetNextAudio()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= constTime)
        {
            if (string.IsNullOrEmpty(webAddress))
            {
                Debug.Log("开始获取Web服务器地址！");
                StartCoroutine(GetServerAddress());
            }
            else
            {
                Debug.Log("开始获取Web地址！");
                StartGetMp3Address(webAddress);
            }
            currentTime = 0f;
        }
    }

    

    public enum AudioSourceStatus
    {
        PlayAudio,
        PlayBack,
        PauseAudio,
        FinishAudio,
        Wait
    }
}
