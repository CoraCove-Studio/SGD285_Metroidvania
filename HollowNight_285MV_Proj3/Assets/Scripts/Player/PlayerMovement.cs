using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 5.0f;
    private CharacterController controller;
    private PlayerControls controls;
    private Vector2 moveInput;

    private void Awake()
    {
        controls = new PlayerControls();
        controller = GetComponent<CharacterController>();

        controls.Gameplay.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 movement = speed * Time.deltaTime * new Vector3(moveInput.x, 0, moveInput.y);
        controller.Move(movement);
    }
}
