using System.Collections.Generic;
using UnityEngine;

public class SpeakerManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject speakerUIPrefab;    // SpeakerGain UI Prefab
    public GameObject speakerImagePrefab; // 3D Speaker Image Prefab

    [Header("Parent Transforms")]
    public Transform uiParent;
    public Transform imageParent;

    [Header("UI Layout")]
    public float uiRowHeight = 100f;

    [Header("XR")]
    public Transform xrOrigin;

    [Header("Speaker Counter")]
    public int currentSpeakerCount = 3;

    // Track speakers by ID so Save/Load can work reliably
    private readonly Dictionary<int, SpeakerPair> _speakers = new();

    private class SpeakerPair
    {
        public GameObject ui;
        public GameObject image;
        public SpeakerGainControl gain; // cache for convenience
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

        if (pair.ui != null) Destroy(pair.ui);
        if (pair.image != null) Destroy(pair.image);

        _speakers.Remove(id);

        currentSpeakerCount = GetMaxExistingId();

        RelayoutUI();

        Debug.Log($"[SpeakerManager] Removed Speaker {id}. Current max id: {currentSpeakerCount}");
    }

    // clear everything (used for Load)
    public void ClearAllSpeakers()
    {
        foreach (var kv in _speakers)
        {
            if (kv.Value.ui != null) Destroy(kv.Value.ui);
            if (kv.Value.image != null) Destroy(kv.Value.image);
        }
        _speakers.Clear();
        currentSpeakerCount = 0;
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

        // Instantiate UI
        GameObject newSpeakerUI = Instantiate(speakerUIPrefab, uiParent);

        var gainControl = newSpeakerUI.GetComponent<SpeakerGainControl>();
        if (gainControl != null)
        {
            gainControl.speakerID = id;
        }
        else
        {
            Debug.LogError("[SpeakerManager] SpeakerUIPrefab missing SpeakerGainControl component.");
        }

        // Instantiate 3D Image
        GameObject newSpeakerImage = Instantiate(speakerImagePrefab, imageParent);
        newSpeakerImage.name = $"SpeakerImage_{id}";

        // Set position
        if (worldPosition.HasValue)
            newSpeakerImage.transform.position = worldPosition.Value;
        else
            newSpeakerImage.transform.localPosition = new Vector3(id * 3f, 16f, 14f);

        // Face the XR Origin
        if (xrOrigin != null)
        {
            Vector3 lookDir = xrOrigin.position - newSpeakerImage.transform.position;
            if (lookDir != Vector3.zero)
                newSpeakerImage.transform.rotation = Quaternion.LookRotation(lookDir);
        }

        // Store mapping
        _speakers[id] = new SpeakerPair
        {
            ui = newSpeakerUI,
            image = newSpeakerImage,
            gain = gainControl
        };

        // Keep counter in sync (max id)
        if (id > currentSpeakerCount) currentSpeakerCount = id;

        RelayoutUI();

        Debug.Log($"[SpeakerManager] Added Speaker {id} (UI + Image).");
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

            Vector3 pos = pair.image != null ? pair.image.transform.position : Vector3.zero;

            // getters for gain/eq/reverb from SpeakerGainControl.
            float mainGain = pair.gain != null ? pair.gain.GetMainGain() : 0f;
            float[] eq = pair.gain != null ? pair.gain.GetEQ6() : new float[6];
            float[] reverb = pair.gain != null ? pair.gain.GetReverb4() : new float[4];

            list.Add(new SpeakerSnapshot
            {
                id = id,
                posX = pos.x, posY = pos.y, posZ = pos.z,
                mainGain = mainGain,
                eq6 = eq,
                reverb4 = reverb
            });
        }

        return list;
    }


    private int GetNextAvailableId()
    {
        return GetMaxExistingId() + 1;
    }

    private int GetMaxExistingId()
    {
        int maxId = 0;
        foreach (var id in _speakers.Keys)
            if (id > maxId) maxId = id;
        return maxId;
    }

    private void RelayoutUI()
    {
        // Order UI by speaker ID top->bottom
        var ids = new List<int>(_speakers.Keys);
        ids.Sort();

        for (int i = 0; i < ids.Count; i++)
        {
            var ui = _speakers[ids[i]].ui;
            if (ui == null) continue;

            ui.transform.localPosition = new Vector3(0, -(i + 1) * uiRowHeight, 0);
        }
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
