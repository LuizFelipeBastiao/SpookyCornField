using UnityEngine;

public class PumpkinController : MonoBehaviour
{

    public float velocidade = 5f;
    private bool isMoving = false;
    public Animator animator;
    private void FixedUpdate()
    {
        andar();
        animator.SetBool("isMoving", isMoving);
    }

    private void andar()
    {
        Vector3 dir = new Vector3(0, 0, velocidade);
        transform.Translate(dir, Space.Self);
        isMoving = true;
        
    }
}
