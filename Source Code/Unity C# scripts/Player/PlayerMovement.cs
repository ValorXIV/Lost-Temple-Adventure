using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    private float moveDirection = 0f;
    [SerializeField] private bool canDoubleJump = false;

    [SerializeField] private LayerMask jumpableGround;

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
    private MQTTWalk mqttwalk;
    private MQTTJump mqttjump;

    private enum MovementState {idle,running,jumping,falling}

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        mqttwalk = GetComponent<MQTTWalk>();
        mqttjump = GetComponent<MQTTJump>();
    }

    private void Update()
    {
        // Move left or right
        if (mqttwalk.receivedMovementPayload == "Left")
        {
            moveDirection = -1f; // move left if left arrow key is pressed
        }
        else if (mqttwalk.receivedMovementPayload == "Right")
        {
            moveDirection = 1f; // move right if right arrow key is pressed
        }
        else
        {
            moveDirection = 0f; // set to 0 if neither key is pressed
        }

        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

        Invoke("IsGrounded",0);

        // Jump
        if (mqttjump.receivedJumpPayload == "True") 
        {
            if (IsGrounded()) 
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                canDoubleJump = true; // set to true on regular jump
                
            }
            else if (canDoubleJump) 
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                canDoubleJump = false; // set to false after double jump
            }
        }

        UpdateAnimationState();

    }

    private void UpdateAnimationState()
    {
        MovementState state;

        // Flip sprite if moving in opposite direction
        // Return whether character is running or not
        if (moveDirection > 0)
        {
            state = MovementState.running;
            sprite.flipX = false;
            transform.GetChild(0).position = transform.position + new Vector3(1f,-.55f, 0); // move child back to default orientation
        }
        else if (moveDirection < 0)
        {
            state = MovementState.running;
            sprite.flipX = true;
            transform.GetChild(0).position = transform.position + new Vector3(-1f,-.55f, 0); // move child to match parent's orientation
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        // Update animator
        anim.SetInteger("state",(int)state);

    }

    private bool IsGrounded()   
    {
        RaycastHit2D hit = Physics2D.BoxCast(coll.bounds.center,coll.bounds.size,0f,Vector2.down,.1f,jumpableGround);
        if (hit.collider != null) 
        {   
            canDoubleJump = true; // reset double jump if grounded again
            return true;
        }
        return false;
    }
}