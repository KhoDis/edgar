# Billboard Character System - Quick Reference

## Component Overview

| Component                                         | Purpose                | Required             |
| ------------------------------------------------- | ---------------------- | -------------------- |
| [`Character`](Core/Character.cs)                  | Main character logic   | ✓ Yes                |
| [`BillboardSprite`](Core/BillboardSprite.cs)      | Camera-facing behavior | ✓ Yes                |
| `SpriteRenderer`                                  | Displays sprite        | ✓ Yes (auto-added)   |
| `Collider`                                        | Enables clicking       | For interactive only |
| [`ICharacterAction`](Actions/ICharacterAction.cs) | Custom behaviors       | Optional             |
| [`CharacterSelector`](Input/CharacterSelector.cs) | Input handling         | ✓ One per scene      |

## Quick Setup Checklist

### For Each Character:

- [ ] Create CharacterData asset (`Create > Characters > Character Data`)
- [ ] Create GameObject with Character + BillboardSprite components
- [ ] Assign CharacterData to Character component
- [ ] Add Collider if interactive
- [ ] Add action components if interactive
- [ ] Save as prefab

### For Scene:

- [ ] Add CharacterSelector component to scene (once)
- [ ] Place character prefabs in scene
- [ ] Test in Play Mode

## Common Patterns

### Interactive NPC

```
GameObject
├── Character (data: InteractiveCharacterData)
├── BillboardSprite
├── SpriteRenderer (auto-added)
├── BoxCollider
└── DebugLogAction (or custom actions)
```

### Decorative Character

```
GameObject
├── Character (data: DecorativeCharacterData, isInteractive=false)
├── BillboardSprite
└── SpriteRenderer (auto-added)
```

### Multiple Actions

```
GameObject
├── Character
├── BillboardSprite
├── BoxCollider
├── DebugLogAction
├── PlaySoundAction
└── CustomAction
```

## Code Snippets

### Create Character at Runtime

```csharp
GameObject character = new GameObject("NPC");
var charComponent = character.AddComponent<Character>();
charComponent.SetCharacterData(myCharacterData);
character.AddComponent<BillboardSprite>();
character.AddComponent<BoxCollider>();
```

### Custom Action Template

```csharp
using UnityEngine;

public class MyAction : MonoBehaviour, ICharacterAction
{
    [SerializeField] private string parameter;

    public void Execute(Character character)
    {
        // Your logic here
        Debug.Log($"Action on {character.Data.displayName}");
    }
}
```

### Check if Character is Interactive

```csharp
Character character = GetComponent<Character>();
if (character.IsInteractive)
{
    character.Interact();
}
```

## Inspector Settings

### BillboardSprite

- **Lock Y Axis**: ✓ (keeps character upright)

### CharacterSelector

- **Max Raycast Distance**: 100 (adjust for scene size)
- **Character Layer**: Everything (or specific layer)
- **Enable Hover Feedback**: ✓ (visual feedback)
- **Hover Tint**: (1, 1, 1, 0.8) - slightly transparent

### Character Collider Sizing

- **Box Collider**: Size to match sprite bounds
- **Center**: Adjust Y to center on sprite
- **Size**: Slightly larger than sprite for easier clicking

## File Locations

| Type           | Location                                   |
| -------------- | ------------------------------------------ |
| Scripts        | `Assets/Scripts/Characters/`               |
| Character Data | `Assets/Data/Characters/`                  |
| Prefabs        | `Assets/Prefabs/Characters/`               |
| Documentation  | `Assets/Scripts/Characters/README.md`      |
| Setup Guide    | `Assets/Scripts/Characters/SETUP_GUIDE.md` |

## Keyboard Shortcuts (Unity Editor)

- **Create CharacterData**: Right-click → Create → Characters → Character Data
- **Add Component**: Select GameObject → Add Component → Search for component name
- **Create Prefab**: Drag GameObject from Hierarchy to Project window

## Troubleshooting Quick Fixes

| Problem                | Solution                                 |
| ---------------------- | ---------------------------------------- |
| Character doesn't show | Assign sprite in CharacterData           |
| Doesn't face camera    | Add BillboardSprite component            |
| Can't click            | Add Collider + verify isInteractive=true |
| No hover effect        | Enable in CharacterSelector              |
| Action doesn't fire    | Check action component is attached       |

## Performance Tips

- Use layer masks in CharacterSelector
- Keep sprite resolutions consistent
- Use sprite atlases for many characters
- Pool characters if spawning/despawning frequently

## Extension Points

1. **New Actions**: Implement `ICharacterAction` interface
2. **Character Data**: Add properties to CharacterData class
3. **Input Methods**: Extend CharacterSelector for touch/gamepad
4. **Visual Effects**: Add to Character component
5. **Animation**: Create SpriteAnimationAction component

## Related Systems

- **Camera Navigation**: `Assets/Scripts/Camera/`
- **UI System**: `Assets/Scripts/UI/`
- **Input System**: Unity's Input System package

---

For detailed information, see:

- [README.md](README.md) - Full documentation
- [SETUP_GUIDE.md](SETUP_GUIDE.md) - Step-by-step setup
- [Architecture Plan](../../../plans/billboard-character-system.md) - System design
