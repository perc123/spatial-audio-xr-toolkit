using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public SpeakerManager speakerManager;

    private string SaveDirectory => Application.persistentDataPath;
    private const string SavePrefix = "session_";
    private const string SaveExtension = ".json";

    // Saves with an auto-generated timestamp name. Returns the filename.
    public string Save(string customName = null)
    {
        string saveName = string.IsNullOrWhiteSpace(customName)
            ? DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")
            : customName.Trim();

        // Sanitize: remove characters that are invalid in filenames
        foreach (char c in Path.GetInvalidFileNameChars())
            saveName = saveName.Replace(c.ToString(), "_");

        string fileName = SavePrefix + saveName + SaveExtension;
        string filePath = Path.Combine(SaveDirectory, fileName);

        var session = new SessionSnapshot();
        session.speakers = speakerManager.GetSnapshot();

        string json = JsonUtility.ToJson(session, true);
        File.WriteAllText(filePath, json);

        Debug.Log($"[SAVE] Saved to: {filePath}");
        return fileName;
    }

    // Returns all save filenames, newest first.
    public List<string> GetSaveFiles()
    {
        var files = new List<string>();

        if (!Directory.Exists(SaveDirectory))
            return files;

        string[] found = Directory.GetFiles(SaveDirectory, SavePrefix + "*" + SaveExtension);
        foreach (string f in found)
            files.Add(Path.GetFileName(f));

        files.Sort();
        files.Reverse(); // newest first (timestamp names sort correctly)
        return files;
    }

    public void LoadFromFile(string fileName)
    {
        string filePath = Path.Combine(SaveDirectory, fileName);

        if (!File.Exists(filePath))
        {
            Debug.LogWarning($"[LOAD] File not found: {filePath}");
            return;
        }

        string json = File.ReadAllText(filePath);
        var session = JsonUtility.FromJson<SessionSnapshot>(json);

        if (session?.speakers == null)
        {
            Debug.LogError("[LOAD] Invalid or empty session file.");
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

                var state = gainControl.GetComponent<SpeakerState>();
                if (state != null)
                {
                    state.ApplyData(new SpeakerData
                    {
                        id       = s.id,
                        posX     = s.posX, posY = s.posY, posZ = s.posZ,
                        mainGain = s.mainGain,
                        eq       = s.eq6,
                        reverb   = s.reverb4,
                    }, updateUIText: false);
                }
            }
        }

        Debug.Log($"[LOAD] Loaded {session.speakers.Count} speakers from: {filePath}");
    }

    public void DeleteSave(string fileName)
    {
        string filePath = Path.Combine(SaveDirectory, fileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log($"[SAVE] Deleted: {filePath}");
        }
    }
}
