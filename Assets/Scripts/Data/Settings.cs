using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public static bool RumbleOn;

    public AudioMixer mixer;
    public Slider effectsSlider;
    public Slider musicSlider;
    public Slider rumbleSlider;

    IEnumerator coroutine;

    // https://gamedevbeginner.com/the-right-way-to-make-a-volume-slider-in-unity-using-logarithmic-conversion/
    void Start()
    {
        effectsSlider.value = PlayerPrefs.GetFloat("EffectsVolume", 0.75f);
        mixer.SetFloat("EffectsVolume", Mathf.Log10(effectsSlider.value) * 20);

        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        mixer.SetFloat("MusicVolume", Mathf.Log10(musicSlider.value) * 20);

        rumbleSlider.value = PlayerPrefs.GetInt("Rumble", 1);

        effectsSlider.onValueChanged.AddListener(delegate
        {
            mixer.SetFloat("EffectsVolume", Mathf.Log10(effectsSlider.value) * 20);
            PlayerPrefs.SetFloat("EffectsVolume", effectsSlider.value);
        });

        musicSlider.onValueChanged.AddListener(delegate
        {
            mixer.SetFloat("MusicVolume", Mathf.Log10(musicSlider.value) * 20);
            PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        });

        rumbleSlider.onValueChanged.AddListener(delegate
        {
            RumbleOn = (int)rumbleSlider.value == 1 ? true : false;
            PlayerPrefs.SetInt("Rumble", (int)rumbleSlider.value);
        });

        RumbleOn = PlayerPrefs.GetInt("Rumble", 1) == 1 ? true : false;
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
}

