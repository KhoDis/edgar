using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles player input for tapping inspectable items in the scene.
/// </summary>
public class ItemSelector : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private float maxRaycastDistance = 100f;
    [SerializeField] private LayerMask interactionMask;

    private Camera _mainCamera;
    private float _lastInteractionTime;
    private const float INTERACTION_COOLDOWN = 0.2f;

    void Awake()
    {
        _mainCamera = Camera.main;
    }

    void OnEnable()
    {
        var action = InputSystem.actions?.FindAction("Player/Interact");
        if (action != null)
            action.performed += OnClick;
    }

    void OnDisable()
    {
        var action = InputSystem.actions?.FindAction("Player/Interact");
        if (action != null)
            action.performed -= OnClick;
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

    private IInspectable GetItemUnderPointer()
    {
        if (_mainCamera == null) return null;

        Vector2 screenPos = GetScreenPosition();
        Ray ray = _mainCamera.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, maxRaycastDistance, interactionMask))
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
