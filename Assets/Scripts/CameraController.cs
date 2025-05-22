using UnityEngine;
using UnityEngine.XR.Management;

//скрипт для переключения камер в зависимости от режима игры
public class CameraController : MonoBehaviour
{
    //ссылки на объекты камер
    [SerializeField] GameObject _VRCamera;
    [SerializeField] GameObject _3DCamera;

    //проверка на старте включён ли VR-режим
    //если да - отключается 3D-камера
    //если нет - отключается VR-камера
    //изначально на сцене оба объекта включены
    private void Start()
    {
        if (XRGeneralSettings.Instance.Manager.isInitializationComplete)
            _3DCamera.SetActive(false);
        else
            _VRCamera.SetActive(false); 
    }

    //метод для "ручного" переключения камер в зависимости от поступающего аргумента
    public void SetVRCameraActive(bool isActive)
    {
        if (isActive)
        {
            _VRCamera.SetActive(true);
            _3DCamera.SetActive(false);
        }
        else
        {
            _VRCamera.SetActive(false);
            _3DCamera.SetActive(true);
        }
    }
}
