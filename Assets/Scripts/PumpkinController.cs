using UnityEngine;

public class PumpkinController : MonoBehaviour
{

    private Vector3 ultimaPos;
    private bool isMoving = false;
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

        animator.SetBool("isMoving", isMoving);
    }

}
