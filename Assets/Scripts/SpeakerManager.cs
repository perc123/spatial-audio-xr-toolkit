using System.Collections.Generic;
using UnityEngine;

public class SpeakerManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject speakerPrefab;

    [Header("XR")]
    public Transform xrOrigin;

    [Header("Speaker Counter")]
    public int currentSpeakerCount = 3;

    // Track speakers by ID so Save/Load can work reliably
    private readonly Dictionary<int, SpeakerPair> _speakers = new();

    private class SpeakerPair
    {
        public GameObject go;
        public SpeakerGainControl gain;
    }

    public void AddSpeaker()
    {
        int newId = GetNextAvailableId();
        AddSpeakerWithID(newId, null);
    }

    public void RemoveSpeaker()
    {
        if (_speakers.Count == 0) return;

        int maxId = -1;
        foreach (var id in _speakers.Keys)
            if (id > maxId) maxId = id;

        RemoveSpeakerById(maxId);
    }

    public void RemoveSpeakerById(int id)
    {
        if (!_speakers.TryGetValue(id, out var pair)) return;

        if (pair.go != null) Destroy(pair.go);

        _speakers.Remove(id);

        Debug.Log($"[SpeakerManager] Removed Speaker {id}.");
    }

    // clear everything (used for Load)
    public void ClearAllSpeakers()
    {
        foreach (var kv in _speakers)
        {
            if (kv.Value.go != null) Destroy(kv.Value.go);
        }
        _speakers.Clear();
        Debug.Log("[SpeakerManager] Cleared all speakers.");
    }

    // spawn speaker with a specific ID (used for Load)
    public SpeakerGainControl AddSpeakerWithID(int id, Vector3? worldPosition)
    {
        // Avoid collisions if loading a session with existing speakers
        if (_speakers.ContainsKey(id))
        {
            Debug.LogWarning($"[SpeakerManager] Speaker ID {id} already exists. Skipping.");
            return _speakers[id].gain;
        }

        GameObject newSpeaker = Instantiate(speakerPrefab);
        newSpeaker.name = $"Speaker_{id}";

        var gainControl = newSpeaker.GetComponent<SpeakerGainControl>();
        if (gainControl != null)
            gainControl.speakerID = id;
        else
            Debug.LogError("[SpeakerManager] speakerPrefab missing SpeakerGainControl component.");

        // Store mapping
        _speakers[id] = new SpeakerPair { go = newSpeaker, gain = gainControl };

        // Keep counter in sync
        if (id > currentSpeakerCount) currentSpeakerCount = id;

        // Set position — x starts at 10, +5 per sorted index; y fixed at 16; z fixed at 10
        if (worldPosition.HasValue)
        {
            newSpeaker.transform.position = worldPosition.Value;
        }
        else
        {
            var sortedIds = new List<int>(_speakers.Keys);
            sortedIds.Sort();
            int index = sortedIds.IndexOf(id);
            newSpeaker.transform.position = new Vector3(10f + index * 5f, 16f, 10f);
        }

        // x is always -90; y and z are derived from LookRotation toward XR Origin
        if (xrOrigin != null)
        {
            Vector3 lookDir = xrOrigin.position - newSpeaker.transform.position;
            if (lookDir != Vector3.zero)
            {
                Vector3 euler = Quaternion.LookRotation(lookDir).eulerAngles;
                newSpeaker.transform.rotation = Quaternion.Euler(-90f, euler.y, euler.z);
            }
            else
            {
                newSpeaker.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
            }
        }
        else
        {
            newSpeaker.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        }

        Debug.Log($"[SpeakerManager] Added Speaker {id}.");
        return gainControl;
    }

    // snapshot for saving
    public List<SpeakerSnapshot> GetSnapshot()
    {
        var list = new List<SpeakerSnapshot>();

        foreach (var kv in _speakers)
        {
            int id = kv.Key;
            var pair = kv.Value;

            Vector3 pos = pair.go != null ? pair.go.transform.position : Vector3.zero;

            float mainGain = pair.gain != null ? pair.gain.GetMainGain() : 0f;

            list.Add(new SpeakerSnapshot
            {
                id = id,
                posX = pos.x, posY = pos.y, posZ = pos.z,
                mainGain = mainGain,
            });
        }

        return list;
    }

    private int GetNextAvailableId()
    {
        // Always start above currentSpeakerCount (default 3 → first ID is 4)
        int maxId = currentSpeakerCount;
        foreach (var id in _speakers.Keys)
            if (id > maxId) maxId = id;
        return maxId + 1;
    }
}

// Serializable snapshot types for saving
[System.Serializable]
public class SpeakerSnapshot
{
    public int id;
    public float posX, posY, posZ;
    public float mainGain;
    public float[] eq6 = new float[6];
    public float[] reverb4 = new float[4];
}
