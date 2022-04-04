using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private Rigidbody2D rb;
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
    


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadious, whatIsGround);


        // 가로 방향 이동
        float x = Input.GetAxis("Horizontal");
        //float y = Input.GetAxis("Vertical");

        rb.velocity = new Vector2(x * speed, rb.velocity.y);

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
        // 땅에 닿았을 때는 점프 + 키보드 s키를 누르면 점프
        // S 버튼 & 최대 점프 횟수2
        if (isGround == true)
        {
            jumps = 2;
        }

        if (Input.GetKeyDown(KeyCode.S) && jumps > 0)
        {
            rb.velocity = Vector2.up * jumpForce;
            jumps--;
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
}
