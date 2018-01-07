using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpectrumAnalyzer : MonoBehaviour {

    public AnalyzerSettings settings;

    public GameObject WavePrefab;
    private List<GameObject> pillars;
    public int Count = 51;
    public Vector3 Pos = new Vector3(-484f, -345, 0);
    public Transform waveroot;
    private float[] spectrum;

    public bool isBuilding = true;
    // Use this for initialization
    void Start ()
    {
        CreatePillarsByShapes();

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
            image.sizeDelta = new Vector2(previousSize.x, previousSize.y);
            Vector3 pos = pillars[i].transform.localPosition;
            pos.y = Pos.y + image.sizeDelta.y/2;
            pillars[i].transform.localPosition = pos;
        }
    }
}
