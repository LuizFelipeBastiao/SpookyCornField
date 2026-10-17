using UnityEngine;

public class PumpkinController : MonoBehaviour
{

    private Vector3 ultimaPos;
    private bool isMoving = false;
    public bool hit = false;
    public bool attacking = false;
    private float stunDuration = 5.0f;
    private float attackDuration = 1.1f;
    private float stunTimer = 0.0f;
    private float attackTimer = 0.0f;
    public float attackWindupTime = 0.85f;
    private bool hitboxActivated = false;

    public Animator animator;

    public Transform playerTransform;
    public GameObject attackHitbox;

    private void Start()
    {
        ultimaPos = transform.position;
        attackHitbox.SetActive(false);    
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) < 5.0f && !hit)
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {
        if (transform.position != ultimaPos)
        {
            isMoving = true;
            ultimaPos = transform.position;
        }
        else
        {
            isMoving = false;
        }

        if (hit)
        {
            if (stunTimer <= 0.0f)
            {
                hit = false;
            }
            stunTimer -= Time.fixedDeltaTime;
        }

        if (attacking)
        {
            attackTimer -= Time.fixedDeltaTime;

            if (!hitboxActivated && attackTimer <= (attackDuration - attackWindupTime))
            {
                attackHitbox.SetActive(true);
                hitboxActivated = true;
            }

            if (attackTimer <= 0.0f)
            {
                attacking = false;
                hitboxActivated = false;
                attackHitbox.SetActive(false);
            }
        }

        animator.SetBool("isMoving", isMoving);
    }

    public void TakeDamage()
    {
        hit = true;
        stunTimer = stunDuration;
        animator.SetTrigger("Hit");
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");
        attacking = true;
        hit = true;
        stunTimer = attackDuration;
        attackTimer = attackDuration;
    }

}
