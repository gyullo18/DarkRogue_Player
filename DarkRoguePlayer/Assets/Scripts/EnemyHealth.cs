using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // 오브젝트가 손상을 입어야 하는지 여부를 결정
    [SerializeField]
    private bool damageable = true;
    // GameObject가 가져야 하는 총 체력 포인트 수
    [SerializeField]
    private int healthAmount = 100;
    // 피해를 입은 후 적이 더 이상 피해를 입을 수 없는 최대 시간 : 동일한 근접 공격이 여러 번 피해를 입히는 것을 방지하는 데 도움
    [SerializeField]
    private float invulnerabilityTime = 0.2f;
    // 플레이어가 적 위로 하향 공격을 수행할 때 강제로 위로
    public bool giveUpwardForce = true;
    // 적이 더 많은 피해를 받을 수 있는지 관리
    private bool hit;
    // 적에게 피해를 입힌 후 현재 HP
    private int currentHealth;

    private void Start()
    {
        // 장면이 로드될 때 적을 최대 체력으로 설정
        currentHealth = healthAmount;
    }

    public void Damage(int amount)
    {
        // 플레이어가 현재 무적 상태인지 확인
        if (damageable && !hit && currentHealth > 0)
        {
            // hit을 true로 설정
            hit = true;
            //  OnTriggerEnter2D() 메서드의 이 자습서에서 이 메서드를 호출하는 스크립트에 의해 설정된 양 값만큼 currentHealthPoints를 줄임
            currentHealth -= amount;
            // wcurrentHealthPoints가 0보다 낮으면 플레이어가 죽은 상태
            if (currentHealth <= 0)
            {
                // currentHealth를 0으로 제한
                currentHealth = 0;
                // 장면에서 GameObject를 제거 : 테스트를 위해 장면에서 비활성화
                gameObject.SetActive(false);
            }
            else
            {
                // 적이 다시 피해를 입도록 실행하는 코루틴
                StartCoroutine(TurnOffHit());
            }
        }
    }

    // 적이 다시 피해를 입도록 실행하는 코루틴
    private IEnumerator TurnOffHit()
    {
        // 0.2초인 invulnerabilityTime만큼 대기
        yield return new WaitForSeconds(invulnerabilityTime);
        // 적이 다시 피해를 입을 수 있도록 hit : false로
        hit = false;
    }
}
