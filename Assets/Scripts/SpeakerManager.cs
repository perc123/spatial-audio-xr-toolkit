using UnityEngine;

public class SpeakerManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject speakerUIPrefab;    // SpeakerGain UI Prefab
    public GameObject speakerImagePrefab; // 3D Speaker Image Prefab

    [Header("Parent Transforms")]
    public Transform uiParent;     // Where UI speakers will appear (scrollview, panel, etc.)
    public Transform imageParent;  // Where 3D speakers will appear (3D scene container)

    [Header("Speaker Counter")]
    public int currentSpeakerCount = 0;

    // Method to add a new speaker (both UI and 3D)
    public void AddSpeaker()
    {
        currentSpeakerCount++;

        // --- Instantiate UI Prefab ---
        GameObject newSpeakerUI = Instantiate(speakerUIPrefab, uiParent);
        newSpeakerUI.transform.localPosition = new Vector3(0, -currentSpeakerCount * 100, 0); // Example positioning

        // Set speaker ID in UI script
        SpeakerGainControl gainControl = newSpeakerUI.GetComponent<SpeakerGainControl>();
        if (gainControl != null)
        {
            gainControl.speakerID = currentSpeakerCount;
            Debug.Log($"[SpeakerManager] Added Speaker UI {currentSpeakerCount}");
        }

        // --- Instantiate 3D Image Prefab ---
        GameObject newSpeakerImage = Instantiate(speakerImagePrefab, imageParent);
        newSpeakerImage.transform.localPosition = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f)); // Randomize in scene

        // Optionally set name or ID for tracking
        newSpeakerImage.name = $"SpeakerImage_{currentSpeakerCount}";

        Debug.Log($"[SpeakerManager] Added SpeakerImage {currentSpeakerCount}");
    }

    // Method to remove the last speaker (both UI and 3D)
    public void RemoveSpeaker()
    {
        if (uiParent.childCount > 0 && imageParent.childCount > 0)
        {
            // Remove last UI speaker
            Transform lastUI = uiParent.GetChild(uiParent.childCount - 1);
            Destroy(lastUI.gameObject);

            // Remove last 3D speaker
            Transform lastImage = imageParent.GetChild(imageParent.childCount - 1);
            Destroy(lastImage.gameObject);

            currentSpeakerCount--;
            Debug.Log($"[SpeakerManager] Removed Speaker, new count: {currentSpeakerCount}");
        }
    }
}
