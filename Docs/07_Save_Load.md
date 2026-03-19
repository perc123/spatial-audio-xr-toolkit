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

