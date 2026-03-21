using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles player input for selecting and interacting with characters.
/// Uses raycasting to detect taps on characters in the 3D world.
/// </summary>
public class CharacterInputManager : MonoBehaviour
{
    public static CharacterInputManager Instance { get; private set; }

    [Header("Input")]
    [SerializeField] private InputAction clickAction;

    [Header("Raycast Settings")]
    [SerializeField] private float maxRaycastDistance = 100f;
    [SerializeField] private LayerMask interactionMask = ~0;

    private Camera _mainCamera;
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

        if (clickAction == null)
        {
            var action = InputSystem.actions?.FindAction("Player/Interact");
            if (action != null)
                clickAction = action;
            else
                CreateFallbackInputAction();
        }

        clickAction.performed += OnClick;
    }

    void CreateFallbackInputAction()
    {
        clickAction = new InputAction(type: InputActionType.Button);
        clickAction.AddBinding("<Mouse>/leftButton");
        clickAction.AddBinding("<Touchscreen>/primaryTouch/tap");
        clickAction.AddBinding("<Touchscreen>/touch*/press");
    }

    void OnEnable() => clickAction?.Enable();

    void OnDisable() => clickAction?.Disable();

    void OnDestroy()
    {
        if (clickAction != null)
        {
            clickAction.performed -= OnClick;
            clickAction.Dispose();
        }
    }

    void Update()
    {
        // Fallback input handling for edge cases
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleInteraction();
            return;
        }

        if (Touchscreen.current != null)
        {
            foreach (var touch in Touchscreen.current.touches)
            {
                if (touch.press.wasPressedThisFrame)
                {
                    HandleInteraction();
                    return;
                }
            }
        }
    }

    void OnClick(InputAction.CallbackContext context)
    {
        if (Time.time - _lastInteractionTime < INTERACTION_COOLDOWN) return;
        _lastInteractionTime = Time.time;
        HandleInteraction();
    }

    void HandleInteraction()
    {
        Character character = GetCharacterUnderPointer();
        if (character != null && character.IsInteractive)
            character.Interact();
    }

    Character GetCharacterUnderPointer()
    {
        if (_mainCamera == null) return null;

        Vector2 screenPosition = GetScreenPosition();
        Ray ray = _mainCamera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, maxRaycastDistance, interactionMask))
            return hit.collider.GetComponent<Character>();

        return null;
    }

    Vector2 GetScreenPosition()
    {
        if (Mouse.current != null)
            return Mouse.current.position.ReadValue();

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            return Touchscreen.current.primaryTouch.position.ReadValue();

        return Vector2.zero;
    }
}
