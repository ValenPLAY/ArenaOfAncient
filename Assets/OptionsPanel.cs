using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsPanel : MonoBehaviour
{
    [Header("Sound Settings")]
    [SerializeField] InteractableSlider volumeSliderMaster;
    private float volumeMasterValue;

    [SerializeField] InteractableSlider volumeSliderMusic;
    private float volumeMusicValue;

    [SerializeField] InteractableSlider volumeSliderSFX;
    private float volumeSFXValue;

    [SerializeField] InteractableSlider volumeSliderUI;
    private float volumeUIValue;

    [SerializeField] AudioMixer soundAudioMixer;

    [Header("Graphics Settings")]
    [SerializeField] TMP_Dropdown dropdownResolution;
    Resolution[] resoulutions;
    int currentResolutionID;

    [SerializeField] TMP_Dropdown dropdownQuality;
    [SerializeField] Toggle radioBtnFullscreen;

    private void OnEnable()
    {
        // Sound Settings
        soundAudioMixer.GetFloat("volumeMaster", out volumeMasterValue);
        volumeSliderMaster.SliderValue = volumeMasterValue;

        soundAudioMixer.GetFloat("volumeMusic", out volumeMusicValue);
        volumeSliderMusic.SliderValue = volumeMusicValue;

        soundAudioMixer.GetFloat("volumeSFX", out volumeSFXValue);
        volumeSliderSFX.SliderValue = volumeSFXValue;

        soundAudioMixer.GetFloat("volumeUI", out volumeUIValue);
        volumeSliderUI.SliderValue = volumeUIValue;

        //Quality Settings
        dropdownQuality.value = QualitySettings.GetQualityLevel();

        radioBtnFullscreen.isOn = Screen.fullScreen;

        resoulutions = Screen.resolutions;

        dropdownResolution.ClearOptions();

        List<string> resolutionOptions = new List<string>();
        for (int i = 0; i < resoulutions.Length; i++)
        {
            string option = resoulutions[i].width + "x" + resoulutions[i].height + " @ " + resoulutions[i].refreshRate + "hz";
            resolutionOptions.Add(option);

            if (resoulutions[i].width == Screen.width &&
                resoulutions[i].height == Screen.height)
            {
                currentResolutionID = i;
            }
        }

        dropdownResolution.AddOptions(resolutionOptions);
        dropdownResolution.value = currentResolutionID;
        dropdownResolution.RefreshShownValue();
    }

    public void UpdateMasterVolume(TMP_Text changingText, float updateValue)
    {
        changingText.text = updateValue + "%";
    }

    public void ApplyChanges()
    {
        soundAudioMixer.SetFloat("volumeMaster", volumeSliderMaster.SliderValue);
        soundAudioMixer.SetFloat("volumeMusic", volumeSliderMusic.SliderValue);
        soundAudioMixer.SetFloat("volumeSFX", volumeSliderSFX.SliderValue);
        soundAudioMixer.SetFloat("volumeUI", volumeSliderUI.SliderValue);

        QualitySettings.SetQualityLevel(dropdownQuality.value);

        Resolution resolution = resoulutions[dropdownResolution.value];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        Screen.fullScreen = radioBtnFullscreen.isOn;
    }
}
