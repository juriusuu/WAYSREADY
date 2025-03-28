using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LoadPrefs : MonoBehaviour
{
    [Header("General Setting")]
    [SerializeField] private bool canUse = false;
    [SerializeField] private MenuController menuController;

    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;

    [Header("Brightness Setting")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTextValue = null;

    [Header("Quality Level Setting")]
    [SerializeField] private TMP_Dropdown qualityDropdown;

    // Removed Sensitivity-related fields as it's no longer in MenuController
    // [Header("Sensitivity Setting")]
    // [SerializeField] private TMP_Text ControllerSenTextValue = null;
    // [SerializeField] private Slider controllerSenSlider = null;

    private void Awake()
    {
        if (canUse)
        {
            if (PlayerPrefs.HasKey("masterVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("masterVolume");

                if (volumeTextValue != null) 
                    volumeTextValue.text = localVolume.ToString("0.0");
                if (volumeSlider != null) 
                    volumeSlider.value = localVolume;
                AudioListener.volume = localVolume;
            }
            else
            {
                if (menuController != null) 
                    menuController.ResetButton("Audio");
            }

            if (PlayerPrefs.HasKey("masterQuality"))
            {
                int localQuality = PlayerPrefs.GetInt("masterQuality");

                if (qualityDropdown != null) 
                    qualityDropdown.value = localQuality;
                QualitySettings.SetQualityLevel(localQuality);
            }

            if (PlayerPrefs.HasKey("masterBrightness"))
            {
                float localBrightness = PlayerPrefs.GetFloat("masterBrightness");

                if (brightnessTextValue != null) 
                    brightnessTextValue.text = localBrightness.ToString("0.0");
                if (brightnessSlider != null) 
                    brightnessSlider.value = localBrightness;
            }
        }
    }
}
