using UnityEngine;
using UnityEngine.UI;
using TMPro;
using extOSC;

public class SpeakerGainControl : MonoBehaviour
{
    [Header("Identity")]
    public int speakerID = 1;

    [Header("UI - Main Gain")]
    public Toggle muteToggle;
    public TextMeshProUGUI gainLevelText;
    public Slider gainSlider;
    public TextMeshProUGUI speakerNumberText;

    [Header("UI - EQ (6 bands)")]
    public Slider[] eqSliders = new Slider[6];

    [Header("UI - Reverb (4 params)")]
    public Slider[] reverbSliders = new Slider[4];

    [Header("OSC")]
    public OSCTransmitter transmitter;

    private bool isMuted = false;

    private void Awake()
    {
        if (eqSliders == null || eqSliders.Length != 6) eqSliders = new Slider[6];
        if (reverbSliders == null || reverbSliders.Length != 4) reverbSliders = new Slider[4];
    }

    private void Start()
    {
        if (transmitter == null)
        {
            transmitter = FindObjectOfType<OSCTransmitter>();
            if (transmitter == null)
                Debug.LogError("[SpeakerGainControl] OSCTransmitter not found in scene!");
        }

        if (speakerNumberText != null)
            speakerNumberText.text = $"Speaker {speakerID}";

        if (gainSlider != null)
            UpdateGainLevelText(gainSlider.value);

        // Listeners
        if (gainSlider != null)
            gainSlider.onValueChanged.AddListener(OnSliderValueChanged);

        if (muteToggle != null)
        {
            isMuted = muteToggle.isOn;
            muteToggle.onValueChanged.AddListener(OnMuteToggleChanged);
            ApplyMuteState(isMuted, sendOsc: true);
        }

        // EQ + reverb listeners for OSC updates
        HookEQListeners();
        HookReverbListeners();
    }

    // -----------------------------
    // MAIN GAIN / MUTE
    // -----------------------------

    private void OnSliderValueChanged(float value)
    {
        if (isMuted)
        {
            if (gainLevelText != null) gainLevelText.text = "MUTED";
            return;
        }

        UpdateGainLevelText(value);
        SendGainValue(value);
    }

    private void OnMuteToggleChanged(bool muted)
    {
        isMuted = muted;
        ApplyMuteState(isMuted, sendOsc: true);
    }

    private void ApplyMuteState(bool muted, bool sendOsc)
    {
        if (muted)
        {
            if (gainLevelText != null) gainLevelText.text = "MUTED";
            if (sendOsc) SendMuteValue(true);

        }
        else
        {
            if (gainSlider != null) UpdateGainLevelText(gainSlider.value);
            if (sendOsc) SendMuteValue(false);

            // re-send current gain after unmute:
            if (sendOsc && gainSlider != null) SendGainValue(gainSlider.value);
        }
    }

    private void UpdateGainLevelText(float value)
    {
        if (gainLevelText != null)
            gainLevelText.text = value.ToString("0.00");
    }

    private void SendGainValue(float value)
    {
        if (transmitter == null) return;

        var message = new OSCMessage($"/vol{speakerID}");
        message.AddValue(OSCValue.Float(value));
        transmitter.Send(message);

        Debug.Log($"[OSC] Sent /vol{speakerID}: {value}");
    }

    private void SendMuteValue(bool mute)
    {
        if (transmitter == null) return;

        var message = new OSCMessage($"/mute{speakerID}");
        message.AddValue(OSCValue.Bool(mute));
        transmitter.Send(message);

        Debug.Log($"[OSC] Sent /mute{speakerID}: {mute}");
    }

    // -----------------------------
    // EQ (6 bands) - OPTIONAL OSC
    // -----------------------------

    private void HookEQListeners()
    {
        for (int i = 0; i < eqSliders.Length; i++)
        {
            int band = i;
            var s = eqSliders[band];
            if (s == null) continue;

            s.onValueChanged.AddListener((val) => SendEQBand(band, val));
        }
    }

    private void SendEQBand(int bandIndex, float value)
    {
        if (transmitter == null) return;
        
        // /eq{speakerID}/{bandIndex} value
        var msg = new OSCMessage($"/eq{speakerID}/{bandIndex}");
        msg.AddValue(OSCValue.Float(value));
        transmitter.Send(msg);

        Debug.Log($"[OSC] Sent /eq{speakerID}/{bandIndex}: {value}");
    }

    // -----------------------------
    // REVERB (4 params)
    // -----------------------------

    private void HookReverbListeners()
    {
        for (int i = 0; i < reverbSliders.Length; i++)
        {
            int p = i;
            var s = reverbSliders[p];
            if (s == null) continue;

            s.onValueChanged.AddListener((val) => SendReverbParam(p, val));
        }
    }

    private void SendReverbParam(int paramIndex, float value)
    {
        if (transmitter == null) return;

      
        // /rev{speakerID}/{paramIndex} value
        var msg = new OSCMessage($"/rev{speakerID}/{paramIndex}");
        msg.AddValue(OSCValue.Float(value));
        transmitter.Send(msg);

        Debug.Log($"[OSC] Sent /rev{speakerID}/{paramIndex}: {value}");
    }

    // -----------------------------
    // SAVE / LOAD API
    // -----------------------------

    public float GetMainGain() => gainSlider != null ? gainSlider.value : 0f;

    public float[] GetEQ6()
    {
        var arr = new float[6];
        for (int i = 0; i < 6; i++)
            arr[i] = (eqSliders != null && i < eqSliders.Length && eqSliders[i] != null) ? eqSliders[i].value : 0f;
        return arr;
    }

    public float[] GetReverb4()
    {
        var arr = new float[4];
        for (int i = 0; i < 4; i++)
            arr[i] = (reverbSliders != null && i < reverbSliders.Length && reverbSliders[i] != null) ? reverbSliders[i].value : 0f;
        return arr;
    }

    public void SetMainGain(float v)
    {
        if (gainSlider != null) gainSlider.value = v;
        if (!isMuted) UpdateGainLevelText(v);
    }

    public void SetEQ6(float[] v)
    {
        for (int i = 0; i < 6; i++)
            if (eqSliders != null && i < eqSliders.Length && eqSliders[i] != null)
                eqSliders[i].value = (v != null && v.Length > i) ? v[i] : 0f;
    }

    public void SetReverb4(float[] v)
    {
        for (int i = 0; i < 4; i++)
            if (reverbSliders != null && i < reverbSliders.Length && reverbSliders[i] != null)
                reverbSliders[i].value = (v != null && v.Length > i) ? v[i] : 0f;
    }
}
