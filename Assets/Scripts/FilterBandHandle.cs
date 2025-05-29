using UnityEngine;
using UnityEngine.EventSystems;
using extOSC;

public class FilterBandHandle : MonoBehaviour, IDragHandler
{ 
    [SerializeField] private int bandIndex;
    [SerializeField] private RectTransform graphArea;
    [SerializeField] private OSCTransmitter transmitter;

    [SerializeField] private float minFreq = 20f;
    [SerializeField] private float maxFreq = 20000f;
    [SerializeField] private float minGain = -24f;
    [SerializeField] private float maxGain = 24f;

    [SerializeField] private string oscAddressRoot = "/filter";

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(graphArea, eventData.position, eventData.pressEventCamera, out localPoint);

        // Clamp position inside graph area
        Vector2 clamped = new Vector2(
            Mathf.Clamp(localPoint.x, -graphArea.rect.width / 2, graphArea.rect.width / 2),
            Mathf.Clamp(localPoint.y, -graphArea.rect.height / 2, graphArea.rect.height / 2)
        );

        GetComponent<RectTransform>().localPosition = clamped;

        float freqNorm = (clamped.x + graphArea.rect.width / 2) / graphArea.rect.width;
        float gainNorm = (clamped.y + graphArea.rect.height / 2) / graphArea.rect.height;

        float frequency = Mathf.Exp(Mathf.Lerp(Mathf.Log(minFreq), Mathf.Log(maxFreq), freqNorm));
        float gain = Mathf.Lerp(minGain, maxGain, gainNorm);

        SendFilterUpdate(frequency, gain);
    }

    private void SendFilterUpdate(float freq, float gain)
    {
        var msg = new OSCMessage($"{oscAddressRoot}");
                                 //$"/{bandIndex}");
        msg.AddValue(OSCValue.Int(bandIndex));
        msg.AddValue(OSCValue.Float(freq));
        msg.AddValue(OSCValue.Float(gain));
        transmitter.Send(msg);

        Debug.Log($"[OSC] Sent /filter/{bandIndex} freq: {freq} Hz, gain: {gain} dB");
    }
}
