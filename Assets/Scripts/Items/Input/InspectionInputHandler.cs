using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles all input while inspection mode is active.
/// Disabled by default — InspectionManager enables/disables this component.
/// </summary>
public class InspectionInputHandler : MonoBehaviour
{
    [SerializeField] private InspectionManager manager;

    private InputAction _rotateAction;
    private InputAction _zoomAction;
    private InputAction _exitAction;
    private InputAction _pointerPressAction;

    void Awake()
    {
        var asset = InputSystem.actions;
        if (asset == null)
        {
            Debug.LogWarning("[InspectionInputHandler] No InputActionAsset found.");
            return;
        }

        _rotateAction = asset.FindAction("Inspection/Rotate");
        _zoomAction = asset.FindAction("Inspection/Zoom");
        _exitAction = asset.FindAction("Inspection/ExitInspect");
        _pointerPressAction = asset.FindAction("Inspection/PointerPress");

        if (_exitAction != null)
            _exitAction.performed += OnExit;
    }

    void OnEnable()
    {
        _rotateAction?.Enable();
        _zoomAction?.Enable();
        _exitAction?.Enable();
        _pointerPressAction?.Enable();
    }

    void OnDisable()
    {
        _rotateAction?.Disable();
        _zoomAction?.Disable();
        _exitAction?.Disable();
        _pointerPressAction?.Disable();
    }

    void OnDestroy()
    {
        if (_exitAction != null)
            _exitAction.performed -= OnExit;
    }

    void Update()
    {
        HandleRotation();
        HandleZoom();
    }

    private void HandleRotation()
    {
        if (_rotateAction == null || _pointerPressAction == null) return;

        // Only rotate while the pointer/finger is pressed
        if (!_pointerPressAction.IsPressed()) return;

        var delta = _rotateAction.ReadValue<Vector2>();
        if (delta.sqrMagnitude > 0.01f)
            manager.ApplyRotation(delta);
    }

    private void HandleZoom()
    {
        if (_zoomAction == null) return;

        var zoom = _zoomAction.ReadValue<float>();
        if (Mathf.Abs(zoom) > 0.01f)
            manager.ApplyZoom(zoom);

        // TODO Phase 7: pinch zoom via EnhancedTouchSupport
    }

    private void OnExit(InputAction.CallbackContext ctx)
    {
        manager.CloseInspection();
    }
}