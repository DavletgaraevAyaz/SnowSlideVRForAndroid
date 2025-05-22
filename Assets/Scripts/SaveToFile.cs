using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using System.Collections;
using System;

//������ ��� ���������� � �������� ������ �� �����
public class SaveToFile : MonoBehaviour
{
    //��������������� �����, ��������� � ���, ����� �� ����� ����� ������, ��� ���� ��������� ������
    private bool _distanceChanged = false, _pointsChanged = false;

    //���������� ��� �������������� ������ � �������� ��� � �������
    private readonly BinaryFormatter _binaryFormatter = new();

    //��������� ���������� ��� ��������� ��������� �����
    private int _maxPoints = 0;
    //��������� ��������, ������� ����� ������ ��������� ����� �����
    public int MaxPoints
    {
        //��� ��������� � MaxPoints ������� �������� _maxPoints
        get => _maxPoints;

        //����� ���������� ����� ��������, ���������� �������������� ��������
        private set
        {
            //���� ������� �������� _maxPoints ������ ����, ������� ��������� (value)...
            if (_maxPoints < value)
            {
                //����� ���������� ��� �������� � ����������
                //������������� ���� ��������� �����
                _maxPoints = value;
                _pointsChanged = true;
            }
        }
    }

    //��������� ���������� ��� ��������� ���������� ���������
    private int _maxDistance = 0;
    //��������� ��������, ������� ����� ������ ��������� ����� ����� (��. ����)
    public int MaxDistance
    {
        get => _maxDistance;

        private set
        {
            if (_maxDistance < value)
            {
                _maxDistance = value;
                _distanceChanged = true;
            }
        }
    }

    //��������� ����������� ���������� - ������ �� ���������� ��������� ������
    private static SaveToFile _instance;

    //��������� ��������, �� �������� ����� �������� ������ � ����������
    public static SaveToFile Instance => _instance;

    private void Awake()
    {
        //��������, ��� ������ ��������� ������ ������������
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        //����� ������ �������� ������ �� �����
        Load();
    }

    //����� ���������� ������
    public void Save(int points, int distance)
    {
        MaxPoints = points;
        MaxDistance = distance;

        //���� ����� ����� ��� ���������� ��������� ������, ��� ��, ��� ���� ��������� (���������) �����,
        //���������� ���������� ������
        if (_pointsChanged || _distanceChanged)
            StartCoroutine(SaveCoroutine());

        //����� ������ ����� ����������
        _pointsChanged = false;
        _distanceChanged = false;
    }

    //��������, ������������ ������ � ����
    private IEnumerator SaveCoroutine()
    {
        //�������� ��� �������� ����� ��� ������
        FileStream file = File.Create(Application.persistentDataPath + "/SaveData.dat");

        //�������� ������ ���������� ������ SaveData � �������� � ���� ��������
        SaveData data = new()
        {
            maxPoints = MaxPoints,
            maxDistance = MaxDistance
        };

        //����������� � ������ �������� ������ � ����
        _binaryFormatter.Serialize(file, data);

        //�������� �����, ����� �� ��������
        file.Close();
        Debug.Log("Game data saved!");

        yield return null;
    }

    //����� �������� ������ �� �����
    public void Load()
    {
        //Debug.Log(Application.persistentDataPath);

        //���� �� ��������� ���� ����� �� ����������, ���������� ����� �� ������
        if (!File.Exists(Application.persistentDataPath + "/SaveData.dat"))
        {
            Debug.Log("There is no save data!");
            return;
        }

        //���� ���� ����������, ��������� ��� ��� ������ ������
        FileStream file = File.Open(Application.persistentDataPath + "/SaveData.dat", FileMode.Open);
        //����������� �������� ������ � ����� SaveData � �������� �����
        SaveData loadData = (SaveData)_binaryFormatter.Deserialize(file);
        file.Close();

        //������ ����������� ���������� � ����������
        MaxPoints = loadData.maxPoints;
        MaxDistance = loadData.maxDistance;

        //����� ������ ����� ��������
        _pointsChanged = false;
        _distanceChanged = false;
        //Debug.Log($"Points: {MaxPoints}, Distance: {MaxDistance}");
        Debug.Log("Game data loaded!");
    }


}

//�����, ���������� ������, ������� ����� ���������
[Serializable]
class SaveData
{
    public int maxPoints;
    public int maxDistance;
}