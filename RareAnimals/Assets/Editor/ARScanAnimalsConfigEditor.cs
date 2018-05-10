
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ARScanAnimalsConfig))]
public class ARScanAnimalsConfigEditor : Editor
{
    [UnityEditor.MenuItem("Assets/Manager/Create/Create Animals Config")]
    public static void CreateConfig()
    {
        ARScanAnimalsConfig newRule = CreateInstance<ARScanAnimalsConfig>();
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
        string newRuleFileName = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(selectionpath, "NewAnimalContentConfig.asset"));
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
