using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    
    public Slider slider;
    public float sliderValue = 0f;
    private float _currentSliderValue = 0f;
    // Add a public variable to control the slider value in Unity
    //public float externalSliderValue = 0.0f;

    private void Update()
    {
        if (_currentSliderValue != sliderValue)
        {
            slider.value = sliderValue;
            _currentSliderValue = sliderValue;
        }
    }
    
}