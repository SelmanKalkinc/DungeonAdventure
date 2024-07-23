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
