using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // ----------- INPUT SYSTEM -----------
    private InputSystem_Actions inputActions;
    private GameManager gameManager;
    [HideInInspector] public Vector2 moveInput;
    [HideInInspector] public Vector2 zoomInput;
    public float CameraMovementSpeed = 3f;
    public float ZoomSpeed = 2f;
    public float ZoomInBound = 1f;
    public float ZoomOutBound = 10f;

    public Camera camera;


    void Awake()
    {
        inputActions = new InputSystem_Actions();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        // stops any code from executing if the game is paused
        if (Time.timeScale == 0f) return;

        HandleMovement();
        HandleZoom();
    }


    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    public void OnZoom(InputAction.CallbackContext ctx)
    {
        zoomInput = ctx.ReadValue<Vector2>();
    }

    public void OnPause(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (gameManager.IsPaused)
            gameManager.ResumeGame();
        else
            gameManager.PauseGame();
    }

    private void HandleMovement()
    {
        // the code below changes the movement so its based on the camera roation and axis and not the gloabl axis
        Vector3 forward = camera.transform.forward;
        Vector3 right = camera.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * moveInput.y + right * moveInput.x;
        camera.transform.position += move * CameraMovementSpeed * Time.unscaledDeltaTime;

    }

    private void HandleZoom()
    {
        // limits zoom in and out amount
        camera.orthographicSize += zoomInput.y * ZoomSpeed * Time.unscaledDeltaTime;
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, ZoomInBound, ZoomOutBound);
    }
}
