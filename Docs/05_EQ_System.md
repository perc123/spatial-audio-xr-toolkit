# Equalizer System

## Overview

The Equalizer (EQ) system enables real-time spectral shaping of spatial audio sources through an interactive graphical interface in Unity.

The system is designed as a **6-band parametric equalizer**, where each band corresponds to a fixed center frequency.  
Users can adjust the gain of each band using UI sliders, and the updated values are transmitted to Max/MSP via OSC.

This allows immediate auditory feedback and supports exploratory sound design workflows in immersive environments.

---

## Design Concept

Traditional audio tools often represent equalization using abstract parameter panels.  
In this project, the EQ interface is integrated into the XR interaction paradigm to:

- improve usability  
- reduce cognitive load  
- provide intuitive spectral control  
- maintain consistency with spatial interaction principles  

The EQ interface is accessible through the **Filter Page** of the hand-menu UI.

---

## Equalizer Structure

The system implements six fixed frequency bands.

| Band Index | Center Frequency |
|-----------|------------------|
| 0 | 40 Hz |
| 1 | 130 Hz |
| 2 | 550 Hz |
| 3 | 1600 Hz |
| 4 | 4300 Hz |
| 5 | 12000 Hz |

Only the **gain value** is modified by user interaction.  
The frequency values remain constant to simplify interaction and routing.

---

## Unity EQ Interface

### Components

Each EQ band is represented by:

- a vertical slider  
- visual gain level indication  
- band index mapping  

All band sliders are managed by a central **EQ Controller** component.

### Interaction Behaviour

- slider movement triggers OSC transmission  
- gain values are updated in real time  
- changes are immediately audible  
- visual feedback confirms parameter adjustment  

This interaction model supports quick experimentation and iterative refinement.

---

## OSC Communication

### Message Address

`/filter/{band}
`

### Arguments

| Index | Type | Description |
|------|------|-------------|
| 0 | float | Center frequency |
| 1 | float | Gain value (dB) |

### Example


`/filter/3 1600 -4.5`

This message indicates:

- band index: 3  
- frequency: 1600 Hz  
- gain: −4.5 dB  

Messages are sent whenever a slider value changes.

---

## Max/MSP Routing Strategy

Incoming OSC messages are handled using the **CNMAT OSC-route object**.

Example routing chain:


`[udpreceive 6161]
|
[OSC-route /filter]
|
[OSC-route /0 /1 /2 /3 /4 /5]`


Each outlet corresponds to one EQ band.

The data is then:

1. unpacked  
2. formatted  
3. routed to the appropriate filter control  

---

## Filtergraph Integration

Each EQ band updates parameters inside a `filtergraph~`-based processing structure.

Typical routing workflow:

- receive frequency and gain  
- update corresponding filter node  
- apply updated filter coefficients  
- audio signal is processed through updated filter chain  

This allows real-time spectral modification without interrupting playback.

---

## Update Strategy

To ensure stable performance:

- EQ values are transmitted **only when sliders change**  
- frequency values are sent alongside gain to maintain stateless communication  
- packet rate remains low to prevent OSC congestion  

This design ensures responsiveness while maintaining network efficiency.

---

## Synchronization (Optional)

Max/MSP may send feedback messages to Unity when:

- presets are loaded  
- parameter automation occurs  
- values are externally modified  

Unity can then update slider positions to maintain UI consistency.

---

## Usability Considerations

The fixed-band design provides several advantages:

- reduces decision complexity  
- improves speed of interaction  
- aligns with common mixing workflows  
- simplifies OSC routing  
- improves reproducibility for evaluation  

However, it also limits flexibility compared to fully parametric EQ systems.

---

## Future Extensions

Possible improvements include:

- adjustable Q factor  
- frequency drag interaction  
- visual EQ curve rendering  
- per-speaker EQ modules  
- preset morphing  
- spectral automation recording  

---

## Summary

The Equalizer System integrates spectral audio control into the XR interaction framework.

By combining:

- structured UI sliders  
- fixed frequency mapping  
- real-time OSC communication  
- Max/MSP filter processing  

the system enables intuitive and responsive spectral shaping within immersive spatial audio scenes.

---

## Related Pages

- [Home](00_Home.md)
- [OSC Protocol](02_OSC_Protocol.md)
- [XR Interaction](03_XR_Interaction.md)
- [Reverb System](06_Reverb_System.md)
- [Speaker System](04_Speaker_System.md)