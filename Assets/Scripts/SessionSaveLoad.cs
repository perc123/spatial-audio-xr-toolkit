using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SessionSnapshot
{
    public int version = 1;
    public List<SpeakerSnapshot> speakers = new();
}
