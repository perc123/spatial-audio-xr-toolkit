using UnityEngine;
using TMPro;

public class SpeakerUI : MonoBehaviour
{
    public static SpeakerUI Instance { get; private set; }

    [Header("Assign existing TMP texts")]
    [SerializeField] private TMP_Text ActiveSpeakerText;
    [SerializeField] private TMP_Text ActiveLocationText;

    [Header("Formatting")]
    [SerializeField] private int decimals = 2;

    private int _activeSpeakerId = -1;

    // Dirty-check state — avoids TMP mesh rebuilds when nothing changed
    private int _lastRenderedSpeakerId = -1;
    private Vector3 _lastRenderedPos;
    private string _fmt;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        _fmt = "F" + decimals;
        RenderIdle();
    }

    public void SetActiveSpeaker(int speakerId, Vector3 position)
    {
        _activeSpeakerId = speakerId;
        RenderMoving(speakerId, position);
    }

    public void UpdateActiveSpeakerPose(int speakerId, Vector3 position)
    {
        if (_activeSpeakerId != speakerId) return;
        RenderMoving(speakerId, position);
    }

    public void ClearActiveSpeaker(int speakerId, Vector3 position)
    {
        if (_activeSpeakerId == speakerId)
            _activeSpeakerId = -1;

        RenderIdle();
    }

    private void RenderMoving(int speakerId, Vector3 pos)
    {
        // Skip TMP writes if nothing changed — avoids canvas dirty + mesh rebuild
        if (_lastRenderedSpeakerId == speakerId && pos == _lastRenderedPos) return;
        _lastRenderedSpeakerId = speakerId;
        _lastRenderedPos = pos;

        if (ActiveSpeakerText != null)
            ActiveSpeakerText.text = $"Moving speaker: {speakerId}";

        if (ActiveLocationText != null)
            ActiveLocationText.text = $"Location: {Fmt(pos.x)}, {Fmt(pos.y)}, {Fmt(pos.z)}";
    }

    private void RenderIdle()
    {
        _lastRenderedSpeakerId = -1;

        if (ActiveSpeakerText != null)
            ActiveSpeakerText.text = "Moving speaker: -";

        if (ActiveLocationText != null)
            ActiveLocationText.text = "Location: -";
    }

    private string Fmt(float v) => v.ToString(_fmt);
}
