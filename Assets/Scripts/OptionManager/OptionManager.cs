using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class OptionManager : MonoBehaviour
{
    public static OptionManager instance { get; private set; }

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

    // 초기화 완료 이벤트
    public static event Action OnSettingsLoaded;

    void Awake()
    {
        // 싱글톤 구현: 이미 인스턴스가 존재하면 파괴, 없으면 설정
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // 씬 전환 시 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject);  // 이미 존재하는 인스턴스가 있으면 이 객체는 파괴
        }        
    }

    private void OnEnable()
    {
        LoadSettings();

        // 혹시라도 currentSettings가 null인 경우 대비
        if (currentSettings == null)
        {
            Debug.LogWarning("currentSettings가 null이라 기본값으로 초기화합니다.");
            currentSettings = new SoundSettings();
        }

        ApplySettingsToUI();
        ApplySettingsToAudio();

        // 슬라이더 이벤트 연결
        masterSlider.onValueChanged.AddListener((v) => OnSliderChanged());
        bgmSlider.onValueChanged.AddListener((v) => OnSliderChanged());
        sfxSlider.onValueChanged.AddListener((v) => OnSliderChanged());
        uiSlider.onValueChanged.AddListener((v) => OnSliderChanged());

        // 설정을 로드한 후, 초기화 완료 이벤트 발생
        OnSettingsLoaded?.Invoke();
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
        AudioListener.volume = currentSettings.bgmVolume;
        AudioListener.volume = currentSettings.sfxVolume;
        AudioListener.volume = currentSettings.uiVolume;
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

    // 추가: 슬라이더 값 불러오기
    public float GetMasterVolume() => currentSettings.masterVolume;
    public float GetBGMVolume() => currentSettings.bgmVolume;
    public float GetSFXVolume() => currentSettings.sfxVolume;
    public float GetUIVolume() => currentSettings.uiVolume;
}
