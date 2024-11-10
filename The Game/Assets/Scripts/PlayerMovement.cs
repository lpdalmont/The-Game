using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]private LayerMask groundLayer;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    private Rigidbody2D playerBody;
    private BoxCollider2D boxCollider;
    private Animator animator;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        Jump();
    }

    void Run()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        playerBody.linearVelocity = new Vector2(horizontalInput * speed, playerBody.linearVelocity.y);

        if (horizontalInput > 0.01f)
        {
            transform.localScale = Vector3.one;
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        animator.SetBool("run", horizontalInput != 0 );
        animator.SetBool("grounded", isGrounded());
    }

    void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && isGrounded())
        {
            playerBody.linearVelocity = new Vector2(playerBody.linearVelocity.x, jumpForce);
            animator.SetTrigger("jump");
        }

    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center,boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
}
