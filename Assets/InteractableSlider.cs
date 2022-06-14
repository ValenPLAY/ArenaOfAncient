using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractableSlider : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text tmpText;
    private float sliderValue;
    public float SliderValue
    {
        get => sliderValue;
        set
        {
            sliderValue = value;
            slider.value = sliderValue;

            float minDifference = Math.Abs(slider.minValue);
            if (slider.maxValue + minDifference != 0)
            {
                tmpText.text = MathF.Round(Math.Abs((((slider.value + minDifference) / (slider.maxValue + minDifference)) * 100.0f))) + "%";
            }
            else
            {
                minDifference += 0.1f;
                tmpText.text = MathF.Round(Math.Abs((((slider.value + minDifference) / (slider.maxValue + minDifference)) * 100.0f))) + "%";
            }



        }
    }

    private void Awake()
    {

    }

    public void UpdateValueText()
    {
        tmpText.text = Math.Abs(((slider.value / slider.maxValue) * 100.0f)) + "%";
    }

    public void UpdateSliderValue(float incomingValue)
    {
        slider.value = incomingValue;
        UpdateValueText();
    }
}
