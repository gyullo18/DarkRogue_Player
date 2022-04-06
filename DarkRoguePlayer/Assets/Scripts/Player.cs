using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //public float moveSpeed = 8;
    //private float x;
    //private float y;
    //private Rigidbody2D rb;

    // ���� ��Ҵ°�
    //public bool isGound;
    //public Transform groundcheck;
    //public float checkradious;
    //public LayerMask whatisground;

    //// ���� ��
    //[SerializeField]
    //private float jumpForce;
    //public int jumps;

    //// �뽬
    //private bool isDash;
    //[SerializeField]
    //private float dashSpeed = 60f;
    //private bool canDash = true;
    //private IEnumerator dashCoroutine;
    //// ����
    //private float dashDirection = 1;

    //// �߷�����
    //private float normalGravity;

    //// ĳ���Ͱ� �������°�
    //private bool faceToRight = true;

    //// �׾��°�
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
    //    // ����Ű ������
    //    x = Input.GetAxisRaw("Horizontal");
    //    // ����()
    //    if (isGround == true)
    //    {
    //        jumps = 1;
    //    }
    //    if ( Input.GetKeyDown(KeyCode.Space) && jumps > 0)
    //    {
    //        Jump();
    //        jumps--;
    //    }

    //    // �뽬(x)
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
    //    // ���� ��Ҵ��� üũ(�÷����� LayerMask�� Ground�� ����)
    //    isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadious, whatIsGround);

    //    rb.velocity = new Vector2(x * moveSpeed, rb.velocity.y);

    //    // �뽬
    //    rb.AddForce(new Vector2(x * 20f, 0));
    //    rb.AddForce(new Vector2(0, y), ForceMode2D.Impulse);
    //    if (isDash )
    //    {
    //        rb.AddForce(new Vector2(dashDirection * dashSpeed, 0), ForceMode2D.Impulse);
    //    }

    //    // ĳ���� �¿� �̵��� �ٶ󺸴� ���� �ٲٱ�.
    //    if (faceToRight == false && x > 0)
    //    {
    //        Flip();
    //    }
    //    else if (faceToRight == true && x < 0)
    //    {
    //        Flip();
    //    }
    //}

    //// ĳ���Ͱ� ������ �ٶ󺸰Բ� ����� �޼���
    //void Flip()
    //{
    //    faceToRight = !faceToRight;
    //    Vector3 Scaler = transform.localScale;
    //    Scaler.x *= -1;
    //    transform.localScale = Scaler;
    //}

    //// ���� �޼���
    //void Jump()
    //{
    //    rb.velocity = Vector2.up * jumpForce;
    //}

    //// �뽬 �ڸ�ƾ
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
    //    //���ӸŴ��� ����� ���� ��
    //    // GameManager.instance.OnPlayerDead();
    //    //���ӸŴ��� ��ũ��Ʈ ����� ���� ��
    //    //public void PlayerDead()
    //    //{
    //    //    ���� ���¸� ���ӿ��� ���·� ����.
    //    //   isGameover = true;
    //    //    gameoverUI������Ʈ - ���ӿ��� UI Ȱ��ȭ
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

    // ���� ��Ҵ°�
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
    //      ���� ��Ҵ��� üũ(�÷����� LayerMask�� Ground�� ����)
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
