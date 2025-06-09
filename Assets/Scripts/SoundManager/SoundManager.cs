using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관련 이벤트를 사용하기 위해 추가

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
    public void StopBGM()
    {
        bgmSource.Stop();
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

    // 배경음 볼륨 조절 함수
    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = Mathf.Clamp(volume, 0f, 1f); // 볼륨을 0에서 1사이 값으로 제한
    }

    // 플레이어 효과음 볼륨 조절 함수
    public void SetPlayerSFXVolume(float volume)
    {
        playerSfxSource.volume = Mathf.Clamp(volume, 0f, 1f); // 볼륨을 0에서 1사이 값으로 제한
    }

    // 몬스터 효과음 볼륨 조절 함수
    public void SetMonsterSFXVolume(float volume)
    {
        monsterSfxSource.volume = Mathf.Clamp(volume, 0f, 1f); // 볼륨을 0에서 1사이 값으로 제한
    }

    // UI음 볼륨 조절 함수
    public void SetUIVolume(float volume)
    {
        uiSource.volume = Mathf.Clamp(volume, 0f, 1f); // 볼륨을 0에서 1사이 값으로 제한
    }

    // BGM을 페이드아웃 시키는 코루틴 함수
    private IEnumerator FadeOutBGM(float duration, Action onFadeComplete)
    {
        float startVolume = bgmSource.volume;

        for(float t=0f; t < duration; t+=Time.deltaTime)
        {
            bgmSource.volume = Mathf.Clamp(startVolume, 0f, t / duration);
            yield return null;
        }

        bgmSource.volume = 0f;
        onFadeComplete?.Invoke(); // 페이드 아웃이 완료되면 다음 작업 실행
    }

    // BGM을 페이드 인 시키는 코루틴 함수
    private IEnumerator FadeInBGM(float duration)
    {
        float startVolume = 0f;
        bgmSource.volume = 0f;

        for (float t = 0f; t < duration; t += Time.deltaTime) 
        {
            bgmSource.volume = Mathf.Clamp(startVolume, 0f, t / duration);
            yield return null;
        }

        bgmSource.volume = 1f; // 최종적으로 볼륨을 1로 설정
    }
}