using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles player input for clicking inspectable items in the scene.
/// Mirrors CharacterSelector — place one instance in the scene.
/// </summary>
public class ItemSelector : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private float maxRaycastDistance = 100f;
    [SerializeField] private LayerMask itemLayer;

    [Header("Visual Feedback")]
    [SerializeField] private bool enableHoverFeedback = true;
    [SerializeField] private Color hoverTint = new Color(1f, 1f, 0.5f, 1f);

    private Camera _mainCamera;
    private IInspectable _hoveredItem;
    private Renderer _hoveredRenderer;
    private Color _originalColor;
    private float _lastInteractionTime;
    private const float INTERACTION_COOLDOWN = 0.2f;

    void Awake()
    {
        _mainCamera = Camera.main;
    }

    void OnEnable()
    {
        var asset = InputSystem.actions;
        if (asset != null)
        {
            var attackAction = asset.FindAction("Player/Attack");
            if (attackAction != null)
                attackAction.performed += OnClick;
        }
    }

    void OnDisable()
    {
        var asset = InputSystem.actions;
        if (asset != null)
        {
            var attackAction = asset.FindAction("Player/Attack");
            if (attackAction != null)
                attackAction.performed -= OnClick;
        }
    }

    void Update()
    {
        if (enableHoverFeedback)
            UpdateHoverFeedback();
    }

    private void OnClick(InputAction.CallbackContext ctx)
    {
        if (Time.time - _lastInteractionTime < INTERACTION_COOLDOWN) return;
        if (InspectionManager.Instance != null && InspectionManager.Instance.IsInspecting) return;

        _lastInteractionTime = Time.time;

        var item = GetItemUnderPointer();
        if (item != null && item.IsInteractive)
            item.BeginInspection();
    }

    private void UpdateHoverFeedback()
    {
        if (InspectionManager.Instance != null && InspectionManager.Instance.IsInspecting)
        {
            ClearHover();
            return;
        }

        var item = GetItemUnderPointer();

        if (_hoveredItem != null && _hoveredItem != item)
            ClearHover();

        if (item != null && item.IsInteractive && item != _hoveredItem)
        {
            _hoveredItem = item;
            _hoveredRenderer = (item as MonoBehaviour)?.GetComponent<Renderer>();
            if (_hoveredRenderer != null)
            {
                _originalColor = _hoveredRenderer.material.color;
                _hoveredRenderer.material.color = hoverTint;
            }
        }
    }

    private void ClearHover()
    {
        if (_hoveredRenderer != null)
            _hoveredRenderer.material.color = _originalColor;

        _hoveredItem = null;
        _hoveredRenderer = null;
    }

    private IInspectable GetItemUnderPointer()
    {
        if (_mainCamera == null) return null;

        Vector2 screenPos = GetScreenPosition();
        Ray ray = _mainCamera.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, maxRaycastDistance, itemLayer))
            return hit.collider.GetComponent<IInspectable>();

        return null;
    }

    private Vector2 GetScreenPosition()
    {
        if (Mouse.current != null)
            return Mouse.current.position.ReadValue();

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            return Touchscreen.current.primaryTouch.position.ReadValue();

        return Vector2.zero;
    }
}
