using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Management;

//������ ��� ���������� �������� UI
public class CanvasUpdater : MonoBehaviour
{
    //������ �� ��������� ����
    [SerializeField] private TextMeshProUGUI _pointsText3D;
    [SerializeField] private TextMeshProUGUI _distanceText3D;
    [SerializeField] private TextMeshProUGUI _pointsTextVR;
    [SerializeField] private TextMeshProUGUI _distanceTextVR;
    //������ �� ���� ���������� ���� � ������� �����
    [SerializeField] private GameObject _gameOverMenu3D;
    [SerializeField] private GameObject _gameOverMenuVR;

    private void Start()
    {
        //���� ��������� ����� ����(1)...
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            //����������� ������ �� ����������
            _pointsText3D.text = SaveToFile.Instance.MaxPoints.ToString();
            _distanceText3D.text = SaveToFile.Instance.MaxDistance.ToString();
        }
    }

    // ����� ��� ������ ���� ���������� ����
    public void ShowGameOverMenu()
    {
        if (XRGeneralSettings.Instance.Manager.isInitializationComplete)
        {
            _gameOverMenuVR.transform.parent = null;
            _gameOverMenuVR.SetActive(true);
        }
        else
            _gameOverMenu3D.SetActive(true);
    }

    // ����� ��� ���������� ����� �����
    public void UpdatePointsText(int points)
    {
        if (XRGeneralSettings.Instance.Manager.isInitializationComplete)
            _pointsTextVR.text = points.ToString();
        else
            _pointsText3D.text = points.ToString();
    }

    // ����� ��� ���������� ����� ����������� ����������
    public void UpdateDistanceText(int distance)
    {
        if (XRGeneralSettings.Instance.Manager.isInitializationComplete)
            _distanceTextVR.text = distance.ToString();
        else
            _distanceText3D.text = distance.ToString();
    }
}
