using UnityEngine;
using UnityEngine.Rendering.PostProcessing; 

public class SettingsManager : MonoBehaviour
{
    public PostProcessVolume postProcessVolume;
    private Bloom bloom;

    void Start()
    {
        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 0.75f);  
            PlayerPrefs.SetFloat("FXVolume", 0.75f);     
            PlayerPrefs.SetFloat("Brightness", 0.5f);    
            PlayerPrefs.SetInt("ResolutionIndex", 0);    
            PlayerPrefs.SetInt("Fullscreen", 1);         
            PlayerPrefs.Save();                          
        }

        float musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        float fxVolume = PlayerPrefs.GetFloat("FXVolume");
        float brightness = PlayerPrefs.GetFloat("Brightness");
        int resolutionIndex = PlayerPrefs.GetInt("ResolutionIndex");
        int fullscreen = PlayerPrefs.GetInt("Fullscreen");

        ApplyAudioSettings(musicVolume, fxVolume);
        ApplyVideoSettings(resolutionIndex, fullscreen);
        ApplyBrightness(brightness);

        Debug.Log("Configuraciones cargadas:");
        Debug.Log($"Music Volume: {musicVolume}, FX Volume: {fxVolume}, Brightness: {brightness}");
    }

    void ApplyAudioSettings(float musicVolume, float fxVolume)
    {
        AudioListener.volume = musicVolume;
    }

    void ApplyVideoSettings(int resolutionIndex, int fullscreen)
    {
        Resolution[] resolutions = Screen.resolutions;
        if (resolutionIndex >= 0 && resolutionIndex < resolutions.Length)
        {
            Resolution selectedResolution = resolutions[resolutionIndex];
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, fullscreen == 1);
        }
    }

    void ApplyBrightness(float brightness)
    {
        if (postProcessVolume != null)
        {
            postProcessVolume.profile.TryGetSettings(out bloom);

            if (bloom != null)
            {
                bloom.intensity.value = brightness;
            }
        }
    }
}
