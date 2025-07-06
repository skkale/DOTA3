using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        // Завантаження збережених значень або дефолтні
        float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 2f);
        float savedVolume = PlayerPrefs.GetFloat("GameVolume", 1f);

        mouseSensitivitySlider.value = savedSensitivity;
        volumeSlider.value = savedVolume;

        ApplySensitivity(savedSensitivity);
        ApplyVolume(savedVolume);
    }

    public void OnMouseSensitivityChanged(float value)
    {
        PlayerPrefs.SetFloat("MouseSensitivity", value);
        ApplySensitivity(value);
    }

    public void OnVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("GameVolume", value);
        ApplyVolume(value);
    }

    private void ApplySensitivity(float sensitivity)
    {
        // Значення просто зберігається, використається в PlayerController
    }

    private void ApplyVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}