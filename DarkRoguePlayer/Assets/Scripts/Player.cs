using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 8;
    private float x;
    private float y;
    private Rigidbody2D rb;

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
    private bool isDash;
    [SerializeField]
    private float dashSpeed = 60f;
    private bool canDash = true;
    private IEnumerator dashCoroutine;
    // ����
    float direction = 1;

    // �߷�����
    private float normalGravity;

    // ĳ���Ͱ� �������°�
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
        // ����Ű ������
        x = Input.GetAxisRaw("Horizontal");

        // ����(Space Bar)
        if (isGround == true)
        {
            jumps = 1;
        }
        if (Input.GetKeyDown(KeyCode.Space) && jumps > 0)
        {
            Jump();
            jumps--;
        }

        // �뽬(��shift)
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
        // ���� ��Ҵ��� üũ(�÷����� LayerMask�� Ground�� ����)
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadious, whatIsGround);

        rb.velocity = new Vector2(x * moveSpeed, rb.velocity.y);

        // �뽬
        rb.AddForce(new Vector2(x * 20f, 0));
        rb.AddForce(new Vector2(0, y), ForceMode2D.Impulse);
        if (isDash )
        {
            rb.AddForce(new Vector2(direction * dashSpeed, 0), ForceMode2D.Impulse);
        }

        // ĳ���� �¿� �̵��� �ٶ󺸴� ���� �ٲٱ�.
        if (faceToRight == false && x > 0)
        {
            Flip();
        }
        else if (faceToRight == true && x < 0)
        {
            Flip();
        }
    }

    // ĳ���Ͱ� ������ �ٶ󺸰Բ� ����� �޼���
    void Flip()
    {
        faceToRight = !faceToRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    // ���� �޼���
    void Jump()
    {
        rb.velocity = Vector2.up * jumpForce;
    }

    // �뽬 �ڸ�ƾ
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
