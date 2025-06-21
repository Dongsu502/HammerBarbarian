using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TextTyping : MonoBehaviour
{
    public bool IsTyping => isTyping;

    private Coroutine typingCoroutine;
    private bool isTyping = false;

    private Text currentText;
    private string fullText;

    // 콜백 등록용 이벤트
    public System.Action onTypingEnd;

    public void StartTyping(Text uiText, string newText, float speed)
    {
        //이미 실행중이라면 멈추고 새로 시작
        StopTyping();

        fullText = newText;
        currentText = uiText;

        typingCoroutine = StartCoroutine(TypingCoroutine(speed));
    }

    public void SkipTyping()
    {
        if (!isTyping) return;

        StopTyping();
        currentText.text = fullText;

        // 콜백 호출
        onTypingEnd?.Invoke();
    }

    private IEnumerator TypingCoroutine(float speed)
    {
        isTyping = true;
        currentText.text = "";

        StringBuilder sb = new StringBuilder();

        foreach(char c in fullText)
        {
            sb.Append(c);
            currentText.text = sb.ToString();
            
            yield return new WaitForSecondsRealtime(speed);
        }

        isTyping = false;

        // 콜백 호출
        onTypingEnd?.Invoke();
    }

    private void StopTyping()
    {
        if(typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        isTyping= false;
    }
}
