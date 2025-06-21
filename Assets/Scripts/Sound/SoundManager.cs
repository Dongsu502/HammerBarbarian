using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    // 싱글톤 패턴 구현
    public static SoundManager instance;

    public AudioSource bgmSource; // 배경음악
    public AudioSource playerSfxSource; // 플레이어효과음
    public AudioSource monsterSfxSource; // 몬스터효과음
    public AudioSource narrationSfxSource; // 나레이션효과음
    public AudioSource uiSource; // UI음

    // 사운드를 이름으로 관리할 수 있도록 Dictionary 사용
    private Dictionary<string, AudioClip> bgmClips = new Dictionary<string, AudioClip>(); // 배경음 저장
    private Dictionary<string, AudioClip> playerSfxClips = new Dictionary<string, AudioClip>(); // 플레이어 효과음 저장
    private Dictionary<string, AudioClip> monsterSfxClips = new Dictionary<string, AudioClip>(); // 몬스터 효과음 저장
    private Dictionary<string, AudioClip> narrationSfxClips = new Dictionary<string, AudioClip>(); // 나레이션 효과음 저장
    private Dictionary<string, AudioClip> uiClips = new Dictionary<string, AudioClip>(); // UI음 저장

    // 오디오 믹서, 오디오의 타입별로 사운드를 조절
    [SerializeField] private AudioMixer audioMixer;

    // class나 struct 위에 선언하여 사용하면 인스펙터 창에 직렬화로 표시됨
    [Serializable]
    public struct NameAudioClip
    {
        public string name;     // 사운드 이름
        public AudioClip clip;  // 실제 오디오 클립
    }

    public NameAudioClip[] bgmClipsList; // 배경음 리스트
    public NameAudioClip[] playerSfxClipsList; // 플레이어 효과음 리스트
    public NameAudioClip[] monsterSfxClipsList; // 몬스터 효과음 리스트
    public NameAudioClip[] narrationSfxClipsList; // 나레이션 효과음 리스트
    public NameAudioClip[] uiClipsList; // 효과음 리스트

    private Coroutine currentBGMCoroutin; // 현재 실행중인 BGM 코루틴을 추절하기 위한 변수

    private void Awake()
    {
        // 싱글톤 패턴 구현
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // 씬 전환시 파괴 불가능하게 설정
            InitializeAudioClips(); // 오디오 클립 초기화
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 파괴
        }
    }
    private void InitializeAudioClips()
    {
        foreach (var bgm in bgmClipsList)
        {
            if (!bgmClips.ContainsKey(bgm.name))
            {
                bgmClips.Add(bgm.name, bgm.clip); // 배경음 이름과 클립을 저장
            }
        }

        foreach (var sfx in playerSfxClipsList)
        {
            if (!playerSfxClips.ContainsKey(sfx.name))
            {
                playerSfxClips.Add(sfx.name, sfx.clip); // 플레이어 효과음 이름과 클립을 저장
            }
        }

        foreach (var sfx in monsterSfxClipsList)
        {
            if (!monsterSfxClips.ContainsKey(sfx.name))
            {
                monsterSfxClips.Add(sfx.name, sfx.clip); // 몬스터 효과음 이름과 클립을 저장
            }
        }

        foreach (var sfx in narrationSfxClipsList)
        {
            if (!narrationSfxClips.ContainsKey(sfx.name))
            {
                narrationSfxClips.Add(sfx.name, sfx.clip); // 몬스터 효과음 이름과 클립을 저장
            }
        }

        foreach (var ui in uiClipsList)
        {
            if (!uiClips.ContainsKey(ui.name))
            {
                uiClips.Add(ui.name, ui.clip); // UI음 이름과 클립을 저장
            }
        }
    }

    private void OnEnable()
    {
        // 옵션매니저가 이미 존재하면 바로 구독
        if (OptionManager.instance != null)
        {
            OptionManager.instance.OnVolumeChanged += ApplyVolumes;
            // 최초 볼륨도 한 번 세팅
            ApplyVolumes();
        }
        // 옵션매니저가 나중에 생성된다면, 옵션매니저의 OnInitialized 이벤트를 구독
        OptionManager.OnInitialized += OnOptionManagerInitialized;
    }

    private void OnDisable()
    {
        // 구독 해제
        if (OptionManager.instance != null)
            OptionManager.instance.OnVolumeChanged -= ApplyVolumes;

        OptionManager.OnInitialized -= OnOptionManagerInitialized;
    }

    private void OnOptionManagerInitialized()
    {
        // 이제 OptionManager.instance가 null이 아님이 보장됨!
        OptionManager.instance.OnVolumeChanged += ApplyVolumes;
        ApplyVolumes();
    }


    private void Start()
    {
        // 씬 시작시 현재 옵션값으로 볼륨 적용
        ApplyVolumes();
    }

    public void ApplyVolumes()
    {
        Debug.Log("OptionTest MasterVolume Setter! 이벤트 호출");
        if (OptionManager.instance == null) return;

        // 볼륨 값이 0일 경우 -80dB(=음소거), 그 외엔 Log 변환
        SetVolume("Master", OptionManager.instance.MasterVolume);
        SetVolume("BGM", OptionManager.instance.BGMVolume);
        SetVolume("SFX", OptionManager.instance.SFXVolume);
        SetVolume("UI", OptionManager.instance.UIVolume);
    }

    private void SetVolume(string parameterName, float value)
    {
        Debug.Log($"SetVolume 호출: {parameterName}, value: {value}");
        if (audioMixer == null)
        {
            Debug.LogError("audioMixer가 할당되지 않았음!");
            return;
        }

        float dB;

        if (value <= 0.0001f)
        {
            // 볼륨이 매우 작으면 완전 음소거(-80dB)
            dB = -80f;
        }
        else
        {
            // 일반 볼륨일 때는 0~1값을 dB로 변환
            dB = Mathf.Log10(value) * 20f;
        }

        Debug.Log($"Mixer SetFloat: {parameterName}, dB: {dB}");

        audioMixer.SetFloat(parameterName, dB);
    }



    // 배경음 재생 함수(이름으로 재생)
    public void PlayBGM(string name, float fadeDuration = 1f)
    {
        if (bgmClips.ContainsKey(name))
        {
            if (currentBGMCoroutin != null)
            {
                StopCoroutine(currentBGMCoroutin); // 기존 페이드 코루틴이 있으면 중단
            }

            // 현재 BGM이 있을 경우 페이드 아웃 후 새로운 BGM으로 페이드인
            currentBGMCoroutin = StartCoroutine(FadeOutBGM(fadeDuration, () =>
            {
                bgmSource.clip = bgmClips[name]; // 해당 이름의 배경음을 재생
                bgmSource.Play();
                currentBGMCoroutin = StartCoroutine(FadeInBGM(fadeDuration)); // 페이드인
            }));
        }
        else
        {
            Debug.Log("해당 이름의 배경음이 존재하지 않음: " + name);
        }
    }

    // 플레이어 효과음 재생 함수(이름으로 재생)
    public void PlayPlayerSFX(string name)
    {
        if (playerSfxClips.ContainsKey(name))
        {
            playerSfxSource.PlayOneShot(playerSfxClips[name]); // 해당 이름의 효과음을 
        }
        else
        {
            Debug.Log("해당 이름의 효과음이 존재하지 않음: " + name);
        }
    }

    // 몬스터 효과음 재생 함수(이름으로 재생)
    public void PlayMonsterSFX(string name)
    {
        if (monsterSfxClips.ContainsKey(name))
        {
            monsterSfxSource.PlayOneShot(monsterSfxClips[name]); // 해당 이름의 효과음을 
        }
        else
        {
            Debug.Log("해당 이름의 효과음이 존재하지 않음: " + name);
        }
    }

    // 나레이션 효과음 재생 함수(이름으로 재생)
    public void PlayNarrationSFX(string name)
    {
        if (narrationSfxClips.ContainsKey(name))
        {
            narrationSfxSource.PlayOneShot(narrationSfxClips[name]); // 해당 이름의 효과음을 
        }
        else
        {
            Debug.Log("해당 이름의 효과음이 존재하지 않음: " + name);
        }
    }

    // UI음 재생 함수(이름으로 재생)
    public void PlayUI(string name)
    {
        if (uiClips.ContainsKey(name))
        {
            uiSource.PlayOneShot(uiClips[name]); // 해당 이름의 효과음을 
        }
        else
        {
            Debug.Log("해당 이름의 UI음이 존재하지 않음: " + name);
        }
    }

    // 배경음 멈춤
    public void StopBGM(float fadeDuration = 1f)
    {
        if (currentBGMCoroutin != null)
        {
            StopCoroutine(currentBGMCoroutin);
        }

        currentBGMCoroutin = StartCoroutine(FadeOutBGM(fadeDuration, () =>
        {
            bgmSource.Stop();
            bgmSource.clip = null;
        }));
    }

    // 플레이어 효과음 멈춤
    public void StopPlayerSFX()
    {
        playerSfxSource.Stop();
    }

    // 몬스터 효과음 멈춤
    public void StopMonsterSFX()
    {
        monsterSfxSource.Stop();
    }

    // 나레이션 효과음 멈춤
    public void StopNarrationSFX()
    {
        narrationSfxSource.Stop();
    }

    // UI음 멈춤
    public void StopUI()
    {
        uiSource.Stop();
    }

    // BGM을 페이드아웃 시키는 코루틴 함수
    private IEnumerator FadeOutBGM(float duration, Action onFadeComplete)
    {
        float startVolume = bgmSource.volume;
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, (Time.time - startTime) / duration);
            yield return null;
        }

        bgmSource.volume = 0f;
        onFadeComplete?.Invoke(); // 페이드 아웃이 완료되면 다음 작업 실행
    }

    // BGM을 페이드 인 시키는 코루틴 함수
    private IEnumerator FadeInBGM(float duration)
    {
        float targetVolume = 0.5f;
        float startTime = Time.time;
        float startVolume = 0f;

        while (Time.time < startTime + duration)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, targetVolume, (Time.time - startTime) / duration);
            yield return null;
        }

        bgmSource.volume = targetVolume;
    }
}