using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages the Save and Load UI panels.
///
/// SCENE SETUP:
/// 1. Assign SaveLoadManager reference.
/// 2. Load Panel: a GameObject (initially inactive) containing:
///    - A Scroll View whose Content transform is assigned to saveListContainer
///    - A Close button wired to OnLoadPanelClose()
/// 3. saveEntryPrefab: a prefab with three named children:
///    - "SaveNameText"  (TextMeshProUGUI)
///    - "LoadButton"    (Button)
///    - "DeleteButton"  (Button)
/// 4. saveStatusText: a TextMeshProUGUI that shows brief feedback after saving.
/// 5. Wire your Save button  → OnSaveClicked()
///    Wire your Load button  → OnLoadClicked()
/// </summary>
public class SaveLoadUI : MonoBehaviour
{
    [Header("Core")]
    public SaveLoadManager saveLoadManager;

    [Header("Load Panel")]
    public GameObject loadPanel;
    public Transform saveListContainer;  // Scroll View > Viewport > Content
    public GameObject saveEntryPrefab;   // see class comment for required structure

    [Header("Save Feedback")]
    public TextMeshProUGUI saveStatusText; // brief "Saved: ..." message
    private Coroutine _clearStatusCoroutine;

    private void Start()
    {
        if (loadPanel != null) loadPanel.SetActive(false);
        if (saveStatusText != null) saveStatusText.text = "";
    }

    // ── Public button callbacks ─────────────────────────────────────────────

    public void OnSaveClicked()
    {
        string fileName = saveLoadManager.Save();
        ShowStatus($"Saved:\n{StripAffixes(fileName)}");
    }

    public void OnLoadClicked()
    {
        PopulateSaveList();
        if (loadPanel != null) loadPanel.SetActive(true);
    }

    public void OnLoadPanelClose()
    {
        if (loadPanel != null) loadPanel.SetActive(false);
    }

    // ── Private helpers ──────────────────────────────────────────────────────

    private void PopulateSaveList()
    {
        // Clear existing entries
        foreach (Transform child in saveListContainer)
            Destroy(child.gameObject);

        List<string> files = saveLoadManager.GetSaveFiles();

        if (files.Count == 0)
        {
            SpawnNoSavesLabel();
            return;
        }

        foreach (string fileName in files)
            SpawnEntry(fileName);
    }

    private void SpawnEntry(string fileName)
    {
        GameObject entry = Instantiate(saveEntryPrefab, saveListContainer);

        // Label
        var label = entry.transform.Find("SaveNameText")?.GetComponent<TextMeshProUGUI>();
        if (label != null)
            label.text = StripAffixes(fileName);

        // Load button
        var loadBtn = entry.transform.Find("LoadButton")?.GetComponent<Button>();
        if (loadBtn != null)
        {
            string captured = fileName;
            loadBtn.onClick.AddListener(() =>
            {
                saveLoadManager.LoadFromFile(captured);
                OnLoadPanelClose();
                ShowStatus($"Loaded:\n{StripAffixes(captured)}");
            });
        }

        // Delete button
        var deleteBtn = entry.transform.Find("DeleteButton")?.GetComponent<Button>();
        if (deleteBtn != null)
        {
            string captured = fileName;
            deleteBtn.onClick.AddListener(() =>
            {
                saveLoadManager.DeleteSave(captured);
                PopulateSaveList(); // refresh
            });
        }
    }

    private void SpawnNoSavesLabel()
    {
        GameObject entry = Instantiate(saveEntryPrefab, saveListContainer);

        var label = entry.transform.Find("SaveNameText")?.GetComponent<TextMeshProUGUI>();
        if (label != null) label.text = "No saves found";

        // Hide action buttons — nothing to load/delete
        var loadBtn  = entry.transform.Find("LoadButton")?.GetComponent<Button>();
        var deleteBtn = entry.transform.Find("DeleteButton")?.GetComponent<Button>();
        if (loadBtn != null)  loadBtn.gameObject.SetActive(false);
        if (deleteBtn != null) deleteBtn.gameObject.SetActive(false);
    }

    private void ShowStatus(string message)
    {
        if (saveStatusText == null) return;

        if (_clearStatusCoroutine != null)
            StopCoroutine(_clearStatusCoroutine);

        saveStatusText.text = message;
        _clearStatusCoroutine = StartCoroutine(ClearStatusAfterDelay(3f));
    }

    private IEnumerator ClearStatusAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (saveStatusText != null) saveStatusText.text = "";
    }

    // "session_2026-04-08_14-30-00.json" → "2026-04-08_14-30-00"
    private static string StripAffixes(string fileName)
    {
        return fileName.Replace("session_", "").Replace(".json", "");
    }
}
