/// <summary>
/// Interface for all item actions.
/// Implement this to create custom behaviors when an item is inspected.
/// </summary>
public interface IItemAction
{
    /// <summary>The point in the inspection lifecycle when this action fires.</summary>
    InspectionTrigger Trigger { get; }

    /// <summary>
    /// Execute this action on the given item.
    /// </summary>
    /// <param name="item">The item being inspected</param>
    void Execute(InspectableItem item);
}

/// <summary>
/// When during inspection this action should fire.
/// </summary>
public enum InspectionTrigger
{
    /// <summary>Fires immediately when inspection opens.</summary>
    OnOpen,

    /// <summary>Fires when the player rotates the item past a defined angle threshold.</summary>
    OnAngle,

    /// <summary>Fires when inspection closes.</summary>
    OnClose
}
