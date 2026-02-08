using UnityEngine;
using UnityEngine.UI;
using extOSC;

public class VolumeSender : MonoBehaviour
{
    public Slider speaker1Slider;
    //public Slider speaker2Slider;
    //public Slider speaker3Slider;
    //public Slider speaker4Slider;

    public OSCTransmitter transmitter; 

    void Start()
    {
        if (transmitter == null)
        {
            Debug.LogError("[VolumeSender] OSCTransmitter not assigned.");
            return;
        }

      
        speaker1Slider.onValueChanged.AddListener((val) => SendVolume("/vol1", val));
        /*speaker2Slider.onValueChanged.AddListener((val) => SendVolume("/vol2", val));
        speaker3Slider.onValueChanged.AddListener((val) => SendVolume("/vol3", val));
        speaker4Slider.onValueChanged.AddListener((val) => SendVolume("/vol4", val));*/
    }

    void SendVolume(string address, float volume)
    {
        var message = new OSCMessage(address);
        message.AddValue(OSCValue.Float(volume));
        transmitter.Send(message);

        Debug.Log($"[OSC] Sent {address}: {volume}");
    }
}