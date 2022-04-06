using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    // ������ �ִ� ���ط�
    [SerializeField]
    private int damageAmount = 20;
    // �÷��̾ ���� �Ǵ� �������� ���ϰ� �ִ� ��� ��(Flip())
    private Player player;
    // �÷��̾��� Rigidbody2D
    private Rigidbody2D rb;
    // �÷��̾��� AttackManager ��ũ��Ʈ
    private AttackManager attackManager;
    // ���Ⱑ ���𰡿� ���� �� �÷��̾ ���� �ϴ� ����
    private Vector2 direction;
    // ���Ⱑ �浹�� �� �÷��̾ �������� �ϴ����� ����
    private bool collided;
    // ������ �Ʒ������� ����(�߷� ����)
    private bool downwardStrike;

    private void Start()
    {
        // �÷��̾��� ĳ���� ��ũ��Ʈ
        player = GetComponentInParent<Player>();
        // �÷��̾��� Rigidbody2D
        rb = GetComponentInParent<Rigidbody2D>();
        // �÷��̾��� MeleeAttackManager ��ũ��Ʈ
        attackManager = GetComponentInParent<AttackManager>();
    }

    private void FixedUpdate()
    {
        //Rigidbody2D AddForce �޼��带 ����Ͽ� �÷��̾ �ùٸ� �������� �̵�
        HandleMovement();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // MeleeWeapon�� �浹�ϴ� GameObject�� EnemyHealth ��ũ��Ʈ�� �ִ��� Ȯ��
        if (collision.GetComponent<EnemyHealth>())
        {
            // ���� ���� �� �÷��̾�� � ���� ���� �� �ִ��� Ȯ���ϴ� �޼���
            HandleCollision(collision.GetComponent<EnemyHealth>());
        }
    }

    private void HandleCollision(EnemyHealth enemyHealth)
    {
        // GameObject�� ���� ���� ����ϴ��� Ȯ���ϰ� ��Ʈ����ũ�� �Ʒ��ʻӸ� �ƴ϶� �����Ǿ� �ִ��� Ȯ��
        if (enemyHealth.giveUpwardForce && Input.GetAxis("Vertical") < 0 && !player.isGround)
        {
            // ������ ���� ����
            direction = Vector2.up;
            // ������ �Ʒ��� ����
            downwardStrike = true;
            // �浹�� true�� ����
            collided = true;
        }
        if (Input.GetAxis("Vertical") > 0 && !player.isGround)
        {
            // ������ ���� ����
            direction = Vector2.down;
            // �浹�� true�� ����
            collided = true;
        }
        // ������ ǥ�� ���� �������� Ȯ��
        if ((Input.GetAxis("Vertical") <= 0 && player.isGround) || Input.GetAxis("Vertical") == 0)
        {
            // �÷��̾ ������ ���ϰ� �ִ��� Ȯ���մϴ�.
            if (transform.parent.localScale.x < 0)
            {
                // ���� ������ ���������� ����
                direction = Vector2.right;
            }
            else
            {
                // ���� ������ �����ʿ��� �������� ����
                direction = Vector2.left;
            }
            // �浹�� true�� ����
            collided = true;
        }
        // DamageAmount��ŭ ������
        enemyHealth.Damage(damageAmount);
        // ���� ���� �浹 �� ����� ���õ� ��� bool�� ���� �ڷ�ƾ
        StartCoroutine(NoLongerColliding());
    }

    // �ٰŸ� �������κ��� �������� �־�� �ϰ�, �ٰŸ� ���� ������ ��ũ��Ʈ���� ���� ���� �翡 ���� ������ �������� ���� ���ϴ� ���
    private void HandleMovement()
    {
        // ���� ������Ʈ�� ���� ������ �浹�� �� �÷��̾ �̵��� �� �ֵ��� �ؾ� �ϴ��� Ȯ��
        if (collided)
        {
            // ������ �Ʒ��� ������ ���
            if (downwardStrike)
            {
                // meleeAttackManager ��ũ��Ʈ���� upsForce��ŭ �÷��̾ ���� �̵�
                rb.AddForce(direction * attackManager.upwardsForce);
            }
            else
            {
                // meleeAttackManager ��ũ��Ʈ���� horizonForce��ŭ �÷��̾ �ڷ� �̵�
                rb.AddForce(direction * attackManager.horizonForce);
            }
        }
    }

    // HandleMovement �޼��忡�� �̵��� ����ϴ� ��� bool�� ���� �ڷ�ƾ
    private IEnumerator NoLongerColliding()
    {
        // meleeAttackManager ��ũ��Ʈ���� ������ �ð���ŭ ���(0.1��)
        yield return new WaitForSeconds(attackManager.movementTime);
        // �浹 : false
        collided = false;
        // downStrike : false
        downwardStrike = false;
    }
}
