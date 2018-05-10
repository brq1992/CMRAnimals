
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Launcher : MonoBehaviour
{
    private bool hasValue = false;

    void Start()
    {
        bool flag = PlayerPrefs.HasKey("MacAddress");
        if (flag)
        {
            string str = PlayerPrefs.GetString("MacAddress");
            hasValue = !string.IsNullOrEmpty(str);
        }
        else
        {
            PlayerPrefs.SetString("MacAddress", string.Empty);
            hasValue = false;
        }
        Button button = transform.Find("Btn").GetComponent<Button>();
        if (!hasValue)
        {
            GameObject obj = new GameObject("MacMannager");
            GettingMacManager manager = obj.AddComponent<GettingMacManager>();
            manager.InIt();
        }
        button.onClick.AddListener(StartNextScene);
    }

    private void StartNextScene()
    {
        StartCoroutine(NextScene());
        
    }

    private IEnumerator NextScene()
    {
        yield return new WaitForSeconds(0.5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }
}
