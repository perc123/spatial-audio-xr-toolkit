using UnityEngine;
using extOSC;

public class UserTransformSender : MonoBehaviour
{
    public Transform xrOrigin; // User XR Rig or Camera Origin
    public OSCTransmitter transmitter;

    private Vector3 lastPos;
    private Quaternion lastRot;
    private float threshold = 0.01f;

    void Update()
    {
        if (xrOrigin == null || transmitter == null) return;

        Vector3 pos = xrOrigin.position;
        Quaternion rot = xrOrigin.rotation;

        if (Vector3.Distance(pos, lastPos) > threshold || Quaternion.Angle(rot, lastRot) > 0.5f)
        {
            // Send position
            var posMsg = new OSCMessage("/listener/xyz");
            posMsg.AddValue(OSCValue.Float(pos.x));
            posMsg.AddValue(OSCValue.Float(pos.y));
            posMsg.AddValue(OSCValue.Float(pos.z));
            transmitter.Send(posMsg);

            // Convert to Euler angles (in degrees)
            Vector3 euler = rot.eulerAngles;
            float yaw = euler.y;
            float pitch = euler.x;
            float roll = euler.z;

            // Send orientation
            var rotMsg = new OSCMessage("/listener/ypr");
            rotMsg.AddValue(OSCValue.Float(yaw));
            rotMsg.AddValue(OSCValue.Float(pitch));
            rotMsg.AddValue(OSCValue.Float(roll));
            transmitter.Send(rotMsg);

            lastPos = pos;
            lastRot = rot;

            Debug.Log($"[OSC] Sent /listener/xyz ({pos}) and /listener/ypr ({yaw}, {pitch}, {roll})");
        }
    }
}
