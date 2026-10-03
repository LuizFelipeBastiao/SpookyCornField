using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{


    public InputActionReference moveAction;
    public InputActionReference lookAction;

    public float velocidade = 25f;
    public float sensibilidade = 0.1f;
    public float limitePitch = 89f;

    float yaw;
    float pitch;

    void Start()
    {
        // Começa com a rotação atual da câmera
        Vector3 euler = transform.eulerAngles;
        yaw = euler.y;
        pitch = euler.x;

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

    private void moverPlayer()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        Vector3 movement = new Vector3(input.x, 0, input.y);

        Vector3 deslocamento = Quaternion.Euler(0, yaw, 0) * movement * velocidade * Time.deltaTime;

        Vector3 pos = transform.position + deslocamento;
        pos.y = transform.position.y; // mantém a altura
        transform.position = pos;
    }

    private void mirarCamera()
    {
        Vector2 look = lookAction.action.ReadValue<Vector2>();

        yaw += look.x * sensibilidade;
        pitch -= look.y * sensibilidade;
        pitch = Mathf.Clamp(pitch, -limitePitch, limitePitch);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        
    }
}
