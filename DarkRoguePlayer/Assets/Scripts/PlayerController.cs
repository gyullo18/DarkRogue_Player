using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float x;
    // �����̴� �ӵ�
    [SerializeField]
    private float moveSpeed;
    private float defaultSpeed;
    // ĳ���Ͱ� �������°�
    private bool faceToRight = true;
    // ���� ��Ҵ°�
    private bool isGround;
    public Transform groundCheck;
    public float checkRadious;
    public LayerMask whatIsGround;
    // ���� ��
    [SerializeField]
    private float jumpForce;
    public int jumps;

    // �뽬
    private bool isDash = true;
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    private float dashDirection;
    // �뽬 ���ӽð�
    //public float defaultTime;
    //private float dashTime;
   
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dashTime = startDashTime;
    }

    private void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadious, whatIsGround);


        // ���� ���� �̵�
        x = Input.GetAxis("Horizontal");
        //float y = Input.GetAxis("Vertical");

        rb.velocity = new Vector2(x * moveSpeed, rb.velocity.y);

        if (faceToRight == false && x > 0)
        {
            Flip();
        }
        else if (faceToRight == true && x < 0)
        {
            Flip();
        }
    }

    private void Update()
    {
        // ���� ����� �� Ű���� spaceŰ�� ������ ����
        // S ��ư & �ִ� ���� Ƚ��2
        if (isGround == true)
        {
            jumps =1;
        }
        if (Input.GetKeyDown(KeyCode.Space) && jumps > 0)
        {
            Jump();
            jumps--;
        }

        // ���� ShiftŰ�� ������ �뽬
        if ( dashDirection == 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if ( x < 0 )
                {
                    dashDirection = 1;
                }
                else if ( x > 0)
                {
                    dashDirection = 2;
                }
            }
        }
        else
        {
            if ( dashTime <= 0)
            {
                dashDirection = 0;
                dashTime = startDashTime;
                rb.velocity = Vector2.zero;
            }
            else
            {
                dashTime -= Time.deltaTime;

                if (dashDirection == 1)
                {
                    rb.velocity = Vector2.left * dashSpeed;
                }
                else if (dashDirection == 2)
                {
                    rb.velocity = Vector2.right * dashSpeed;
                }
            }
        }
        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    isDash = true;
        //}
        //if ( dashTime <= 0)
        //{
        //    //defaultSpeed = moveSpeed;
        //    if ( isDash)
        //    {
        //        dashTime = defaultTime;
        //    }
        //    else
        //    {
        //        dashTime -= Time.deltaTime;
        //        defaultSpeed = dashSpeed;
        //    }
        //    isDash = false;
        //}

    }

    // ���� �޼���
    void Jump()
    {
        rb.velocity = Vector2.up * jumpForce;
    }

    // ĳ���Ͱ� ������ �ٶ󺸰Բ� ����� �޼���
    void Flip()
    {
        faceToRight = !faceToRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
}
