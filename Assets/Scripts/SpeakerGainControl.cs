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

    // Pre-computed OSC address strings — avoids per-event string allocation
    private string _volAddress;
    private string _muteAddress;
    private string[] _eqAddresses;
    private string[] _revAddresses;

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

        _volAddress  = $"/vol{speakerID}";
        _muteAddress = $"/mute{speakerID}";
        _eqAddresses  = new string[6];
        for (int i = 0; i < 6; i++) _eqAddresses[i]  = $"/eq{speakerID}/{i}";
        _revAddresses = new string[4];
        for (int i = 0; i < 4; i++) _revAddresses[i] = $"/rev{speakerID}/{i}";

        if (speakerNumberText != null)
            speakerNumberText.text = $"Speaker {speakerID}";

        if (gainSlider != null)
            UpdateGainLevelText(gainSlider.value);

        if (gainSlider != null)
            gainSlider.onValueChanged.AddListener(OnSliderValueChanged);

        if (muteToggle != null)
        {
            isMuted = muteToggle.isOn;
            muteToggle.onValueChanged.AddListener(OnMuteToggleChanged);
            ApplyMuteState(isMuted, sendOsc: true);
        }

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
        var message = new OSCMessage(_volAddress);
        message.AddValue(OSCValue.Float(value));
        transmitter.Send(message);
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        Debug.Log($"[OSC] Sent {_volAddress}: {value}");
#endif
    }

    private void SendMuteValue(bool mute)
    {
        if (transmitter == null) return;
        var message = new OSCMessage(_muteAddress);
        message.AddValue(OSCValue.Bool(mute));
        transmitter.Send(message);
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        Debug.Log($"[OSC] Sent {_muteAddress}: {mute}");
#endif
    }

    // -----------------------------
    // EQ (6 bands)
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
        var msg = new OSCMessage(_eqAddresses[bandIndex]);
        msg.AddValue(OSCValue.Float(value));
        transmitter.Send(msg);
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        Debug.Log($"[OSC] Sent {_eqAddresses[bandIndex]}: {value}");
#endif
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
        var msg = new OSCMessage(_revAddresses[paramIndex]);
        msg.AddValue(OSCValue.Float(value));
        transmitter.Send(msg);
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        Debug.Log($"[OSC] Sent {_revAddresses[paramIndex]}: {value}");
#endif
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
