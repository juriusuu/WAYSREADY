using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
  [Header("Volume Setting")]
  [SerializeField] private TMP_Text volumeTextValue = null;
  [SerializeField] private Slider volumeSlider = null;
  [SerializeField] private float defaultVolume = 1.0f;

  [Header("Graphics Settings")]
  [SerializeField] private Slider brightnessSlider = null;
  [SerializeField] private TMP_Text brightnessTextValue = null;
  [SerializeField] private float defaultBrightness = 1;

  [Space(10)]
  [SerializeField] private TMP_Dropdown qualityDropdown;

  private int _qualityLevel;
  private float _brightnessLevel;

  [Header("Confirmation")]
  [SerializeField] private GameObject confirmationPrompt = null;

  [Header("Levels To Load")]
  public string _newGameLevel;
  private string levelToLoad;
  [SerializeField] private GameObject noSavedGameDialog = null;

  public void NewGameDialogYes()
  {
    SceneManager.LoadScene(_newGameLevel);
  }

  public void LoadGameDialogYes()
  {
    if (PlayerPrefs.HasKey("SavedLevel"))
    {
        levelToLoad = PlayerPrefs.GetString("SavedLevel");
        SceneManager.LoadScene(levelToLoad);
    }
    else
    {
        noSavedGameDialog.SetActive(true);
    }
  }

  public void ExitButton()
  {
    Application.Quit();
  }

  public void SetVolume(float volume)
  {
    AudioListener.volume = volume;
    if (volumeTextValue != null)
    {
      volumeTextValue.text = volume.ToString("0.0");
    }
  }

  public void VolumeApply()
  {
    PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
    StartCoroutine(ConfirmationBox());
  }

  public void SetBrightness(float brightness)
  {
    _brightnessLevel = brightness;
    if (brightnessTextValue != null)
    {
      brightnessTextValue.text = brightness.ToString("0.0");
    }
  }

  public void SetQuality(int qualityIndex)
  {
    _qualityLevel = qualityIndex;
  }

  public void GraphicsApply()
  {
    PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);
    PlayerPrefs.SetInt("masterQuality", _qualityLevel);
    QualitySettings.SetQualityLevel(_qualityLevel);
    StartCoroutine(ConfirmationBox());
  }

  public void ResetButton(string MenuType)
  {
    if(MenuType == "Graphics")
    {
      brightnessSlider.value = defaultBrightness;
      brightnessTextValue.text = defaultBrightness.ToString("0.0");

      qualityDropdown.value = 2;
      QualitySettings.SetQualityLevel(2);
      GraphicsApply();
    }

    if (MenuType == "Audio")
    {
      AudioListener.volume = defaultVolume;
      if (volumeSlider != null)
      {
        volumeSlider.value = defaultVolume;
      }
      if (volumeTextValue != null)
      {
        volumeTextValue.text = defaultVolume.ToString("0.0");
      }
      VolumeApply();
    }
  }

  public IEnumerator ConfirmationBox()
  {
    if (confirmationPrompt != null)
    {
      confirmationPrompt.SetActive(true);
      yield return new WaitForSeconds(2);
      confirmationPrompt.SetActive(false);
    }
  }
}
