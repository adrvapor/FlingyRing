using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public static string EffectsVolumeKey = "EffectsVolume";
    public static string MusicVolumeKey = "MusicVolume";
    public static string RumbleKey = "Rumble";
    public static string TooltipsKey = "Tooltips";

    public static bool RumbleOn;
    public static bool TooltipsOn;

    public AudioMixer mixer;
    public Slider effectsSlider;
    public Slider musicSlider;
    public Slider rumbleSlider;
    public Slider tooltipSlider;

    IEnumerator coroutine;

    // https://gamedevbeginner.com/the-right-way-to-make-a-volume-slider-in-unity-using-logarithmic-conversion/
    void Start()
    {
        effectsSlider.value = PlayerPrefs.GetFloat(EffectsVolumeKey, 0.75f);
        mixer.SetFloat(EffectsVolumeKey, Mathf.Log10(effectsSlider.value) * 20);

        musicSlider.value = PlayerPrefs.GetFloat(MusicVolumeKey, 0.75f);
        mixer.SetFloat(MusicVolumeKey, Mathf.Log10(musicSlider.value) * 20);

        rumbleSlider.value = PlayerPrefs.GetInt(RumbleKey, 1);

        tooltipSlider.value = PlayerPrefs.GetInt(TooltipsKey, 1);

        effectsSlider.onValueChanged.AddListener(delegate
        {
            mixer.SetFloat(EffectsVolumeKey, Mathf.Log10(effectsSlider.value) * 20);
            PlayerPrefs.SetFloat(EffectsVolumeKey, effectsSlider.value);
        });

        musicSlider.onValueChanged.AddListener(delegate
        {
            mixer.SetFloat(MusicVolumeKey, Mathf.Log10(musicSlider.value) * 20);
            PlayerPrefs.SetFloat(MusicVolumeKey, musicSlider.value);
        });

        rumbleSlider.onValueChanged.AddListener(delegate
        {
            RumbleOn = (int)rumbleSlider.value == 1 ? true : false;
            PlayerPrefs.SetInt(RumbleKey, (int)rumbleSlider.value);
        });

        RumbleOn = PlayerPrefs.GetInt(RumbleKey, 1) == 1 ? true : false;

        tooltipSlider.onValueChanged.AddListener(delegate
        {
            TooltipsOn = (int)tooltipSlider.value == 1 ? true : false;
            PlayerPrefs.SetInt(TooltipsKey, (int)tooltipSlider.value);
        });

        TooltipsOn = PlayerPrefs.GetInt(TooltipsKey, 1) == 1 ? true : false;
    }
    
    public void ChangeLanguage(int i)
    {
        coroutine = ChangeLanguageCoroutine(i);
        StartCoroutine(coroutine);
    }

    public IEnumerator ChangeLanguageCoroutine(int i)
    {
        yield return LocalizationSettings.InitializationOperation;
        if (i >= 0 && i < LocalizationSettings.AvailableLocales.Locales.Count)
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[i];
    }

    public void ToggleRumble()
    {
        rumbleSlider.value = (-rumbleSlider.value + 1);
    }
    public void ToggleTooltips()
    {
        tooltipSlider.value = (-tooltipSlider.value + 1);
    }
}

