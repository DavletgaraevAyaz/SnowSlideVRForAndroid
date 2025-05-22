using UnityEngine;
using UnityEngine.SceneManagement;

// скрипт для загрузки сцен
public class SceneLoader : MonoBehaviour
{
    // загружать ли новую сцену на старте?
    [SerializeField] bool _loadAtStart = false;
    // индекс загружаемой сцены
    [SerializeField] int _startLoadSceneIndex;

    void Start()
    {
        // загружаем при старте следующую сцену, если необходимо
        if (_loadAtStart)
            LoadScene(_startLoadSceneIndex);
    }

    // метод загрузки новой сцены
    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    // метод выхода из приложения
    public void ApplicationQuit()
    {
        Application.Quit();
    }
}