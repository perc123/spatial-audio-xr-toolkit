using UnityEngine;
using UnityEngine.UI;
using extOSC;

public class ReverbOscController : MonoBehaviour
{
    [Header("OSC (Unity<-Max: 9001, Unity->Max: 6161)")]
    public OSCTransmitter transmitter; 
    public OSCReceiver receiver;      

    [Header("Reverb Sliders (4 params)")]
    public Slider[] reverbSliders = new Slider[4];

    // Prevents feedback loops (Max echoes back what Unity sent)
    private bool _applyingIncoming = false;

    private void Awake()
    {
        if (reverbSliders == null || reverbSliders.Length != 4)
            reverbSliders = new Slider[4];
    }

    private void Start()
    {
        if (transmitter == null) transmitter = FindObjectOfType<OSCTransmitter>();
        if (receiver == null) receiver = FindObjectOfType<OSCReceiver>();

        if (transmitter == null) Debug.LogError("[ReverbOscController] OSCTransmitter missing.");
        if (receiver == null) Debug.LogError("[ReverbOscController] OSCReceiver missing.");

        // UI -> Max
        for (int i = 0; i < reverbSliders.Length; i++)
        {
            int p = i;
            if (reverbSliders[p] == null) continue;

            reverbSliders[p].onValueChanged.AddListener(v => OnSliderChanged(p, v));
        }

        // Max -> Unity
        receiver.Bind("/reverb/0", msg => ApplyIncoming(0, msg));
        receiver.Bind("/reverb/1", msg => ApplyIncoming(1, msg));
        receiver.Bind("/reverb/2", msg => ApplyIncoming(2, msg));
        receiver.Bind("/reverb/3", msg => ApplyIncoming(3, msg));
    }

    private void OnSliderChanged(int paramIndex, float value)
    {
        if (_applyingIncoming) return;

        // Send /reverb/<paramIndex> <float>
        var msg = new OSCMessage($"/reverb/{paramIndex}");
        msg.AddValue(OSCValue.Float(value));
        transmitter.Send(msg);
        
        Debug.Log($"[OSC] Sent /reverb/{paramIndex} {value}");
    }

    private void ApplyIncoming(int paramIndex, OSCMessage msg)
    {
        if (msg.Values.Count < 1) return;
        if (paramIndex < 0 || paramIndex >= reverbSliders.Length) return;
        if (reverbSliders[paramIndex] == null) return;

        float v = msg.Values[0].FloatValue;

        _applyingIncoming = true;
        reverbSliders[paramIndex].value = v;
        _applyingIncoming = false;
        
        Debug.Log($"[OSC] Received /reverb/{paramIndex} {v}");
    }
}
