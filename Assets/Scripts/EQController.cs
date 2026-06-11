using UnityEngine;
using UnityEngine.UI;
using extOSC;

public class EQController : MonoBehaviour
{
    [Header("Speaker / Channel")]
    [Tooltip("Which speaker/channel this EQ controls. If you don't need per-speaker EQ, leave at 1 and ignore.")]
    public int speakerID = 1;

    [Header("UI (6 gain sliders)")]
    public Slider[] bandGainSliders = new Slider[6];

    [Header("OSC")]
    public OSCTransmitter transmitter;
    [Tooltip("Sends to /filter/<bandIndex> with args: freqHz gainDb")]
    public string oscAddressRoot = "/filter";

    [Header("Fixed band frequencies (Hz)")]
    // 40 Hz, 130 Hz, 550 Hz, 1.6 kHz, 4.3 kHz, 12 kHz
    public readonly float[] bandFrequenciesHz = new float[6] {0, 1, 2, 3, 4, 5};

    [Header("Send behavior")]
    public bool sendAllOnStart = true;
    public float sendEpsilonDb = 0.01f;

    private float[] _lastSentGain;
    private string[] _bandAddresses;

    private void Awake()
    {
        if (bandGainSliders == null || bandGainSliders.Length != 6)
            bandGainSliders = new Slider[6];

        _lastSentGain = new float[6];
        for (int i = 0; i < _lastSentGain.Length; i++)
            _lastSentGain[i] = float.NaN;
    }

    private void Start()
    {
        if (transmitter == null)
        {
            transmitter = FindObjectOfType<OSCTransmitter>();
            if (transmitter == null)
                Debug.LogError("[EQController] OSCTransmitter not found in scene!");
        }

        _bandAddresses = new string[6];
        for (int i = 0; i < 6; i++) _bandAddresses[i] = $"{oscAddressRoot}/{i}";

        for (int i = 0; i < 6; i++)
        {
            int band = i;
            var s = bandGainSliders[band];
            if (s == null) continue;
            s.onValueChanged.AddListener((gainDb) => OnBandGainChanged(band, gainDb));
        }

        if (sendAllOnStart)
            SendAllBands();
    }

    // Max's filtergraph~ setfilter expects gain as an amplitude ratio (log scale),
    // not dB. 0 dB = 1.0, +18 dB ≈ 7.94, -18 dB ≈ 0.126.
    // Sliders remain in dB (linear, symmetric) for the user; we convert on send.
    private static float DbToAmplitude(float db) => Mathf.Pow(10f, db / 20f);

    private void OnBandGainChanged(int bandIndex, float gainDb)
    {
        if (transmitter == null) return;
        if (bandIndex < 0 || bandIndex >= 6) return;

        if (!float.IsNaN(_lastSentGain[bandIndex]) && Mathf.Abs(gainDb - _lastSentGain[bandIndex]) < sendEpsilonDb)
            return;

        float freq = bandFrequenciesHz[bandIndex];
        SendBand(bandIndex, freq, gainDb);
        _lastSentGain[bandIndex] = gainDb;
    }

    public void SendAllBands()
    {
        for (int band = 0; band < 6; band++)
        {
            var s = bandGainSliders[band];
            if (s == null) continue;

            float freq   = bandFrequenciesHz[band];
            float gainDb = s.value;
            SendBand(band, freq, gainDb);
            _lastSentGain[band] = gainDb;
        }
    }

    private void SendBand(int bandIndex, float banclaudedNum, float gainDb)
    {
        // Convert dB → amplitude for filtergraph~ (log scale: above 0 = boost >1.0, below 0 = cut <1.0)
        float amplitude = DbToAmplitude(gainDb);

        var msg = new OSCMessage(_bandAddresses[bandIndex]);
        msg.AddValue(OSCValue.Float(bandNum));
        msg.AddValue(OSCValue.Float(amplitude));
        transmitter.Send(msg);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        Debug.Log($"[OSC] Sent {_bandAddresses[bandIndex]} Band:{bandNum} gain:{gainDb}dB (amp:{amplitude:F4})");
#endif
    }
}
