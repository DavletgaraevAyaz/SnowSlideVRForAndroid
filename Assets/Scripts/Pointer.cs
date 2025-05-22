using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//скрипт для вызова событий на объекте, на который смотрит камера
//должен находиться на объекте VR Camera
public class Pointer : MonoBehaviour
{
    //маркер, показывающий точку, куда направлен взгляд
    [SerializeField] GameObject _target;
    //изображение маркера взгляда
    private Image _targetImage;

    //длина луча
    private const float _maxDistance = 50;
    //объект, на который смотрит камера
    private GameObject _lookedAtObject = null;

    //система отслеживания событий UI
    private PointerEventData _pointerEventData;
    //UI объект, с которым идёт взаимодействие
    private Selectable _uiComponent;

    //переменнай - таймер времени для вызова событий
    private float _timer;
    //время до вызова событий
    private readonly float _timeToEvent = 3f;

    void Start()
    {
        _pointerEventData = new PointerEventData(EventSystem.current);
        _targetImage = _target.GetComponent<Image>();
    }

    public void FixedUpdate()
    {
        //выпускаем луч в направлении оси Z камеры, чтобы найти какой-либо объект
        //если ничего не найдено...
        if (!Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _maxDistance))
        {
            //выключаем маркер, завершаем метод
            _targetImage.enabled = false;
            return;
        }

        //если объект был найден (луч с чем-то столкнулся)...
        //включаем маркер
        _targetImage.enabled = true;
        //обновляем положение маркера
        _target.transform.position = hit.point;

        //проверка объекта по тегу (задать тег для кнопок)
        if (hit.transform.CompareTag("InteractableUI"))
        {
            //если в этом кадре попали в новый объект...
            if (_lookedAtObject != hit.transform.gameObject)
            {
                //отправляем на предыдущий объект событие, что с него убрали курсор (луч)
                if (_uiComponent != null) _uiComponent.OnPointerExit(_pointerEventData);

                //задаём объект в качестве текущего
                _lookedAtObject = hit.transform.gameObject;

                //пробуем найти на нём компонент Selectable (см. тип переменной _uiComponent)
                //если компонент будет найден, то он будет назначен переменной _uiComponent
                if (_lookedAtObject.TryGetComponent(out _uiComponent))
                {
                    //отправляем на компонент событие, что на него навели курсор
                    //в данном случае луч
                    _uiComponent.OnPointerEnter(_pointerEventData);
                }

                //сбрасываем таймер удержания взгляда на объекте
                ResetTimer();
            }

            //если объект не изменился...
            //увеличиваем таймер
            IncreaseTimer();
        }
        //если объект не интерактивный...
        else
        {
            //сбрасываем таймер
            ResetTimer();
            //отправляем отправляем на предыдущий объект событие, что с него убрали курсор (луч)
            if (_uiComponent != null) _uiComponent.OnPointerExit(_pointerEventData);

            _lookedAtObject = null;
        }
    }

    //метод увеличения значения таймера
    private void IncreaseTimer()
    {
        _timer += Time.fixedDeltaTime;

        //меняем заполнение маркера по времени
        //в компоненте Image: Image Type - Filled
        //                    Fill Method - Radial 360
        _targetImage.fillAmount = 1 - _timer / _timeToEvent;

        //если таймер достиг заданного времени...
        if (_timer >= _timeToEvent)
        {
            //вызываем на компоненте метод OnPointerClick - нажаитие кнопки / переключателя
            if (_uiComponent != null) _uiComponent.SendMessage("OnPointerClick", _pointerEventData);
            //сбрасываем таймер
            ResetTimer();
        }
    }

    //метод сброса таймера
    private void ResetTimer()
    {
        _timer = 0;
        _targetImage.fillAmount = 1f;
    }
}