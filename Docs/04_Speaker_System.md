# Speaker System

## Overview

The speaker system represents spatial audio sources inside the Unity XR environment and provides the primary interaction mechanism for shaping the auditory scene.

Each virtual speaker corresponds to a spatial source in the Max/MSP spatialization engine (spat5).  
Users can manipulate speakers directly in 3D space and control their audio parameters via UI modules.

The system supports:

- dynamic speaker creation and removal  
- spatial positioning in real time  
- per-speaker gain control  
- parameter synchronization via OSC  
- serialization for save/load functionality  

---

## Conceptual Model

Speakers are treated as **interactive spatial objects** rather than abstract parameter entries.

This allows users to:

- visually understand spatial layouts  
- manipulate sound sources through direct movement  
- experiment with spatial configurations intuitively  

Each speaker consists of two main representations:

1. **3D Scene Representation** (spatial object)
2. **UI Control Representation** (parameter control module)

---

## Speaker Identification

Each speaker is assigned a unique integer ID.

This ID is used to:

- construct OSC message addresses  
- link UI modules to scene objects  
- maintain consistency during serialization  
- identify sources inside the Max spatial engine  

Example:
`/source/{id}/xyz
`

Arguments:

| Index | Type | Description |
|------|------|-------------|
| 0 | float | X position |
| 1 | float | Y position |
| 2 | float | Z position |

Unity sends this message when:

- a speaker is moved  
- scene state is restored  
- synchronization is required  

Max/MSP updates the corresponding spat source position.

---

## Gain Control Module

Each speaker has a dedicated gain control UI component.

### Components

- mute toggle  
- gain value display  
- vertical slider  
- speaker number label  

### OSC Message


`/vol{id}
`

Arguments:

| Index | Type | Description |
|------|------|-------------|
| 0 | float | Gain value |

Example:


`/vol2 0.8
`

### Mute Message


`/mute{id}
`

Arguments:

| Index | Type | Description |
|------|------|-------------|
| 0 | int or bool | Mute state |

This module allows fast balancing of multiple spatial sources.

---

## Synchronization Strategy

To ensure consistency between Unity and Max:

- gain updates are sent on slider value change  
- mute state updates are sent on toggle events  
- spatial transforms are sent only when movement exceeds a threshold  

Optional feedback messages from Max can update Unity UI values.

---

## Runtime Speaker Management

Users can dynamically:

- add speakers  
- remove speakers  
- reposition speakers  

This supports iterative spatial design.

### Add Speaker Workflow

1. Speaker Manager increments ID counter.
2. UI prefab is instantiated.
3. 3D speaker prefab is instantiated.
4. Both receive the new ID.
5. OSC routing becomes active.

### Remove Speaker Workflow

1. Last speaker UI module is destroyed.
2. Corresponding 3D object is destroyed.
3. Speaker count is decremented.

---

## Save / Load Integration

Speaker state is included in serialization data.

### Stored Parameters

- Speaker ID  
- Position (Vector3)  
- Gain value  
- EQ band values  
- Reverb parameter values  

This allows reconstruction of spatial scenes across sessions.

---

## Interaction Constraints

To improve usability:

- speaker rotation may be locked  
- movement may be limited to floor plane  
- accidental grabs should be minimized  
- visual highlighting indicates selection  

These constraints help maintain spatial coherence.

---

## Design Rationale

Representing speakers as physical objects provides several advantages:

- reduces abstraction compared to traditional audio software  
- improves spatial cognition  
- enables embodied interaction  
- supports exploratory sound design  

This aligns with the human-centered design goals of the project.

---

## Future Extensions

Possible enhancements include:

- speaker grouping  
- trajectory automation  
- distance-based visual cues  
- collision-aware placement  
- spatial snapping  
- multi-user speaker editing  

---

## Summary

The speaker system bridges:

- XR interaction  
- OSC communication  
- spatial audio rendering  

By combining real-time spatial manipulation with parameter control, it forms the core mechanism through which users shape the auditory environment.

---

## Related Pages

- [Home](00_Home.md)
- [System Architecture](01_System_Architecture.md)
- [OSC Protocol](02_OSC_Protocol.md)
- [XR Interaction](03_XR_Interaction.md)
- [Save / Load System](07_Save_Load.md)