using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class OptionMenu : MonoBehaviour
{
    [Header("Audio Settings")]
    public Slider volumeMusicSlider;
    public Slider volumeFXSlider;
    public AudioMixer audioMixer;
    public AudioMixer audioMixer2;

    [Header("Video Settings")]
    public Slider brightnessSlider;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public Image tick;

    private Resolution[] resolutions;
    private string currentLanguage;

    void Awake()
    {
        resolutions = Screen.resolutions;

        if (resolutionDropdown != null)
        {
            resolutionDropdown.ClearOptions();
            List<string> options = new List<string>();
            int currentResolutionIndex = 0;

            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }
        LoadSettings(); 
    }


    public void SetMusicAudio(float musicValue)
    {
        audioMixer.SetFloat("MusicVolumen", Mathf.Log10(musicValue) * 20); 
        PlayerPrefs.SetFloat("MusicVolume", musicValue);
    }

    public void SetFXAudio(float FXValue)
    {
        audioMixer2.SetFloat("FXVolumen", Mathf.Log10(FXValue) * 20); 
        PlayerPrefs.SetFloat("FXVolume", FXValue);
    }

    public void SetBrightness(float brightness)
    {
        RenderSettings.ambientLight = Color.white * brightness; 
        PlayerPrefs.SetFloat("Brightness", brightness);
    }

    public void SetResolution(int resolutionIndex)
    {
        if (resolutions == null || resolutionIndex < 0 || resolutionIndex >= resolutions.Length)
        {
            return;
        }

        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex); 
        PlayerPrefs.Save();
    }


    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }

    public void LoadSettings()
    {
        if (resolutionDropdown != null && resolutions != null)
        {
            int savedResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
            if (savedResolutionIndex >= 0 && savedResolutionIndex < resolutions.Length)
            {
                resolutionDropdown.value = savedResolutionIndex;
                resolutionDropdown.RefreshShownValue();
                SetResolution(savedResolutionIndex);
            }
        }

        if (brightnessSlider != null)
        {
            brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 1f);
        }

        if (volumeMusicSlider != null)
        {
            volumeMusicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        }

        if (volumeFXSlider != null)
        {
            volumeFXSlider.value = PlayerPrefs.GetFloat("FXVolume", 0.75f);
        }
    }

}
