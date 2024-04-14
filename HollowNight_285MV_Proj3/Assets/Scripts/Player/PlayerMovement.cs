using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerLook))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 5.0f;
    [SerializeField] private Transform cameraTransform;
    private CharacterController controller;
    private PlayerControls controlScheme;
    private PlayerLook playerLook;
    private Vector2 moveInput;

    private void Awake()
    {
        controlScheme = new PlayerControls();
        controller = GetComponent<CharacterController>();
        playerLook = GetComponent<PlayerLook>();

        controlScheme.Gameplay.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controlScheme.Gameplay.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnEnable()
    {
        controlScheme.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controlScheme.Gameplay.Disable();
    }

    void Update()
    {
        HandleMovement();
    }

    private void LateUpdate()
    {
        playerLook.ProcessLook(controlScheme.Gameplay.Look.ReadValue<Vector2>());
    }

    private void HandleMovement()
    {
        // Calculate the direction based on camera orientation
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Ignore the y component for horizontal movement
        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // Calculate the desired movement direction based on input
        Vector3 movementDirection = moveInput.y * forward + moveInput.x * right;

        // Apply speed and the time factor
        Vector3 movement = speed * Time.deltaTime * movementDirection;

        // Move the character controller
        controller.Move(movement);
    }

}
