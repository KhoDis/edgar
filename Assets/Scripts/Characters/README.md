# Billboard Sprite Character System

A clean, extensible system for adding billboard sprite characters to your Unity project.

## Quick Start

### 1. Create a Character Data Asset

1. Right-click in Project window
2. Select `Create > Characters > Character Data`
3. Configure the character:
   - **Character ID**: Unique identifier (e.g., "npc_merchant")
   - **Display Name**: Name shown to player (e.g., "Merchant")
   - **Sprite**: Assign a sprite asset
   - **Is Interactive**: Check for interactive characters, uncheck for decorative

### 2. Create a Character in Scene

#### Option A: Manual Setup

1. Create empty GameObject in scene
2. Add components:
   - `Character` (required)
   - `BillboardSprite` (required for camera-facing)
   - `SpriteRenderer` (auto-added by Character)
   - `BoxCollider` or `SphereCollider` (for clicking)
3. Assign your CharacterData to the Character component
4. For interactive characters, add action components (e.g., `DebugLogAction`)

#### Option B: Use Prefabs (Recommended)

1. Drag `InteractiveCharacter.prefab` or `DecorativeCharacter.prefab` into scene
2. Assign CharacterData in Inspector
3. Configure actions as needed

### 3. Add Character Selector to Scene

1. Create empty GameObject (e.g., "CharacterInputManager")
2. Add `CharacterSelector` component
3. Configure settings:
   - **Max Raycast Distance**: How far clicks can reach
   - **Character Layer**: Layer mask for character detection
   - **Enable Hover Feedback**: Visual feedback on mouse over

## Components

### Core Components

#### [`BillboardSprite`](Core/BillboardSprite.cs)

Makes sprite always face the camera.

- **Lock Y Axis**: Keep character upright (recommended for characters)

#### [`Character`](Core/Character.cs)

Main character component that manages data and interactions.

- Automatically applies sprite from CharacterData
- Executes all attached ICharacterAction components when interacted with

#### [`CharacterData`](Core/CharacterData.cs) (ScriptableObject)

Stores character properties as reusable assets.

- Create via: `Create > Characters > Character Data`

### Action System

#### [`ICharacterAction`](Actions/ICharacterAction.cs) (Interface)

Implement this interface to create custom character behaviors.

```csharp
public class MyCustomAction : MonoBehaviour, ICharacterAction
{
    public void Execute(Character character)
    {
        // Your custom logic here
        Debug.Log($"Custom action on {character.Data.displayName}");
    }
}
```

#### [`DebugLogAction`](Actions/DebugLogAction.cs)

Example action that logs to console when character is clicked.

- Useful for testing interactions

### Input System

#### [`CharacterSelector`](Input/CharacterSelector.cs)

Handles player input for character interaction.

- Uses raycasting to detect clicks
- Provides optional hover feedback
- Works with Unity's Input System

## Examples

### Interactive Character with Multiple Actions

```csharp
// Character will execute all actions when clicked
GameObject character = new GameObject("NPC");
character.AddComponent<Character>().SetCharacterData(myCharacterData);
character.AddComponent<BillboardSprite>();
character.AddComponent<BoxCollider>();
character.AddComponent<DebugLogAction>();
// Add more actions as needed
```

### Decorative Character

```csharp
// Create CharacterData with isInteractive = false
// Character will face camera but won't respond to clicks
GameObject character = new GameObject("Background Person");
character.AddComponent<Character>().SetCharacterData(decorativeData);
character.AddComponent<BillboardSprite>();
// No collider or actions needed
```

## Future Extensions

### Dialogue System Integration

```csharp
public class DialogueAction : MonoBehaviour, ICharacterAction
{
    [SerializeField] private DialogueTree dialogueTree;

    public void Execute(Character character)
    {
        DialogueManager.Instance.StartDialogue(dialogueTree);
    }
}
```

### Story Node System

```csharp
public class StoryNodeAction : MonoBehaviour, ICharacterAction
{
    [SerializeField] private string nodeId;

    public void Execute(Character character)
    {
        StoryManager.Instance.ActivateNode(nodeId);
    }
}
```

### Sprite Animation

```csharp
public class SpriteAnimationAction : MonoBehaviour, ICharacterAction
{
    [SerializeField] private Sprite[] frames;
    [SerializeField] private float frameRate = 10f;

    public void Execute(Character character)
    {
        StartCoroutine(PlayAnimation(character));
    }

    IEnumerator PlayAnimation(Character character)
    {
        var sr = character.GetComponent<SpriteRenderer>();
        foreach (var frame in frames)
        {
            sr.sprite = frame;
            yield return new WaitForSeconds(1f / frameRate);
        }
    }
}
```

## Tips

- **Performance**: Use layer masks in CharacterSelector to avoid unnecessary raycasts
- **Organization**: Store CharacterData assets in `Assets/Data/Characters/`
- **Colliders**: Size colliders appropriately for easy clicking
- **Multiple Actions**: Add multiple action components to create complex behaviors
- **Testing**: Use DebugLogAction to verify interactions are working

## Troubleshooting

**Character doesn't face camera**

- Ensure BillboardSprite component is attached
- Check that Camera.main is set correctly

**Clicks don't register**

- Add a Collider component to the character
- Verify CharacterSelector is in the scene
- Check character layer matches CharacterSelector's layer mask
- Ensure CharacterData has isInteractive = true

**Sprite doesn't show**

- Assign a sprite in CharacterData
- Check SpriteRenderer component is present
- Verify sprite import settings (Texture Type: Sprite 2D)

## Architecture

See [`plans/billboard-character-system.md`](../../../plans/billboard-character-system.md) for detailed architecture documentation.
