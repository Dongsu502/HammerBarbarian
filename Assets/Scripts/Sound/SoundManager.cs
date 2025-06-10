using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관련 이벤트를 사용하기 위해 추가
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    // 싱글톤 패턴 구현
    public static SoundManager instance;

    public AudioSource bgmSource; // 배경음악
    public AudioSource playerSfxSource; // 플레이어효과음
    public AudioSource monsterSfxSource; // 몬스터효과음
    public AudioSource uiSource; // UI음

    // 사운드를 이름으로 관리할 수 있도록 Dictionary 사용
    private Dictionary<string, AudioClip> bgmClips = new Dictionary<string, AudioClip>(); // 배경음 저장
    private Dictionary<string, AudioClip> playerSfxClips = new Dictionary<string, AudioClip>(); // 플레이어 효과음 저장
    private Dictionary<string, AudioClip> monsterSfxClips = new Dictionary<string, AudioClip>(); // 몬스터 효과음 저장
    private Dictionary<string, AudioClip> uiClips = new Dictionary<string, AudioClip>(); // UI음 저장

    // 오디오 믹서, 오디오의 타입별로 사운드를 조절
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterVolumeController;
    [SerializeField] private Slider bgmVolumeController;
    [SerializeField] private Slider sfxVolumeController;
    [SerializeField] private Slider uiVolumeController;

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
    public NameAudioClip[] uiClipsList; // 효과음 리스트

    private Coroutine currentBGMCoroutin; // 현재 실행중인 BGM 코루틴을 추절하기 위한 변수

    private void Awake()
    {
        // 싱글톤 패턴 구현
        if(instance == null)
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

    private void Start()
    {
        // 현재 활성화된 씬을 가져와 OnSceneLoaded를 호출하여 BGM 재생
        string activeSceneName = SceneManager.GetActiveScene().name;
        OnSeceneLoaded(activeSceneName); // 현재 씬에 맞는 BGM을 설정
    }

    public void MasterControl()
    {
        float sound = masterVolumeController.value;

        if (sound <= 0.0001f)
        {
            audioMixer.SetFloat("Master", -80);
        }
        else
        {
            float dB = Mathf.Log10(sound) * 20f;
            audioMixer.SetFloat("Master", dB);
        }
    }
    public void BGMControl()
    {
        float sound = bgmVolumeController.value;

        if (sound <= 0.0001f)
        {
            audioMixer.SetFloat("BGM", -80);
        }
        else
        {
            float dB = Mathf.Log10(sound) * 20f;
            audioMixer.SetFloat("BGM", dB);
        }
    }
    public void SFXControl()
    {
        float sound = sfxVolumeController.value;

        if (sound <= 0.0001f)
        {
            audioMixer.SetFloat("SFX", -80);
        }
        else
        {
            float dB = Mathf.Log10(sound) * 20f;
            audioMixer.SetFloat("SFX", dB);
        }
    }
    public void UIControl()
    {
        float sound = uiVolumeController.value;

        if (sound <= 0.0001f)
        {
            audioMixer.SetFloat("UI", -80);
        }
        else
        {
            float dB = Mathf.Log10(sound) * 20f;
            audioMixer.SetFloat("UI", dB);
        }
    }

    // AudioClip 리스트를 Dictionary로 변환하여 이름으로 접근 가능하게 만듬
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

        foreach (var ui in uiClipsList)
        {
            if (!uiClips.ContainsKey(ui.name))
            {
                uiClips.Add(ui.name, ui.clip); // UI음 이름과 클립을 저장
            }
        }
    }

    // 씬 이름을 받아서 BGM을 설정하는 함수
    public void OnSeceneLoaded(string sceneName)
    {
        // 씬 이름에 따라 다른 배경음악을 재생
        if (sceneName == "")
        {
            PlayBGM("Forest02", 1f);
        }
        else
        {
            StopBGM();
        }
    }

    // 배경음 재생 함수(이름으로 재생)
    public void PlayBGM(string name, float fadeDuration = 1f)
    {
        if(bgmClips.ContainsKey(name))
        {
            if(currentBGMCoroutin != null)
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