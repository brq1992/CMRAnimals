
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private BaseManager baseManager;
    private Transform ContentRoot;
    private MainMenuManager MainMenuManager;
    public static MenuItem MenuItem;


#if UNITY_EDITOR && !UNITY_ANDROID && !UNITY_IOS
    private List<string> fileNameList = new List<string>();
    private int currentCount = 0;
    private int lastCount = -1;
    private int showCount = 10;
    private int currentShowCount = 0;
    private float showTime = 1f;
    private float changTime = 0.5f;
    private float nextTime = 1.5f;
    private float time = 0f;
    private RawImage rawImage;
    private Texture2D texture2D;
    private bool showing = false;

    private void Callback(ScanResult obj)
    {
        if (obj.result)
        {
            string fileName = Path.GetFileNameWithoutExtension(fileNameList[currentCount]);
            if (fileName.Contains(obj.TargetName))
            {
                Debug.Log(fileName + " equals " + obj.TargetName);
            }
            else
            {
                Debug.Log(fileName + " Nequals " + obj.TargetName);
            }
        }
    }


    void Update()
    {
        time += Time.deltaTime;
        if (time > changTime && !showing)
        {
            if (currentCount > fileNameList.Count)
            {
                return;
            }
            if (currentShowCount > showCount)
            {
                currentCount++;
                currentShowCount = 0;
            }
            if(currentCount != lastCount)
            {
                string path = fileNameList[currentCount];
                lastCount = currentCount;
                StartCoroutine(GetImage(path));
            }
            else
            {
                rawImage.texture = texture2D;
            }
            currentShowCount++;
            showing = true;
        }
        if (time > showTime)
        {
            rawImage.texture = null;
        }
        if (time > nextTime)
        {
            showing = false;
            time = 0f;
        }    
    }

    private IEnumerator GetImage(string path)
    {
        path = @"e:\AnimalScans\" + path;
        WWW www = new WWW(path);
        yield return www;
        while (!www.isDone)
        {
            yield return null;
        }
        Debug.Log(File.Exists(path));
            if (string.IsNullOrEmpty(www.error))
            {
                texture2D = www.texture;
                rawImage.texture = www.texture;
            }
        changTime = 0;
        www.Dispose();
    }
#endif
    private void Awake()
    {
        //GameObject debug = new GameObject("Debug");
        //debug.AddComponent<DebugOutside>();

#if UNITY_EDITOR && !UNITY_ANDROID && !UNITY_IOS
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            Camera camera = new GameObject("TestCamera").AddComponent<Camera>();
            camera.targetDisplay = 1;
            GameObject prefab = Resources.Load("Canvas") as GameObject;
            GameObject canvas = Instantiate(prefab);
            if (canvas)
            {
                Canvas cn = canvas.GetComponent<Canvas>();
                cn.worldCamera = camera;
                rawImage = cn.transform.Find("RawImage").GetComponent<RawImage>();
            }
            string path = @"C:\Users\Public\Project\New folder\CMRAnimals\RareAnimals\Assets\Editor\Vuforia\ImageTargetTextures\AnimalScans";
            if (Directory.Exists(path))
            {
                fileNameList = Directory.GetFiles(path).ToList();
                bool state = true;
                while (state)
                {
                    int index = fileNameList.FindIndex(x => x.Contains(".meta"));
                    if (index != -1)
                    {
                        fileNameList.RemoveAt(index);
                    }
                    else
                    {
                        state = false;
                    }
                }
            }
            else
            {
                Debug.Log("Directory isn't exist!");
            }
            for (int i = 0; i < fileNameList.Count; i++)
            {
                fileNameList[i] = Path.GetFileName(fileNameList[i]);
            }
        }

        Core.NotificationEx.getSingleton().AddObserver<ScanResult>(GlobelConst.FOUNDTARGET, Callback);
#endif
    }

    

    void Start ()
    {
		PlayWelcomeAudio();
        MenuItem = MenuItem.ARScan;
        ContentRoot = transform.Find("Canvas/Content");
        if(null == ContentRoot)
        {
            Debug.LogError("ContentRoot can't be find!");
        }

        MainMenuManager = transform.GetChild(0).Find("MainMenu").GetComponent<MainMenuManager>();
        MainMenuManager.Init();
        MainMenuManager.OnClickAREvent += OnClickAR;
        MainMenuManager.OnClickAudioEvent += OnClickAudio;
        MainMenuManager.OnClickBookEvent += OnClickBook;
        OnClickAR();
    }


	private void PlayWelcomeAudio()
	{
        AudioClip clip = Resources.Load("welcome") as AudioClip;
	    if (!clip)
	    {
            Debug.LogError("cant find clip!");
	        return;
	    }
		GameObject obj = new GameObject ("AudioWelcome");
		AudioSource source = obj.AddComponent<AudioSource> ();
		source.clip = clip;
		source.Play ();
		Destroy(obj,clip.length);
	}

   
    private void OnClickAR()
    {
        MenuItem = MenuItem.ARScan;
        MainMenuManager.SetActiveMenu(MenuItem);
        GetViewManager("Prefabs/ARScan");
        Vuforia.VuforiaBehaviour.Instance.enabled = true;
    }

    private void OnClickAudio()
    {
        MenuItem = MenuItem.AudioNav;
        MainMenuManager.SetActiveMenu(MenuItem);
        GetViewManager("Prefabs/AudioNav");
        Vuforia.VuforiaBehaviour.Instance.enabled = false;
    }

    private void OnClickBook()
    {
        MenuItem = MenuItem.AnimalsBook;
        MainMenuManager.SetActiveMenu(MenuItem);
        GetViewManager("Prefabs/AnimalBooks");
        Vuforia.VuforiaBehaviour.Instance.enabled = false;
    }

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
