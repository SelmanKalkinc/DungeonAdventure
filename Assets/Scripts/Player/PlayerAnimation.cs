using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        // Reference the Animator component on the Visual child object
        animator = GetComponentInChildren<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator component not found on Visual child object!");
        }
        else
        {
            Debug.Log("Animator component assigned successfully");
        }
    }

    public void UpdateAnimation(Vector2 direction, float speed)
    {
        if (animator != null)
        {
            if (animator.HasParameter("Horizontal"))
                animator.SetFloat("Horizontal", direction.x);
            if (animator.HasParameter("Vertical"))
                animator.SetFloat("Vertical", direction.y);
            if (animator.HasParameter("Speed"))
                animator.SetFloat("Speed", speed);
        }
        else
        {
            Debug.LogError("Animator is not assigned!");
        }
    }
}

public static class AnimatorExtensions
{
    public static bool HasParameter(this Animator self, string parameterName)
    {
        foreach (AnimatorControllerParameter param in self.parameters)
        {
            if (param.name == parameterName)
                return true;
        }
        return false;
    }
}
