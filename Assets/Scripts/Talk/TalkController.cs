using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    대사 상호작용 제어 스크립트
*/

public class TalkController : MonoBehaviour
{
    // Singleton 패턴 인스턴스
    private static TalkController instance = null;

    // 에피소드 별 화자 선택
    private enum talkPlayerType {Player, Boss};    
    [SerializeField] private talkPlayerType type;
    private string talkPlayer;

    // 외부 대사 데이터 import를 위한 IOController
    IOController _IOController;

    // 선택지 제어 스크립트
    ChoiceController _ChoiceController;

    // 퀘스트 제어 스크립트
    QuestController _QuestController;
    QuestActivator _QuestActivator;

    // Talk Panel 관련 오브젝트
    public GameObject talkPanel;
    public Image npcImage;
    public Image playerImage;
    public Image nameLabel;
    public Text talkText;
    public Text talkName;
    public GameObject choiceList;

    // 대화 상호작용 여부
    [HideInInspector] public bool isTalking = false;

    [SerializeField] float charPerSecond;    // 대사 출력 속도
    private string typeDialogue;    // 타이핑을 위한 문자열 임시 저장 변수
    private bool isTyping = false;    // 대사 타이핑 여부

    private int textIdx;    // 대사 index
    private int charIdx;    // 대사 문자 index

    // 대사 데이터
    private string[] dialogue;    

    // 선택지 여부
    private bool isChoice = false;

    // 퀘스트 시작/클리어 대사, 이벤트 발생 대사 여부
    private bool startQuest = false;
    private bool endQuest = false;
    private bool isEvent = false;

    // 대사 자동 출력 오브젝트 저장용 변수
    private GameObject autoTalker;

    public void Awake()
    {
        // Singleton 패턴 구현
        if (null == instance) {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else {
            Destroy(this.gameObject);
        }

        // Script Component 초기화
        _IOController = FindObjectOfType<IOController>();
        _ChoiceController = FindObjectOfType<ChoiceController>();
        _QuestController = FindObjectOfType<QuestController>();
        _QuestActivator = FindObjectOfType<QuestActivator>();
        
        // 화자 구분
        if (type == talkPlayerType.Player)
            talkPlayer = "소니아";
        else
            talkPlayer = "보스";

        textIdx = 0;
    }

    // Talk Panel 초기화
    public void InitTalkPanel(GameObject obj)
    {
        isTyping = true;
        isTalking = true;

        // 외부 대사 데이터 import
        TalkObjData tObjData = obj.GetComponent<TalkObjData>();
        dialogue = _IOController.LoadDialogue(tObjData.objID, tObjData.dialogueID);

        // NPC가 아닌 경우 업샷 비활성화
        if (!tObjData.isNPC)
            npcImage.enabled = false;

        // 대사 자동 시작 여부 확인
        if (tObjData.autoStart)
            autoTalker = tObjData.gameObject;

        // 대사 패널 및 플레이어 이미지 활성화, 텍스트 출력 위치 지정 
        talkPanel.SetActive(isTalking);
        playerImage.sprite = Resources.Load<Sprite>("Upshot/" + talkPlayer + "_blank");
        talkText.rectTransform.anchoredPosition = new Vector2(600.0f, 50.0f);

        int questCommandSkip = 0;
        if (dialogue[0] == "EPISODE_START") {
            questCommandSkip = 1;
            textIdx++;
        }
        if (dialogue[0] == "QUEST_START") {
            questCommandSkip = 1;
            startQuest = true;
            textIdx++;
        }
        if (dialogue[0] == "QUEST_CLEAR") {
            questCommandSkip = 1;
            endQuest = true;
            textIdx++;
        }
        if(dialogue[0] == "SuddenEvent") {
            questCommandSkip = 1;
            isEvent = true;
            textIdx++;
        }

        // 대사 파싱 (이름-업샷-대사 분리)
        string[] splitDialogue;
        splitDialogue = dialogue[0+questCommandSkip].Split('&');

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
                if (splitDialogue[0].Equals(talkPlayer)) {
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

    // 대화 상호작용
    public void Talk()
    {
        // 타이핑 효과 조기 종료
        if(isTyping) {
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
                if(_QuestController.episode1Start)
                    _QuestController.episode1Start = false;

                if (startQuest) {
                    startQuest = false;
                    _QuestActivator.QuestActivate();
                }
                if (endQuest) {
                    _QuestController.questActivated = 1;
                    endQuest = false;
                }
                if (isEvent) {
                    SearchLightContactController _SearchLightContactController = FindObjectOfType<SearchLightContactController>();
                    _SearchLightContactController.ContactEvent();
                    isEvent = false;
                }
                
                textIdx = 0;
                isTalking = false;
                npcImage.enabled = false;
                talkPanel.SetActive(false);

                // 대사 자동 시작 오브젝트 비활성화 (중복 인식 방지)
                if (autoTalker != null) {
                    if (autoTalker.GetComponent<TalkObjData>().autoStartRemove) {
                        autoTalker.SetActive(false);
                    }
                }    

                return;
            }

            // 대사 파싱 (이름-업샷-대사 분리)
            string[] splitDialogue;
            splitDialogue = dialogue[textIdx].Split('&');

            talkName.text = splitDialogue[0];

            if (splitDialogue[0].Equals("CHOICE")) {
                if(splitDialogue.Length == 4) {
                    talkName.text = splitDialogue[1];
                    npcImage.enabled = true;
                    npcImage.sprite = Resources.Load<Sprite>("Upshot/" + splitDialogue[1] + "_" + splitDialogue[2]);
                    npcImage.color = new Color(1.0f, 1.0f, 1.0f);
                    playerImage.color = new Color(0.4f, 0.4f, 0.4f);
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
                        if (splitDialogue[0].Equals(talkPlayer)) {
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

    // 타이핑 효과 메서드
    void TypeEffect()
    {
        // 타이핑 효과 종료 (혹은 조기 종료)
        if (talkText.text.Equals(typeDialogue) || !isTyping) {
            talkText.text = typeDialogue;
            isTyping = false;
            
            // 선택지 목록 초기화
            if(isChoice) {
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
