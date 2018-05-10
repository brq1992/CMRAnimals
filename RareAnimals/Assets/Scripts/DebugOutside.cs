using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LogObject
{
    public string output = "";
    public string stack = "";
    public LogType logType;
}

public class DebugOutside : MonoBehaviour
{
    //public long maxLogCount = 200;

    private List<LogObject> lo = new List<LogObject>();
    private Rect m_logWinRect = new Rect(Screen.width - 1200, 0, 1200, 800);
    private Rect m_minBtnRect = new Rect(Screen.width - 140, 50, 140, 55);
    private Vector2 scrollPosition;
    private bool m_showLog = true;
    private bool m_showWarning = false;
    private bool m_showError = true;
    private bool m_showLogWin = false;

    void Start()
    {
    }

    void OnEnable()
    {
        Application.RegisterLogCallback(HandleLog);
    }
    void OnDisable()
    {
        Application.RegisterLogCallback(null);
    }
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        LogObject o = new LogObject();
        o.output = logString;
        o.stack = stackTrace;
        o.logType = type;
        if (lo.Count > 200)
        {
            lo.RemoveAt(0);
        }
        lo.Add(o);
    }
    void OnGUI()
    {
        if (m_showLogWin)
        {
            m_logWinRect = GUI.Window(0, m_logWinRect, LogWin, "输   出");
        }
        else
        {
            m_minBtnRect = GUI.Window(1, m_minBtnRect, MinBtnWin, "输   出");
        }
        GUI.skin.label.fontSize = 30;
    }

    void MinBtnWin(int id)
    {
        GUILayout.Space(5);
        if (GUILayout.Button("展   开"))
        {
            m_showLogWin = !m_showLogWin;
        }
        GUI.DragWindow(new Rect(0, 0, 1000, 20000));
    }

    public void ClearLog()
    {
        lo.Clear();
    }
    void LogWin(int id)
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true, GUILayout.Width(1185), GUILayout.Height(750));
        foreach (LogObject item in lo)
        {
            if (item.logType == LogType.Log && !m_showLog)
            {
                continue;
            }
            if (item.logType == LogType.Warning && !m_showWarning)
            {
                continue;
            }
            if (item.logType == LogType.Error && !m_showError)
            {
                continue;
            }
            switch (item.logType)
            {
                case LogType.Log:
                    GUI.color = Color.white;
                    break;
                case LogType.Warning:
                    GUI.color = Color.yellow;
                    break;
                case LogType.Error:
                    GUI.color = Color.red;
                    break;
                default:
                   // MessageBoxInstance.Instance.CancelBtnMsgBox(item.output + "\n" + item.stack, null);
                    GUI.color = new Color(.3f, .3f, .3f, 1f);
                    break;
            }



            GUILayout.Label(item.output + "\n" + item.stack);
            GUILayout.Space(-25);
            GUI.color = Color.gray;
            GUILayout.Label("------------------------------------------------------------------------------------------------------------------------------------------");
            GUILayout.Space(-10);
        }
        GUILayout.EndScrollView();
        GUILayout.BeginHorizontal();
        GUI.color = Color.white;
        if (GUILayout.Button("折   叠"))
        {
            m_showLogWin = !m_showLogWin;
            //m_minBtnRect.x = m_logWinRect.center.x - m_minBtnRect.x / 2;
            //m_minBtnRect.y = m_logWinRect.center.y - m_minBtnRect.y / 2;
            m_minBtnRect = new Rect(Screen.width - 140, 0, 140, 55);
        }
        if (GUILayout.Button("清   除"))
        {
            lo.Clear();
        }
        m_showLog = GUILayout.Toggle(m_showLog, "日   志");
        m_showWarning = GUILayout.Toggle(m_showWarning, "警   告");
        m_showError = GUILayout.Toggle(m_showError, "错   误");
        GUILayout.EndHorizontal();
        GUI.DragWindow(new Rect(0, 0, 1000, 20000));
    }
}
