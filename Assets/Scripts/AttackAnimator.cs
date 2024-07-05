using UnityEngine;

public class AttackAnimator : MonoBehaviour
{
    private Animator animator;
    private Vector2 lookDirection;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found.");
        }
    }

    public void UpdateLookDirection(Vector2 direction)
    {
        lookDirection = direction;
    }

    public void PlayAttackAnimation()
    {
        if (animator != null)
        {
            if (lookDirection.y > 0)
            {
                animator.SetTrigger("AttackUp");
            }
            else if (lookDirection.y < 0)
            {
                animator.SetTrigger("AttackDown");
            }
            else if (lookDirection.x > 0)
            {
                animator.SetTrigger("AttackRight");
            }
            else
            {
                animator.SetTrigger("AttackRight"); // Default to right if no vertical movement
            }
        }
    }
}
