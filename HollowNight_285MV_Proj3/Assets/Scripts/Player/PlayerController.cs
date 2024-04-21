using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private PlayerControls controlScheme;
    private PlayerLook playerLook;
    private Vector2 moveInput;
    private bool isGrounded;
    private float vSpeed = 0f; // Vertical speed
    public float speed = 5f; // Horizontal movement speed
    public float jumpSpeed = 8f;
    public float gravity = 9.8f;

    private void Awake()
    {
        controlScheme = new PlayerControls();
        controller = GetComponent<CharacterController>();
        playerLook = GetComponent<PlayerLook>();

        controlScheme.Gameplay.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controlScheme.Gameplay.Move.canceled += ctx => moveInput = Vector2.zero;
        controlScheme.Gameplay.Jump.performed += ctx => AttemptJump();
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
        isGrounded = controller.isGrounded; // You can replace this with IsReallyGrounded if needed
        Debug.DrawLine(transform.position, transform.position + Vector3.down * (controller.height / 2 + 0.1f), isGrounded ? Color.green : Color.red);
    }

    private void FixedUpdate()
    {
        ApplyGravity();
        HandleMovement();
    }

    private void LateUpdate()
    {
        playerLook.ProcessLook(controlScheme.Gameplay.Look.ReadValue<Vector2>());
    }

    private void HandleMovement()
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 movementDirection = moveInput.y * forward + moveInput.x * right;
        Vector3 movement = speed * movementDirection * Time.deltaTime;
        movement.y = vSpeed * Time.deltaTime; // Apply vertical speed

        controller.Move(movement);
    }

    private void ApplyGravity()
    {
        if (isGrounded && vSpeed < 0)
        {
            vSpeed = -2f; // Keep a small downward force when grounded to maintain solid contact
        }
        else
        {
            vSpeed -= gravity * Time.deltaTime;
        }
    }

    private void AttemptJump()
    {
        print("Attempting jump");
        if (isGrounded)
        {
            print("Jumping!");
            vSpeed = jumpSpeed; // Ensure this is only called when grounded
        }
    }
}
