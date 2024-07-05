using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Transform visualTransform; // Reference to the Visual child object

    private Vector2 movement;

    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                Debug.LogError("Rigidbody2D component not found!");
            }
        }

        if (visualTransform == null)
        {
            visualTransform = transform.Find("Visual");
            if (visualTransform == null)
            {
                Debug.LogError("Visual child object not found!");
            }
        }
    }

    public void Move(Vector2 direction)
    {
        movement = direction.normalized; // Normalize the direction vector
        if (rb != null)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            Debug.LogError("Rigidbody2D is not assigned!");
        }

        // Flip the sprite based on the horizontal direction
        if (visualTransform != null)
        {
            if (movement.x < 0)
            {
                visualTransform.localScale = new Vector3(-1, 1, 1); // Flip left
            }
            else if (movement.x > 0)
            {
                visualTransform.localScale = new Vector3(1, 1, 1); // Flip right
            }
        }
    }
}
