using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetDataPopup : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private Text question;

    private const string NEW_GAME_QUESTION = "기존 데이터를 삭제하시겠습니까?";
    private const string LOAD_DATA_QUESTION = "초기화하시겠습니까?";

    private void OnEnable()
    {
        bool isNewGame = UIWhiteBox.GetisNewGame();

        if(isNewGame)
        {
            question.text = NEW_GAME_QUESTION;
        }
        else
        {
            question.text = LOAD_DATA_QUESTION;
        }
    }
}
