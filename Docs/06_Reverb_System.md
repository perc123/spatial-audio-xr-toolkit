# Reverb System

## Overview

The Reverb System enables real-time control of environmental acoustic characteristics within the spatial audio scene.

Users can manipulate reverb parameters through UI sliders in the Unity XR interface.  
These parameters are transmitted via OSC to the Max/MSP audio engine, where they are mapped to reverb processing modules.

The system allows users to explore how spatial placement and environmental acoustics interact perceptually.

---

## Design Goals

The reverb interaction design aims to:

- provide intuitive control over environmental acoustics  
- support exploratory spatial sound design  
- enable immediate auditory feedback  
- maintain consistency with the hand-menu interaction paradigm  
- simplify routing and reproducibility for evaluation  

Rather than exposing complex DSP structures, the system presents **high-level perceptual parameters**.

---

## Reverb Parameters

The current implementation supports four primary parameters.

| Parameter Index | Conceptual Meaning |
|-----------------|-------------------|
| 0 | Room Size |
| 1 | Decay Time |
| 2 | Diffusion / Density |
| 3 | Wet / Dry Mix |

These parameters are mapped to corresponding DSP controls inside Max/MSP.

---

## Unity Interface

### UI Components

Each parameter is controlled using:

- vertical sliders  
- numerical value display (optional)  
- visual grouping inside the Filter / Environment page  

The interface is designed to:

- minimize clutter  
- allow quick experimentation  
- maintain consistency with gain and EQ controls  

---

## Interaction Behaviour

- slider movement triggers OSC transmission  
- parameter changes are immediately audible  
- real-time auditory feedback supports perceptual evaluation  
- parameter values can be serialized during Save/Load  

---

## OSC Communication

### Message Address

`/reverb/{param}
`

### Arguments

| Index | Type | Description |
|------|------|-------------|
| 0 | float | Parameter value |

### Example


`/reverb/1 0.75
`

This example updates the **Decay Time parameter**.

---

## Max/MSP Routing

Incoming messages are handled using CNMAT OSC tools.

Example routing structure:

`
[udpreceive 6161]
|
[OSC-route /reverb]
|
[OSC-route /0 /1 /2 /3]`


Each outlet corresponds to one reverb parameter.

The values are then:

- scaled if necessary  
- mapped to DSP control signals  
- applied to the reverb processing module  

---

## DSP Mapping Strategy

Reverb parameter values from Unity are typically normalized (0–1).  
Inside Max/MSP they may be mapped to meaningful ranges such as:

- Decay Time → 0.3 s – 8 s  
- Wet Mix → 0 – 100 %  
- Room Size → algorithm-specific range  
- Diffusion → filter or feedback parameters  

This mapping layer ensures perceptual usability while maintaining DSP flexibility.

---

## Synchronization

Optional feedback messages from Max/MSP may update Unity sliders when:

- presets are loaded  
- parameters are automated  
- system state is restored  

This maintains UI consistency.

---

## Interaction Rationale

Controlling reverb in XR provides strong perceptual affordances:

- users can associate spatial placement with environmental response  
- changes are evaluated through embodied listening  
- acoustic space becomes a manipulable design dimension  

This supports the human-centered design objectives of the project.

---

## Performance Considerations

- reverb parameters are transmitted only on slider change  
- high-rate continuous updates are avoided  
- UDP packet loss is tolerated due to stateless parameter control  
- DSP smoothing may be applied in Max/MSP to avoid artifacts  

---

## Save / Load Integration

The following values are serialized:

- parameter index  
- parameter value  

During scene reconstruction:

- Unity restores slider positions  
- OSC messages reinitialize the DSP state  

---

## Future Extensions

Possible improvements include:

- spatially localized reverberation zones  
- multiple reverb buses  
- distance-dependent wet mix  
- impulse response convolution support  
- environmental visualization cues  
- preset morphing  

---

## Summary

The Reverb System extends the spatial interaction paradigm to environmental acoustics.

By combining:

- perceptual parameter sliders  
- OSC-based control  
- real-time DSP mapping  

users can intuitively shape the acoustic character of immersive sound scenes.

---

## Related Pages

- [Home](00_Home.md)
- [OSC Protocol](02_OSC_Protocol.md)
- [Equalizer System](05_EQ_System.md)
- [Speaker System](04_Speaker_System.md)
- [Save / Load System](07_Save_Load.md)