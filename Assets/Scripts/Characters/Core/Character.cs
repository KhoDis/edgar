using UnityEngine;

/// <summary>
/// Main character component that manages character data and interactions.
/// Coordinates between visual representation, data, and actions.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class Character : MonoBehaviour, IInteractable
{
    [SerializeField] private CharacterData data;
    
    private SpriteRenderer _spriteRenderer;
    private ICharacterAction[] _actions;

    public CharacterData Data => data;
    public bool IsInteractive => data != null && data.isInteractive;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Cache all action components
        _actions = GetComponents<ICharacterAction>();
        
        // Apply character data
        if (data != null)
        {
            ApplyCharacterData();
        }
        else
        {
            Debug.LogWarning($"[Character.Start] Character data is null for {gameObject.name}");
        }
    }

    void ApplyCharacterData()
    {
        if (_spriteRenderer != null && data.sprite != null)
        {
            _spriteRenderer.sprite = data.sprite;
        }
        
        gameObject.name = $"Character_{data.characterId}";
    }

    /// <summary>
    /// Called when the character is interacted with (e.g., clicked).
    /// Executes all attached actions.
    /// </summary>
    public void Interact()
    {
        if (!IsInteractive)
        {
            return;
        }

        if (_actions == null || _actions.Length == 0)
        {
            Debug.LogWarning($"[Character.Interact] No actions found on character {gameObject.name}");
            // Try to refresh actions array
            _actions = GetComponents<ICharacterAction>();
        }

        foreach (var action in _actions)
        {
            if (action != null)
            {
                action.Execute(this);
            }
            else
            {
                Debug.LogWarning($"[Character.Interact] Null action found in actions array");
            }
        }
    }

    /// <summary>
    /// Update character data at runtime.
    /// </summary>
    public void SetCharacterData(CharacterData newData)
    {
        data = newData;
        if (_spriteRenderer != null)
        {
            ApplyCharacterData();
        }
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        // Update sprite in editor when data changes
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
