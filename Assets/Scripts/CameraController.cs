using UnityEngine;
using UnityEngine.XR.Management;

//������ ��� ������������ ����� � ����������� �� ������ ����
public class CameraController : MonoBehaviour
{
    //������ �� ������� �����
    [SerializeField] GameObject _VRCamera;
    [SerializeField] GameObject _3DCamera;

    //�������� �� ������ ������� �� VR-�����
    //���� �� - ����������� 3D-������
    //���� ��� - ����������� VR-������
    //���������� �� ����� ��� ������� ��������
    private void Start()
    {
        if (XRGeneralSettings.Instance.Manager.isInitializationComplete)
            _3DCamera.SetActive(false);
        else
            _VRCamera.SetActive(false); 
    }

    //����� ��� "�������" ������������ ����� � ����������� �� ������������ ���������
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
