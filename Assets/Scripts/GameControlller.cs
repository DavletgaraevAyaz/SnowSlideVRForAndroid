using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//������ �������� �� ������� �������
public class GameController : MonoBehaviour
{
    //������ �� Transform ������ (������ ������ � 0 �� ��� Z ��� ���������� ������)
    [SerializeField] private Transform _player;
    [SerializeField] private CanvasUpdater _canvasUpdater;

    //���������� ��� ��������� ����� � ���������� ���������
    private int _points = 0;
    private int _distance = 0;

    //����, ����������� �� �������� (����� ������������)
    private bool _isGameOver = false;

    //��������� ����������� ���������� - ������ �� ���������� ��������� ������
    private static GameController _instance;
    //��������� ��������, �� �������� ����� �������� ������ � ����������
    public static GameController Instance { get { return _instance; } }

    private void Awake()
    {
        //��������, ��� ������ ��������� ������ ������������
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        //�������� � ������ ����
        StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine()
    {
        yield return new WaitForSeconds(3f);

        _player.GetComponent<Rigidbody>().isKinematic = false;
    }

    private void Update()
    {
        //��������� ���������� ���������
        UpdateDistance();
    }

    private void UpdateDistance()
    {
        //������� �� Z ����������� �� ������ �����
        _distance = Mathf.RoundToInt(_player.position.z);
        //��������� ����� �� ������
        _canvasUpdater.UpdateDistanceText(_distance);
    }

    //����� �������� ��������� �������
    //���������� �� ������� Points
    public void PickedUpPoints(int points)
    {
        //����������� ����� �� �������� ��������� �������
        _points += points;
        //��������� ����� �� ������
        _canvasUpdater.UpdatePointsText(_points);
    }

    //����� ���������� ����
    //���������� �� ������� BlockCollider
    public void GameOver()
    {
        //���� ���� ��� true (��� � ���-�� ���� ������������)
        //������ �� ������ - return
        if (_isGameOver) return;

        //�������� �������� ���������� ����, ������ �������� �����
        StartCoroutine(GameOverCoroutine());
        _isGameOver = true;
    }

    //�������� ���������� ����
    private IEnumerator GameOverCoroutine()
    {
        //��������� ��������� ��������
        SaveToFile.Instance.Save(_points, _distance);

        //��� 3 �������, ����� ���� ��������� ���� ���������� ����
        yield return new WaitForSeconds(3f);

        _player.GetComponent<Rigidbody>().isKinematic = true;
        _canvasUpdater.ShowGameOverMenu();
    }
}
