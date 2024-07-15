using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HouseEntrance : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(SaveAndChangeScene());
        }
        else
        {
            Debug.Log("Collision with non-player object: " + other.name);
        }
    }

    private IEnumerator SaveAndChangeScene()
    {
        GameManager.Instance.SaveGameState(); // Save the game state
        yield return new WaitForEndOfFrame(); // Wait for the end of the frame to ensure the save is complete
        GameManager.Instance.GoToHouse();
    }
}
