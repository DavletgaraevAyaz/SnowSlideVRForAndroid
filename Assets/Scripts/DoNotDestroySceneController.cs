using UnityEngine;

//вспомогательный класс для нулевой сцены
public class DoNotDestroySceneController : MonoBehaviour
{
    private void Awake()
    {
        //перед загрузкой первого кадра
        //делаем объект неудаляемым при загрузке другой сцены
        //то есть он будет постоянно доступен в приложении на любой сцене
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        //команда для выхода из приложения по кнопке Escape
        //кнопка Назад на смартфонах
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}