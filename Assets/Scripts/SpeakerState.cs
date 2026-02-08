using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpeakerState : MonoBehaviour
{
    [Header("Identity")]
    public int speakerID;

    [Header("3D object for position")]
    public Transform speakerImageTransform;

    [Header("UI - Main Gain")]
    public Slider gainSlider;
    public TextMeshProUGUI gainText;

    [Header("UI - EQ (6 sliders)")]
    public Slider[] eqSliders = new Slider[6];

    [Header("UI - Reverb (4 sliders)")]
    public Slider[] reverbSliders = new Slider[4];

    public SpeakerData ToData()
    {
        var p = speakerImageTransform != null ? speakerImageTransform.position : Vector3.zero;

        var d = new SpeakerData
        {
            id = speakerID,
            posX = p.x,
            posY = p.y,
            posZ = p.z,
            mainGain = gainSlider != null ? gainSlider.value : 0f,
            eq = new float[6],
            reverb = new float[4],
        };

        for (int i = 0; i < 6; i++)
            d.eq[i] = (eqSliders != null && i < eqSliders.Length && eqSliders[i] != null) ? eqSliders[i].value : 0f;

        for (int i = 0; i < 4; i++)
            d.reverb[i] = (reverbSliders != null && i < reverbSliders.Length && reverbSliders[i] != null) ? reverbSliders[i].value : 0f;

        return d;
    }

    public void ApplyData(SpeakerData d, bool updateUIText = true)
    {
        speakerID = d.id;

        if (speakerImageTransform != null)
            speakerImageTransform.position = new Vector3(d.posX, d.posY, d.posZ);

        if (gainSlider != null)
            gainSlider.value = d.mainGain;

        if (updateUIText && gainText != null && gainSlider != null)
            gainText.text = gainSlider.value.ToString("0.00");

        for (int i = 0; i < 6; i++)
            if (eqSliders != null && i < eqSliders.Length && eqSliders[i] != null)
                eqSliders[i].value = (d.eq != null && d.eq.Length > i) ? d.eq[i] : 0f;

        for (int i = 0; i < 4; i++)
            if (reverbSliders != null && i < reverbSliders.Length && reverbSliders[i] != null)
                reverbSliders[i].value = (d.reverb != null && d.reverb.Length > i) ? d.reverb[i] : 0f;
    }
}
