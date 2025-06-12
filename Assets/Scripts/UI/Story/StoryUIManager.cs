using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryUIManager : MonoBehaviour
{
    [SerializeField] ItemDataReader storyData;
    [SerializeField] Image storyImage;
    [SerializeField] Text storyText;
    [SerializeField] string storyContent;

    private TextTyping textTypingClass;
    private List<ItemData> storyList = new List<ItemData>();

    private void Awake()
    {
        textTypingClass = GetComponent<TextTyping>();

        InitializeStory();
    }

    private void InitializeStory()
    {
        storyImage.gameObject.SetActive(false);
        storyText.gameObject.SetActive(false);

        storyList = storyData.DataList;
    }

    [ContextMenu("이미지 등장")]
    private void SpawnImage()
    {
        storyImage.gameObject.SetActive(true);
    }
    [ContextMenu("텍스트 등록")]
    private void RegistarStoryText()
    {
        storyContent = storyList[0].description;
    }
    [ContextMenu("텍스트 시작")]
    private void StartStory()
    {
        storyText.gameObject.SetActive(true);

        textTypingClass.StartTyping(storyText, storyContent, 0.05f);
    }
}
