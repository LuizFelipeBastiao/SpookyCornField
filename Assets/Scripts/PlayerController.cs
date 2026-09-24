using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{


    public InputActionReference moveAction;
    public float velocidade = 25f;

    void OnEnable()
    {
        moveAction.action.Enable();
    }

    void OnDisable()
    {
        moveAction.action.Disable();
    }

    void Update()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();

        Vector3 movement = new Vector3(input.x, 0, input.y);

        transform.Translate(movement * velocidade * Time.deltaTime);
    }
}
