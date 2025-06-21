using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TutorialUIManager : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject KeyDescriptionPanel;

    [Space]
    [Header("Texts")]
    [SerializeField] private Text comboName;
    [SerializeField] private Text comboDescription;
    [SerializeField] private string[] comboNames;
    private string[] comboDescriptions = new string[]
    {
        "마우스 좌클릭, 최대 2번\n\n망치를 좌 우로 휘두른다.\n약한 데미지를 준다.\n몬스터가 넉백되지 않는다.",
        "마우스 우클릭, 최대 2번\n\n망치를 올려치고 내려친다.\n올려치기는 약한 데미지를 주고 넉백시키지 않는다.\n내려치기는 강한 데미지를 주고 몬스터를 넉백시킨다.",
        "마우스 좌클릭 1번, 우클릭 최대 2번\n\n가로 휘두르기방향 그대로 한바퀴 돌아서 올려치고\n 뒤로 넘어지면서 강한 데미지를 준다.",
        "마우스 좌클릭 2번, 우클릭 홀드\n\n망치를 잡고 돌면서 약한 지속 데미지를 준다.",
        "마우스 좌클릭 2번, 우클릭 홀드, 좌클릭 1번\n\n망치를 강하게 휘두르며 강한 데미지를 주고 넉백시킨다.",
        "쉬프트 홀드\n\n전방의 공격을 막는다.",
        "ESC키를 눌러 옵션창을 연다.\n조작키버튼을 눌러서 키를 확인한다.",
        "R키, 마우스 좌클릭 1번\n\nR키를 눌러 아이템 선택창을 열고\n 마우스로 아이템을 선택하면 우측하단에 표시된다.",
        "E키\n\nE키를 한번 누르면 아이템이 장착된다.\n장착된 상태에서 E키를 한번 누르면 장착해제된다.\n(아이템착용은 우측하단 아이템창에 UI표시)",
        "마우스 우클릭 홀드\n\n마우스 우클릭을 누르고 있으면 카메라가 줌인되고\n 화면 가운데에 조준점이 생긴다.",
        "마우스 좌클릭 1번\n\n조준한 상태에서 마우스 좌클릭을 하면 망치를 던진다."
    };

    [Space]
    [Header("Images")]
    [SerializeField] private Image[] comboImage;

    [Space]
    [Header("Videos")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VideoClip[] videoClips;

    private int comboIndex;
    private const int COMBO_INDEX_MIN = 0;
    private const int COMBO_INDEX_MAX = 6;
    private const int FIRSTITEM_INDEX_MIN = 7;
    private const int FIRSTITEM_INDEX_MAX = 10;

    [ContextMenu("아이템 설명")]
    private void Item1Tutorial()
    {
        UIWhiteBox.TutorialUICurrentState = TutorialUIState.FIRSTITEM;
        SetActiveKeyPanel(true);
    }

    private void Awake()
    {
        UIWhiteBox.SetTutorialUIWB(this);

        KeyDescriptionPanel.SetActive(false);
    }

    public void SetActiveKeyPanel(bool isActive)
    {
        KeyDescriptionPanel.SetActive(isActive);

        UIWhiteBox.CursorLock(isActive);

        if (!isActive)
        {
            //일시정지 해제
            WorldWhiteBox.WhiteBox.ResumeGame();
            return;
        }
        
        switch(UIWhiteBox.TutorialUICurrentState)
        {
            case TutorialUIState.COMBO:
                comboIndex = COMBO_INDEX_MIN;
                break;
            case TutorialUIState.FIRSTITEM:
                comboIndex = FIRSTITEM_INDEX_MIN;
                break;
        }
        SetTutorialUI(comboIndex);

        //일시정지
        WorldWhiteBox.WhiteBox.PauseGame();
    }

    /// <summary>
    /// 비디오, 텍스트, 이미지 재설정
    /// </summary>
    /// <param name="index">설정할 인덱스</param>
    public void SetTutorialUI(int index)
    {
        PlayVideo(index);
        SetComboText(index);
        SetComboImage(index);
    }

    private void SetComboText(int index)
    {
        if (index >= 0 && index < comboNames.Length)
        {
            comboName.text = comboNames[index];
            comboDescription.text = comboDescriptions[index];
        }
        else
        {
            Debug.LogWarning("잘못된 텍스트 인덱스입니다.");
        }
    }

    private void SetComboImage(int index)
    {
        if (index >= 0 && index < comboImage.Length)
        {
            for(int i = 0; i < comboImage.Length; i++)
            {
                comboImage[i].gameObject.SetActive(false);
            }
            comboImage[index].gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("잘못된 이미지 인덱스입니다.");
        }
    }

    // 특정 클립으로 전환
    private void PlayVideo(int index)
    {
        if (index >= 0 && index < videoClips.Length)
        {
            videoPlayer.clip = videoClips[index];
            videoPlayer.Play();
        }
        else
        {
            Debug.LogWarning("잘못된 비디오 인덱스입니다.");
        }
    }

    #region ButtonEvent

    public void Click_ExitButton()
    {
        SetActiveKeyPanel(false);

        if(UIWhiteBox.TutorialUICurrentState == TutorialUIState.COMBO)
        {
            //대사 호출
            UIWhiteBox.StartScripting(1133, 1133, 0.02f);
        }
    }

    public void Click_PreviousButton()
    {
        switch(UIWhiteBox.TutorialUICurrentState)
        {
            case TutorialUIState.COMBO:
                if (comboIndex <= COMBO_INDEX_MIN) return;
                break;
            case TutorialUIState.FIRSTITEM:
                if (comboIndex <= FIRSTITEM_INDEX_MIN) return;
                break;
        }
        comboIndex--;
        SetTutorialUI(comboIndex);
    }

    public void Click_NextButton()
    {
        switch (UIWhiteBox.TutorialUICurrentState)
        {
            case TutorialUIState.COMBO:
                if (comboIndex >= COMBO_INDEX_MAX) return;
                break;
            case TutorialUIState.FIRSTITEM:
                if (comboIndex >= FIRSTITEM_INDEX_MAX) return;
                break;
        }
        comboIndex++;
        SetTutorialUI(comboIndex);
    }

    #endregion
}
