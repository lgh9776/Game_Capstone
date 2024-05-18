using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrologueController : MonoBehaviour
{
    public GameObject prologueControlObject;
    public GameObject talkPanel;
    public GameObject choiceList;
    public Image npcImage;
    public Image playerImage;
    public Image nameLabel;
    public Text talkName;
    public Text talkText;

    public GameObject secondTitle;

    [SerializeField] float charPerSecond;

    private bool isTyping = false;
    private bool isTalking = false;
    private bool isChoice = false;

    private int textIdx;
    private int charIdx;

    private string talkPlayer;
    private string[] dialogue;
    private string typeDialogue;

    IOController _IOController;
    ChoiceController _ChoiceController;

    private void Awake()
    {
        _IOController = FindObjectOfType<IOController>();
        _ChoiceController = FindObjectOfType<ChoiceController>();
        talkPlayer = "???";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isTalking) {
            PrologueTalk();
        }
    }

    public void PrologueStart()
    {
        Invoke("PrologueDialogueStart", 1.0f);
    }

    public void PrologueSecondStart()
    {
        talkPlayer = "소니아";
        secondTitle.SetActive(true);
        prologueControlObject.GetComponent<PrologueTalkObjData>().dialogueID = 1;
    }

    private void PrologueDialogueStart()
    {
        isTyping = true;
        isTalking = true;

        // 외부 대사 데이터 import
        PrologueTalkObjData tObjData = prologueControlObject.GetComponent<PrologueTalkObjData>();
        dialogue = _IOController.LoadDialogue(tObjData.objID, tObjData.dialogueID);

        // 대사 패널 및 플레이어 이미지 활성화, 텍스트 출력 위치 지정 
        talkPanel.SetActive(isTalking);
        if(talkPlayer.Equals("???"))
            playerImage.sprite = Resources.Load<Sprite>("Upshot/" + "someone" + "_silhouette");    // !! 화자 업샷 정보로 수정할 것 !!
        else if(talkPlayer.Equals("소니아"))
            playerImage.sprite = Resources.Load<Sprite>("Upshot/" + "소니아" + "_blank");    // !! 화자 업샷 정보로 수정할 것 !!

        npcImage.enabled = false;
        talkText.rectTransform.anchoredPosition = new Vector2(600.0f, 50.0f);

        // 대사 파싱 (이름-업샷-대사 분리)
        string[] splitDialogue;
        splitDialogue = dialogue[0].Split('&');

        talkName.text = splitDialogue[0];

        switch (splitDialogue.Length) {
            // 효과음, 시스템 문구 (이름 x, 업샷 x, 대사 o)
            case 1:
                // 필요없는 이미지 비활성화
                playerImage.enabled = false;
                npcImage.enabled = false;
                talkName.enabled = false;
                nameLabel.enabled = false;

                talkText.rectTransform.anchoredPosition = new Vector2(395.0f, 50.0f);    // 텍스트 위치 재설정
                TypeEffectStart(splitDialogue[0]);
                break;
            // 독백 (이름 o, 업샷 x, 대사 o)
            case 2:
                // NPC 업샷 비활성화
                npcImage.enabled = false;
                playerImage.color = new Color(0.4f, 0.4f, 0.4f);
                TypeEffectStart(splitDialogue[1]);
                break;
            // 일반 대사 (이름 o, 업샷 o, 대사 o)
            case 3:
                if (splitDialogue[0].Equals(talkPlayer)) {    // !! 화자 업샷 정보로 수정할 것 !!
                    if(talkPlayer.Equals("???"))
                        playerImage.sprite = Resources.Load<Sprite>("Upshot/" + "someone" + "_" + splitDialogue[1]);
                    else
                        playerImage.sprite = Resources.Load<Sprite>("Upshot/" + splitDialogue[0] + "_" + splitDialogue[1]);
                    playerImage.color = new Color(1.0f, 1.0f, 1.0f);
                    npcImage.color = new Color(0.4f, 0.4f, 0.4f);
                }
                else {
                    npcImage.enabled = true;
                    npcImage.sprite = Resources.Load<Sprite>("Upshot/" + splitDialogue[0] + "_" + splitDialogue[1]);
                    playerImage.color = new Color(0.4f, 0.4f, 0.4f);
                    npcImage.color = new Color(1.0f, 1.0f, 1.0f);
                    
                }
                TypeEffectStart(splitDialogue[2]);
                break;
        }

        textIdx++;
    }

    public void PrologueTalk()
    {
        // 타이핑 효과 조기 종료
        if (isTyping) {
            isTyping = false;
        }
        // 대사 출력
        else {
            // 선택지 출력 여부
            if (isChoice) {
                // 선택 종료 여부
                if (_ChoiceController.finChoice) {
                    isChoice = false;
                    choiceList.SetActive(false);
                    textIdx += 2;
                }
                else {
                    return;
                }
            }

            isTyping = true;

            // 이름, 업샷 활성화, 텍스트 위치 재설정
            playerImage.enabled = true;
            talkName.enabled = true;
            nameLabel.enabled = true;
            talkText.rectTransform.anchoredPosition = new Vector2(600.0f, 50.0f);

            // 모든 대사 출력 완료 시 대화 종료
            if (textIdx == dialogue.Length) {
                textIdx = 0;
                isTalking = false;
                npcImage.enabled = false;
                talkPanel.SetActive(false);
                if (talkPlayer.Equals("???"))
                    Invoke("PrologueSecondStart", 2.0f);
                else if (talkPlayer.Equals("소니아")) {
                    FindObjectOfType<FadeEffectController>().SceneChange(1);
                }
                return;
            }

            // 대사 파싱 (이름-업샷-대사 분리)
            string[] splitDialogue;
            splitDialogue = dialogue[textIdx].Split('&');

            talkName.text = splitDialogue[0];

            if (splitDialogue[0].Equals("CHOICE")) {
                if (splitDialogue.Length == 4) {
                    talkName.text = splitDialogue[1];
                    npcImage.enabled = true;
                    npcImage.sprite = Resources.Load<Sprite>("Upshot/" + splitDialogue[1] + "_" + splitDialogue[2]);
                    npcImage.color = new Color(1.0f, 1.0f, 1.0f);
                    playerImage.color = new Color(0.4f, 0.4f, 0.4f);
                    isChoice = true;
                    TypeEffectStart(splitDialogue[3]);
                }
            }
            else {
                switch (splitDialogue.Length) {
                    // 효과음, 시스템 문구 (이름 x, 업샷 x, 대사 o)
                    case 1:
                        // 필요없는 이미지 비활성화
                        playerImage.enabled = false;
                        npcImage.enabled = false;
                        talkName.enabled = false;
                        nameLabel.enabled = false;

                        talkText.rectTransform.anchoredPosition = new Vector2(395.0f, 50.0f);    // 텍스트 위치 재설정
                        TypeEffectStart(splitDialogue[0]);
                        break;
                    // 독백 (이름 o, 업샷 x, 대사 o)
                    case 2:
                        // NPC 업샷 비활성화
                        npcImage.enabled = false;
                        playerImage.color = new Color(0.4f, 0.4f, 0.4f);
                        TypeEffectStart(splitDialogue[1]);
                        break;
                    // 일반 대사 (이름 o, 업샷 o, 대사 o)
                    case 3:
                        if (splitDialogue[0].Equals(talkPlayer)) {    // !! 화자 업샷 정보로 수정할 것 !!
                            if (talkPlayer.Equals("???"))
                                playerImage.sprite = Resources.Load<Sprite>("Upshot/" + "someone" + "_" + splitDialogue[1]);
                            else
                                playerImage.sprite = Resources.Load<Sprite>("Upshot/" + splitDialogue[0] + "_" + splitDialogue[1]);
                            playerImage.color = new Color(1.0f, 1.0f, 1.0f);
                            npcImage.color = new Color(0.4f, 0.4f, 0.4f);
                        }
                        else {
                            npcImage.enabled = true;
                            npcImage.sprite = Resources.Load<Sprite>("Upshot/" + splitDialogue[0] + "_" + splitDialogue[1]);
                            playerImage.color = new Color(0.4f, 0.4f, 0.4f);
                            npcImage.color = new Color(1.0f, 1.0f, 1.0f);
                        }
                        TypeEffectStart(splitDialogue[2]);
                        break;
                }
            }

            textIdx++;
        }
    }

    // 타이핑 효과 초기화 메서드
    void TypeEffectStart(string curDialogue)
    {
        typeDialogue = curDialogue;
        talkText.text = "";
        charIdx = 0;

        Invoke("TypeEffect", 1 / charPerSecond);
    }

    void TypeEffect()
    {
        // 타이핑 효과 종료 (혹은 조기 종료)
        if (talkText.text.Equals(typeDialogue) || !isTyping) {
            talkText.text = typeDialogue;
            isTyping = false;

            // 선택지 목록 초기화
            if (isChoice) {
                choiceList.SetActive(true);
                _ChoiceController.InitAnswer(dialogue[textIdx], dialogue[textIdx + 1]);
            }
            return;
        }

        talkText.text += typeDialogue[charIdx];
        charIdx++;

        Invoke("TypeEffect", 1 / charPerSecond);
    }
}
