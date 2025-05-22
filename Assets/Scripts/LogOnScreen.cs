using System.IO;
using UnityEngine;

//������ ��� ������ ������ ���� �� ����� � � ����
public class LogOnScreen : MonoBehaviour
{
    //���� ����������, ���������� �� ��� � ����
    [SerializeField] bool _writeToFile;
    //���� ����������, �������� �� ��� �� ������
    [SerializeField] bool _showConsole;

    private string output;
    private string logWritePath;
    private Vector2 scroll;
    private StreamWriter sw;

    private void Start()
    {
        //���� ����� ���������� ��� � ����...
        if (_writeToFile)
        {
            //������������ ���� � ��� ����� ��� ������
            logWritePath = Application.persistentDataPath + "/" + System.DateTime.Now.ToString("yy.MM.dd HH.mm.ss") + ".txt";
            //����������� ����� �����, ������� ����� ���������� ��������� ������
            sw = new StreamWriter(logWritePath, true, System.Text.Encoding.Default);
        }
    }

    //����� ���������� ��� ��������� ������������ ���������� � ������ ������� UI
    void OnGUI()
    {
        GUI.depth = -10000;

        if (_showConsole)
            ShowConsole();
    }

    //����� ������������ ������� �� ������
    void ShowConsole()
    {
        GUILayout.BeginArea(new Rect(1, 1, Screen.width, Screen.height / 2));
        scroll = GUILayout.BeginScrollView(scroll);
        GUILayout.Label(output);
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    //�������� �� ��� ������� ��� �������� �����
    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        output += type + ": " + logString + "\n";
        scroll.y = 10000000000;

        //������ ���� � ����
        if (_writeToFile && File.Exists(logWritePath))
        {
            sw.WriteLineAsync(logString);
        }
    }

    //�������� ����� ��� ������ �� ����������
    private void OnDisable()
    {
        if (_writeToFile && File.Exists(logWritePath))
            sw.Close();
    }
}