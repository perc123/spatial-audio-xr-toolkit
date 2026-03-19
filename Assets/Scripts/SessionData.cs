using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SessionData : MonoBehaviour
{
    public int version = 1;
    public List<SpeakerData> speakers = new();
}

[Serializable]
public class SpeakerData
{
    public int id;

    // Use plain float fields for JSON safety.
    public float posX, posY, posZ;

    public float mainGain;

    // Fixed sizes: 6 EQ bands, 4 reverb params
    public float[] eq = new float[6];
    public float[] reverb = new float[4];
}