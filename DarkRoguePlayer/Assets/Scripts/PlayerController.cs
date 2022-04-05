using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    // �����̴� �ӵ�
    [SerializeField]
    private float moveSpeed;
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

    //// �뽬
    //private I
   
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadious, whatIsGround);


        // ���� ���� �̵�
        float x = Input.GetAxis("Horizontal");
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
            rb.velocity = Vector2.up * jumpForce;
            jumps--;
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
}
