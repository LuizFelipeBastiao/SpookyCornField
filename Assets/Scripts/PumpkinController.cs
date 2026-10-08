using UnityEngine;

public class PumpkinController : MonoBehaviour
{

    private Vector3 ultimaPos;
    private bool isMoving = false;
    public bool hit = false;
    private float stunDuration = 5.0f;
    private float stunTimer = 0.0f;

    public Animator animator;

    private void Start()
    {
        ultimaPos = transform.position;
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

        animator.SetBool("isMoving", isMoving);
    }

    public void TakeDamage()
    {
        hit = true;
        stunTimer = stunDuration;
        animator.SetTrigger("Hit");
    }

}
