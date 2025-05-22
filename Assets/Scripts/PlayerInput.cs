using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //������ �� Rigidbody ������
    [SerializeField] private Rigidbody _player;
    //����������� �������� �������� ������-�����
    [SerializeField] private float _force;
    //���������� �������� �������� �����
    [SerializeField] private float _maxSpeed = 20f;

    //������ ��� �������� ���������� � ������� ������ AD
    private Vector3 _xForce;

    private void Start()
    {
        Input.gyro.enabled = true;
    }
    void Update()
    {
        _xForce = new Vector3(-Input.gyro.rotationRateUnbiased.z, 0, 0);

#if UNITY_EDITOR
        _xForce = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
#endif
    }

    private void FixedUpdate()
    {
        //������� ������ ������ ��� �����, ���� ������ �������
        _player.AddForce(_xForce * _force);

        //���� �������� ������ ��������� ����������...
        if (_player.velocity.z > _maxSpeed)
        {
            //������������ � � ����������� ���������� �� ��� Z (��� �����)
            _player.velocity = new Vector3(_player.velocity.x, _player.velocity.y, _maxSpeed);
        }
    }
}