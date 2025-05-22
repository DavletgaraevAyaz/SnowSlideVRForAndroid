using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //ссылка на Rigidbody игрока
    [SerializeField] private Rigidbody _player;
    //коэффициент скорости движения вправо-влево
    [SerializeField] private float _force;
    //предельная скорость движения вперёд
    [SerializeField] private float _maxSpeed = 20f;

    //вектор для хранения информации о нажатии клавиш AD
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
        //толкаем игрока вправо или влево, если нажата клавиша
        _player.AddForce(_xForce * _force);

        //если скорость игрока превышает предельную...
        if (_player.velocity.z > _maxSpeed)
        {
            //приравниваем её к максимально допустимой по оси Z (ось вперёд)
            _player.velocity = new Vector3(_player.velocity.x, _player.velocity.y, _maxSpeed);
        }
    }
}