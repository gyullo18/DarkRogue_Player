using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    // 공격이 주는 피해량
    [SerializeField]
    private int damageAmount = 20;
    // 플레이어가 왼쪽 또는 오른쪽을 향하고 있는 경우 값(Flip())
    private Player player;
    // 플레이어의 Rigidbody2D
    private Rigidbody2D rb;
    // 플레이어의 AttackManager 스크립트
    private AttackManager attackManager;
    // 무기가 무언가에 닿은 후 플레이어가 가야 하는 방향
    private Vector2 direction;
    // 무기가 충돌한 후 플레이어가 움직여야 하는지를 관리
    private bool collided;
    // 공격이 아래쪽인지 결정(중력 여부)
    private bool downwardStrike;

    private void Start()
    {
        // 플레이어의 캐릭터 스크립트
        player = GetComponentInParent<Player>();
        // 플레이어의 Rigidbody2D
        rb = GetComponentInParent<Rigidbody2D>();
        // 플레이어의 MeleeAttackManager 스크립트
        attackManager = GetComponentInParent<AttackManager>();
    }

    private void FixedUpdate()
    {
        //Rigidbody2D AddForce 메서드를 사용하여 플레이어를 올바른 방향으로 이동
        HandleMovement();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // MeleeWeapon이 충돌하는 GameObject에 EnemyHealth 스크립트가 있는지 확인
        if (collision.GetComponent<EnemyHealth>())
        {
            // 근접 공격 시 플레이어에게 어떤 힘을 가할 수 있는지 확인하는 메서드
            HandleCollision(collision.GetComponent<EnemyHealth>());
        }
    }

    private void HandleCollision(EnemyHealth enemyHealth)
    {
        // GameObject가 위쪽 힘을 허용하는지 확인하고 스트라이크가 아래쪽뿐만 아니라 접지되어 있는지 확인
        if (enemyHealth.giveUpwardForce && Input.GetAxis("Vertical") < 0 && !player.isGround)
        {
            // 방향을 위로 설정
            direction = Vector2.up;
            // 방향을 아래로 설정
            downwardStrike = true;
            // 충돌을 true로 설정
            collided = true;
        }
        if (Input.GetAxis("Vertical") > 0 && !player.isGround)
        {
            // 방향을 위로 설정
            direction = Vector2.down;
            // 충돌을 true로 설정
            collided = true;
        }
        // 공격이 표준 근접 공격인지 확인
        if ((Input.GetAxis("Vertical") <= 0 && player.isGround) || Input.GetAxis("Vertical") == 0)
        {
            // 플레이어가 왼쪽을 향하고 있는지 확인합니다.
            if (transform.parent.localScale.x < 0)
            {
                // 방향 변수를 오른쪽으로 설정
                direction = Vector2.right;
            }
            else
            {
                // 방향 변수를 오른쪽에서 왼쪽으로 설정
                direction = Vector2.left;
            }
            // 충돌을 true로 설정
            collided = true;
        }
        // DamageAmount만큼 데미지
        enemyHealth.Damage(damageAmount);
        // 근접 공격 충돌 및 방향과 관련된 모든 bool을 끄는 코루틴
        StartCoroutine(NoLongerColliding());
    }

    // 근거리 공격으로부터 움직임이 있어야 하고, 근거리 공격 관리자 스크립트에서 받은 힘의 양에 따라 적절한 방향으로 힘을 가하는 방식
    private void HandleMovement()
    {
        // 게임 오브젝트가 근접 공격이 충돌할 때 플레이어가 이동할 수 있도록 해야 하는지 확인
        if (collided)
        {
            // 공격이 아래쪽 방향인 경우
            if (downwardStrike)
            {
                // meleeAttackManager 스크립트에서 upsForce만큼 플레이어를 위로 이동
                rb.AddForce(direction * attackManager.upwardsForce);
            }
            else
            {
                // meleeAttackManager 스크립트에서 horizonForce만큼 플레이어를 뒤로 이동
                rb.AddForce(direction * attackManager.horizonForce);
            }
        }
    }

    // HandleMovement 메서드에서 이동을 허용하는 모든 bool을 끄는 코루틴
    private IEnumerator NoLongerColliding()
    {
        // meleeAttackManager 스크립트에서 설정한 시간만큼 대기(0.1초)
        yield return new WaitForSeconds(attackManager.movementTime);
        // 충돌 : false
        collided = false;
        // downStrike : false
        downwardStrike = false;
    }
}
