using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    //public Dropdown resolutionDropdown;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Text label;
    //public Image image;
    Resolution[] screenResolutions;

    private void Start()
    {
        screenResolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < screenResolutions.Length; i++)
        {
            string option = screenResolutions[i].width + " x " + screenResolutions[i].height;
            options.Add(option);

            if (screenResolutions[i].width == Screen.currentResolution.width && 
                screenResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        //label.text = resolution.width + " x " + resolution.height;
        //resolutionDropdown.itemImage = image;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = screenResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        //resolutionDropdown.RefreshShownValue();
        label.text = resolution.width + " x " + resolution.height;
    }

    public void SetVolume(float volume)
    {
        //we may have to set the volume of the game for the master mixer on startup
        audioMixer.SetFloat("Volume", volume);
    }

    //use later
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
