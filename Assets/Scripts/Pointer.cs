using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//������ ��� ������ ������� �� �������, �� ������� ������� ������
//������ ���������� �� ������� VR Camera
public class Pointer : MonoBehaviour
{
    //������, ������������ �����, ���� ��������� ������
    [SerializeField] GameObject _target;
    //����������� ������� �������
    private Image _targetImage;

    //����� ����
    private const float _maxDistance = 50;
    //������, �� ������� ������� ������
    private GameObject _lookedAtObject = null;

    //������� ������������ ������� UI
    private PointerEventData _pointerEventData;
    //UI ������, � ������� ��� ��������������
    private Selectable _uiComponent;

    //���������� - ������ ������� ��� ������ �������
    private float _timer;
    //����� �� ������ �������
    private readonly float _timeToEvent = 3f;

    void Start()
    {
        _pointerEventData = new PointerEventData(EventSystem.current);
        _targetImage = _target.GetComponent<Image>();
    }

    public void FixedUpdate()
    {
        //��������� ��� � ����������� ��� Z ������, ����� ����� �����-���� ������
        //���� ������ �� �������...
        if (!Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _maxDistance))
        {
            //��������� ������, ��������� �����
            _targetImage.enabled = false;
            return;
        }

        //���� ������ ��� ������ (��� � ���-�� ����������)...
        //�������� ������
        _targetImage.enabled = true;
        //��������� ��������� �������
        _target.transform.position = hit.point;

        //�������� ������� �� ���� (������ ��� ��� ������)
        if (hit.transform.CompareTag("InteractableUI"))
        {
            //���� � ���� ����� ������ � ����� ������...
            if (_lookedAtObject != hit.transform.gameObject)
            {
                //���������� �� ���������� ������ �������, ��� � ���� ������ ������ (���)
                if (_uiComponent != null) _uiComponent.OnPointerExit(_pointerEventData);

                //����� ������ � �������� ��������
                _lookedAtObject = hit.transform.gameObject;

                //������� ����� �� �� ��������� Selectable (��. ��� ���������� _uiComponent)
                //���� ��������� ����� ������, �� �� ����� �������� ���������� _uiComponent
                if (_lookedAtObject.TryGetComponent(out _uiComponent))
                {
                    //���������� �� ��������� �������, ��� �� ���� ������ ������
                    //� ������ ������ ���
                    _uiComponent.OnPointerEnter(_pointerEventData);
                }

                //���������� ������ ��������� ������� �� �������
                ResetTimer();
            }

            //���� ������ �� ���������...
            //����������� ������
            IncreaseTimer();
        }
        //���� ������ �� �������������...
        else
        {
            //���������� ������
            ResetTimer();
            //���������� ���������� �� ���������� ������ �������, ��� � ���� ������ ������ (���)
            if (_uiComponent != null) _uiComponent.OnPointerExit(_pointerEventData);

            _lookedAtObject = null;
        }
    }

    //����� ���������� �������� �������
    private void IncreaseTimer()
    {
        _timer += Time.fixedDeltaTime;

        //������ ���������� ������� �� �������
        //� ���������� Image: Image Type - Filled
        //                    Fill Method - Radial 360
        _targetImage.fillAmount = 1 - _timer / _timeToEvent;

        //���� ������ ������ ��������� �������...
        if (_timer >= _timeToEvent)
        {
            //�������� �� ���������� ����� OnPointerClick - �������� ������ / �������������
            if (_uiComponent != null) _uiComponent.SendMessage("OnPointerClick", _pointerEventData);
            //���������� ������
            ResetTimer();
        }
    }

    //����� ������ �������
    private void ResetTimer()
    {
        _timer = 0;
        _targetImage.fillAmount = 1f;
    }
}