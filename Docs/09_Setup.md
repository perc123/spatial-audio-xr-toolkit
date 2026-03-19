# Setup Guide

## Overview

This document explains how to set up and run the **3D Spatial Audio Interaction Toolkit**.

The system consists of:

- Unity XR Application (interaction + visualization)
- Max/MSP Spatial Audio Engine (audio processing)
- OSC communication over UDP

Both systems must be correctly configured for real-time operation.

---

## Unity Setup

### Requirements

- Unity 6.x (or compatible LTS)
- XR Interaction Toolkit
- extOSC package
- XR Device Simulator (optional for testing without headset)

### Opening the Project

1. Open Unity Hub.
2. Add the project folder.
3. Open the project.
4. Allow package import / script compilation.

### Scene Setup

Ensure the main scene contains:

- XR Origin
- OSC Manager (Transmitter + Receiver)
- Speaker Manager
- Hand Menu UI
- Event System

### OSC Transmitter Configuration

Set the following values in the OSC Transmitter component:

| Parameter | Value |
|----------|------|
| Remote Host | IP address of the Max computer |
| Remote Port | `6161` |

### OSC Receiver Configuration

| Parameter | Value |
|----------|------|
| Local Port | `9001` |

---

## Max/MSP Setup

### Requirements

- Max 8.x
- CNMAT OSC externals
- spat5 library

### Installing CNMAT OSC

1. Open Max.
2. Go to **File → Show Package Manager**.
3. Search for **CNMAT OSC**.
4. Install the package.

### Installing spat5

Download from IRCAM and place inside:

`Documents/Max 8/Packages/`

Restart Max.

---

## OSC Patch Initialization

Basic OSC input structure:


`[udpreceive 6161]
|
[OSC-route]`


Ensure the main patch includes routing for:

- listener position
- speaker position
- gain values
- EQ messages
- reverb messages

---

## Audio Setup

Configure audio output in Max:

1. Open **Audio Status**.
2. Select audio interface.
3. Set buffer size (recommended: 256–512 samples).
4. Enable DSP.

For binaural monitoring:

- use headphones
- verify spat5 rendering configuration

---

## Network Setup (Very Important)

### Same Network Requirement

Unity device (PC or Quest) and Max computer must be on:

- the same WiFi network
- or the same router subnet

### Finding Max Computer IP

On Windows:

`ipconfig
`

Use IPv4 address (example):


`192.168.0.105`


Enter this in Unity OSC Transmitter.

---

## Quest Standalone Setup

When building to Meta Quest:

- ensure Quest is connected to same WiFi
- firewall on laptop must allow UDP
- avoid public networks with isolation enabled

Recommended:

- dedicated router or hotspot
- fixed IP configuration if possible

---

## Firewall Configuration

If OSC messages are not received:

- allow **Max.exe** in Windows Firewall
- allow UDP port **6161**
- allow UDP port **9001**

---

## Testing OSC Communication

### Unity → Max

Add debug object in Max:

`[print OSC_IN]
`

Move speaker or slider in Unity.

If working, Max console prints messages.

### Max → Unity

Send test message:


`/reverb/0 0.5`


Verify Unity slider updates.

---

## XR Testing Without Headset

Use **XR Device Simulator**:

- simulate head movement
- simulate controller interaction
- test grabbing and UI behaviour

---

## Build Settings (Quest)

1. Switch platform to Android.
2. Enable OpenXR.
3. Enable hand tracking or controllers.
4. Build and deploy via USB or wireless.

---

## Troubleshooting

### No OSC Messages Received

- check IP address
- check ports
- check firewall
- confirm same network

### Speakers Do Not Move

- verify XR Grab Interactable setup
- verify interactor selection binding
- check Rigidbody constraints

### Audio Not Updating

- confirm DSP is on
- verify OSC routing chain
- verify spat5 initialization

---

## Recommended Workflow

1. Start Max patch first.
2. Enable DSP.
3. Run Unity scene.
4. Test OSC communication.
5. Begin interaction.

---

## Summary

Correct setup of:

- Unity XR environment
- Max spatial engine
- OSC network configuration

is essential for stable real-time spatial audio interaction.

---

## Related Pages

- [Home](00_Home.md)
- [OSC Protocol](02_OSC_Protocol.md)
- [System Architecture](01_System_Architecture.md)
- [Evaluation](08_Evaluation.md)