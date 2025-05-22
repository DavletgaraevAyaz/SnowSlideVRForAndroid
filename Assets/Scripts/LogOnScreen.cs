using System.IO;
using UnityEngine;

//скрипт для вывода данных лога на экран и в файл
public class LogOnScreen : MonoBehaviour
{
    //флаг определяет, записывать ли лог в файл
    [SerializeField] bool _writeToFile;
    //флаг определяет, показать ли лог на экране
    [SerializeField] bool _showConsole;

    private string output;
    private string logWritePath;
    private Vector2 scroll;
    private StreamWriter sw;

    private void Start()
    {
        //если нужно записывать лог в файл...
        if (_writeToFile)
        {
            //определяется путь и имя файла для записи
            logWritePath = Application.persistentDataPath + "/" + System.DateTime.Now.ToString("yy.MM.dd HH.mm.ss") + ".txt";
            //открывается новый поток, который будет записывать текстовые данные
            sw = new StreamWriter(logWritePath, true, System.Text.Encoding.Default);
        }
    }

    //метод вызывается для отрисовки графического интерфейса в старой системе UI
    void OnGUI()
    {
        GUI.depth = -10000;

        if (_showConsole)
            ShowConsole();
    }

    //метод отрисовывает консоль на экране
    void ShowConsole()
    {
        GUILayout.BeginArea(new Rect(1, 1, Screen.width, Screen.height / 2));
        scroll = GUILayout.BeginScrollView(scroll);
        GUILayout.Label(output);
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    //подписка на лог событий при загрузке сцены
    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        output += type + ": " + logString + "\n";
        scroll.y = 10000000000;

        //запись лога в файл
        if (_writeToFile && File.Exists(logWritePath))
        {
            sw.WriteLineAsync(logString);
        }
    }

    //закрытие файла при выходе из приложения
    private void OnDisable()
    {
        if (_writeToFile && File.Exists(logWritePath))
            sw.Close();
    }
}