using UnityEngine;
using UnityEngine.SceneManagement;

// ������ ��� �������� ����
public class SceneLoader : MonoBehaviour
{
    // ��������� �� ����� ����� �� ������?
    [SerializeField] bool _loadAtStart = false;
    // ������ ����������� �����
    [SerializeField] int _startLoadSceneIndex;

    void Start()
    {
        // ��������� ��� ������ ��������� �����, ���� ����������
        if (_loadAtStart)
            LoadScene(_startLoadSceneIndex);
    }

    // ����� �������� ����� �����
    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    // ����� ������ �� ����������
    public void ApplicationQuit()
    {
        Application.Quit();
    }
}