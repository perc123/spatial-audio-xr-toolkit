# 3D Spatial Audio Interaction Toolkit  
### Unity XR Interface ↔ Max/MSP Spatial Audio Engine (OSC)

## Overview

This project explores a **human-centered 3D user interface for spatial audio design** in immersive environments.  
A Unity XR application allows users to position virtual loudspeakers, control gain, equalization, and reverberation parameters in real time, while communicating with a **Max/MSP spatial audio engine** via OSC.

The goal is to investigate intuitive interaction techniques for spatial sound design workflows, particularly in contexts such as concert hall simulation, music production, and immersive media.

This project is developed as part of a **Bachelor Thesis** at HTW Berlin (Informatik in Kultur und Gesundheit).

---

## Main Features

- 3D speaker positioning in XR space  
- Per-speaker gain control  
- 6-band graphical equalizer interface  
- Reverb parameter control  
- Real-time listener position & rotation tracking  
- Bidirectional OSC communication (Unity ↔ Max)  
- Save / Load spatial audio setups  
- Human-centered XR hand-menu interface  

---

## System Architecture

The system consists of two main components:

1. **Unity XR Application**
2. **Max/MSP Spatial Audio Engine**

The communication between both systems is implemented via **OSC over UDP**.

### Interaction Flow

- The user interacts with virtual speakers and UI controls inside Unity.
- Unity sends spatial and parameter data via OSC.
- Max receives the data and performs spatial audio processing.
- The processed audio is rendered binaurally or through loudspeakers.

### Data Flow Overview
#### User (XR Interaction)

    ↓

#### Unity Interaction & Visualization Layer

    ↓ 

#### OSC (UDP) Max/MSP Spatial Audio Engine (spat5)

    ↓

#### Binaural Rendering / Loudspeaker Output


### Responsibilities

**Unity handles:**

- XR interaction
- UI rendering
- speaker visualization
- OSC transmission
- scene serialization

**Max handles:**

- spatialization (spat5)
- EQ processing
- reverb processing
- audio rendering

## Requirements

### Unity

- Unity 6.x (or compatible LTS)
- XR Interaction Toolkit
- XR Hands / Meta XR (optional)
- extOSC

### Max

- Max 8.x
- CNMAT OSC externals
- spat5 library

---

## How to Run

### Unity Side

1. Open the project in Unity.
2. Ensure OSC Transmitter settings:
   - Remote Host = IP of Max computer
   - Remote Port = `6161`
3. Press **Play** (XR Device Simulator supported).
4. For standalone deployment, build to Quest.

### Max Side

1. Open the main spatial audio patch.
2. Ensure UDP receive port:
[udpreceive 6161]


3. Verify OSC routing and spat5 initialization.
4. Confirm audio output configuration.

---

## OSC Communication

### Listener

| Address | Description |
|--------|-------------|
| `/listener/xyz` | Listener position (x y z) |
| `/listener/ypr` | Listener rotation (yaw pitch roll) |

### Speaker

| Address | Description |
|--------|-------------|
| `/source/{id}/xyz` | Speaker position |
| `/vol{id}` | Speaker gain |

### Equalizer

| Address | Description |
|--------|-------------|
| `/filter/{band}` | EQ band update (frequency gain) |

### Reverb

| Address | Description |
|--------|-------------|
| `/reverb/{param}` | Reverb parameter control |

---

## Save / Load System

Speaker configurations are serialized including:

- Speaker ID  
- Position  
- Gain value  
- 6 EQ band values  
- Reverb parameters  

This enables reconstruction of spatial audio scenes for evaluation and experimentation.

---

## XR Interaction Design

The interface is implemented as a **hand-held menu panel** containing multiple UI pages:

- Home page  
- Save / Load page  
- Filter (EQ) page  
- Speaker Position page  

Users can:

- Grab and reposition speakers in 3D space  
- Adjust audio parameters via sliders  
- Observe spatial layout visually  

---

## Thesis Context

This prototype supports research into:

- usability of 3D spatial audio interfaces  
- spatial cognition in XR environments  
- real-time audio design workflows  
- integration of game engines with DSP environments  

---

## Author

Theofanis Gkioles Blatsoukas  
HTW Berlin  
Bachelor Thesis – Informatik in Kultur und Gesundheit  

---

## License

Academic research prototype.  

