using UnityEngine;

/// <summary>
/// Main item component. Attach to any scene object that the detective can inspect.
/// Supports both sprite-based and mesh-based items.
/// </summary>
public class InspectableItem : MonoBehaviour, IInspectable
{
    [SerializeField] private ItemData data;

    private IItemAction[] _actions;
    private Renderer _renderer;
    private bool _isBeingInspected;

    public ItemData Data => data;
    public bool IsInteractive => data != null && data.isInteractive;

    void Start()
    {
        _actions = GetComponents<IItemAction>();
        _renderer = GetComponent<Renderer>();

        if (data != null)
        {
            ApplyItemData();
        }
        else
        {
            Debug.LogWarning($"[InspectableItem] Item data is null on {gameObject.name}");
        }
    }

    void ApplyItemData()
    {
        gameObject.name = $"Item_{data.itemId}";

        // Apply sprite if this is a sprite-based item
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && data.sprite != null)
        {
            spriteRenderer.sprite = data.sprite;
        }
    }

    /// <summary>
    /// Called by ItemSelector when the player clicks this item.
    /// </summary>
    public void BeginInspection()
    {
        if (!IsInteractive || _isBeingInspected)
            return;

        InspectionManager.Instance.OpenInspection(this);
    }

    /// <summary>
    /// Called by InspectionManager when the player exits inspection.
    /// </summary>
    public void EndInspection()
    {
        _isBeingInspected = false;
        RestoreVisuals();
    }

    /// <summary>
    /// Called by InspectionManager to fire actions at the appropriate trigger point.
    /// </summary>
    public void FireActions(InspectionTrigger trigger)
    {
        if (_actions == null || _actions.Length == 0)
            return;

        foreach (var action in _actions)
        {
            if (action != null && action.Trigger == trigger)
            {
                action.Execute(this);
            }
        }
    }

    /// <summary>
    /// Called by InspectionManager after spawning the copy — hides or highlights this item.
    /// </summary>
    public void OnCopySpawned()
    {
        _isBeingInspected = true;
        // TODO: apply highlight material or hide — wired up in Phase 3
    }

    private void RestoreVisuals()
    {
        // TODO: restore original material — wired up in Phase 3
    }

    public void SetItemData(ItemData newData)
    {
        data = newData;
        if (_renderer != null)
        {
            ApplyItemData();
        }
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (data != null && data.sprite != null)
        {
            var sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = data.sprite;
            }
        }
    }
#endif
}
