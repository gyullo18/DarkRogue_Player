using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    private Player player;
    // ������ EnemyHealth ��ũ��Ʈ�� �ִ� ���� ������Ʈ�� �浹�� ��
    // �÷��̾ �Ʒ��� �Ǵ� �������� �󸶳� �������� �ϴ���
    public float horizonForce = 300;
    // �÷��̾ �������� �󸶳� �������� �ϴ���
    public float upwardsForce = 600;
    // �÷��̾ �̵��ؾ� �ϴ� �ð�
    public float movementTime = 0.1f;
    // ������ �����ϴ� ��ư�� ���ȴ��� Ȯ���ϱ� ���� �Է� ����
    private bool attack;
    //// meleePrefab�� �ִϸ�����
    //private Animator attackAnimator;
    //// �÷��̾��� �ִϸ����� ���� ���
    //private Animator anim;

    //
    private void Start()
    {
        // �÷��̾� �ִϸ����� ������Ʈ
        //anim = GetComponent<Animator>();
        // Player ��ũ��Ʈ -  isGround ���¸� ����
        player = GetComponent<Player>();
        // attakPrefab�� �ִϸ�����
        //attackAnimator = GetComponentInChildren<Attack>().gameObject.GetComponent<Animator>();
    }


    private void Update()
    {
        // � Ű�� ���ȴ��� Ȯ���ϴ� �޼ҵ�
        AttakInput();
    }

    private void AttakInput()
    {
        // �������� �����ϴ� A�� ���ȴ��� Ȯ��
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // attack bool�� true�� ����
            attack = true;
        }
        else
        {
            //attack bool : false
            attack = false;
        }
        //// attack�� true���� Ȯ���ϰ� ���� ����
        //if (attack && Input.GetAxis("Vertical") > 0)
        //{
        //    // �÷��̾ ���� ���� ������ �����ϵ��� �ִϸ��̼� ON.
        //    anim.SetTrigger("UpwardAttack");
        //    // ������ �ִϸ��̼��� �Ѽ� ������ ���� �������� ������ �������� ǥ��
        //    attackAnimator.SetTrigger("UpwardAttackSwipe");
        //}
        //// attack�� true���� Ȯ���ϰ� isGround���� ���� ���¿��� ������
        //if (attack && Input.GetAxis("Vertical") < 0 && !player.isGround)
        //{
        //    // �÷��̾ ���� ������ �����ϵ��� �ִϸ��̼� ON
        //    anim.SetTrigger("DownwardAttack");
        //    // ������ �ִϸ��̼��� �Ѽ� ������ ���� �������� ������ �Ʒ������� ǥ��
        //    attackAnimator.SetTrigger("DownwardAttackSwipe");
        //}
        //// attack�� true�̰� � ���⵵ ������ �ʾҴ��� Ȯ��
        //if ((attack && Input.GetAxis("Vertical") == 0)
        //    // �Ǵ� ������ true�̰� isGround�� ���¿��� ������ ���
        //    || attack && (Input.GetAxis("Vertical") < 0 && player.isGround))
        //{
        //    // �÷��̾ ���� ������ �����ϵ��� �ִϸ��̼� ON
        //    anim.SetTrigger("ForwardAttack");
        //    // ������ �ִϸ��̼��� �Ѽ� �������� ������ ���� �������� ������ ǥ��
        //    attackAnimator.SetTrigger("ForwardAttackSwipe");
        //}
    }
}
