using System;
using System.IO;
using UnityEngine;

[Serializable]

public class SaveLoadManager : MonoBehaviour
{
    public SpeakerManager speakerManager;
    public string fileName = "session.json";

    private string FilePath => Path.Combine(Application.persistentDataPath, fileName);

    public void Save()
    {
        var session = new SessionSnapshot();
        session.speakers = speakerManager.GetSnapshot();

        string json = JsonUtility.ToJson(session, true);
        File.WriteAllText(FilePath, json);

        Debug.Log($"[SAVE] Saved to: {FilePath}");
    }

    public void Load()
    {
        if (!File.Exists(FilePath))
        {
            Debug.LogWarning($"[LOAD] File not found: {FilePath}");
            return;
        }

        string json = File.ReadAllText(FilePath);
        var session = JsonUtility.FromJson<SessionSnapshot>(json);

        if (session?.speakers == null)
        {
            Debug.LogError("[LOAD] Invalid JSON session.");
            return;
        }

        speakerManager.ClearAllSpeakers();

        foreach (var s in session.speakers)
        {
            var gainControl = speakerManager.AddSpeakerWithID(
                s.id,
                new Vector3(s.posX, s.posY, s.posZ)
            );

            if (gainControl != null)
            {
                gainControl.SetMainGain(s.mainGain);
            }
        }

        Debug.Log($"[LOAD] Loaded {session.speakers.Count} speakers from: {FilePath}");
    }
}