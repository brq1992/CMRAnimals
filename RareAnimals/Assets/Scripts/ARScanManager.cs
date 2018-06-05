
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vuforia;
using Image = UnityEngine.UI.Image;

public class ARScanManager : BaseManager
{
    private Transform lineTrans;
    public bool update;
    private int topPos;
    private int bottomPos;
    private float currentPos;
    public int speed = 5;
    private Transform warnTrans;
    private Transform image1;
    private Transform image2;
    private List<GameObject> animaList;
    private ARScanAnimalsConfig config;
    private GameObject activeAnimal;
    private Transform idle;
    private Transform roar;
    private Transform detect;
    private Transform activeAnimalName;
    private Action<int, float> OnClickAnimation;
    private bool isAnimationPlaying;
    public override void InitView()
    {
        base.InitView();
        update = false;
        isAnimationPlaying = false;
        image1 = transform.Find("Image");
        image2 = transform.Find("Image (1)");
        warnTrans = transform.Find("WarningBtn");
        Button warnBtn = warnTrans.GetChild(0).GetChild(0).GetComponent<Button>();
        warnBtn.onClick.AddListener(OnClickWarning);
        warnTrans.gameObject.SetActive(false);

        lineTrans = transform.Find("ScanLine");
        Image image = lineTrans.GetComponent<Image>();
        int center =(int) (image.GetComponent<RectTransform>().sizeDelta.y / 2) ;
        int halfScreen = Screen.height / 2;
        //int halfScreen = 2208 / 2;
        topPos = halfScreen + center;
        currentPos  = topPos;
        bottomPos = -topPos;
        //Debug.LogError("halfScreen: " + halfScreen + " topPos: " + topPos + " bottomPos: " + bottomPos);
        RectTransform rect = lineTrans.GetComponent<RectTransform>();
        Vector3 vector3 = new Vector3(rect.localPosition.x, topPos, rect.localPosition.z);
        rect.localPosition = vector3;
        Core.NotificationEx.getSingleton().AddObserver<ScanResult>(GlobelConst.FOUNDTARGET, Callback);

        animaList = new List<GameObject>();


        idle = transform.Find("Idle");
        roar = transform.Find("Roar");
        detect = transform.Find("Detect");
        

        activeAnimalName = transform.Find("Name");
        activeAnimalName.gameObject.SetActive(false);

        idle.transform.Find("Image").gameObject.SetActive(false);
        roar.transform.Find("Image").gameObject.SetActive(false);
        detect.transform.Find("Image").gameObject.SetActive(false);
        idle.gameObject.SetActive(false);
        roar.gameObject.SetActive(false);
        detect.gameObject.SetActive(false);

        VuforiaBehaviour.Instance.transform.GetComponent<Camera>().cullingMask = -1;
        config = Resources.Load("NewAnimalContentConfig") as ARScanAnimalsConfig;
    }


    private void Add3DClick()
    {
        Button idleBtn = idle.GetComponent<Button>();
        idleBtn.onClick.RemoveAllListeners();
        idleBtn.onClick.AddListener(() =>
        {
            if (activeAnimal)
            {
                Animator animator = activeAnimal.GetComponent<Animator>();
                if (animator)
                {
                    animator.Play(idle.name);
                    idle.transform.Find("Image").gameObject.SetActive(true);
                    roar.transform.Find("Image").gameObject.SetActive(false);
                    detect.transform.Find("Image").gameObject.SetActive(false);
                }
            }
        });

        Button roarBtn = roar.GetComponent<Button>();
        roarBtn.onClick.RemoveAllListeners();
        roarBtn.onClick.AddListener(() =>
        {
            if (activeAnimal)
            {
                Animator animator = activeAnimal.GetComponent<Animator>();
                if (animator)
                {
                    animator.Play(roar.name);
                    idle.transform.Find("Image").gameObject.SetActive(false);
                    roar.transform.Find("Image").gameObject.SetActive(true);
                    detect.transform.Find("Image").gameObject.SetActive(false);
                }
            }
        });

        Button detectBtn = detect.GetComponent<Button>();
        detectBtn.onClick.RemoveAllListeners();
        detectBtn.onClick.AddListener(() =>
        {
            if (activeAnimal)
            {
                Animator animator = activeAnimal.GetComponent<Animator>();
                if (animator)
                {
                    animator.Play(detect.name);
                    idle.transform.Find("Image").gameObject.SetActive(false);
                    roar.transform.Find("Image").gameObject.SetActive(false);
                    detect.transform.Find("Image").gameObject.SetActive(true);
                }
            }
        });
    }

    private void Add2DClick()
    {
        Button idleBtn = idle.GetComponent<Button>();
        idleBtn.onClick.RemoveAllListeners();
        idleBtn.onClick.AddListener(() =>
        {
            if (activeAnimal)
            {
                Animator animator = activeAnimal.GetComponent<Animator>();
                if (animator)
                {
                    //StopCoroutine("ActiveGameObject");
                    VuforiaBehaviour.Instance.transform.GetComponent<Camera>().cullingMask = ~(1 << 8);
                    //List<AnimationClip> ans = animator.runtimeAnimatorController.animationClips.ToList();
                    //AnimationClip clip = ans.Find(x => x.name.Equals(idle.name));
                    //ExcuteCamera(0, clip.length);
                    animator.Play(idle.name);
                    idle.transform.Find("Image").gameObject.SetActive(true);
                    roar.transform.Find("Image").gameObject.SetActive(false);
                    detect.transform.Find("Image").gameObject.SetActive(false);
                    SwitchCam(0);
                }
            }
        });

        Button roarBtn = roar.GetComponent<Button>();
        roarBtn.onClick.RemoveAllListeners();
        roarBtn.onClick.AddListener(() =>
        {
            if (activeAnimal)
            {
                Animator animator = activeAnimal.GetComponent<Animator>();
                if (animator)
                {
                    //StopCoroutine("ActiveGameObject");
                    VuforiaBehaviour.Instance.transform.GetComponent<Camera>().cullingMask = ~(1 << 8);
                    //List<AnimationClip> ans = animator.runtimeAnimatorController.animationClips.ToList();
                    //AnimationClip clip = ans.Find(x => x.name.Equals(roar.name));
                    //ExcuteCamera(1, clip.length);
                    animator.Play(roar.name);
                    idle.transform.Find("Image").gameObject.SetActive(false);
                    roar.transform.Find("Image").gameObject.SetActive(true);
                    detect.transform.Find("Image").gameObject.SetActive(false);
                    SwitchCam(1);
                }
            }
        });

        Button detectBtn = detect.GetComponent<Button>();
        detectBtn.onClick.RemoveAllListeners();
        detectBtn.onClick.AddListener(() =>
        {
            if (activeAnimal)
            {
                Animator animator = activeAnimal.GetComponent<Animator>();
                if (animator)
                {
                    //StopCoroutine("ActiveGameObject");
                    VuforiaBehaviour.Instance.transform.GetComponent<Camera>().cullingMask = ~(1 << 8);
                    //List<AnimationClip> ans = animator.runtimeAnimatorController.animationClips.ToList();
                    //AnimationClip clip = ans.Find(x => x.name.Equals(detect.name));
                    //ExcuteCamera(2, clip.length);
                    animator.Play(detect.name);
                    idle.transform.Find("Image").gameObject.SetActive(false);
                    roar.transform.Find("Image").gameObject.SetActive(false);
                    detect.transform.Find("Image").gameObject.SetActive(true);
                    SwitchCam(2);
                }
            }
        });
    }


    private void ExcuteCamera(int i, float f)
    {
        if (OnClickAnimation != null)
        {
            OnClickAnimation(i, f);
        }
    }

    private void OnDestroy()
    {
        Core.NotificationEx.getSingleton().RemoveObserver<ScanResult>(GlobelConst.FOUNDTARGET, Callback);
    }

    private void HasFoundTarget(bool update)
    {
        if (!update)
        {
            lineTrans.gameObject.SetActive(true);
            image1.gameObject.SetActive(true);
            image2.gameObject.SetActive(true);
        }
        else
        {
            lineTrans.gameObject.SetActive(false);
            image1.gameObject.SetActive(false);
            image2.gameObject.SetActive(false);
        }
    }

    private void GetConfigScript()
    {
        if (config == null)
        {
            config = Resources.Load("NewAnimalContentConfig") as ARScanAnimalsConfig;
        }
    }

    private AnimalContents GetAnimalContents(ScanResult result)
    {
        AnimalContents contents = null;
        for (int i = 0; i < config.list.Count; i++)
        {
            if (result.TargetName.Contains(config.list[i].Key))
            {
                contents = config.list[i];
                break;
            }
        }
        if (null == contents)
        {
            Debug.Log("can't find contents");
        }
        return contents;
    }

    private void CheckAnimalType(ScanResult result)
    {
        AnimalContents contents = GetAnimalContents(result);
        //Location_num_3D_0(horiontal)
        if (result.TargetName.Contains("3D"))
        {
            string[] str = result.TargetName.Split('_');
            if (str.Length != 4)
            {
                Debug.Log("str length is " + str.Length);
                return;
            }
            GameObject prefab = contents.prefab;
            GameObject live = Instantiate(prefab);
            live.transform.SetParent(result.root.transform);
            live.transform.localScale = Vector3.one;
            activeAnimal = live;
            if (str[3].Equals("0"))
            {
                Add2DClick();
                live.transform.localPosition = new Vector3(0, 0.6f, -0.3f);
                live.transform.localEulerAngles = new Vector3(0, -90f, -90f);
            }
            else
            {
                Add3DClick();
            }
            if (activeAnimal != null)
            {
                idle.name = contents.ANames[0];
                idle.transform.Find("Text").GetComponent<Text>().text = contents.ANames[0];
                roar.name = contents.ANames[1];
                roar.transform.Find("Text").GetComponent<Text>().text = contents.ANames[1];
                detect.name = contents.ANames[2];
                detect.transform.Find("Text").GetComponent<Text>().text = contents.ANames[2];
                activeAnimalName.GetComponent<Image>().sprite = contents.AnimalName;
                activeAnimalName.gameObject.SetActive(true);
                idle.transform.Find("Image").gameObject.SetActive(false);
                roar.transform.Find("Image").gameObject.SetActive(false);
                detect.transform.Find("Image").gameObject.SetActive(false);
                idle.gameObject.SetActive(true);
                roar.gameObject.SetActive(true);
                detect.gameObject.SetActive(true);
                Transform camereTrans = activeAnimal.transform.Find("Camera");
                if (!camereTrans)
                {
                    Debug.LogError("cant find camera!");
                    return;
                }
                camereTrans.gameObject.SetActive(false);
                //OnClickAnimation = delegate(int i, float f)
                //{
                //    if (!camereTrans.gameObject.activeSelf)
                //    {
                //        camereTrans.gameObject.SetActive(true);
                //    }
                //    if (camereTrans)
                //    {
                //        //camereTrans.localPosition = contents.AnimationTrans[i].LocalPos;
                //        //camereTrans.rotation = Quaternion.Euler(contents.AnimationTrans[i].LocalRot);
                //        for (int j = 0; j < camereTrans.childCount; j++)
                //        {
                //            if (j == i)
                //            {
                //                camereTrans.GetChild(i).gameObject.SetActive(true);
                //            }
                //            else
                //            {
                //                camereTrans.GetChild(j).gameObject.SetActive(false);
                //            }
                //        }
                //    }
                //    StartCoroutine(ActiveGameObject(camereTrans.gameObject, f));
                //};
            }
        }
        else
        {
            string[] str = result.TargetName.Split('_');
            if (str.Length != 2)
            {
                Debug.Log("str length is " + str.Length);
                return;
            }
            int index = int.Parse(str[1]);
            GameObject Cavas = Resources.Load("Prefabs/2DAnimals") as GameObject;
            GameObject cavs = Instantiate(Cavas, transform.parent);
            AnimalViewManager manager = cavs.GetComponent<AnimalViewManager>();
            manager.Init(contents.Texture2Ds[index - 1]);
            animaList.Add(cavs);
        }
        Button idleBtn = idle.GetComponent<Button>();
        idleBtn.onClick.Invoke();
    }


    private void SwitchCam(int i)
    {
        Transform camereTrans = activeAnimal.transform.Find("Camera");
        if (!camereTrans)
        {
            Debug.LogError("cant find camera!");
            return;
        }
        if (!camereTrans.gameObject.activeSelf)
        {
            camereTrans.gameObject.SetActive(true);
        }
        if (camereTrans)
        {
            //camereTrans.localPosition = contents.AnimationTrans[i].LocalPos;
            //camereTrans.rotation = Quaternion.Euler(contents.AnimationTrans[i].LocalRot);
            for (int j = 0; j < camereTrans.childCount; j++)
            {
                if (j == i)
                {
                    camereTrans.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    camereTrans.GetChild(j).gameObject.SetActive(false);
                }
            }
        }
    }

    private void TrackingNotFound(GameObject obj)
    {
        for (int i = 0; i < animaList.Count; i++)
        {
            if (animaList[i] != null)
            {
                Destroy(animaList[i]);
            }
        }
        animaList.Clear();
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            Transform trans = obj.transform.GetChild(i);
            if (!trans.name.Contains("snow"))
            {
                Destroy(trans.gameObject);
            }
        }
        activeAnimal = null;
        OnClickAnimation = null;
        idle.gameObject.SetActive(false);
        roar.gameObject.SetActive(false);
        detect.gameObject.SetActive(false);
        activeAnimalName.gameObject.SetActive(false);
        //ActiveAnimationBtn(false);
    }

    private void Callback(ScanResult result)
    {
        update = result.result;
        HasFoundTarget(result.result);
        if (result.result)
        {
            GetConfigScript();
            CheckAnimalType(result);
        }
        else
        {
            TrackingNotFound(result.root);
            ActiveGameObject();
        }
    }

    private IEnumerator ActiveGameObject(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        VuforiaBehaviour.Instance.transform.GetComponent<Camera>().cullingMask = -1;
        if(obj)
            obj.SetActive(false);
    }


    private void ActiveGameObject()
    {
        VuforiaBehaviour.Instance.transform.GetComponent<Camera>().cullingMask = -1;
        if(!activeAnimal)
        { return;}
        Transform camereTrans = activeAnimal.transform.Find("Camera");
        if (!camereTrans)
        {
            Debug.LogError("cant find camera!");
            return;
        }
        camereTrans.gameObject.SetActive(false);
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

    private void Update()
    {
        if(!update)
        {
            Scaning();
        }
    }


}


public class ScanResult
{
    public GameObject root;
    public bool result;
    public string TargetName;
}
