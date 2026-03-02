# Billboard Character System - Setup Guide

Follow these steps to set up the billboard character system in Unity Editor.

## Step 1: Create Folder Structure

In Unity Project window:

1. Create folder: `Assets/Data`
2. Create folder: `Assets/Data/Characters`
3. Create folder: `Assets/Prefabs/Characters`

## Step 2: Create Example Character Data Assets

### Interactive Character Data

1. In Project window, navigate to `Assets/Data/Characters`
2. Right-click → `Create > Characters > Character Data`
3. Name it: `InteractiveCharacter_Example`
4. In Inspector, configure:
   - **Character ID**: `npc_example`
   - **Display Name**: `Example NPC`
   - **Sprite**: (Assign any sprite - you can use Unity's built-in sprites or create your own)
   - **Is Interactive**: ✓ (checked)

### Decorative Character Data

1. In `Assets/Data/Characters`
2. Right-click → `Create > Characters > Character Data`
3. Name it: `DecorativeCharacter_Example`
4. In Inspector, configure:
   - **Character ID**: `bg_person_01`
   - **Display Name**: `Background Person`
   - **Sprite**: (Assign any sprite)
   - **Is Interactive**: ☐ (unchecked)

## Step 3: Create Interactive Character Prefab

1. In Hierarchy, create empty GameObject: `InteractiveCharacter`
2. Add components (in this order):
   - `Character` component
   - `BillboardSprite` component
   - `Box Collider` component
   - `DebugLogAction` component
3. Configure components:
   - **Character**: Assign `InteractiveCharacter_Example` data asset
   - **BillboardSprite**: Keep default settings (Lock Y Axis = true)
   - **Box Collider**:
     - Size: X=1, Y=2, Z=0.1 (adjust based on your sprite)
     - Center: Y=1 (to center on sprite)
   - **DebugLogAction**: Leave default or add custom message
4. Drag GameObject from Hierarchy to `Assets/Prefabs/Characters/` to create prefab
5. Delete from Hierarchy (prefab is saved)

## Step 4: Create Decorative Character Prefab

1. In Hierarchy, create empty GameObject: `DecorativeCharacter`
2. Add components:
   - `Character` component
   - `BillboardSprite` component
   - (No collider or actions needed)
3. Configure components:
   - **Character**: Assign `DecorativeCharacter_Example` data asset
   - **BillboardSprite**: Keep default settings
4. Drag GameObject to `Assets/Prefabs/Characters/` to create prefab
5. Delete from Hierarchy

## Step 5: Add Character Selector to Scene

1. In Hierarchy, create empty GameObject: `CharacterInputManager`
2. Add `CharacterSelector` component
3. Configure in Inspector:
   - **Click Action**: Leave as default (will use left mouse button)
   - **Max Raycast Distance**: 100
   - **Character Layer**: Everything (or create a "Characters" layer)
   - **Enable Hover Feedback**: ✓ (checked)
   - **Hover Tint**: Slightly transparent white (R=1, G=1, B=1, A=0.8)

## Step 6: Test the System

### Add Interactive Character to Scene

1. Drag `InteractiveCharacter` prefab into scene
2. Position it in front of camera (e.g., position: 0, 1, 5)
3. Enter Play Mode
4. Click on the character
5. Check Console - should see: "Interacted with character: Example NPC (ID: npc_example)"

### Add Decorative Character to Scene

1. Drag `DecorativeCharacter` prefab into scene
2. Position it nearby (e.g., position: 2, 1, 5)
3. Enter Play Mode
4. Character should face camera but not respond to clicks

## Step 7: Create Your Own Sprites (Optional)

If you don't have sprites yet:

### Option A: Use Unity's Built-in Sprites

1. In Hierarchy, create `2D Object > Sprites > Square`
2. Copy the sprite reference from its Sprite Renderer
3. Use this sprite in your CharacterData assets

### Option B: Create Simple Test Sprites

1. Create a 256x256 image in any image editor
2. Draw a simple character shape
3. Save as PNG
4. Import to Unity (drag into Project window)
5. Select the image in Project window
6. In Inspector:
   - **Texture Type**: Sprite (2D and UI)
   - Click Apply
7. Use this sprite in your CharacterData assets

### Option C: Use Placeholder Sprites

1. Create folder: `Assets/Sprites/Characters`
2. Right-click in Project → `Create > Sprites > Square` (or any shape)
3. Assign to CharacterData assets

## Troubleshooting

### Character doesn't show in scene

- Check that CharacterData has a sprite assigned
- Verify SpriteRenderer component exists on character
- Check sprite import settings (Texture Type: Sprite 2D)

### Character doesn't face camera

- Ensure BillboardSprite component is attached
- Verify Camera.main is set (tag Main Camera on your camera)

### Clicks don't work

- Add a Collider component to interactive characters
- Verify CharacterSelector is in the scene
- Check that CharacterData has `isInteractive = true`
- Ensure character is on a layer included in CharacterSelector's layer mask

### Hover effect doesn't work

- Enable "Enable Hover Feedback" in CharacterSelector
- Verify character has a collider
- Check that character is within Max Raycast Distance

## Next Steps

Once the basic system is working:

1. **Create more character data assets** for different NPCs
2. **Create custom actions** by implementing `ICharacterAction` interface
3. **Organize characters** by creating subfolders in Data/Characters
4. **Add character layers** for better organization and performance
5. **Create character variants** by duplicating and modifying CharacterData assets

## Advanced: Creating Custom Actions

Example of a custom action:

```csharp
using UnityEngine;

public class PlaySoundAction : MonoBehaviour, ICharacterAction
{
    [SerializeField] private AudioClip soundClip;

    public void Execute(Character character)
    {
        if (soundClip != null)
        {
            AudioSource.PlayClipAtPoint(soundClip, character.transform.position);
        }
    }
}
```

To use:

1. Create this script in `Assets/Scripts/Characters/Actions/`
2. Add component to your character prefab
3. Assign an AudioClip in Inspector
4. Character will play sound when clicked

## Tips

- **Naming Convention**: Use descriptive IDs like `npc_merchant`, `bg_crowd_01`
- **Sprite Size**: Keep sprites consistent (e.g., all 256x256 or 512x512)
- **Collider Size**: Match collider to visible sprite area for better UX
- **Layer Organization**: Create a "Characters" layer for better performance
- **Prefab Variants**: Use prefab variants for character variations

## Resources

- Main Documentation: [`README.md`](README.md)
- Architecture Plan: [`../../../plans/billboard-character-system.md`](../../../plans/billboard-character-system.md)
