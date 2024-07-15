using UnityEngine;
using UnityEngine.SceneManagement;

public class GardenEntrance : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.GoToGarden();
        }
    }
}
