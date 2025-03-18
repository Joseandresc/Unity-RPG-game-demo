using UnityEngine;

public class Entity : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb2d;
    [Header("Collision info")]
    protected bool isGrounded;
    [SerializeField]protected float groundCheckDistance;
    [SerializeField]protected LayerMask groundLayer; // Layer mask for ground detection
    [SerializeField] protected Transform groundCheck;
    [Header("Flip controls")]
    protected int facingDirection = 1; // 1 for right, -1 for left    
    protected bool facingRight = true; // Start facing right  






    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
   protected virtual void Update()
    {
        CollisionCheck();
        Debug.Log("Grounded: " + groundCheck.position);
    }

    protected virtual void CollisionCheck()
    {
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
    }
    protected virtual void FlipCharacter()
    {
        // Flip the facing direction by multiplying the current direction by -1
        facingDirection *= -1; // If facing right (1), change to -1 (left), and vice versa.

        // Toggle the boolean flag indicating the character is facing right
        facingRight = !facingRight; // If currently facing right, it will set to false (facing left), and vice versa.

        // Rotate the character 180 degrees on the Y-axis to visually flip the character
        // transform.Rotate(0, 180, 0); // This rotates the character's sprite/model to the opposite direction (left or right).
        transform.localScale = new Vector3(facingDirection * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
    }
   
}
