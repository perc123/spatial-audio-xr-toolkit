using UnityEngine;
using extOSC;

public class SimpleOSCReceiver : MonoBehaviour
{
    public OSCReceiver Receiver;
    public Transform CubeTransform;

    private void Start()
    {
        if (Receiver != null)
        {
            Debug.Log("[OSC] Receiver is set up and listening on port " + Receiver.LocalPort);
            Receiver.Bind("/cubeScale", OnCubeScaleReceived);
        }
        else
        {
            Debug.LogError("[OSC] Receiver not assigned!");
        }
    }

    private void OnCubeScaleReceived(OSCMessage message)
    {
        if (message.Values.Count > 0)
        {
            float newScale = message.Values[0].FloatValue;
            Debug.Log("[OSC] Received /cubeScale value: " + newScale);

            if (CubeTransform != null)
            {
                CubeTransform.localScale = Vector3.one * newScale;
            }
            else
            {
                Debug.LogWarning("[OSC] CubeTransform not assigned!");
            }
        }
        else
        {
            Debug.LogWarning("[OSC] Received /cubeScale with no values!");
        }
    }
}