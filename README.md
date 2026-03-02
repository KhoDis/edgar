# EDGAR Project

Unity project with camera navigation system and billboard sprite characters.

## Features

### Camera Navigation System

- Point-and-click navigation between camera positions
- Cinemachine-based smooth transitions
- UI buttons for navigation between connected points

**Location**: `Assets/Scripts/Camera/`

### Billboard Sprite Character System

- Camera-facing sprite characters
- Interactive and decorative character types
- Extensible action system for future features (dialogue, animations, story nodes)
- Click-based interaction

**Location**: `Assets/Scripts/Characters/`
**Documentation**: See [`Assets/Scripts/Characters/README.md`](Assets/Scripts/Characters/README.md)

## Quick Start

### Camera Navigation

1. Create camera points with `CameraPoint` and `CameraPointMarker` components
2. Add `CinemachineCamera` to each point
3. Configure neighbors in CameraPoint data
4. Add `CameraNavigationSystem` to scene
5. Add `NavigationUI` for button-based navigation

### Billboard Characters

1. Create CharacterData asset: `Create > Characters > Character Data`
2. Add Character prefab to scene
3. Assign CharacterData
4. Add `CharacterSelector` to scene for interaction

## Project Structure

```
Assets/
├── Scripts/
│   ├── Camera/           # Camera navigation system
│   ├── Characters/       # Billboard character system
│   └── UI/              # UI components
├── Prefabs/
│   ├── CameraPoint_*.prefab
│   └── Characters/      # Character prefabs
├── Scenes/
│   └── SampleScene.unity
└── Settings/            # URP settings
```

## Requirements

- Unity 6000.3.9f1 or later
- Universal Render Pipeline (URP)
- Cinemachine 3.1.6
- Input System 1.18.0

## Documentation

- [Billboard Character System](Assets/Scripts/Characters/README.md)
- [Architecture Plan](plans/billboard-character-system.md)
