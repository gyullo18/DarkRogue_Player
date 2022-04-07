using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public int health;
    public float moveSpeed;
    public float jumpSpeed;
    public int jumpLeft;
    public Vector2 climbJumpForce;
    public float fallSpeed;
    public float sprintSpeed;
    public float sprintTime;
    public float sprintInterval;
    public float attackInterval;

    public Color invulnerableColor;
    public Vector2 hurtRecoil;
    public float hurtTime;
    public float hurtRecoverTime;
    public Vector2 deathRecoil;
    public float deathDelay;

    public Vector2 attackUpRecoil;
    public Vector2 attackForwardRecoil;
    public Vector2 attackDownRecoil;

    public GameObject attackUpEffect;
    public GameObject attackForwardEffect;
    public GameObject attackDownEffect;

    private bool isGrounded;
    private bool isClimb;
    private bool isSprintable;
    private bool isSprintReset;
    private bool isInputEnabled;
    private bool isFalling;
    private bool isAttackable;

    private float climbJumpDelay = 0.2f;
    private float attackEffectLifeTime = 0.05f;

    private Animator animator;
    private Rigidbody2D rigidbody;
    private Transform transform;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    // Start is called before the first frame update
    private void Start()
    {
        isInputEnabled = true;
        isSprintReset = true;
        isAttackable = true;

        animator = gameObject.GetComponent<Animator>();
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        transform = gameObject.GetComponent<Transform>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        updatePlayerState();
        if (isInputEnabled)
        {
            move();
            jumpControl();
            fallControl();
            sprintControl();
            attackControl();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // enter climb state
        if (collision.collider.tag == "Wall" && !isGrounded)
        {
            rigidbody.gravityScale = 0;

            Vector2 newVelocity;
            newVelocity.x = 0;
            newVelocity.y = -2;

            rigidbody.velocity = newVelocity;

            isClimb = true;
            animator.SetBool("IsClimb", true);

            isSprintable = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Wall" && isFalling && !isClimb)
        {
            OnCollisionEnter2D(collision);
        }
    }

    public void hurt(int damage)
    {
        gameObject.layer = LayerMask.NameToLayer("PlayerInvulnerable");

        health = Mathf.Max(health - damage, 0);

        if (health == 0)
        {
            die();
            return;
        }

        // enter invulnerable state
        animator.SetTrigger("IsHurt");

        // stop player movement
        Vector2 newVelocity;
        newVelocity.x = 0;
        newVelocity.y = 0;
        rigidbody.velocity = newVelocity;

        // visual effect
        spriteRenderer.color = invulnerableColor;

        // death recoil
        Vector2 newForce;
        newForce.x = -transform.localScale.x * hurtRecoil.x;
        newForce.y = hurtRecoil.y;
        rigidbody.AddForce(newForce, ForceMode2D.Impulse);

        isInputEnabled = false;

        StartCoroutine(recoverFromHurtCoroutine());
    }

    private IEnumerator recoverFromHurtCoroutine()
    {
        yield return new WaitForSeconds(hurtTime);
        isInputEnabled = true;
        yield return new WaitForSeconds(hurtRecoverTime);
        spriteRenderer.color = Color.white;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // exit climb state
        if (collision.collider.tag == "Wall")
        {
            isClimb = false;
            animator.SetBool("IsClimb", false);

            rigidbody.gravityScale = 1;
        }
    }

    /* ######################################################### */

    private void updatePlayerState()
    {
        isGrounded = checkGrounded();
        animator.SetBool("IsGround", isGrounded);

        float verticalVelocity = rigidbody.velocity.y;
        animator.SetBool("IsDown", verticalVelocity < 0);

        if (isGrounded && verticalVelocity == 0)
        {
            animator.SetBool("IsJump", false);
            animator.ResetTrigger("IsJumpFirst");
            animator.ResetTrigger("IsJumpSecond");
            animator.SetBool("IsDown", false);

            jumpLeft = 2;
            isClimb = false;
            isSprintable = true;
        }
        else if (isClimb)
        {
            // one remaining jump chance after climbing
            jumpLeft = 1;
        }
    }

    private void move()
    {
        // calculate movement
        float horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed;

        // set velocity
        Vector2 newVelocity;
        newVelocity.x = horizontalMovement;
        newVelocity.y = rigidbody.velocity.y;
        rigidbody.velocity = newVelocity;

        if (!isClimb)
        {
            // the sprite itself is inversed 
            float moveDirection = -transform.localScale.x * horizontalMovement;

            if (moveDirection < 0)
            {
                // flip player sprite
                Vector3 newScale;
                newScale.x = horizontalMovement < 0 ? 1 : -1;
                newScale.y = 1;
                newScale.z = 1;

                transform.localScale = newScale;

                if (isGrounded)
                {
                    // turn back animation
                    animator.SetTrigger("IsRotate");
                }
            }
            else if (moveDirection > 0)
            {
                // move forward
                animator.SetBool("IsRun", true);
            }
        }

        // stop
        if (Input.GetAxis("Horizontal") == 0)
        {
            animator.SetTrigger("stopTrigger");
            animator.ResetTrigger("IsRotate");
            animator.SetBool("IsRun", false);
        }
        else
        {
            animator.ResetTrigger("stopTrigger");
        }
    }

    private void jumpControl()
    {
        if (!Input.GetButtonDown("Jump"))
            return;

        if (isClimb)
            climbJump();
        else if (jumpLeft > 0)
            jump();
    }

    private void fallControl()
    {
        if (Input.GetButtonUp("Jump") && !isClimb)
        {
            isFalling = true;
            fall();
        }
        else
        {
            isFalling = false;
        }
    }

    private void sprintControl()
    {
        if (Input.GetKeyDown(KeyCode.K) && isSprintable && isSprintReset)
            sprint();
    }

    private void attackControl()
    {
        if (Input.GetKeyDown(KeyCode.J) && !isClimb && isAttackable)
            attack();
    }

    private void die()
    {
        animator.SetTrigger("IsDead");

        isInputEnabled = false;

        // stop player movement
        Vector2 newVelocity;
        newVelocity.x = 0;
        newVelocity.y = 0;
        rigidbody.velocity = newVelocity;

        // visual effect
        spriteRenderer.color = invulnerableColor;

        // death recoil
        Vector2 newForce;
        newForce.x = -transform.localScale.x * deathRecoil.x;
        newForce.y = deathRecoil.y;
        rigidbody.AddForce(newForce, ForceMode2D.Impulse);

        StartCoroutine(deathCoroutine());
    }

    private IEnumerator deathCoroutine()
    {
        var material = boxCollider.sharedMaterial;
        material.bounciness = 0.3f;
        material.friction = 0.3f;
        // unity bug, need to disable and then enable to make it work
        boxCollider.enabled = false;
        boxCollider.enabled = true;

        yield return new WaitForSeconds(deathDelay);

        material.bounciness = 0;
        material.friction = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /* ######################################################### */

    private bool checkGrounded()
    {
        Vector2 origin = transform.position;

        float radius = 0.2f;

        // detect downwards
        Vector2 direction;
        direction.x = 0;
        direction.y = -1;

        float distance = 0.5f;
        LayerMask layerMask = LayerMask.GetMask("Platform");

        RaycastHit2D hitRec = Physics2D.CircleCast(origin, radius, direction, distance, layerMask);
        return hitRec.collider != null;
    }

    private void jump()
    {
        Vector2 newVelocity;
        newVelocity.x = rigidbody.velocity.x;
        newVelocity.y = jumpSpeed;

        rigidbody.velocity = newVelocity;

        animator.SetBool("IsJump", true);
        jumpLeft -= 1;
        if (jumpLeft == 0)
        {
            animator.SetTrigger("IsJumpSecond");
        }
        else if (jumpLeft == 1)
        {
            animator.SetTrigger("IsJumpFirst");
        }
    }

    private void climbJump()
    {
        Vector2 realClimbJumpForce;
        realClimbJumpForce.x = climbJumpForce.x * transform.localScale.x;
        realClimbJumpForce.y = climbJumpForce.y;
        rigidbody.AddForce(realClimbJumpForce, ForceMode2D.Impulse);

        animator.SetTrigger("IsClimbJump");
        animator.SetTrigger("IsJumpFirst");

        isInputEnabled = false;
        StartCoroutine(climbJumpCoroutine(climbJumpDelay));
    }

    private IEnumerator climbJumpCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        isInputEnabled = true;

        animator.ResetTrigger("IsClimbJump");

        // jump to the opposite direction
        Vector3 newScale;
        newScale.x = -transform.localScale.x;
        newScale.y = 1;
        newScale.z = 1;

        transform.localScale = newScale;
    }

    private void fall()
    {
        Vector2 newVelocity;
        newVelocity.x = rigidbody.velocity.x;
        newVelocity.y = fallSpeed;

        rigidbody.velocity = newVelocity;
    }

    private void sprint()
    {
        // reject input during sprinting
        isInputEnabled = false;
        isSprintable = false;
        isSprintReset = false;

        Vector2 newVelocity;
        newVelocity.x = transform.localScale.x * (isClimb ? sprintSpeed : -sprintSpeed);
        newVelocity.y = 0;

        rigidbody.velocity = newVelocity;

        if (isClimb)
        {
            // sprint to the opposite direction
            Vector3 newScale;
            newScale.x = -transform.localScale.x;
            newScale.y = 1;
            newScale.z = 1;

            transform.localScale = newScale;
        }

        animator.SetTrigger("IsSprint");
        StartCoroutine(sprintCoroutine(sprintTime, sprintInterval));
    }

    private IEnumerator sprintCoroutine(float sprintDelay, float sprintInterval)
    {
        yield return new WaitForSeconds(sprintDelay);
        isInputEnabled = true;
        isSprintable = true;

        yield return new WaitForSeconds(sprintInterval);
        isSprintReset = true;
    }

    private void attack()
    {
        float verticalDirection = Input.GetAxis("Vertical");
        if (verticalDirection > 0)
            attackUp();
        else if (verticalDirection < 0 && !isGrounded)
            attackDown();
        else
            attackForward();
    }

    private void attackUp()
    {
        animator.SetTrigger("IsAttackUp");
        attackUpEffect.SetActive(true);

        Vector2 detectDirection;
        detectDirection.x = 0;
        detectDirection.y = 1;

        StartCoroutine(attackCoroutine(attackUpEffect, attackEffectLifeTime, attackInterval, detectDirection, attackUpRecoil));
    }

    private void attackForward()
    {
        animator.SetTrigger("IsAttack");
        attackForwardEffect.SetActive(true);

        Vector2 detectDirection;
        detectDirection.x = -transform.localScale.x;
        detectDirection.y = 0;

        Vector2 recoil;
        recoil.x = transform.localScale.x > 0 ? -attackForwardRecoil.x : attackForwardRecoil.x;
        recoil.y = attackForwardRecoil.y;

        StartCoroutine(attackCoroutine(attackForwardEffect, attackEffectLifeTime, attackInterval, detectDirection, recoil));
    }

    private void attackDown()
    {
        animator.SetTrigger("IsAttackDown");
        attackDownEffect.SetActive(true);

        Vector2 detectDirection;
        detectDirection.x = 0;
        detectDirection.y = -1;

        StartCoroutine(attackCoroutine(attackDownEffect, attackEffectLifeTime, attackInterval, detectDirection, attackDownRecoil));
    }

    private IEnumerator attackCoroutine(GameObject attackEffect, float effectDelay, float attackInterval, Vector2 detectDirection, Vector2 attackRecoil)
    {
        Vector2 origin = transform.position;

        float radius = 0.6f;

        float distance = 1.5f;
        LayerMask layerMask = LayerMask.GetMask("Enemy") | LayerMask.GetMask("Trap") | LayerMask.GetMask("Switch") | LayerMask.GetMask("Projectile");

        RaycastHit2D[] hitRecList = Physics2D.CircleCastAll(origin, radius, detectDirection, distance, layerMask);

        foreach (RaycastHit2D hitRec in hitRecList)
        {
            GameObject obj = hitRec.collider.gameObject;

            string layerName = LayerMask.LayerToName(obj.layer);

            if (layerName == "Switch")
            {
                Switch swithComponent = obj.GetComponent<Switch>();
                if (swithComponent != null)
                    swithComponent.turnOn();
            }
            else if (layerName == "Enemy")
            {
                EnemyController enemyController = obj.GetComponent<EnemyController>();
                if (enemyController != null)
                    enemyController.hurt(1);
            }
            else if (layerName == "Projectile")
            {
                Destroy(obj);
            }
        }

        if (hitRecList.Length > 0)
        {
            rigidbody.velocity = attackRecoil;
        }

        yield return new WaitForSeconds(effectDelay);

        attackEffect.SetActive(false);

        // attack cool down
        isAttackable = false;
        yield return new WaitForSeconds(attackInterval);
        isAttackable = true;
    }
}
