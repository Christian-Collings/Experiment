using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 0.8f;

    private float speed;

    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction sprintAction;
    private InputAction shootAction;

    public CinemachineVirtualCamera aimCamera;
    public Animator animator;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;
        moveAction = playerInput.actions["Movement"];
        lookAction = playerInput.actions["Look"];
        sprintAction = playerInput.actions["Sprint"];
        shootAction = playerInput.actions["Shoot"];
    }

    void Update()
    {
        Movement();
    }

    /// <summary>
    /// Moves the player, based on received Vector 2 input.
    /// </summary>
    void Movement()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        if (moveAction.ReadValue<Vector2>() == Vector2.zero)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
        // Moves the player at a faster speed, if the sprint button is held.
        else if (sprintAction.ReadValue<float>() > .1f)
        {
            speed = playerSpeed * 4;

            animator.SetBool("isRunning", true);
        }
        else
        {
            speed = playerSpeed;

            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);
        }

        Vector2 inputValues = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(inputValues.x, 0, inputValues.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0.0f;
        controller.Move(move * Time.deltaTime * speed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if(aimCamera.Priority >= 10)
        {
            float targetAngle = cameraTransform.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
