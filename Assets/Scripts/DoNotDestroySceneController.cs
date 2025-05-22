using UnityEngine;

//��������������� ����� ��� ������� �����
public class DoNotDestroySceneController : MonoBehaviour
{
    private void Awake()
    {
        //����� ��������� ������� �����
        //������ ������ ����������� ��� �������� ������ �����
        //�� ���� �� ����� ��������� �������� � ���������� �� ����� �����
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        //������� ��� ������ �� ���������� �� ������ Escape
        //������ ����� �� ����������
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}