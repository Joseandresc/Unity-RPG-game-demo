using System;
using System.Runtime;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class Player : Entity
{
    //Allows the editor to be able to access a private variable
    [Header("Movement")]
    [SerializeField] private float xInput;
    [SerializeField] private float jumpForce = 2f;
    [SerializeField] private float MoveSpeed = 7f;
 

    [Header("Dashes")]
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashTime;// Distance to dash
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashCooldownTimer;
    private bool isDashing;

    [Header("Attack")]
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask enemyLayer; // Layer mask for enemy detection
    [SerializeField] private float attackDamage;
    [SerializeField] private float comboTime = 1f;
    [SerializeField] private float comboTimeWindow;
    private int comboCounter;
    private bool isAttacking;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        Movement();
        CheckInput();
        AnimatorControllers();
        FlipController();
        CollisionCheck();
        dashTime -= Time.deltaTime; // Decrease dash time over time
        dashCooldownTimer -= Time.deltaTime; // Decrease cooldown timer
        comboTimeWindow -= Time.deltaTime; // Decrease combo counter over time
        
    }

    public void AttackOver()
    {
        isAttacking = false; // Reset attack flag
        comboCounter++;
        if (comboCounter > 2)
        {
            comboCounter = 0; // Reset combo counter if it exceeds 2
        }
        
    }


    private void CheckInput()
    {
        xInput = UnityEngine.Input.GetAxis("Horizontal");

        // Check for jump input
        if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.LeftShift)) // Check for dash input

        {
            DashAbility();
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse0)) // Check for attack input
        {
            StartAttack();
        }


    }

    private void StartAttack()
    {
        if(!isGrounded) return; // Prevent attack if not grounded   
        if (comboTimeWindow < 0)
        {
            comboCounter = 0;
        }
        isAttacking = true;
        comboTimeWindow = comboTime; // Reset combo time counter
    }


    private void DashAbility()
    {
        if (dashCooldownTimer < 0 && !isAttacking)
        {
            dashCooldownTimer = dashCooldown; // Reset cooldown timer
            dashTime = dashDuration; // Reset dash time
        }
    }

    private void Movement()
    {
        if (isAttacking)
        {
            rb2d.linearVelocity = new Vector2(0, 0);
        }
        else if (dashTime > 0)
        {
            //Multiplying by facing direction ensures that we can dash without moving
            //As the facing direction is only 1 or -1, multiplyiong the speed by facing dir ensures it goes to correct side of X
            rb2d.linearVelocity = new Vector2(facingDirection * dashSpeed, 0);
        }
        else
        {
            rb2d.linearVelocity = new Vector2(xInput * MoveSpeed, rb2d.linearVelocity.y);
        }
        }

    private void Jump()
    {
        if (!isGrounded) return; // Prevent jumping if not grounded
        rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, jumpForce * MoveSpeed);
        rb2d.gravityScale = 3f; // Set gravity scale to 1 for normal jump
    }

    private void AnimatorControllers()
    {
        anim.SetFloat("yVelocity", rb2d.linearVelocityY);
        anim.SetBool("isMoving", xInput != 0);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isDashing", dashTime >0);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetInteger("comboCounter", comboCounter); // Set combo counter in animator


    }

    // Method to flip the character's facing direction
    
    // Method to check if the character needs to be flipped based on movement velocity
    private void FlipController()
    {
        // Check if the character is moving right (positive velocity) but is currently facing left
        if (rb2d.linearVelocity.x > 0 && !facingRight)
        {
            // If the character is moving right and facing left, flip the character to face right
            FlipCharacter();
        }
        // Check if the character is moving left (negative velocity) but is currently facing right
        else if (rb2d.linearVelocity.x < 0 && facingRight)
        {
            // If the character is moving left and facing right, flip the character to face left
            FlipCharacter();
        }
    }
  
}
