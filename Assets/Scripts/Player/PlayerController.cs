using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerAnimation playerAnimation;
    private Vector2 movement;

    private void Awake()
    {
        Debug.Log("PlayerController Awake called");
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();

        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement component not found!");
        }
        else
        {
            Debug.Log("PlayerMovement component assigned successfully");
        }

        if (playerAnimation == null)
        {
            Debug.LogError("PlayerAnimation component not found!");
        }
        else
        {
            Debug.Log("PlayerAnimation component assigned successfully");
        }
    }

    void Update()
    {

        // Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");


        // Animation
        if (playerAnimation != null)
        {
            playerAnimation.UpdateAnimation(movement, movement.sqrMagnitude);
        }
    }

    void FixedUpdate()
    {

        // Movement
        if (playerMovement != null)
        {
            playerMovement.Move(movement);
        }
    }
}
