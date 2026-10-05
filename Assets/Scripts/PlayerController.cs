using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public InputActionReference moveAction;
    public InputActionReference lookAction;
    public Transform cam;

    public float velocidade = 5f;
    public float sensibilidade = 0.1f;
    public float limitePitch = 89f;
    public float gravidade = -20f;

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
    }

    void OnEnable()
    {
        moveAction.action.Enable();
        lookAction.action.Enable();
    }

    void OnDisable()
    {
        moveAction.action.Disable();
        lookAction.action.Disable();
    }

    void Update()
    {
        mirarCamera();
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
}