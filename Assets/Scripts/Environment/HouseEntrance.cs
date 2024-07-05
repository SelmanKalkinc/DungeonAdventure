using UnityEngine;
using UnityEngine.SceneManagement;

public class HouseEntrance : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.GoToHouse();
        }
        else
        {
            Debug.Log("Collision with non-player object: " + other.name);
        }
    }
}
