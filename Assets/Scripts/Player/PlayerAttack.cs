using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRate = 1f; // Number of attacks per second
    public float attackRange = 1f;
    public int attackDamage = 20;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public float attackAngle = 90f;
    public GameObject visualGameObject;
    public GameObject projectilePrefab;

    private float nextAttackTime = 0f;
    private Vector2 lookDirection;
    private Animator animator;
    private IAttack currentAttack;
    private Transform visualTransform;

    private IAttack meleeAttack;
    private IAttack rangedAttack;
    private Collider2D playerCollider;

    void Start()
    {
        animator = visualGameObject.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on visualGameObject.");
        }

        visualTransform = visualGameObject.transform;
        if (visualTransform == null)
        {
            Debug.LogError("Visual transform not found!");
        }

        playerCollider = GetComponent<Collider2D>();

        meleeAttack = new MeleeAttackFactory(attackDamage, attackRange, attackPoint, enemyLayers, attackAngle).CreateAttack(Vector2.zero);
        rangedAttack = new RangedAttackFactory(attackDamage, attackRange, attackPoint, projectilePrefab, enemyLayers, playerCollider).CreateAttack(Vector2.zero);

        SetAttack(meleeAttack);
    }

    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lookDirection = (mousePosition - attackPoint.position).normalized;

        if (lookDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            attackPoint.rotation = Quaternion.Euler(0, 0, angle);
        }

        if (Input.GetMouseButton(0)) // Left mouse button held down
        {
            TryAttack();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetAttack(meleeAttack);
            Debug.Log("Switched to melee attack");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetAttack(rangedAttack);
            Debug.Log("Switched to ranged attack");
        }
    }

    private void TryAttack()
    {
        if (Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + 1f / attackRate; // Update next attack time based on attack rate
        }
 
    }

    void Attack()
    {

        if (animator != null)
        {
            // Prioritize horizontal movement
            if (Mathf.Abs(lookDirection.x) > Mathf.Abs(lookDirection.y))
            {
                animator.SetTrigger("AttackRight");
                visualTransform.localScale = new Vector3(lookDirection.x < 0 ? -1 : 1, 1, 1);
            }
            else if (lookDirection.y > 0)
            {
                animator.SetTrigger("AttackUp");
            }
            else if (lookDirection.y < 0)
            {
                animator.SetTrigger("AttackDown");
            }
        }
        currentAttack.ExecuteAttack(lookDirection);
    }

    public void SetAttack(IAttack newAttack)
    {
        currentAttack = newAttack;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.color = Color.red;

        Vector3 leftBoundary = Quaternion.Euler(0, 0, attackAngle / 2) * attackPoint.right;
        Vector3 rightBoundary = Quaternion.Euler(0, 0, -attackAngle / 2) * attackPoint.right;

        Gizmos.DrawLine(attackPoint.position, attackPoint.position + leftBoundary * attackRange);
        Gizmos.DrawLine(attackPoint.position, attackPoint.position + rightBoundary * attackRange);
    }
}
