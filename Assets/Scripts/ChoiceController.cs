using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/* 선택지 제어 스크립트 */
public class ChoiceController : MonoBehaviour
{
    // Singleton 패턴 인스턴스
    private static ChoiceController instance = null;

    private void Awake()
    {
        // Singleton 패턴 구현
        if (null == instance) {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else {
            Destroy(this.gameObject);
        }
    }

    public bool finChoice = false;    // 선택 완료 여부
    public GameObject talkController;

    // 선택지 Text
    [SerializeField] private TMP_Text answerA;
    [SerializeField] private TMP_Text answerB;

    // 선택지 활성화
    public void InitAnswer(string a, string b)
    {
        finChoice = false;
        answerA.text = a;
        answerB.text = b;
    }

    // 선택 시 다음 대사로 이동
    public void AnswerBtnClick()
    {
        finChoice = true;

        if(SceneManager.GetActiveScene().name == "Prologue") {
            talkController.GetComponent<PrologueController>().PrologueTalk();
            return;
        }

        talkController.GetComponent<TalkController>().Talk();
    }
}
