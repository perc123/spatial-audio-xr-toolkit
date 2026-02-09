using UnityEngine;
using UnityEngine.UI;

public class ToggleColorChanger : MonoBehaviour
{
    public Graphic targetGraphic;
    public Color onColor = Color.green;
    public Color offColor = Color.red;

    private Toggle toggle;

    void Start()
    {
        toggle = GetComponent<Toggle>();
        if (toggle == null)
        {
            Debug.LogError("No Toggle component found!");
            return;
        }

        // Set initial color
        UpdateColor(toggle.isOn);

        // Add listener
        toggle.onValueChanged.AddListener(UpdateColor);
    }

    void UpdateColor(bool isOn)
    {
        if (targetGraphic != null)
            targetGraphic.color = isOn ? onColor : offColor;
    }
}