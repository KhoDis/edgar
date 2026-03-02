using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles player input for selecting and interacting with characters.
/// Uses raycasting to detect clicks on characters in the 3D world.
/// </summary>
public class CharacterSelector : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputAction clickAction;
    
    [Header("Raycast Settings")]
    [SerializeField] private float maxRaycastDistance = 100f;
    [SerializeField] private LayerMask characterLayer = ~0; // All layers by default

    [Header("Visual Feedback")]
    [SerializeField] private bool enableHoverFeedback = true;
    [SerializeField] private Color hoverTint = new Color(1f, 1f, 1f, 0.8f);

    private Camera _mainCamera;
    private Character _hoveredCharacter;
    private SpriteRenderer _hoveredRenderer;
    private Color _originalColor;
    private bool _isMobilePlatform;
    private float _lastInteractionTime;
    private const float INTERACTION_COOLDOWN = 0.2f; // Prevent rapid multiple clicks

    void Awake()
    {
        _mainCamera = Camera.main;
        
        // Detect platform for optimization
        _isMobilePlatform = Application.isMobilePlatform ||
                           SystemInfo.deviceType == DeviceType.Handheld;
        
        // Setup cross-platform input action with multiple bindings
        if (clickAction == null)
        {
            // Try to find the existing "Attack" action from the Input System asset
            var inputActionAsset = UnityEngine.InputSystem.InputSystem.actions;
            if (inputActionAsset != null)
            {
                var attackAction = inputActionAsset.FindAction("Player/Attack");
                if (attackAction != null)
                {
                    clickAction = attackAction;
                }
                else
                {
                    CreateFallbackInputAction();
                }
            }
            else
            {
                CreateFallbackInputAction();
            }
        }
        
        clickAction.performed += OnClick;
    }
    
    void CreateFallbackInputAction()
    {
        clickAction = new InputAction(type: InputActionType.Button);
        
        // Desktop bindings
        clickAction.AddBinding("<Mouse>/leftButton");
        clickAction.AddBinding("<Keyboard>/space"); // Alternative keyboard input
        
        // Mobile/touch bindings
        clickAction.AddBinding("<Touchscreen>/primaryTouch/tap");
        clickAction.AddBinding("<Touchscreen>/touch*/press"); // Multiple touch support
    }

    void OnEnable()
    {
        clickAction?.Enable();
    }

    void OnDisable()
    {
        clickAction?.Disable();
    }

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
        if (enableHoverFeedback)
        {
            UpdateHoverFeedback();
        }
        
        // Fallback input handling for edge cases
        HandleFallbackInput();
    }
    
    /// <summary>
    /// Fallback input handling for cases where Input System might not work properly
    /// This provides redundancy and ensures input works across different platforms
    /// </summary>
    void HandleFallbackInput()
    {
        // Check for mouse click (fallback)
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleInteraction();
            return;
        }
        
        // Check for touch input (fallback)
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
    
    /// <summary>
    /// Unified interaction handler that works for both primary and fallback input
    /// </summary>
    void HandleInteraction()
    {
        Character character = GetCharacterUnderMouse();
        
        if (character != null && character.IsInteractive)
        {
            character.Interact();
        }
    }

    void UpdateHoverFeedback()
    {
        Character character = GetCharacterUnderMouse();

        // Clear previous hover
        if (_hoveredCharacter != null && _hoveredCharacter != character)
        {
            ClearHover();
        }

        // Apply new hover
        if (character != null && character.IsInteractive && character != _hoveredCharacter)
        {
            _hoveredCharacter = character;
            _hoveredRenderer = character.GetComponent<SpriteRenderer>();
            if (_hoveredRenderer != null)
            {
                _originalColor = _hoveredRenderer.color;
                _hoveredRenderer.color = hoverTint;
            }
        }
    }

    void ClearHover()
    {
        if (_hoveredRenderer != null)
        {
            _hoveredRenderer.color = _originalColor;
        }
        _hoveredCharacter = null;
        _hoveredRenderer = null;
    }

    void OnClick(InputAction.CallbackContext context)
    {
        // Prevent rapid multiple interactions
        if (Time.time - _lastInteractionTime < INTERACTION_COOLDOWN)
        {
            return;
        }
        
        _lastInteractionTime = Time.time;
        
        HandleInteraction();
    }

    Character GetCharacterUnderMouse()
    {
        if (_mainCamera == null) return null;

        Vector2 screenPosition = GetScreenPosition();
        Ray ray = _mainCamera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, maxRaycastDistance, characterLayer))
        {
            return hit.collider.GetComponent<Character>();
        }

        return null;
    }
    
    /// <summary>
    /// Gets screen position based on current input device (mouse or touch)
    /// </summary>
    Vector2 GetScreenPosition()
    {
        // Try mouse first
        if (Mouse.current != null)
        {
            return Mouse.current.position.ReadValue();
        }
        
        // Try touch input
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            return Touchscreen.current.primaryTouch.position.ReadValue();
        }
        
        // Fallback to zero
        return Vector2.zero;
    }
}

