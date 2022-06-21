using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float gravityValue = -9.81f;

    private float speed;

    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction sprintAction;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Movement"];
        lookAction = playerInput.actions["Look"];
        sprintAction = playerInput.actions["Sprint"];
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
            
        }
        // Moves the player at a faster speed, if the sprint button is held.
        else if (sprintAction.ReadValue<float>() > .1f)
        {
            speed = playerSpeed * 4;
        }
        else
        {
            speed = playerSpeed;
        }

        Vector2 inputValues = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(inputValues.x, 0, inputValues.y);
        move = Camera.main.transform.TransformDirection(move);
        move.y = 0.0f;
        controller.Move(move * Time.deltaTime * speed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
