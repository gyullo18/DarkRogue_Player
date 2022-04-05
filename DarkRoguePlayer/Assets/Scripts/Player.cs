using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 8;
    private float x;
    private float y;
    private Rigidbody2D rb;

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
    private bool isDash;
    [SerializeField]
    private float dashSpeed = 60f;
    private bool canDash = true;
    private IEnumerator dashCoroutine;
    // 방향
    float direction = 1;

    // 중력제어
    private float normalGravity;

    // 캐릭터가 우측보는가
    private bool faceToRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        normalGravity = rb.gravityScale;
    }

    void Update()
    {
        if (x != 0)
        {
            direction = x;
        }
        // 방향키 움직임
        x = Input.GetAxisRaw("Horizontal");

        // 점프(Space Bar)
        if (isGround == true)
        {
            jumps = 1;
        }
        if (Input.GetKeyDown(KeyCode.Space) && jumps > 0)
        {
            Jump();
            jumps--;
        }

        // 대쉬(좌shift)
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash == true)
        {
            if (dashCoroutine != null)
            {
                StopCoroutine(dashCoroutine);

            }
            dashCoroutine = Dash(0.1f, 0.6f);
            StartCoroutine(dashCoroutine);
        }
    }

    private void FixedUpdate()
    {
        // 땅에 닿았는지 체크(플랫폼의 LayerMask를 Ground로 변경)
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadious, whatIsGround);

        rb.velocity = new Vector2(x * moveSpeed, rb.velocity.y);

        // 대쉬
        rb.AddForce(new Vector2(x * 20f, 0));
        rb.AddForce(new Vector2(0, y), ForceMode2D.Impulse);
        if (isDash )
        {
            rb.AddForce(new Vector2(direction * dashSpeed, 0), ForceMode2D.Impulse);
        }

        // 캐릭터 좌우 이동시 바라보는 방향 바꾸기.
        if (faceToRight == false && x > 0)
        {
            Flip();
        }
        else if (faceToRight == true && x < 0)
        {
            Flip();
        }
    }

    // 캐릭터가 왼쪽을 바라보게끔 만드는 메서드
    void Flip()
    {
        faceToRight = !faceToRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    // 점프 메서드
    void Jump()
    {
        rb.velocity = Vector2.up * jumpForce;
    }

    // 대쉬 코르틴
    private IEnumerator Dash(float dashDuration, float dashCooldown)
    {
        Vector2 originalVelocity = rb.velocity;
        isDash = true;
        canDash = false;
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(dashDuration);
        isDash = false;
        rb.gravityScale = normalGravity;
        rb.velocity = originalVelocity;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
