using TMPro;
using UnityEngine;

public class Points : MonoBehaviour
{
    [SerializeField] private int[] _points;
    [SerializeField] private AnimationCurve _chanceFromDistance;
    [SerializeField] private Gradient _color;
    [SerializeField] private TextMeshPro[] _pointsText;

    private int _currentPoints = 10;
    private MeshRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponentInChildren<MeshRenderer>();

        Vector3 currentPosition = transform.position;
        transform.position = new Vector3(Random.Range(-4f, 4f), currentPosition.y, currentPosition.z);

        GenerateParameters(currentPosition.z);

        foreach (TextMeshPro text in _pointsText)
            text.text = _currentPoints.ToString();
    }

    public void GenerateParameters(float distance)
    {
        float randomDistance = Random.Range(0.0f, distance);
        float chance = _chanceFromDistance.Evaluate(randomDistance);

        for (int i = _points.Length - 1; i >= 0; i--)
        {
            if (_points[i] <= chance)
            {
                _currentPoints = _points[i];
                _renderer.material.color = _color.Evaluate((float)i / (_points.Length - 1));
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.Instance.PickedUpPoints(_currentPoints);
            Destroy(gameObject);
        }
    }
}
