using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderValueDisplay : MonoBehaviour
{
    public Slider slider; // Reference to the slider
    public TextMeshProUGUI valueText; // Reference to the text component displaying the value

    private void Start()
    {
        UpdateValueText(slider.value);
        slider.onValueChanged.AddListener(UpdateValueText);
    }

    private void UpdateValueText(float value)
    {
        valueText.text = value.ToString("0"); // Format the value as an integer
    }

    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(UpdateValueText);
    }
}
