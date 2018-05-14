using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class BuildWindow : EditorWindow
{
    static float windowWidth = 800;
    static float windowHeight = 600;
    static BuildWindow s_instance;
    string appName = "";
    string newPath = "";
    static string PATH_KEY = "BUILD_PATH_KEY";
    string buildRes = "";
    string appFullName;
    bool isBuildSuccess;
    static string bassDll = "bass.dll";
    static string s_3rdDir = "3rd";
    static string s_encypt_tool = "Tools";

    static string buildPath
    {
        get
        {
            if (PlayerPrefs.HasKey(PATH_KEY))
            {
                return PlayerPrefs.GetString(PATH_KEY);
            }
            else
            {
               string currentDirectory = Application.dataPath;
                currentDirectory = currentDirectory.Substring(0, currentDirectory.LastIndexOf("/"));
                currentDirectory = currentDirectory.Replace("/", "\\");
                return currentDirectory;
            }
        }
    }

    static string[] s_scenes =
    {
        //"Assets/Scenes/ConcertStage.unity",
    };

    public void Init()
    {
        newPath = "";
        buildRes = "";
        isBuildSuccess = false;
        appName = Application.productName;
    }

    [UnityEditor.MenuItem("Tools/Build", false, 0)]
    static void Build()
    {
        if (s_instance == null)
        {
            s_instance = EditorWindow.GetWindow(typeof(BuildWindow)) as BuildWindow;
            s_instance.newPath = "";
            s_instance.Init();
            s_instance.Show();
        }
    }


    private void OnGUI()
    {
        //if (s_instance != null)
        {
            GUI.skin.label.fontSize = 16;
            GUILayout.Label(Application.productName);
            GUILayout.Label("打包存放目录:");
            GUI.skin.label.fontSize = 14;
            GUILayout.Label(buildPath);
            GUI.skin.button.fontSize = 16;
            this.appName = GUILayout.TextField(this.appName,new GUILayoutOption[]{GUILayout.MaxWidth(200)});
            GUI.skin.button.fontSize = 14;
            if (GUILayout.Button("重新选择目录"))
            {
                newPath = EditorUtility.SaveFolderPanel("打包存放目录", "d", "");
                if (newPath.Length != 0)
                {
                    PlayerPrefs.SetString(PATH_KEY, newPath);
                }
            }
            if (GUILayout.Button("Build"))
            {
                isBuildSuccess = false;
                string folderName = appName + "_" + DateTime.Now.ToString("yyyy.MM.dd_HH.mm.ss");
                string targetDir = buildPath + "/" + folderName;
#if UNITY_ANDROID 
                BuildTarget buildTarget = BuildTarget.Android;
                appFullName = string.Format("{0}/{1}.apk", targetDir, appName);
#endif
#if UNITY_IOS 
                BuildTarget buildTarget = BuildTarget.iOS;
                appFullName = string.Format("{0}/{1}", targetDir, appName);
#endif 
#if UNITY_EDITOR_WIN&&! UNITY_ANDROID&&!UNITY_IOS
                BuildTarget buildTarget = BuildTarget.StandaloneWindows64;
                appFullName = string.Format("{0}/{1}.exe", targetDir, appName);
#endif
                if (!Directory.Exists(targetDir))
                    Directory.CreateDirectory(targetDir);
                //AddScenes(s_scenes);
                //PlayerSettings.virtualRealitySupported = true;
                PlayerSettings.usePlayerLog = true;
                //PlayerSettings.logObjCUncaughtExceptions = true;

                //CopyDependencies(targetDir);

                buildRes = BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, appFullName, buildTarget, BuildOptions.None);
                if (string.IsNullOrEmpty(buildRes))
                {
                    //EditorUtility.DisplayDialog("打包成功", string.Format("文件输出目录{0}", appFullName), "确  定");
                    isBuildSuccess = true;
                    Debug.LogFormat("文件输出目录:{0}", appFullName);
                    //if (File.Exists(bassDll))
                    //{
                    //    File.Copy(bassDll, targetDir + "/" + bassDll);
                    //}
                }
                //CopyDependencies(targetDir);
                //Encrypt(appName, targetDir, folderName);
            }
            if (GUILayout.Button("打开目录"))
            {
                if (Directory.Exists(buildPath))
                {
                    Application.OpenURL(buildPath);
                }
                else
                {
                    Debug.Log(buildPath + " 不存在，自动创建目录!");
                    Directory.CreateDirectory(buildPath);
                    Application.OpenURL(buildPath);
                }
                //System.Diagnostics.Process.Start("explorer.exe", buildPath);
            }
            if (isBuildSuccess)
            {
                GUILayout.Label("打包成功!!!!");
                GUILayout.Label(appFullName);
            }
            GUI.skin.label.fontSize = 0;
            GUI.skin.button.fontSize = 0;
        }
    }

    static void CopyDependencies(string targetDir)
    {
        if (File.Exists(bassDll))
        {
            File.Copy(bassDll, targetDir + "/" + bassDll);
        }
        if (Directory.Exists(s_3rdDir))
        {
            FileUtil.CopyFileOrDirectory(s_3rdDir, targetDir + "/" + s_3rdDir);
        }
    }

    static void AddScenes(string[] scenes)
    {
        var allScenes = new EditorBuildSettingsScene[scenes.Length];
        for (int i = 0; i < scenes.Length; i++)
        {
            var scene = new EditorBuildSettingsScene(scenes[i], true);
            allScenes[i] = scene;
        }
        EditorBuildSettings.scenes = allScenes;
    }

    static void Encrypt(string appName, string targetDir, string folderName)
    {
        targetDir = targetDir.Replace("/", "\\");

        string currentDirectory = Application.dataPath;
        currentDirectory = currentDirectory.Substring(0, currentDirectory.LastIndexOf("/"));
        currentDirectory = currentDirectory.Replace("/", "\\");

        string managedPath = string.Format("{0}\\{1}_Data\\Managed", targetDir, appName);
        string toolPath = currentDirectory + "\\" + s_encypt_tool;

        string encryptDllPath = string.Format("{0}/Assembly-CSharp.dll", managedPath);
        string dllName = "Assembly-CSharp_" + folderName + ".dll";
        File.Copy(encryptDllPath, s_encypt_tool + "\\" + dllName, true);
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.FileName = s_encypt_tool + "\\encrypt.bat";
        startInfo.CreateNoWindow = true;
        startInfo.UseShellExecute = false;
        //startInfo.Arguments = dllName + ";" + currentDirectory + "\\" + s_encypt_tool;
        startInfo.Arguments = dllName + ";" + toolPath + ";" + targetDir + ";" + appName;

        System.Diagnostics.Process exe = new System.Diagnostics.Process();
        exe.StartInfo = startInfo;
        exe.Start();

        exe.Close();
        exe.Dispose();
    }

    private void OnDestroy()
    {

    }
}


