# XR Interaction Design

## Overview

The XR interaction layer is responsible for enabling intuitive and spatially meaningful interaction with the audio scene.

The system is designed according to **human-centered design principles**, aiming to reduce the cognitive and technical barriers typically associated with spatial audio tools.

Users interact with virtual loudspeakers and audio parameters inside an immersive 3D environment using:

- XR controllers
- hand tracking (future extension)
- ray-based or direct manipulation techniques

The interaction paradigm focuses on **natural spatial manipulation**, allowing users to treat sound sources as physical objects.

---

## Interaction Goals

The XR interaction design aims to:

- improve spatial understanding of audio scenes  
- enable direct manipulation of sound sources  
- reduce reliance on abstract parameter panels  
- provide real-time auditory feedback  
- support exploratory sound design workflows  

The design prioritizes:

- visibility of system state  
- consistency of interaction  
- minimal motor effort  
- clear mapping between action and auditory result  

---

## Interaction Modalities

### Object Grabbing

Users can grab virtual speakers and reposition them in 3D space.

#### Characteristics

- speakers behave as movable scene objects  
- translation is allowed while rotation may be constrained  
- movement is continuous and spatially grounded  
- visual feedback confirms selection and manipulation  

This interaction allows users to **directly sculpt spatial sound layouts**.

---

### Ray-Based Interaction

A ray interactor enables interaction with:

- distant speakers  
- UI sliders  
- menu buttons  

This reduces the need for physical locomotion and allows efficient scene editing.

---

### Direct Interaction

When using hand tracking or near interaction:

- users can directly touch UI elements  
- speakers can be manipulated through proximity-based grabbing  

This supports more natural workflows in immersive environments.

---

## Hand Menu Interface

The user interface is implemented as a **hand-held menu panel**, conceptually similar to holding a tablet or control surface.

### Layout Structure

The menu consists of two main columns:

#### Navigation Column

Contains buttons for switching between UI pages:

- Home  
- Save / Load  
- Filter (EQ)  
- Position  

#### Content Column

Displays the active page content.

This layout provides:

- consistent navigation  
- reduced visual clutter  
- predictable interaction patterns  

---

## Speaker Interaction Workflow

A typical workflow for spatial editing:

1. The user selects a speaker using ray or direct grab.
2. The speaker is repositioned in the 3D environment.
3. Unity updates the spatial transform.
4. OSC messages are sent to the Max/MSP spatial engine.
5. The auditory scene updates in real time.

This immediate feedback loop supports **exploratory spatial design**.

---

## Parameter Interaction

Audio parameters are controlled using UI elements such as:

- sliders
- toggles
- graphical EQ handles

### Gain Control

Each speaker has an associated gain control module:

- mute toggle
- gain value display
- vertical slider

This allows fast per-source level balancing.

---

### Equalizer Interaction

A graphical equalizer interface allows:

- manipulation of fixed frequency bands  
- visual representation of spectral shaping  
- real-time auditory response  

The interaction is designed to resemble familiar audio tools while benefiting from spatial context.

---

### Reverb Interaction

Reverb parameters are adjusted via sliders that control:

- room characteristics  
- decay behaviour  
- wet/dry balance  

These controls allow users to experiment with environmental acoustics.

---

## Listener Representation

The listener position corresponds to the XR Origin.

Movement of the user in physical or virtual space updates:

- spatial audio reference frame  
- binaural rendering perspective  

Orientation changes affect:

- perceived direction of sound sources  
- spatial coherence of the scene  

---

## Feedback Mechanisms

The interaction design includes multiple feedback channels:

### Visual Feedback

- speaker highlighting on hover  
- movement visualization  
- UI state indication  

### Auditory Feedback

- real-time spatial updates  
- gain and EQ changes reflected immediately  
- reverb environment perception  

### Interaction Feedback

- grab confirmation  
- slider response  
- page switching transitions  

These feedback layers help users maintain **situational awareness**.

---

## Design Challenges

Several challenges arise in XR audio interaction design:

- avoiding accidental object movement  
- maintaining spatial reference consistency  
- preventing UI occlusion  
- ensuring precision in parameter adjustment  
- balancing realism and usability  

Design decisions such as constrained rotation and hand-menu positioning address these challenges.

---

## Future Interaction Extensions

Possible future improvements include:

- gesture-based speaker placement  
- spatial automation recording  
- multi-user collaborative editing  
- voice-controlled parameter adjustment  
- haptic feedback integration  
- adaptive UI scaling  

---

## Summary

The XR interaction layer transforms spatial audio design from a parameter-driven task into a **direct spatial manipulation experience**.

By combining:

- object-based interaction  
- real-time auditory feedback  
- structured UI navigation  

the system supports intuitive exploration of complex audio scenes.

---

## Related Pages

- [Home](00_Home.md)
- [System Architecture](01_System_Architecture.md)
- [Speaker System](04_Speaker_System.md)
- [Equalizer System](05_EQ_System.md)
- [Reverb System](06_Reverb_System.md)
- [Evaluation](08_Evaluation.md)