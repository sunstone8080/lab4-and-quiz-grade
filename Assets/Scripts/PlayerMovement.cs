using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Camera-relative third-person movement using the new Input System.
/// Requires a CharacterController on the same GameObject.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference moveAction;    // Vector2
    [SerializeField] private InputActionReference jumpAction;    // Button
    [SerializeField] private InputActionReference sprintAction;  // Button (hold)

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float sprintSpeed = 7f;
    [SerializeField] private float acceleration = 12f;
    [SerializeField] private float rotationSpeed = 12f;

    [Header("Jump & Gravity")]
    [SerializeField] private float jumpHeight = 1.2f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpBufferTime = 0.1f;

    [Header("References")]
    [Tooltip("Movement is relative to this transform. Defaults to the main camera.")]
    [SerializeField] private Transform cameraTransform;

    private CharacterController controller;
    private Vector3 horizontalVelocity;
    private float verticalVelocity;
    private float lastGroundedTime = float.NegativeInfinity;
    private float lastJumpPressedTime = float.NegativeInfinity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
        sprintAction.action.Enable();

        jumpAction.action.performed += OnJump;
    }

    private void OnDisable()
    {
        jumpAction.action.performed -= OnJump;

        moveAction.action.Disable();
        jumpAction.action.Disable();
        sprintAction.action.Disable();
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        lastJumpPressedTime = Time.time;
    }

    private void Update()
    {
        HandleHorizontalMovement();
        HandleVerticalMovement();

        Vector3 velocity = horizontalVelocity + Vector3.up * verticalVelocity;
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleHorizontalMovement()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        bool sprinting = sprintAction.action.IsPressed();
        float targetSpeed = sprinting ? sprintSpeed : walkSpeed;

        // Camera-relative direction, flattened onto the XZ plane
        Vector3 forward = Vector3.forward;
        Vector3 right = Vector3.right;
        if (cameraTransform != null)
        {
            forward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
            right = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;
        }

        Vector3 moveDir = forward * input.y + right * input.x;
        moveDir = Vector3.ClampMagnitude(moveDir, 1f);

        // Smooth acceleration / deceleration
        Vector3 targetVelocity = moveDir * targetSpeed;
        horizontalVelocity = Vector3.MoveTowards(
            horizontalVelocity, targetVelocity, acceleration * Time.deltaTime);

        // Face movement direction
        if (moveDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }
    }

    private void HandleVerticalMovement()
    {
        if (controller.isGrounded)
        {
            lastGroundedTime = Time.time;
            if (verticalVelocity < 0f)
                verticalVelocity = -2f; // small downward push keeps us snapped to the ground
        }

        bool canJump = Time.time - lastGroundedTime <= coyoteTime;
        bool jumpBuffered = Time.time - lastJumpPressedTime <= jumpBufferTime;

        if (canJump && jumpBuffered)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            lastJumpPressedTime = float.NegativeInfinity;
            lastGroundedTime = float.NegativeInfinity;
        }

        verticalVelocity += gravity * Time.deltaTime;
    }
}
