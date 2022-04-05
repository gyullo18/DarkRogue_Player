using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float x;
    // 움직이는 속도
    [SerializeField]
    private float moveSpeed;
    private float defaultSpeed;
    // 캐릭터가 우측보는가
    private bool faceToRight = true;
    // 땅에 닿았는가
    private bool isGround;
    public Transform groundCheck;
    public float checkRadious;
    public LayerMask whatIsGround;
    // 점프 힘
    [SerializeField]
    private float jumpForce;
    public int jumps;

    // 대쉬
    private bool isDash = true;
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    private float dashDirection;
    // 대쉬 지속시간
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


        // 가로 방향 이동
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
        // 땅에 닿았을 때 키보드 space키를 누르면 점프
        // S 버튼 & 최대 점프 횟수2
        if (isGround == true)
        {
            jumps =1;
        }
        if (Input.GetKeyDown(KeyCode.Space) && jumps > 0)
        {
            Jump();
            jumps--;
        }

        // 좌측 Shift키를 누르면 대쉬
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

    // 점프 메서드
    void Jump()
    {
        rb.velocity = Vector2.up * jumpForce;
    }

    // 캐릭터가 왼쪽을 바라보게끔 만드는 메서드
    void Flip()
    {
        faceToRight = !faceToRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
}
