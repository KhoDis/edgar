# EDGAR - TODO

## Item Inspection System

### Phase 5 — Sprite Support
- Test with a sprite-based item (add SpriteRenderer instead of MeshRenderer)
- Verify BillboardSprite is stripped from the copy
- Verify sprite is converted to a 3D quad with correct aspect ratio
- Verify free rotation works on the quad

### Phase 6 — Actions
- Write `LogClueAction.cs` — fires on inspection open, logs clue to notebook (stub: Debug.Log for now)
- Write `RevealSecretAction.cs` — fires when item is rotated past an angle threshold, reveals hidden detail

### Phase 7 — Pinch Zoom (Mobile)
- Enable `EnhancedTouchSupport` in `InspectionInputHandler.Awake()`
- In `Update`, read `Touch.activeTouches` — when count == 2, compute pinch delta and call `manager.ApplyZoom()`
- Test on device or Unity Remote

### Phase 8 — Polish
- ~~Hover highlight on `ItemSelector`~~ — removed, mobile-first (no cursor)
- Add permanent visual cue for interactable characters and items (outline, glow, or icon above head)
  - Options: outline shader (URP has built-in), sprite overlay (exclamation mark icon), idle animation (pulse/bob)
  - Should be driven by `isInteractive` on `CharacterData` / `ItemData` — non-interactive objects get no cue
  - Lives on the `Character` / `InspectableItem` component, not on input managers
- UI fade animation on show/hide (replace SetActive with CanvasGroup alpha tween)
- Test `CharacterInputManager` + `ItemSelector` coexistence (tap characters and items in same scene)
- Test on mobile end to end (tap to open, drag to rotate, pinch to zoom, close button to exit)
