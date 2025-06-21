using Google.GData.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryUIManager : MonoBehaviour
{
    [SerializeField] ItemDataReader storyData;
    [SerializeField] Image storyImage;
    [SerializeField] Text storyText;
    [SerializeField] private Image InputKeyImage;
    [SerializeField] private float typingSpeed;

    [SerializeField] private Sprite[] storyResources;
     
    private UIInputAction inputAction;
    private TextTyping textTypingClass;
    private List<ItemData> storyList = new List<ItemData>();
    private int currentIndex = 0;

    private bool isEnd = false;

    private void Awake()
    {
        textTypingClass = GetComponent<TextTyping>();
        inputAction = new UIInputAction();

        textTypingClass.onTypingEnd = () =>
        {
            InputKeyImage.gameObject.SetActive(true);
        };

        InitializeStory();
    }

    private void OnEnable()
    {
        inputAction.Enable();

        inputAction.StoryUI.Script.started += Input_F;
    }

    private void OnDisable()
    {
        inputAction.Disable();

        inputAction.StoryUI.Script.started -= Input_F;
    }

    private void Start()
    {
        StartCoroutine(StartStory());
    }

    private void PlayNarration(int _id)
    {
        SoundManager.instance.PlayNarrationSFX("StoryNarration" + _id);
    }

    private void InitializeStory()
    {
        InputKeyImage.gameObject.SetActive(false);
    }

    private IEnumerator StartStory()
    {
        yield return new WaitUntil(() => storyData.DataList != null && storyData.DataList.Count > 0);

        SetStoryImage(0);
        NextStory(1);
    }

    private void SetStoryImage(int _index)
    {
        storyImage.sprite = storyResources[_index];
    }

    /// <summary>
    /// 원하는 id값의 데이터가져오기
    /// </summary>
    /// <param name="start">시작id</param>
    /// <param name="end">마지막id</param>
    public void GetScriptData(int start, int end, float speed)
    {
        storyList.Clear();
        currentIndex = 0;
        isEnd = false;

        //start부터 end까지의 데이터 리스트 가져오기
        for (int i = 0; i < storyData.DataList.Count; i++)
        {
            int id = storyData.DataList[i].id;
            if (id >= start && id <= end)
            {
                storyList.Add(storyData.DataList[i]);
            }
        }

        //첫번째 대사 UI에 표시
        if (storyList.Count > 0)
        {
            SetScriptUI(storyList[0].description, speed);
        }
        else
        {
            Debug.LogWarning("선택된 범위에 대사가 없습니다.");
        }
    }

    /// <summary>
    /// 화면 UI에 이름, 대사 텍스트 연결
    /// </summary>
    /// <param name="newName">이름</param>
    /// <param name="newScript">대사</param>
    private void SetScriptUI(string newScript, float speed)
    {
        InputKeyImage.gameObject.SetActive(false);

        //나레이션 플레이
        SoundManager.instance.StopNarrationSFX();
        PlayNarration(storyList[currentIndex].id);

        //타이핑효과로 대사 추가
        textTypingClass.StartTyping(storyText, newScript, speed);
    }

    #region InputAction

    private void Input_F(InputAction.CallbackContext context)
    {
        if (context.started && UIWhiteBox.MainUICurrentState == MainUIState.SCRIPT)
        {
            NextScript();
        }
    }

    #endregion

    #region ButtonEvent
    //버튼 클릭 이벤트로 다음 대사로 넘기기
    public void NextScript()
    {
        if (textTypingClass.IsTyping)
        {
            textTypingClass.SkipTyping();
            return;
        }

        currentIndex++;

        if (currentIndex < storyList.Count)
        {
            var data = storyList[currentIndex];
            SetScriptUI(data.description, typingSpeed);
        }
        else
        {
            isEnd = true;

            int currentStoryNumber = int.Parse(storyList[0].name.Substring(storyList[0].name.Length - 1));

            if (currentStoryNumber >= 7)
            {
                UIWhiteBox.SceneName = "New Map";
                SceneManager.LoadScene("Loading");

                return;
            }

            SetStoryImage(currentStoryNumber);
            NextStory(currentStoryNumber + 1);
        }
    }

    private void NextStory(int storyName)
    {
        switch (storyName)
        {
            case 1:
                GetScriptData(1, 3, typingSpeed);
                break;
            case 2:
                GetScriptData(4, 6, typingSpeed);
                break;
            case 3:
                GetScriptData(7, 9, typingSpeed);
                break;
            case 4:
                GetScriptData(10, 13, typingSpeed);
                break;
            case 5:
                GetScriptData(14, 17, typingSpeed);
                break;
            case 6:
                GetScriptData(18, 20, typingSpeed);
                break;
            case 7:
                GetScriptData(21, 22, typingSpeed);
                break;
        }
    }

    #endregion
}
