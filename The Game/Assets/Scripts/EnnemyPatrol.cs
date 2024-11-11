using System;
using UnityEngine;

public class EnnemyPatrol : MonoBehaviour
{
    [Header ("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;
    [SerializeField] private Transform player;
    
    [Header ("Ennemy")]
    [SerializeField] private Transform ennemy;
    
    [Header ("Movement parameters")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;

    [Header("Enemy Triggered")] 
    [SerializeField] private float triggeredSpeed;
    private bool triggered;
    
    [Header("Idle Behaviour")]
    [SerializeField]private float idleDuration;
    private float idleTimer;
    
    [Header("Ennemy Animator")]
    [SerializeField] private Animator animator;
    private void Awake()
    {
        initScale = ennemy.localScale;
    }
    private void Update()
    {
        if (isTriggered())
        {
            if (ennemy.position.x < player.position.x)
            {
                MoveInDirection(1, triggeredSpeed);
            }
            else
            {
                MoveInDirection(-1, triggeredSpeed);
            };
        }
        else
        {
            if (movingLeft)
            {
                if (ennemy.position.x >= leftEdge.position.x)
                    MoveInDirection(-1, speed);
                else
                    DirectionChange();
            }
            else
            {
                if (ennemy.position.x <= rightEdge.position.x)
                    MoveInDirection(1, speed);
                else
                    DirectionChange();
            }
        }

    }

    private void OnDisable()
    {
        animator.SetBool("moving", false);
    }
    
    private void DirectionChange()
    {
        animator.SetBool("moving", false);
        idleTimer += Time.deltaTime;
        
        if (idleTimer > idleDuration)
            movingLeft = !movingLeft; 
    }
    private void MoveInDirection(int direction, float generalSpeed)
    {
        idleTimer = 0;
        animator.SetBool("moving", true);
        
        ennemy.localScale = new Vector3(Mathf.Abs(initScale.x) * direction, initScale.y, initScale.z);
        
        ennemy.position += new Vector3(Time.deltaTime * direction * generalSpeed, 0, 0);
    }
    

    public void setTriggered(bool value)
    {
        triggered = value;
    }
    private bool isTriggered()
    {
        return triggered;
    }
    
    private bool PlayerInSight(BoxCollider2D boxCollider, float sightRangeX, float sightRangeY, float colliderDistance, LayerMask playerLayer)
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * sightRangeX * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * sightRangeX, boxCollider.bounds.size.y * sightRangeY, boxCollider.bounds.size.z), 0f, Vector2.left, 0f, playerLayer);
        return hit.collider != null;
    }
    
    
}
