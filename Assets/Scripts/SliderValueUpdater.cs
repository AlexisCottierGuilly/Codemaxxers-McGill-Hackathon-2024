using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderValueUpdater : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI valueText;

    void Start()
    {
        UpdateText();
    }

    void Update()
    {
        UpdateText();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            slider.value = (int)(Mathf.Round((float)slider.value / 5f) * 5f);
        }
    }

    void UpdateText()
    {
        valueText.text = Mathf.Round(slider.value).ToString();
    }
}
