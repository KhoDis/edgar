using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles all player tap/click input for the scene.
/// Raycasts once per tap — items take priority over characters.
/// </summary>
public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }

    [Header("Raycast Settings")]
    [SerializeField] private float maxRaycastDistance = 100f;
    [SerializeField] private LayerMask interactionMask;

    private Camera _mainCamera;
    private InputAction _interactAction;
    private float _lastInteractionTime;
    private const float INTERACTION_COOLDOWN = 0.2f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _mainCamera = Camera.main;

        _interactAction = InputSystem.actions?.FindAction("Player/Interact");
        if (_interactAction == null)
        {
            _interactAction = new InputAction(type: InputActionType.Button);
            _interactAction.AddBinding("<Mouse>/leftButton");
            _interactAction.AddBinding("<Touchscreen>/primaryTouch/tap");
            _interactAction.AddBinding("<Touchscreen>/touch*/press");
        }

        _interactAction.performed += OnClick;
    }

    void OnEnable() => _interactAction?.Enable();

    void OnDisable() => _interactAction?.Disable();

    void OnDestroy()
    {
        if (_interactAction != null)
        {
            _interactAction.performed -= OnClick;
            _interactAction.Dispose();
        }
    }

    private void OnClick(InputAction.CallbackContext ctx)
    {
        if (Time.time - _lastInteractionTime < INTERACTION_COOLDOWN) return;
        _lastInteractionTime = Time.time;

        var hit = GetPointerHit();
        if (hit == null) return;

        var interactable = hit.Value.collider.GetComponent<IInteractable>();
        if (interactable != null && interactable.IsInteractive)
            interactable.Interact();
    }

    private RaycastHit? GetPointerHit()
    {
        if (_mainCamera == null) return null;

        Ray ray = _mainCamera.ScreenPointToRay(GetScreenPosition());
        if (Physics.Raycast(ray, out RaycastHit hit, maxRaycastDistance, interactionMask))
            return hit;

        return null;
    }

    private Vector2 GetScreenPosition()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            return Touchscreen.current.primaryTouch.position.ReadValue();

        if (Mouse.current != null)
            return Mouse.current.position.ReadValue();

        return Vector2.zero;
    }
}
