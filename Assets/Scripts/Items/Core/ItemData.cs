using UnityEngine;

/// <summary>
/// ScriptableObject that stores item/clue properties.
/// Create via: Assets > Create > Items > Item Data
/// </summary>
[CreateAssetMenu(fileName = "ItemData", menuName = "Items/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("Identity")]
    [Tooltip("Unique identifier for this item")]
    public string itemId;

    [Tooltip("Display name shown to player during inspection")]
    public string displayName;

    [Tooltip("Description shown in the inspection UI")]
    [TextArea(2, 5)]
    public string description;

    [Header("Visual")]
    [Tooltip("Sprite to display — used when meshPrefab is null")]
    public Sprite sprite;

    [Tooltip("3D mesh prefab to display during inspection — leave null for sprite-based items")]
    public GameObject meshPrefab;

    [Tooltip("Scale applied to the item copy during inspection — use to normalize oddly-sized assets")]
    public Vector3 inspectionScale = Vector3.one;

    [Header("Behavior")]
    [Tooltip("Can this item be clicked and inspected?")]
    public bool isInteractive = true;

    [Header("Detective")]
    [Tooltip("Room or area where this clue was found")]
    public string roomLocation;

    [Tooltip("Mark as key evidence — highlighted in detective notebook")]
    public bool isKeyEvidence;

    [Tooltip("Note automatically added to the detective notebook on first inspection")]
    [TextArea(2, 4)]
    public string clueNote;
}
