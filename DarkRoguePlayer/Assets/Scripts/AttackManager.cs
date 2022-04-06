using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    private Player player;
    // 공격이 EnemyHealth 스크립트가 있는 게임 오브젝트와 충돌할 때
    // 플레이어가 아래쪽 또는 수평으로 얼마나 움직여야 하는지
    public float horizonForce = 300;
    // 플레이어가 위쪽으로 얼마나 움직여야 하는지
    public float upwardsForce = 600;
    // 플레이어가 이동해야 하는 시간
    public float movementTime = 0.1f;
    // 공격을 수행하는 버튼이 눌렸는지 확인하기 위한 입력 감지
    private bool attack;
    //// meleePrefab의 애니메이터
    //private Animator attackAnimator;
    //// 플레이어의 애니메이터 구성 요소
    //private Animator anim;

    //
    private void Start()
    {
        // 플레이어 애니메이터 컴포넌트
        //anim = GetComponent<Animator>();
        // Player 스크립트 -  isGround 상태를 관리
        player = GetComponent<Player>();
        // attakPrefab의 애니메이터
        //attackAnimator = GetComponentInChildren<Attack>().gameObject.GetComponent<Animator>();
    }


    private void Update()
    {
        // 어떤 키가 눌렸는지 확인하는 메소드
        AttakInput();
    }

    private void AttakInput()
    {
        // 공격으로 정의하는 A가 눌렸는지 확인
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // attack bool을 true로 설정
            attack = true;
        }
        else
        {
            //attack bool : false
            attack = false;
        }
        //// attack이 true인지 확인하고 위로 누름
        //if (attack && Input.GetAxis("Vertical") > 0)
        //{
        //    // 플레이어가 상향 근접 공격을 수행하도록 애니메이션 ON.
        //    anim.SetTrigger("UpwardAttack");
        //    // 무기의 애니메이션을 켜서 공격을 위한 스와이프 영역을 위쪽으로 표시
        //    attackAnimator.SetTrigger("UpwardAttackSwipe");
        //}
        //// attack이 true인지 확인하고 isGround되지 않은 상태에서 누르기
        //if (attack && Input.GetAxis("Vertical") < 0 && !player.isGround)
        //{
        //    // 플레이어가 하향 공격을 수행하도록 애니메이션 ON
        //    anim.SetTrigger("DownwardAttack");
        //    // 무기의 애니메이션을 켜서 공격을 위한 스와이프 영역을 아래쪽으로 표시
        //    attackAnimator.SetTrigger("DownwardAttackSwipe");
        //}
        //// attack이 true이고 어떤 방향도 누르지 않았는지 확인
        //if ((attack && Input.GetAxis("Vertical") == 0)
        //    // 또는 공격이 true이고 isGround된 상태에서 누르는 경우
        //    || attack && (Input.GetAxis("Vertical") < 0 && player.isGround))
        //{
        //    // 플레이어가 전방 공격을 수행하도록 애니메이션 ON
        //    anim.SetTrigger("ForwardAttack");
        //    // 무기의 애니메이션을 켜서 전방으로 공격을 위한 스와이프 영역을 표시
        //    attackAnimator.SetTrigger("ForwardAttackSwipe");
        //}
    }
}
