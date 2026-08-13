using UnityEngine;
using UnityEngine.InputSystem;

public class FlyCamera : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float normalSpeed = 40f;
    [SerializeField] private float boostedSpeed = 80f;
    [SerializeField] private float verticalSpeed = 10f;

    [Header("Mouse Settings")]
    [SerializeField] private float lookSensitivity = 0.3f;
    [SerializeField] private bool invertY = false;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool isBoosting;
    private bool isLooking;
    private Vector3 verticalMovement = Vector3.zero;
    private float xRotation = 0f;
    private float yRotation = 0f;

    private Camera cam;
    private PlayerInputActions inputActions;

    private void Awake()
    {
        Vector3 initialRotation = transform.localEulerAngles;
        xRotation = initialRotation.x;
        yRotation = initialRotation.y;
    }
    void OnEnable()
    {
        inputActions = new PlayerInputActions();
        inputActions.Enable();

        inputActions.FlyCam.Move.performed += OnMove;
        inputActions.FlyCam.Move.canceled += OnMove;

        inputActions.FlyCam.Look.performed += OnLook;
        inputActions.FlyCam.Look.canceled += OnLook;

        inputActions.FlyCam.MoveUp.performed += OnMoveUp;
        inputActions.FlyCam.MoveUp.canceled += OnMoveUp;

        inputActions.FlyCam.MoveDown.performed += OnMoveDown;
        inputActions.FlyCam.MoveDown.canceled += OnMoveDown;

        inputActions.FlyCam.Boost.performed += OnBoost;
        inputActions.FlyCam.Boost.canceled += OnBoost;

        inputActions.FlyCam.LeftClickHold.performed += OnLeftClickHold;
        inputActions.FlyCam.LeftClickHold.canceled += OnLeftClickHold;
       
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnMoveUp(InputAction.CallbackContext context)
    {
        verticalMovement = context.performed ? Vector3.up : Vector3.zero;
    }

    public void OnMoveDown(InputAction.CallbackContext context)
    {
        verticalMovement = context.performed ? Vector3.down : Vector3.zero;
    }

    public void OnBoost(InputAction.CallbackContext context)
    {
        isBoosting = context.performed;
    }

    public void OnLeftClickHold(InputAction.CallbackContext context)
    {
        isLooking = context.performed;
    }

    void Update()
    {
        if (isLooking)
        {
            LookAround();
        }
        MoveCamera();
    }

    void MoveCamera()
    {
        float speed = isBoosting ? boostedSpeed : normalSpeed;

        Vector3 forwardMovement = Vector3.Normalize(transform.forward) * moveInput.y * speed * Time.deltaTime;
        Vector3 rightMovement = Vector3.Normalize(transform.right) * moveInput.x * speed * Time.deltaTime;

        transform.position += forwardMovement + rightMovement + (verticalMovement * verticalSpeed * Time.deltaTime);
    }

    void LookAround()
    {
        float mouseX = lookInput.x * lookSensitivity;
        float mouseY = lookInput.y * lookSensitivity * (invertY ? -1 : 1);

        yRotation += mouseX;

        xRotation = Mathf.Clamp(xRotation - mouseY, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }


}