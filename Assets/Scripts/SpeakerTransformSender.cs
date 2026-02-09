using UnityEngine;
using extOSC;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]

public class SpeakerTransformSender : MonoBehaviour
{

    public int sourceID = 1; // speaker ID
    public OSCTransmitter transmitter;

    private Vector3 lastPosition;
    private float threshold = 0.01f;

    private XRGrabInteractable _grab;
    private bool _isGrabbed;

    private void Awake()
    {
        _grab = GetComponent<XRGrabInteractable>();
    }
    
    private void OnEnable()
    {
        _grab.selectEntered.AddListener(OnSelectEntered);
        _grab.selectExited.AddListener(OnSelectExited);
    }

    private void OnDisable()
    {
        _grab.selectEntered.RemoveListener(OnSelectEntered);
        _grab.selectExited.RemoveListener(OnSelectExited);
    }
    void Update()
    {
        
        if (_isGrabbed)
        {
            SpeakerUI.Instance?.UpdateActiveSpeakerPose(sourceID, transform.position);
        }
        
        
        if (transmitter == null) return;

        Vector3 pos = transform.position;

        if (Vector3.Distance(pos, lastPosition) > threshold)
        {
            var msg = new OSCMessage("/source/xyz");
            msg.AddValue(OSCValue.Int(sourceID));
            msg.AddValue(OSCValue.Float(pos.x/10f)); 
            msg.AddValue(OSCValue.Float(pos.z/10f)); // reverse y and z because spat accepts them the other way around
            msg.AddValue(OSCValue.Float(pos.y/10f));

            transmitter.Send(msg);
            lastPosition = pos;

            Debug.Log($"[OSC] Sent /source/xyz {sourceID}: ({pos.x}, {pos.y}, {pos.z})");
        }
    }
    
    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        _isGrabbed = true;
        SpeakerUI.Instance?.SetActiveSpeaker(sourceID, transform.position);
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        _isGrabbed = false;
        SpeakerUI.Instance?.ClearActiveSpeaker(sourceID, transform.position);
    }
}