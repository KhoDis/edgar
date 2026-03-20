/// <summary>
/// Contract for any scene object that can be inspected by the player.
/// Used by ItemSelector and InspectionManager to avoid coupling to the concrete class.
/// </summary>
public interface IInspectable
{
    ItemData Data { get; }
    bool IsInteractive { get; }

    /// <summary>
    /// Called by ItemSelector when the player clicks this object.
    /// Should delegate to InspectionManager.
    /// </summary>
    void BeginInspection();
}
