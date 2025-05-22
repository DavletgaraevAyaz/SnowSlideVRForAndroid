using UnityEngine;

public class SelectRandomChild : MonoBehaviour
{
    [SerializeField] int _countToLeave = 1;

    private void Start()
    {
        while (transform.childCount > _countToLeave)
        {
            Transform childToDestroy = transform.GetChild(Random.Range(0, transform.childCount));
            DestroyImmediate(childToDestroy.gameObject);
        }
    }
}