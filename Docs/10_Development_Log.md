# Development Log

## Overview

This development log documents the iterative design and implementation process of the **3D Spatial Audio Interaction Toolkit**.

The log reflects:

- technical milestones  
- design decisions  
- encountered challenges  
- usability considerations  
- system refinements  

Maintaining this record supports research transparency and assists in structuring the thesis methodology chapter.

---

## Week 1 — Requirements Analysis & Concept Definition

### Goals

- Define research focus
- Identify target user group
- Analyze existing spatial audio tools (e.g., ODEON, EASE)
- Define interaction paradigm

### Key Decisions

- Use Unity for XR interaction and visualization
- Use Max/MSP (spat5) for spatial audio processing
- Use OSC for real-time communication
- Adopt human-centered design principles

### Challenges

- Defining scope appropriate for thesis timeframe
- Balancing technical ambition and feasibility

---

## Week 2 — OSC Communication Prototype

### Goals

- Establish basic OSC connection between Unity and Max
- Send and receive simple float values
- Verify network reliability

### Achievements

- Successful UDP communication established
- Parameter synchronization tested
- Initial debugging workflow defined

### Challenges

- Port conflicts
- firewall configuration
- message formatting issues

---

## Week 3 — Listener Tracking Integration

### Goals

- Send XR Origin position and rotation to Max
- Integrate listener control with spat5.viewer

### Achievements

- Real-time spatial reference updates working
- Orientation mapping refined (Yaw / Pitch / Roll)
- Threshold-based update strategy implemented

### Challenges

- OSC message routing errors
- coordinate system alignment
- handling quaternion vs Euler rotation formats

---

## Week 4 — Speaker Interaction System

### Goals

- Implement grabbable speaker objects
- Enable spatial manipulation
- Link speaker IDs to OSC messages

### Achievements

- Speaker Manager implemented
- Dynamic speaker spawning supported
- Real-time `/source/{id}/xyz` transmission working

### Challenges

- XR interactor configuration
- unintended rotation during grabbing
- object snapping / attachment behaviour

---

## Week 5 — Gain Control UI

### Goals

- Implement per-speaker gain module
- Add mute functionality
- Synchronize UI and audio engine

### Achievements

- Slider-based gain control working
- OSC routing stable
- Visual feedback improved

### Challenges

- UI layout constraints in XR
- prefab parameter referencing
- debugging event listeners

---

## Week 6 — Equalizer Interface

### Goals

- Design 6-band EQ interaction
- Define fixed frequency mapping
- Implement OSC band routing

### Achievements

- EQ Controller implemented
- Filtergraph integration in Max working
- Real-time spectral shaping confirmed

### Challenges

- graphical vs slider interaction trade-offs
- routing efficiency
- perceptual parameter scaling

---

## Week 7 — Reverb Control Integration

### Goals

- Implement environmental parameter control
- Define high-level perceptual parameters
- Map values to DSP ranges

### Achievements

- Reverb slider interface working
- OSC parameter routing stable
- Auditory feedback validated

### Challenges

- choosing meaningful parameter mappings
- avoiding parameter oversensitivity
- maintaining UI clarity

---

## Week 8 — Save / Load System

### Goals

- Implement JSON serialization
- Support dynamic speaker counts
- Ensure DSP state restoration

### Achievements

- Scene persistence working
- Reconstruction workflow stable
- Parameter synchronization ensured

### Challenges

- prefab state reconstruction
- ID consistency
- file path handling across platforms

---

## Week 9 — XR Interaction Refinement

### Goals

- Improve usability
- refine hand-menu layout
- stabilize grabbing mechanics

### Achievements

- movement constraints improved
- UI navigation clarity enhanced
- feedback mechanisms strengthened

### Challenges

- balancing precision vs freedom of movement
- preventing accidental interactions
- optimizing spatial layout readability

---

## Week 10 — Evaluation Preparation

### Goals

- finalize prototype stability
- define evaluation tasks
- prepare questionnaire

### Achievements

- testing workflow defined
- participant recruitment planned
- performance logging methods prepared

### Challenges

- time constraints
- ensuring reliable build deployment
- designing meaningful tasks

---

## Week 11 — Evaluation & Documentation

### Goals

- conduct usability sessions
- analyze results
- finalize thesis writing

### Expected Outcomes

- identification of usability strengths
- recognition of design limitations
- formulation of future research directions

---

## Reflection

The iterative development process highlights the importance of:

- modular architecture
- rapid prototyping tools
- continuous usability consideration
- balancing technical depth and interaction clarity

This log demonstrates how the system evolved from a communication prototype into a functional spatial audio interaction environment.

---

## Related Pages

- [Home](00_Home.md)
- [System Architecture](01_System_Architecture.md)
- [Evaluation](08_Evaluation.md)
- [Setup Guide](09_Setup.md)