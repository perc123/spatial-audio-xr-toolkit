using UnityEngine;
using extOSC;

public class UserTransformSender : MonoBehaviour
{
    public Transform xrOrigin;
    public OSCTransmitter transmitter;

    private Vector3 lastPos;
    private Quaternion lastRot;
    private float threshold = 0.01f;
    private float _thresholdSq;
    // Equivalent to Quaternion.Angle > 0.5 degrees, avoids Acos per frame.
    // Derived from: 1 - cos(angleDeg/2 * Deg2Rad) where angleDeg = 0.5
    private float _rotDotThreshold;

    private void Awake()
    {
        _thresholdSq = threshold * threshold;
        _rotDotThreshold = 1f - Mathf.Cos(0.25f * Mathf.Deg2Rad);
    }

    void Update()
    {
        if (xrOrigin == null || transmitter == null) return;

        Vector3 pos = xrOrigin.position;
        Quaternion rot = xrOrigin.rotation;

        bool posChanged = (pos - lastPos).sqrMagnitude > _thresholdSq;
        bool rotChanged = (1f - Mathf.Abs(Quaternion.Dot(rot, lastRot))) > _rotDotThreshold;

        if (posChanged || rotChanged)
        {
            var posMsg = new OSCMessage("/listener/xyz");
            posMsg.AddValue(OSCValue.Float(pos.x / 10f));
            posMsg.AddValue(OSCValue.Float(pos.y / 10f));
            posMsg.AddValue(OSCValue.Float(pos.z / 10f));
            transmitter.Send(posMsg);

            Vector3 euler = rot.eulerAngles;
            float yaw   = euler.y;
            float pitch = euler.x;
            float roll  = euler.z;

            var rotMsg = new OSCMessage("/listener/ypr");
            rotMsg.AddValue(OSCValue.Float(yaw));
            rotMsg.AddValue(OSCValue.Float(pitch));
            rotMsg.AddValue(OSCValue.Float(roll));
            transmitter.Send(rotMsg);

            lastPos = pos;
            lastRot = rot;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[OSC] Sent /listener/xyz ({pos}) and /listener/ypr ({yaw}, {pitch}, {roll})");
#endif
        }
    }
}
