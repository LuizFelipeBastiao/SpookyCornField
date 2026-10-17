using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public InputActionReference moveAction;
    public InputActionReference lookAction;
    public InputActionReference shootAction;
    public AudioClip shootSound;

    public Transform cam;
    public Animator animatorArma;   

    public float velocidade = 5f;
    public float sensibilidade = 0.1f;
    public float limitePitch = 89f;
    public float gravidade = -20f;

    public int ammo;

    public float alcanceTiro = 28f;
    public float raioDaMira = 1.5f;

    CharacterController controller;
    float yaw;
    float pitch;
    float velocidadeY;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        yaw = transform.eulerAngles.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ammo = 0;
    }

    void OnEnable()
    {
        moveAction.action.Enable();
        lookAction.action.Enable();
        shootAction.action.Enable();
    }

    void OnDisable()
    {
        moveAction.action.Disable();
        lookAction.action.Disable();
        shootAction.action.Disable();
    }

    void Update()
    {
        mirarCamera();
        atirar();
        moverPlayer();
    }

    private void mirarCamera()
    {
        Vector2 look = lookAction.action.ReadValue<Vector2>();

        yaw += look.x * sensibilidade;
        pitch -= look.y * sensibilidade;
        pitch = Mathf.Clamp(pitch, -limitePitch, limitePitch);

        transform.rotation = Quaternion.Euler(0f, yaw, 0f);      // corpo: só yaw
        cam.localRotation = Quaternion.Euler(pitch, 0f, 0f);     // câmera: só pitch
    }

    private void moverPlayer()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        Vector3 move = transform.right * input.x + transform.forward * input.y;
        move = Vector3.ClampMagnitude(move, 1f) * velocidade;

        if (controller.isGrounded && velocidadeY < 0f) velocidadeY = -2f;
        velocidadeY += gravidade * Time.deltaTime;
        move.y = velocidadeY;

        controller.Move(move * Time.deltaTime);
    }

    // NOVO: só deixa atirar quando a arma está no estado "Parado"
    private bool podeAtirar()
    {
        if (animatorArma == null) return true;   // sem animator, atira normalmente

        var estado = animatorArma.GetCurrentAnimatorStateInfo(0);
        return estado.IsName("Parado") && !animatorArma.IsInTransition(0);
    }

    private void atirar()
    {
        if (shootAction.action.triggered && ammo > 0 && podeAtirar())   
        {
            ammo -= 1;

            GameManager.Instance.ChangeAmmo();

            AudioSource.PlayClipAtPoint(shootSound, transform.position);

            if (animatorArma != null)
                animatorArma.SetTrigger("Atirar");

            RaycastHit hit;
            if (Physics.SphereCast(cam.position, raioDaMira, cam.forward, out hit, alcanceTiro))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    hit.collider.GetComponent<PumpkinController>().TakeDamage();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ammo"))
        {
            ammo += 1;
            Destroy(other.gameObject);
            GameManager.Instance.ChangeAmmo();
        }

        if (other.CompareTag("Finish"))
        {
            GameManager.Instance.WinGame();
        }

        if (other.CompareTag("EnemyAttack"))
        {
            GameManager.Instance.LoseGame();
        }
    }
}