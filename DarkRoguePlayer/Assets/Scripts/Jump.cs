using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    private Rigidbody2D rb;
    private Player player;

    public int maxJumps;
    public float jumpForce;
    public float maxButtonHoldTime;
    public float holdForce;
    public float distanceToCollider;
    public float maxJumpSpeed;
    public float maxFallSpeed;
    public float fallSpeed;
    public float gravityMultipler;
    public LayerMask collisionLayer;

    private bool jumpPressed;
    private bool jumpHeld;
    private float buttonHoldTime;
    private float originalGravity;
    private int numberOfJumpsLeft;

    //private Animator anim;


    private void Start()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        buttonHoldTime = maxButtonHoldTime;
        originalGravity = rb.gravityScale;
        numberOfJumpsLeft = maxJumps;
        //anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpPressed = true;
        }
        else
            jumpPressed = false;
        if (Input.GetKey(KeyCode.Space))
        {
            jumpHeld = true;
        }
        else
            jumpHeld = false;
        CheckForJump();
        GroundCheck();
    }

    private void FixedUpdate()
    {
        IsJumping();
    }

    private void CheckForJump()
    {
        if (jumpPressed)
        {
            if (!player.isGround && numberOfJumpsLeft == maxJumps)
            {
                player.isJumping = false;
                return;
            }
            numberOfJumpsLeft--;
            if (numberOfJumpsLeft >= 0)
            {
                rb.gravityScale = originalGravity;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                buttonHoldTime = maxButtonHoldTime;
                player.isJumping = true;
            }
        }
    }

    private void IsJumping()
    {
        if (player.isJumping)
        {
            rb.AddForce(Vector2.up * jumpForce);
            AdditionalAir();
        }
        if (rb.velocity.y > maxJumpSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxJumpSpeed);
        }
        Falling();
    }

    private void AdditionalAir()
    {
        if (jumpHeld)
        {
            buttonHoldTime -= Time.deltaTime;
            if (buttonHoldTime <= 0)
            {
                buttonHoldTime = 0;
                player.isJumping = false;
            }
            else
                rb.AddForce(Vector2.up * holdForce);
        }
        else
        {
            player.isJumping = false;
        }
    }

    private void Falling()
    {
        if (!player.isJumping && rb.velocity.y < fallSpeed)
        {
            rb.gravityScale = gravityMultipler;
        }
        if (rb.velocity.y < maxFallSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
        }
    }

    private void GroundCheck()
    {
        if (player.CollisionCheck(Vector2.down, distanceToCollider, collisionLayer) && !player.isJumping)
        {
            player.isGround = true;
            //anim.SetBool("Grounded", true);
            numberOfJumpsLeft = maxJumps;
            rb.gravityScale = originalGravity;
        }
        else
        {
            //anim.SetFloat("yVelocity", rb.velocity.y);
            //anim.SetBool("Grounded", false);
            player.isGround = false;
        }
    }
}
