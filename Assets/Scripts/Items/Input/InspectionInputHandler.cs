using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

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
    private float _previousPinchDistance;

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

        EnhancedTouchSupport.Enable();
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

        EnhancedTouchSupport.Disable();
    }

    void Update()
    {
        HandleRotation();
        HandleZoom();
    }

    private void HandleRotation()
    {
        if (_rotateAction == null || _pointerPressAction == null) return;

        // Skip rotation when pinching
        if (Touch.activeTouches.Count >= 2) return;

        if (!_pointerPressAction.IsPressed()) return;

        var delta = _rotateAction.ReadValue<Vector2>();
        if (delta.sqrMagnitude > 0.01f)
            manager.ApplyRotation(delta);
    }

    private void HandleZoom()
    {
        // Pinch zoom (mobile)
        if (Touch.activeTouches.Count == 2)
        {
            var t0 = Touch.activeTouches[0];
            var t1 = Touch.activeTouches[1];

            float currentDistance = Vector2.Distance(t0.screenPosition, t1.screenPosition);

            if (t0.phase == UnityEngine.InputSystem.TouchPhase.Began ||
                t1.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                _previousPinchDistance = currentDistance;
                return;
            }

            float delta = currentDistance - _previousPinchDistance;
            _previousPinchDistance = currentDistance;

            if (Mathf.Abs(delta) > 0.01f)
                manager.ApplyZoom(delta);

            return;
        }

        // Scroll wheel zoom (desktop)
        if (_zoomAction == null) return;
        var scroll = _zoomAction.ReadValue<float>();
        if (Mathf.Abs(scroll) > 0.01f)
            manager.ApplyZoom(scroll);
    }

    private void OnExit(InputAction.CallbackContext ctx)
    {
        manager.CloseInspection();
    }
}
