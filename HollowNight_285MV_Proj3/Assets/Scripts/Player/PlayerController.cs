///////////////////////
// Script Contributors:
// 
// Zeb Baukhagen
// 
///////////////////////

using UnityEditor.Animations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private PlayerControls controlScheme;
    private PlayerLook playerLook;
    private Vector2 moveInput;
    private bool isGrounded;
    private bool isSprinting;
    private float vSpeed = 0f; // Vertical speed
    [SerializeField] private float speed = 2f; // Horizontal movement speed
    [SerializeField] private float sprintingSpeed = 3f; // Horizontal movement speed
    [SerializeField] private float jumpSpeed = 8f;
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private Animator animator;
    [SerializeField] private Collider swordCollider;

    private void Awake()
    {
        controlScheme = new PlayerControls();
        controller = GetComponent<CharacterController>();
        playerLook = GetComponent<PlayerLook>();
        animator = GetComponent<Animator>();

        controlScheme.Gameplay.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controlScheme.Gameplay.Move.canceled += ctx => moveInput = Vector2.zero;
        controlScheme.Gameplay.Jump.performed += ctx => AttemptJump();
        controlScheme.Gameplay.LightAttack.performed += ctx => LightAttack();
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
        isGrounded = IsReallyGrounded(0.1f);
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
        isSprinting = controlScheme.Gameplay.Sprint.IsPressed();

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 movementDirection = moveInput.y * forward + moveInput.x * right;
        movementDirection.Normalize();  // Normalize to maintain consistent speed in all directions
        Vector3 movement = (isSprinting ? sprintingSpeed : speed) * movementDirection * Time.deltaTime;
        movement.y = vSpeed * Time.deltaTime; // Apply vertical speed

        animator.SetFloat("WalkingSpeed", moveInput.magnitude * (isSprinting ? sprintingSpeed : speed));
        controller.Move(movement);
    }

    bool IsReallyGrounded(float distance)
    {
        RaycastHit hit;
        bool hasHit = Physics.Raycast(transform.position, -Vector3.up, out hit, controller.height / 2 + distance);
        return hasHit && hit.distance < controller.skinWidth + 0.1f;
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

    private void LightAttack()
    {
        print("Light attack attempted.");
        swordCollider.enabled = true;
        animator.SetTrigger("LightAttack");
        Invoke(nameof(DisableColliders), 1f);
    }

    private void DisableColliders()
    {
        swordCollider.enabled = false;
    }
}
