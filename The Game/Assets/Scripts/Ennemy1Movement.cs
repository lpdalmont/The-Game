using UnityEngine;

public class Ennemy1Movement : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private float rangeX;
    [SerializeField] private float rangeY;
    [SerializeField] private float sightrangeX;
    [SerializeField] private float sightRangeY;
    [SerializeField] private float colliderDistance;
    [SerializeField] private int damage;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity; 
    Animator animator;
    
    private Health playerHealth;
    
    private EnnemyPatrol ennemyPatrol;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        animator = GetComponent<Animator>();
        ennemyPatrol = GetComponentInParent<EnnemyPatrol>();
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (PlayerInSight())
        {
            ennemyPatrol.setTriggered(true);
            if (RdyTofight() && playerHealth.GetCurrentHealth() != 0)
            {
                if (cooldownTimer >= attackCooldown)
                {
                    cooldownTimer = 0;
                    animator.SetTrigger("meleeAttack");
                }
            }
        }
        else
        {   
            ennemyPatrol.setTriggered(false);
            if (RdyTofight() && playerHealth.GetCurrentHealth() != 0)
            {
                if (cooldownTimer >= attackCooldown)
                {
                    cooldownTimer = 0;
                    animator.SetTrigger("meleeAttack");
                }
            }
        }
        

        if (ennemyPatrol != null)
        {
            ennemyPatrol.enabled = !RdyTofight();
        }
        
    }

    private bool RdyTofight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * rangeX * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * rangeX, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        
        if(hit.collider != null)
            playerHealth = hit.collider.GetComponent<Health>();
        
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * rangeX * transform.localScale.x * colliderDistance, new Vector3(boxCollider.bounds.size.x * rangeX, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * sightrangeX * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * sightrangeX, boxCollider.bounds.size.y * sightRangeY, boxCollider.bounds.size.z));
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * sightrangeX * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * sightrangeX, boxCollider.bounds.size.y * sightRangeY, boxCollider.bounds.size.z), 0f, Vector2.left, 0f, playerLayer);
        return hit.collider != null;
    }

    private void DamagePlayer()
    {

            if (RdyTofight())
            {
                playerHealth.TakeDamage(damage);
            }

    }
    
}
