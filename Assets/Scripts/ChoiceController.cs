using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/* ������ ���� ��ũ��Ʈ */
public class ChoiceController : MonoBehaviour
{
    // Singleton ���� �ν��Ͻ�
    private static ChoiceController instance = null;

    private void Awake()
    {
        // Singleton ���� ����
        if (null == instance) {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else {
            Destroy(this.gameObject);
        }
    }

    public bool finChoice = false;    // ���� �Ϸ� ����
    public GameObject talkController;

    // ������ Text
    [SerializeField] private TMP_Text answerA;
    [SerializeField] private TMP_Text answerB;

    // ������ Ȱ��ȭ
    public void InitAnswer(string a, string b)
    {
        finChoice = false;
        answerA.text = a;
        answerB.text = b;
    }

    // ���� �� ���� ���� �̵�
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
