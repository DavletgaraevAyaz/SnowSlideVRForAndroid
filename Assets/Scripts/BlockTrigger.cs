using UnityEngine;

public class BlockTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Spawner.Instance.SpawnBlock();
            Spawner.Instance.SpawnPoints();
        }
    }
}
