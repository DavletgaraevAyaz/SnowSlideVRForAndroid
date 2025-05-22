using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using System.Collections;
using System;

//скрипт для сохранения и загрузки данных из файла
public class SaveToFile : MonoBehaviour
{
    //вспомогательные флаги, говорящие о том, стало ли число очков больше, чем было сохранено раньше
    private bool _distanceChanged = false, _pointsChanged = false;

    //инструмент для преобразования данных в бинарный вид и обратно
    private readonly BinaryFormatter _binaryFormatter = new();

    //приватная переменная для максимума набранных очков
    private int _maxPoints = 0;
    //публичное свойство, которое задаёт логику изменения числа очков
    public int MaxPoints
    {
        //при обращении к MaxPoints вернётся значение _maxPoints
        get => _maxPoints;

        //когда записываем новое значение, происходит дополнительная проверка
        private set
        {
            //если текущее значение _maxPoints меньше того, которое передаётся (value)...
            if (_maxPoints < value)
            {
                //тогда записываем это значение в переменную
                //устанавливаем флаг изменения очков
                _maxPoints = value;
                _pointsChanged = true;
            }
        }
    }

    //приватная переменная для максимума пройденной дистанции
    private int _maxDistance = 0;
    //публичное свойство, которое задаёт логику изменения числа очков (см. выше)
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

    //приватная статическая переменная - ссылка на конкретный экземпляр класса
    private static SaveToFile _instance;

    //публичное свойство, по которому можно получить доступ к экземпляру
    public static SaveToFile Instance => _instance;

    private void Awake()
    {
        //проверка, что данный экземпляр класса единственный
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        //вызов метода загрузки данных из файла
        Load();
    }

    //метод сохранения данных
    public void Save(int points, int distance)
    {
        MaxPoints = points;
        MaxDistance = distance;

        //если число очков или пройденная дистанция больше, чем те, что были загружены (сохранены) ранее,
        //начинается сохранение данных
        if (_pointsChanged || _distanceChanged)
            StartCoroutine(SaveCoroutine());

        //сброс флагов после сохранения
        _pointsChanged = false;
        _distanceChanged = false;
    }

    //корутина, записывающая данные в файл
    private IEnumerator SaveCoroutine()
    {
        //создание или открытие файла для записи
        FileStream file = File.Create(Application.persistentDataPath + "/SaveData.dat");

        //создание нового экземпляра класса SaveData и передача в него значений
        SaveData data = new()
        {
            maxPoints = MaxPoints,
            maxDistance = MaxDistance
        };

        //конвертация и запись бинарных данных в файл
        _binaryFormatter.Serialize(file, data);

        //закрытие файла, выход из корутины
        file.Close();
        Debug.Log("Game data saved!");

        yield return null;
    }

    //метод загрузки данных из файла
    public void Load()
    {
        //Debug.Log(Application.persistentDataPath);

        //если по заданному пути файла не существует, происходит выход из метода
        if (!File.Exists(Application.persistentDataPath + "/SaveData.dat"))
        {
            Debug.Log("There is no save data!");
            return;
        }

        //если файл существует, открываем его для чтения данных
        FileStream file = File.Open(Application.persistentDataPath + "/SaveData.dat", FileMode.Open);
        //конвертация бинарных данных в класс SaveData и закрытие файла
        SaveData loadData = (SaveData)_binaryFormatter.Deserialize(file);
        file.Close();

        //запись загруженной информации в переменные
        MaxPoints = loadData.maxPoints;
        MaxDistance = loadData.maxDistance;

        //сброс флагов после загрузки
        _pointsChanged = false;
        _distanceChanged = false;
        //Debug.Log($"Points: {MaxPoints}, Distance: {MaxDistance}");
        Debug.Log("Game data loaded!");
    }


}

//класс, содержащий данные, которые нужно сохранить
[Serializable]
class SaveData
{
    public int maxPoints;
    public int maxDistance;
}