using UnityEngine;
using UnityEngine.UI;
using TMPro;
using extOSC;

public class SpeakerGainControl : MonoBehaviour
{
    [Header("Identity")]
    public int speakerID = 1;

    [Header("UI")]
    public Toggle muteToggle;
    public Slider gainSlider;
    public TextMeshProUGUI gainLevelText;
    public TextMeshProUGUI speakerNumberText;

    [Header("OSC")]
    public OSCTransmitter transmitter;

    private bool _isMuted = false;
    private string _volAddress;

    private void Start()
    {
        if (transmitter == null)
        {
            transmitter = FindObjectOfType<OSCTransmitter>();
            if (transmitter == null)
                Debug.LogError("[SpeakerGainControl] OSCTransmitter not found in scene!");
        }

        _volAddress = $"/vol{speakerID}";

        if (speakerNumberText != null)
            speakerNumberText.text = $"Speaker {speakerID}";

        if (gainSlider != null)
        {
            UpdateGainText(gainSlider.value);
            gainSlider.onValueChanged.AddListener(OnGainSliderChanged);
        }

        if (muteToggle != null)
        {
            _isMuted = muteToggle.isOn;
            muteToggle.onValueChanged.AddListener(OnMuteToggleChanged);
        }

        // Send initial state
        SendCurrentGain();
    }

    private void OnGainSliderChanged(float value)
    {
        if (_isMuted) return;
        UpdateGainText(value);
        SendGain(value);
    }

    private void OnMuteToggleChanged(bool muted)
    {
        _isMuted = muted;

        if (_isMuted)
        {
            if (gainLevelText != null) gainLevelText.text = "MUTED";
            SendGain(0f);
        }
        else
        {
            float restored = gainSlider != null ? gainSlider.value : 0f;
            UpdateGainText(restored);
            SendGain(restored);
        }
    }

    private void SendCurrentGain()
    {
        float value = _isMuted ? 0f : (gainSlider != null ? gainSlider.value : 0f);
        SendGain(value);
    }

    private void SendGain(float value)
    {
        if (transmitter == null) return;
        var msg = new OSCMessage(_volAddress);
        msg.AddValue(OSCValue.Float(value));
        transmitter.Send(msg);
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        Debug.Log($"[OSC] Sent {_volAddress}: {value}");
#endif
    }

    private void UpdateGainText(float value)
    {
        if (gainLevelText != null)
            gainLevelText.text = value.ToString("0.00");
    }

    // --- Save / Load API ---

    public float GetMainGain() => gainSlider != null ? gainSlider.value : 0f;
    public bool GetMuted() => _isMuted;

    public void SetMainGain(float v)
    {
        if (gainSlider != null) gainSlider.value = v;
        if (!_isMuted) UpdateGainText(v);
    }

    public void SetMuted(bool muted)
    {
        if (muteToggle != null) muteToggle.isOn = muted;
        else OnMuteToggleChanged(muted);
    }
}
