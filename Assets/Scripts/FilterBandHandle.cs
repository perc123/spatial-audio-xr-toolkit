using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using extOSC;

[RequireComponent(typeof(RectTransform))]
public class FilterBandHandle : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [Header("Band")]
    [SerializeField] private int bandIndex = 0;

    [Header("UI")]
    [SerializeField] private RectTransform graphArea;

    [Header("OSC")]
    [SerializeField] private OSCTransmitter transmitter;
    [SerializeField] private string oscAddressRoot = "/filter";

    [Header("Mapping")]
    [SerializeField] private float minFreq = 20f;
    [SerializeField] private float maxFreq = 20000f;
    [SerializeField] private float minGain = -24f;
    [SerializeField] private float maxGain = 24f;

    private RectTransform _self;
    private bool _allowDrag = true;

    private void Awake()
    {
        _self = GetComponent<RectTransform>();
        if (graphArea == null) graphArea = _self.parent as RectTransform;
    }

    private void Start()
    {
        if (transmitter == null)
            transmitter = FindObjectOfType<OSCTransmitter>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // If the drag started on a Slider (or inside one), do NOT move this object.
        // This prevents fighting with Unity's Slider drag logic (especially in XR).
        _allowDrag = true;

        if (eventData.pointerPress != null)
        {
            var slider = eventData.pointerPress.GetComponentInParent<Slider>();
            if (slider != null)
                _allowDrag = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_allowDrag) return;
        if (graphArea == null || transmitter == null) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            graphArea, eventData.position, eventData.pressEventCamera, out var localPoint);

        float halfW = graphArea.rect.width * 0.5f;
        float halfH = graphArea.rect.height * 0.5f;

        Vector2 clamped = new Vector2(
            Mathf.Clamp(localPoint.x, -halfW, halfW),
            Mathf.Clamp(localPoint.y, -halfH, halfH)
        );

        _self.localPosition = clamped;

        float freqNorm = (clamped.x + halfW) / (halfW * 2f);
        float gainNorm = (clamped.y + halfH) / (halfH * 2f);

        float frequency = Mathf.Exp(Mathf.Lerp(Mathf.Log(minFreq), Mathf.Log(maxFreq), Mathf.Clamp01(freqNorm)));
        float gain = Mathf.Lerp(minGain, maxGain, Mathf.Clamp01(gainNorm));

        SendFilterUpdate(frequency, gain);
    }

    private void SendFilterUpdate(float freq, float gain)
    {
        string address = $"{oscAddressRoot}/{bandIndex}";
        var msg = new OSCMessage(address);
        msg.AddValue(OSCValue.Float(freq));
        msg.AddValue(OSCValue.Float(gain));
        transmitter.Send(msg);
    }
}
