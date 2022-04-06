using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Player player;
    public float speed;
    public float distanceToCollider;
    public LayerMask collisionLayer;

    private float horizontalInput;

    private void Start()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame 
    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            horizontalInput = Input.GetAxis("Horizontal");
        }
        else
        {
            horizontalInput = 0;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalInput * speed * Time.deltaTime, rb.velocity.y);
        if (horizontalInput > 0 && player.isFacingLeft)
        {
            player.isFacingLeft = false;
            player.Flip();
        }
        if (horizontalInput < 0 && !player.isFacingLeft)
        {
            player.isFacingLeft = true;
            player.Flip();
        }
        SpeedModifier();
    }

    private void SpeedModifier()
    {
        if ((rb.velocity.x > 0 && player.CollisionCheck(Vector2.right, distanceToCollider, collisionLayer)) || (rb.velocity.x < 0 && player.CollisionCheck(Vector2.left, distanceToCollider, collisionLayer)))
        {
            rb.velocity = new Vector2(.01f, rb.velocity.y);
        }
    }
}
