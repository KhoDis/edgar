using UnityEngine;

/// <summary>
/// Example action that logs a message when the character is interacted with.
/// Useful for testing and debugging character interactions.
/// </summary>
public class DebugLogAction : MonoBehaviour, ICharacterAction
{
    [SerializeField] private string customMessage = "";

    public void Execute(Character character)
    {
        if (character.Data == null)
        {
            Debug.LogError($"[DebugLogAction.Execute] Character data is null for {character.gameObject.name}");
            return;
        }

        if (!string.IsNullOrEmpty(customMessage))
        {
            Debug.Log($"[{character.Data.displayName}]: {customMessage}");
        }
        else
        {
            Debug.Log($"Interacted with character: {character.Data.displayName} (ID: {character.Data.characterId})");
        }
    }
}
