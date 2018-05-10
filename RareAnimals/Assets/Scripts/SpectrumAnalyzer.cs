using System.Collections.Generic;
using UnityEngine;

public class SpectrumAnalyzer : MonoBehaviour {

    public AnalyzerSettings settings;

    public GameObject WavePrefab;
    private List<GameObject> pillars;
    public int Count = 51;
    public Vector3 Pos = new Vector3(-484f, -345, 0);
    public Transform waveroot;
    private float[] spectrum;

    public bool isBuilding = true;


    public Transform test;
    public GameObject pre;
    // Use this for initialization
    void Awake ()
    {
        pillars = GetAll();
        //CreatePillarsByShapes();

    }

    private List<GameObject>  GetAll()
    {
        GameObject rootObject = Instantiate(pre);
        rootObject.transform.SetParent(transform.parent);
        rootObject.name = "AudioSource";
        test = rootObject.transform;
        test.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -145);
        test.transform.localScale = Vector3.one;
        List<GameObject> objects = new List<GameObject>(Count);
        int dis = 19;
        for (int i = 0; i < Count; i++)
        {
            GameObject obj = test.GetChild(i).gameObject;
            objects.Add(obj);
        }
        return objects;
    }

    private void CreatePillarsByShapes()
    {
        //get current pillar types
        pillars = GetWaveList(Count, Pos, WavePrefab, waveroot);

    }

    private List<GameObject> GetWaveList(int count, Vector3 pos, GameObject pf, Transform root)
    {
        List<GameObject> objects = new List<GameObject>(count);
        int dis = 19;
        for(int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(pf,Vector3.zero, Quaternion.identity) as GameObject;
            obj.transform.SetParent(root);
            Vector3 ps =  new Vector3(pos.x + i * dis, pos.y, pos.z);
            obj.transform.localPosition = ps;
            objects.Add(obj);
        }
        return objects;
    }
    // Update is called once per frame
    void Update ()
    {

        if(!isBuilding)
        {
            return;
        }

        spectrum = AudioListener.GetSpectrumData((int)settings.spectrum.sampleRate, 0, settings.spectrum.FffWindowType);

        //Debug.Log("settings.spectrum.sampleRate: " + settings.spectrum.sampleRate+ "settings.spectrum.FffWindowType: " + settings.spectrum.FffWindowType);
        //foreach (var item in spectrum)
        //{
        //    Debug.Log("item: " + item);
        //}

        //return;

        for (int i = 0; i < pillars.Count; i++) //needs to be <= sample rate or error
        {
            float level = spectrum[i] * settings.pillar.sensitivity * Time.deltaTime * 1000; //0,1 = l,r for two channels
            RectTransform image = pillars[i].GetComponent<RectTransform>();
            Vector2 previousSize = image.sizeDelta;
            previousSize.y = Mathf.Lerp(previousSize.y, level, settings.pillar.speed * Time.deltaTime);
            previousSize.y = Mathf.Min(previousSize.y, 320);
            image.sizeDelta = new Vector2(previousSize.x, previousSize.y);
            Vector3 pos = pillars[i].transform.localPosition;
            pos.y = Pos.y + image.sizeDelta.y/2;
            pillars[i].transform.localPosition = pos;

        }
    }
}
