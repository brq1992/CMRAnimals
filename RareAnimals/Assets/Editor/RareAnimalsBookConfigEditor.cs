using System.IO;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(RareAnimalsBookConfig))]
public class RareAnimalsBookConfigEditor : Editor {

    [UnityEditor.MenuItem("Assets/Manager/Create/Create Book Config")]
    public static void CreateConfig()
    {
        RareAnimalsBookConfig newRule = CreateInstance<RareAnimalsBookConfig>();
        string selectionpath = "Assets";
        foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
        {
            selectionpath = AssetDatabase.GetAssetPath(obj);
            if (File.Exists(selectionpath))
            {
                Debug.Log("File.Exists: " + selectionpath);
                selectionpath = Path.GetDirectoryName(selectionpath);
            }
            break;
        }

        Debug.Log("selectionpath: " + selectionpath);
        string newRuleFileName = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(selectionpath, "NewConfig.asset"));
        newRuleFileName = newRuleFileName.Replace("\\", "/");
        AssetDatabase.CreateAsset(newRule, newRuleFileName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = newRule;
    }


    protected void OnEnable()
    {
    }

    protected void OnInspectorGUI()
    {
    }
}

