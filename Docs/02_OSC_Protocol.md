# OSC Protocol Specification

## Overview

This document describes the **Open Sound Control (OSC) communication protocol** used between the Unity XR application and the Max/MSP spatial audio engine.

OSC is used to transmit **real-time control data** including:

- listener spatial transforms
- speaker spatial transforms
- gain values
- equalizer parameters
- reverb parameters

Communication is **bidirectional** and occurs via **UDP**.

---

## Network Configuration

| Direction | Description | Port |
|----------|-------------|------|
| Unity → Max | Control data transmission | `6161` |
| Max → Unity | Parameter feedback / synchronization | `9001` |

### Important Notes

- Both devices must be on the same network.
- Firewalls must allow UDP traffic on the specified ports.
- When using standalone XR devices (e.g. Quest), correct IP configuration is required.

---

## Message Structure

All OSC messages follow the general format:

`/address value1 value2 value3 ...`


Addresses are hierarchical and parameter-specific.

---

## Listener Messages

### Listener Position

**Address**

`/listener/xyz`


**Arguments**

| Index | Type | Description |
|------|------|-------------|
| 0 | float | X position (meters) |
| 1 | float | Y position (meters) |
| 2 | float | Z position (meters) |

**Description**

Sent continuously when the listener position changes in Unity.

Used in Max/MSP to update the spatial rendering reference point.

---

### Listener Rotation

**Address**

`/listener/ypr`

**Arguments**

| Index | Type | Description |
|------|------|-------------|
| 0 | float | Yaw (degrees) |
| 1 | float | Pitch (degrees) |
| 2 | float | Roll (degrees) |

**Description**

Represents the orientation of the user in XR space.

Used by the spatial engine to align the auditory scene with the user's viewpoint.

---

## Speaker Messages

### Speaker Position

**Address**

`/source/{id}/xyz`

**Arguments**

| Index | Type | Description |
|------|------|-------------|
| 0 | float | X position |
| 1 | float | Y position |
| 2 | float | Z position |

**Description**

Sent whenever a virtual speaker is moved in Unity.

Each speaker has a unique integer ID.

Example:

`/source/2/xyz 1.5 0.0 -2.3`

---

### Speaker Gain

**Address**

`/vol{id}`


**Arguments**

| Index | Type | Description |
|------|------|-------------|
| 0 | float | Gain value (normalized or dB depending on mapping) |

Example:


``/vol3 0.75
``

**Description**

Controls the output gain of a specific spatial source.

---

### Speaker Mute

**Address**


``/mute{id}
``

**Arguments**

| Index | Type | Description |
|------|------|-------------|
| 0 | int or bool | Mute state |

Example:


``/mute2 1
``

---

## Equalizer Messages

The system implements a **6-band equalizer**.

Each band corresponds to a fixed center frequency.

| Band | Frequency |
|------|-----------|
| 0 | 40 Hz |
| 1 | 130 Hz |
| 2 | 550 Hz |
| 3 | 1600 Hz |
| 4 | 4300 Hz |
| 5 | 12000 Hz |

### EQ Band Update

**Address**


``/filter/{band}
``

**Arguments**

| Index | Type | Description |
|------|------|-------------|
| 0 | float | Frequency |
| 1 | float | Gain (dB) |

Example:


``/filter/3 1600 -4.5
``

**Description**

Unity sends updated EQ band values when the user moves an EQ slider.

Max/MSP routes the message using `OSC-route` and updates the corresponding filter in `filtergraph~`.

---

## Reverb Messages

Reverb parameters are controlled via OSC.

Example parameters may include:

- room size
- decay
- wet/dry mix
- diffusion

### Generic Reverb Parameter

**Address**


``/reverb/{param}
``

**Arguments**

| Index | Type | Description |
|------|------|-------------|
| 0 | float | Parameter value |

Example:


``/reverb/decay 0.65
``

---

## Feedback Messages (Max → Unity)

Max/MSP may send values back to Unity for:

- UI synchronization
- preset loading
- parameter updates
- visualization feedback

Example:


``/reverb/wet 0.4
/filter/2 550 -3.0``



Unity receives these messages via **extOSC Receiver** and updates UI sliders accordingly.

---

## Update Strategy

To reduce network load and jitter:

- Spatial transforms are sent **only when changes exceed a threshold**
- Slider updates are sent **on value change events**
- Continuous DSP parameters may be rate-limited if necessary

---

## Timing Considerations

- OSC operates on UDP → packets may be lost
- Messages should be stateless where possible
- Unity and Max must tolerate occasional dropped packets
- Large bursts of OSC messages should be avoided

---

## Debugging Tools

### Unity

- Debug.Log OSC message content
- extOSC Monitor tools

### Max/MSP

- `print` object
- OSC-route debug outlets
- visual monitoring in spat.viewer

---

## Future Extensions

The OSC protocol can be extended to support:

- multiple listener models
- automation envelopes
- preset management
- spatial trajectories
- spectral parameter control
- scene metadata transmission

---

## Related Pages

- [Home](00_Home.md)
- [System Architecture](01_System_Architecture.md)
- [Equalizer System](05_EQ_System.md)
- [Reverb System](06_Reverb_System.md)
- [Save / Load System](07_Save_Load.md)
- [Setup Guide](09_Setup.md)