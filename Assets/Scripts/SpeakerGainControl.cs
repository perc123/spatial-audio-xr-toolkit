using UnityEngine;
using UnityEngine.UI;
using TMPro;
using extOSC;
using UnityEngine.Events; // Assuming you're using extOSC

public class SpeakerGainControl : MonoBehaviour
{
    public int speakerID = 1;  // This will define the OSC address: /vol{ID}

    public Toggle muteToggle;
    public TextMeshProUGUI gainLevelText;
    public Slider gainSlider;
    public TextMeshProUGUI speakerNumberText;

    public OSCTransmitter transmitter;

    private bool isMuted = false;

    void Start()
    {
        // If no transmitter is assigned, try to find the OSC Transmitter in the scene
        if (transmitter == null)
        {
            transmitter = FindObjectOfType<OSCTransmitter>();

            if (transmitter == null)
                Debug.LogError("[SpeakerGainControl] OSCTransmitter not found in scene!");
        }

        // Update speaker number label
        if (speakerNumberText != null)
            speakerNumberText.text = $"Speaker {speakerID}";

        // Initialize gain text
        UpdateGainLevelText(gainSlider.value);

        // Add listeners
        gainSlider.onValueChanged.AddListener(OnSliderValueChanged);
        muteToggle.onValueChanged.AddListener(OnMuteButtonClicked());
    }

    void OnSliderValueChanged(float value)
    {
        if (!isMuted)
        {
            UpdateGainLevelText(value);
            SendGainValue(value);
        }
    }

    UnityAction<bool> OnMuteButtonClicked()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            gainLevelText.text = "MUTED";
            SendMuteValue(true);
        }
        else
        {
            UpdateGainLevelText(gainSlider.value);
            SendMuteValue(false);
        }

        return null;
    }

    void UpdateGainLevelText(float value)
    {
        gainLevelText.text = value.ToString("0.00");
    }

    void SendGainValue(float value)
    {
        if (transmitter != null)
        {
            var message = new OSCMessage($"/vol{speakerID}");
            message.AddValue(OSCValue.Float(value));
            transmitter.Send(message);

            Debug.Log($"[OSC] Sent /vol{speakerID}: {value}");
        }
    }

    void SendMuteValue(bool mute)
    {
        if (transmitter != null)
        {
            var message = new OSCMessage($"/mute{speakerID}");
            message.AddValue(OSCValue.Bool(mute));
            transmitter.Send(message);

            Debug.Log($"[OSC] Sent /mute{speakerID}: {mute}");
        }
    }
}
