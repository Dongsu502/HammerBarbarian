using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class OptionManager : MonoBehaviour
{
    public static OptionManager instance { get; private set; }

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

    // 볼륨 변경 시 알림 이벤트
    public event Action OnVolumeChanged;

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

    // 각 볼륨의 Getter/Setter
    public float MasterVolume
    {
        get => currentSettings.masterVolume;
        set
        {
            if (!Mathf.Approximately(currentSettings.masterVolume, value))
            {
                currentSettings.masterVolume = value;
                OnVolumeChanged?.Invoke();
            }
        }
    }
    public float BGMVolume
    {
        get => currentSettings.bgmVolume;
        set
        {
            if (!Mathf.Approximately(currentSettings.bgmVolume, value))
            {
                currentSettings.bgmVolume = value;
                OnVolumeChanged?.Invoke();
            }
        }
    }
    public float SFXVolume
    {
        get => currentSettings.sfxVolume;
        set
        {
            if (!Mathf.Approximately(currentSettings.sfxVolume, value))
            {
                currentSettings.sfxVolume = value;
                OnVolumeChanged?.Invoke();
            }
        }
    }
    public float UIVolume
    {
        get => currentSettings.uiVolume;
        set
        {
            if (!Mathf.Approximately(currentSettings.uiVolume, value))
            {
                currentSettings.uiVolume = value;
                OnVolumeChanged?.Invoke();
            }
        }
    }
}
