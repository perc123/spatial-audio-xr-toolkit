# Save / Load System

## Overview

The Save / Load system enables persistent storage and reconstruction of spatial audio scenes created in the Unity XR interface.

Users can save the current configuration of:

- speaker positions  
- gain values  
- equalizer parameters  
- reverb parameters  

This allows:

- iterative design workflows  
- reproducible evaluation sessions  
- preset management  
- restoration of complex spatial layouts  

The system is implemented using **JSON-based serialization** inside Unity.

---

## Design Goals

The Save / Load system was designed to:

- preserve complete scene state  
- support dynamic speaker counts  
- allow fast reconstruction of spatial layouts  
- remain human-readable for debugging and research purposes  
- maintain OSC synchronization with the Max/MSP audio engine  

---

## Stored Scene Data

Each saved scene contains a list of speaker configurations.

### Speaker Data Structure

Each speaker entry stores:

| Field | Type | Description |
|------|------|-------------|
| id | int | Unique speaker identifier |
| position | Vector3 | Speaker world position |
| gain | float | Main gain value |
| eqBands | float[6] | Gain values for EQ bands |
| reverbParams | float[4] | Reverb parameter values |

---

## Example JSON Structure

```json
{
  "speakers": [
    {
      "id": 1,
      "position": { "x": 1.2, "y": 0.0, "z": -2.5 },
      "gain": 0.8,
      "eqBands": [0.0, -3.0, 1.5, 0.0, 2.0, -1.0],
      "reverbParams": [0.5, 0.7, 0.4, 0.3]
    },
    {
      "id": 2,
      "position": { "x": -1.0, "y": 0.0, "z": 1.8 },
      "gain": 0.6,
      "eqBands": [1.0, 0.0, -2.0, 0.5, 0.0, 0.0],
      "reverbParams": [0.6, 0.8, 0.5, 0.4]
    }
  ]
}
```

This format allows manual inspection and editing if necessary.

## Save Workflow

1. The user presses the **Save** button in the UI.  
2. Unity queries the **Speaker Manager** for all active speakers.  
3. Each speaker’s state is collected.  
4. A scene data object is constructed.  
5. The data is serialized to JSON.  
6. The JSON file is written to persistent storage.  

This process is lightweight and can be executed during runtime.


## Load Workflow

1. The user selects a saved configuration.  
2. Unity reads the JSON file.  
3. Existing speakers are cleared or updated.  
4. New speaker prefabs are instantiated if needed.  
5. Stored parameter values are applied to UI components.  
6. OSC messages are sent to Max/MSP to restore DSP state.  

This ensures visual and auditory synchronization.


## OSC Reinitialization Strategy

During loading, Unity sends:

- `/source/{id}/xyz` messages for all speakers  
- `/vol{id}` gain messages  
- `/filter/{band}` EQ messages  
- `/reverb/{param}` messages  

This guarantees that the Max/MSP spatial engine reflects the restored scene.


## Dynamic Speaker Handling

The system supports:

- Variable number of speakers  
- Runtime addition and removal  
- Consistent ID mapping  

When loading a scene:

- Missing speakers are instantiated  
- Extra speakers are removed  
- IDs remain stable to preserve routing logic  


## Storage Location

Saved configurations are typically stored using:

- `Application.persistentDataPath`  

This ensures compatibility with:

- Desktop builds  
- Standalone XR devices  
- Different operating systems  


## Usability Considerations

The **Save / Load** feature supports:

- Iterative design experimentation  
- Structured evaluation sessions  
- Fast comparison of spatial layouts  
- Recovery from interaction errors  

Providing persistent scene state improves user confidence and workflow continuity.


## Limitations

Current implementation constraints include:

- No versioning of saved scenes  
- No preset metadata (e.g., author, timestamp)  
- No interpolation between presets  
- No remote synchronization  

These limitations are acceptable within the scope of a prototype.


## Future Extensions

Possible improvements:

- Preset naming system  
- Scene thumbnails  
- Spatial automation storage  
- Cloud synchronization  
- Collaborative preset sharing  
- Undo / redo history  


## Summary

The **Save / Load system** enables persistent storage of spatial audio scenes, ensuring reproducibility and continuity in immersive sound design workflows.

By combining:

- JSON serialization  
- Dynamic speaker reconstruction  
- OSC reinitialization  

The system maintains synchronization between interaction, visualization, and audio processing layers.


## Related Pages

- [Home](00_Home.md)  
- [Speaker System](04_Speaker_System.md)  
- [OSC Protocol](02_OSC_Protocol.md)  
- [Evaluation](08_Evaluation.md)  
- [Setup Guide](09_Setup.md)  
