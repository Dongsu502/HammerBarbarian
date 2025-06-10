using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class OptionManager : MonoBehaviour
{
    [Header("Sliders")]
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Slider uiSlider;

    private const string FileName = "sound_settings.json";

    [Serializable]
    public class SoundSettings
    {
        public float masterVolume = 1f;
        public float bgmVolume = 1f;
        public float sfxVolume = 1f;
        public float uiVolume = 1f;
    }

    private SoundSettings currentSettings;

    void Start()
    {
        LoadSettings();
        ApplySettingsToUI();
        ApplySettingsToAudio();

        // 슬라이더 이벤트 연결
        masterSlider.onValueChanged.AddListener((v) => OnSliderChanged());
        bgmSlider.onValueChanged.AddListener((v) => OnSliderChanged());
        sfxSlider.onValueChanged.AddListener((v) => OnSliderChanged());
        uiSlider.onValueChanged.AddListener((v) => OnSliderChanged());
    }

    void OnSliderChanged()
    {
        // 현재 슬라이더 값 저장
        currentSettings.masterVolume = masterSlider.value;
        currentSettings.bgmVolume = bgmSlider.value;
        currentSettings.sfxVolume = sfxSlider.value;
        currentSettings.uiVolume = uiSlider.value;

        ApplySettingsToAudio();
        SaveSettings();
    }

    void ApplySettingsToUI()
    {
        masterSlider.value = currentSettings.masterVolume;
        bgmSlider.value = currentSettings.bgmVolume;
        sfxSlider.value = currentSettings.sfxVolume;
        uiSlider.value = currentSettings.uiVolume;
    }

    void ApplySettingsToAudio()
    {
        // 실제 적용: 이 예시는 마스터만 적용하지만 나중에 세부 오디오 믹서에 연동 가능
        AudioListener.volume = currentSettings.masterVolume;
        // TODO: BGM, SFX, UI 볼륨은 AudioMixer 사용 시 적용
    }

    void OnApplicationQuit()
    {
        SaveSettings();
    }

    void SaveSettings()
    {
        string json = JsonUtility.ToJson(currentSettings, true);
        string path = Path.Combine(Application.persistentDataPath, FileName);
        File.WriteAllText(path, json);
    }

    void LoadSettings()
    {
        string path = Path.Combine(Application.persistentDataPath, FileName);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            currentSettings = JsonUtility.FromJson<SoundSettings>(json);
        }
        else
        {
            currentSettings = new SoundSettings();
            SaveSettings(); // 기본값 저장
        }
    }
}
