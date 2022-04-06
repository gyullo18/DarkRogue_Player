using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //public float moveSpeed = 8;
    //private float x;
    //private float y;
    //private Rigidbody2D rb;

    // 땅에 닿았는가
    //public bool isGound;
    //public Transform groundcheck;
    //public float checkradious;
    //public LayerMask whatisground;

    //// 점프 힘
    //[SerializeField]
    //private float jumpForce;
    //public int jumps;

    //// 대쉬
    //private bool isDash;
    //[SerializeField]
    //private float dashSpeed = 60f;
    //private bool canDash = true;
    //private IEnumerator dashCoroutine;
    //// 방향
    //private float dashDirection = 1;

    //// 중력제어
    //private float normalGravity;

    //// 캐릭터가 우측보는가
    //private bool faceToRight = true;

    //// 죽었는가
    //private bool isDead = false;

    //private Animator anim;

    //void Start()
    //{
    //    rb = GetComponent<Rigidbody2D>();
    //    anim = GetComponent<Animator>();
    //    normalGravity = rb.gravityScale;
    //}

    //void Update()
    //{
    //    if (isDead) return;

    //    if (x != 0)
    //    {
    //        dashDirection = x;
    //    }
    //    // 방향키 움직임
    //    x = Input.GetAxisRaw("Horizontal");
    //    // 점프()
    //    if (isGround == true)
    //    {
    //        jumps = 1;
    //    }
    //    if ( Input.GetKeyDown(KeyCode.Space) && jumps > 0)
    //    {
    //        Jump();
    //        jumps--;
    //    }

    //    // 대쉬(x)
    //    if (Input.GetKeyDown(KeyCode.X) && canDash == true)
    //    {
    //        if (dashCoroutine != null)
    //        {
    //            StopCoroutine(dashCoroutine);

    //        }
    //        dashCoroutine = Dash(0.13f, 0.6f);
    //        StartCoroutine(dashCoroutine);
    //    }
    //}

    //private void FixedUpdate()
    //{
    //    // 땅에 닿았는지 체크(플랫폼의 LayerMask를 Ground로 변경)
    //    isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadious, whatIsGround);

    //    rb.velocity = new Vector2(x * moveSpeed, rb.velocity.y);

    //    // 대쉬
    //    rb.AddForce(new Vector2(x * 20f, 0));
    //    rb.AddForce(new Vector2(0, y), ForceMode2D.Impulse);
    //    if (isDash )
    //    {
    //        rb.AddForce(new Vector2(dashDirection * dashSpeed, 0), ForceMode2D.Impulse);
    //    }

    //    // 캐릭터 좌우 이동시 바라보는 방향 바꾸기.
    //    if (faceToRight == false && x > 0)
    //    {
    //        Flip();
    //    }
    //    else if (faceToRight == true && x < 0)
    //    {
    //        Flip();
    //    }
    //}

    //// 캐릭터가 왼쪽을 바라보게끔 만드는 메서드
    //void Flip()
    //{
    //    faceToRight = !faceToRight;
    //    Vector3 Scaler = transform.localScale;
    //    Scaler.x *= -1;
    //    transform.localScale = Scaler;
    //}

    //// 점프 메서드
    //void Jump()
    //{
    //    rb.velocity = Vector2.up * jumpForce;
    //}

    //// 대쉬 코르틴
    //private IEnumerator Dash(float dashDuration, float dashCooldown)
    //{
    //    Vector2 originalVelocity = rb.velocity;
    //    isDash = true;
    //    canDash = false;
    //    rb.gravityScale = 0;
    //    rb.velocity = Vector2.zero;
    //    yield return new WaitForSeconds(dashDuration);
    //    isDash = false;
    //    rb.gravityScale = normalGravity;
    //    rb.velocity = originalVelocity;
    //    yield return new WaitForSeconds(dashCooldown);
    //    canDash = true;
    //}

    //void Die()
    //{
    //    isDead = true;
    //    //게임매니저 만들면 넣을 거
    //    // GameManager.instance.OnPlayerDead();
    //    //게임매니저 스크립트 만들면 넣을 거
    //    //public void PlayerDead()
    //    //{
    //    //    현재 상태를 게임오버 상태로 변경.
    //    //   isGameover = true;
    //    //    gameoverUI오브젝트 - 게임오버 UI 활성화
    //    //    gameoverUI.SetActive(true);
    //    //}
    //}

    private Rigidbody2D rb;
    private Collider2D collider;
    private Player player;

    public bool isFacingLeft;
    public bool isJumping;

    public bool spawnFacingLeft;
    private Vector2 facingLeft;

    // 땅에 닿았는가
    public bool isGround;
    //public Transform groundCheck;
    //public float checkRadious;
    //public LayerMask whatIsGround;



    private void Start()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        facingLeft = new Vector2(-transform.localScale.x, transform.localScale.y);
        if (spawnFacingLeft)
        {
            transform.localScale = facingLeft;
            isFacingLeft = true;
        }
    }

    //private void FixedUpdate()
    //{
    //      땅에 닿았는지 체크(플랫폼의 LayerMask를 Ground로 변경)
    //    isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadious, whatIsGround);
    //}

    public void Flip()
    {
        if (isFacingLeft)
        {
            transform.localScale = facingLeft;
        }
        if (!isFacingLeft)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }

    public bool CollisionCheck(Vector2 direction, float distance, LayerMask collision)
    {
        RaycastHit2D[] hits = new RaycastHit2D[10];
        int numHits = collider.Cast(direction, hits, distance);
        for (int i = 0; i < numHits; i++)
        {
            if ((1 << hits[i].collider.gameObject.layer & collision) != 0)
            {
                return true;
            }
        }
        return false;
    }
}
