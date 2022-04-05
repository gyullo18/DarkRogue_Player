using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashMove : MonoBehaviour
{
    private Rigidbody2D rb;
    // 대쉬
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    private int direction;
    // Start is called before the first frame update
    void Start()
    {
        dashTime = startDashTime;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // 방향에 따른 대쉬
        if (direction == 0)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                direction = 1;
            }
            else if (Input.GetKeyDown(KeyCode.H))
            {
                direction = 2;
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                direction = 3;
            }
            else if (Input.GetKeyDown(KeyCode.G))
            {
                direction = 4;
            }
        }
        else
        {
            if (dashTime <= 0)
            {
                direction = 0;
                dashTime = startDashTime;
                rb.velocity = Vector2.zero;
            }
            else
            {
                dashTime -= Time.deltaTime;

                if (direction == 1)
                {
                    rb.velocity = Vector2.left * dashSpeed;
                }
                else if (direction == 2)
                {
                    rb.velocity = Vector2.right * dashSpeed;
                }
                else if (direction == 3)
                {
                    rb.velocity = Vector2.up * dashSpeed;
                }
                else if (direction == 4)
                {
                    rb.velocity = Vector2.down * dashSpeed;
                }
            }
        }
    }
}
