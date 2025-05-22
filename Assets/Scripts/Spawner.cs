using System.Collections.Generic;
using UnityEngine;

//скрипт генерирует случайным образом препятсвия и монетки на трассе
public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] _blockPrefabs;
    [SerializeField] GameObject _pointsPrefab;

    [SerializeField] int _startCount;
    [SerializeField] float _blocksDeltaDistance;

    private float _blocksCurrentDistance = 0;
    private float _pointsCurrentDistance = 0;
    private float _pointsDeltaDistance = 0;
    private float _pointsStartOffset = 0;

    private readonly List<GameObject> _spawnedBlocks = new();
    private readonly List<GameObject> _spawnedPoints = new();

    private static Spawner _instance;

    public static Spawner Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _pointsDeltaDistance = _blocksDeltaDistance;
        _pointsStartOffset = _pointsDeltaDistance / 2f;

        for (int i = 0; i < _startCount; i++)
        {
            SpawnBlock();
            SpawnPoints();

            _pointsStartOffset = 0;
        }
    }

    public void SpawnBlock()
    {
        _blocksCurrentDistance = Spawn(_blockPrefabs[Random.Range(0, _blockPrefabs.Length)],
                _blocksCurrentDistance, _blocksDeltaDistance, _spawnedBlocks, 12);
    }

    public void SpawnPoints()
    {
        _pointsCurrentDistance = Spawn(_pointsPrefab, _pointsCurrentDistance + _pointsStartOffset,
            _pointsDeltaDistance, _spawnedPoints, 12);
    }

    private float Spawn(GameObject prefab, float currentDistance, float deltaDistance, List<GameObject> spawnedObjects, int maxObjects)
    {
        currentDistance += deltaDistance;

        GameObject newBlock = Instantiate(prefab, new Vector3(0, 0, currentDistance), Quaternion.identity);

        spawnedObjects.Add(newBlock);

        if (spawnedObjects.Count > maxObjects)
        {
            Destroy(spawnedObjects[0]);
            spawnedObjects.RemoveAt(0);
        }

        return currentDistance;
    }
}
