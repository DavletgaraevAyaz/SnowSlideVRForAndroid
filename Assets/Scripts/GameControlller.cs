using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//скрипт отвечает за игровой процесс
public class GameController : MonoBehaviour
{
    //ссылка на Transform игрока (должен сто€ть в 0 по оси Z дл€ корректной работы)
    [SerializeField] private Transform _player;
    [SerializeField] private CanvasUpdater _canvasUpdater;

    //переменные дл€ набранных очков и пройденной дистанции
    private int _points = 0;
    private int _distance = 0;

    //флаг, указывающий на проигрыш (после столкновени€)
    private bool _isGameOver = false;

    //приватна€ статическа€ переменна€ - ссылка на конкретный экземпл€р класса
    private static GameController _instance;
    //публичное свойство, по которому можно получить доступ к экземпл€ру
    public static GameController Instance { get { return _instance; } }

    private void Awake()
    {
        //проверка, что данный экземпл€р класса единственный
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        //задержка в начале игры
        StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine()
    {
        yield return new WaitForSeconds(3f);

        _player.GetComponent<Rigidbody>().isKinematic = false;
    }

    private void Update()
    {
        //обновл€ем пройденную дистанцию
        UpdateDistance();
    }

    private void UpdateDistance()
    {
        //позици€ по Z округл€етс€ до целого числа
        _distance = Mathf.RoundToInt(_player.position.z);
        //обновл€ем текст на экране
        _canvasUpdater.UpdateDistanceText(_distance);
    }

    //метод подсчЄта собранных монеток
    //вызываетс€ из скрипта Points
    public void PickedUpPoints(int points)
    {
        //увеличиваем сумму на величину собранной монетки
        _points += points;
        //обновл€ем текст на экране
        _canvasUpdater.UpdatePointsText(_points);
    }

    //метод завершени€ игры
    //вызываетс€ из скрипта BlockCollider
    public void GameOver()
    {
        //если флаг уже true (уже с чем-то было столкновение)
        //ничего не делаем - return
        if (_isGameOver) return;

        //запукаем корутину завершени€ игры, мен€ем значение флага
        StartCoroutine(GameOverCoroutine());
        _isGameOver = true;
    }

    //корутина завершени€ игры
    private IEnumerator GameOverCoroutine()
    {
        //сохран€ем набранные значени€
        SaveToFile.Instance.Save(_points, _distance);

        //ждЄм 3 секунды, после чего открываем меню завершени€ игры
        yield return new WaitForSeconds(3f);

        _player.GetComponent<Rigidbody>().isKinematic = true;
        _canvasUpdater.ShowGameOverMenu();
    }
}
