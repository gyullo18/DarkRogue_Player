using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // ������Ʈ�� �ջ��� �Ծ�� �ϴ��� ���θ� ����
    [SerializeField]
    private bool damageable = true;
    // GameObject�� ������ �ϴ� �� ü�� ����Ʈ ��
    [SerializeField]
    private int healthAmount = 100;
    // ���ظ� ���� �� ���� �� �̻� ���ظ� ���� �� ���� �ִ� �ð� : ������ ���� ������ ���� �� ���ظ� ������ ���� �����ϴ� �� ����
    [SerializeField]
    private float invulnerabilityTime = 0.2f;
    // �÷��̾ �� ���� ���� ������ ������ �� ������ ����
    public bool giveUpwardForce = true;
    // ���� �� ���� ���ظ� ���� �� �ִ��� ����
    private bool hit;
    // ������ ���ظ� ���� �� ���� HP
    private int currentHealth;

    private void Start()
    {
        // ����� �ε�� �� ���� �ִ� ü������ ����
        currentHealth = healthAmount;
    }

    public void Damage(int amount)
    {
        // �÷��̾ ���� ���� �������� Ȯ��
        if (damageable && !hit && currentHealth > 0)
        {
            // hit�� true�� ����
            hit = true;
            //  OnTriggerEnter2D() �޼����� �� �ڽ������� �� �޼��带 ȣ���ϴ� ��ũ��Ʈ�� ���� ������ �� ����ŭ currentHealthPoints�� ����
            currentHealth -= amount;
            // wcurrentHealthPoints�� 0���� ������ �÷��̾ ���� ����
            if (currentHealth <= 0)
            {
                // currentHealth�� 0���� ����
                currentHealth = 0;
                // ��鿡�� GameObject�� ���� : �׽�Ʈ�� ���� ��鿡�� ��Ȱ��ȭ
                gameObject.SetActive(false);
            }
            else
            {
                // ���� �ٽ� ���ظ� �Ե��� �����ϴ� �ڷ�ƾ
                StartCoroutine(TurnOffHit());
            }
        }
    }

    // ���� �ٽ� ���ظ� �Ե��� �����ϴ� �ڷ�ƾ
    private IEnumerator TurnOffHit()
    {
        // 0.2���� invulnerabilityTime��ŭ ���
        yield return new WaitForSeconds(invulnerabilityTime);
        // ���� �ٽ� ���ظ� ���� �� �ֵ��� hit : false��
        hit = false;
    }
}
