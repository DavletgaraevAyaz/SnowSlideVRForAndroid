using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockCollider : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameController.Instance.GameOver();
        }
    }
}
