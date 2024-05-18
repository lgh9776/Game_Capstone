using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    ��� ��ȣ�ۿ� ���� ��ũ��Ʈ
*/

public class TalkController : MonoBehaviour
{
    // Singleton ���� �ν��Ͻ�
    private static TalkController instance = null;

    // ���Ǽҵ� �� ȭ�� ����
    private enum talkPlayerType {Player, Boss};    
    [SerializeField] private talkPlayerType type;
    private string talkPlayer;

    // �ܺ� ��� ������ import�� ���� IOController
    IOController _IOController;

    // ������ ���� ��ũ��Ʈ
    ChoiceController _ChoiceController;

    // ����Ʈ ���� ��ũ��Ʈ
    QuestController _QuestController;
    QuestActivator _QuestActivator;

    // Talk Panel ���� ������Ʈ
    public GameObject talkPanel;
    public Image npcImage;
    public Image playerImage;
    public Image nameLabel;
    public Text talkText;
    public Text talkName;
    public GameObject choiceList;

    // ��ȭ ��ȣ�ۿ� ����
    [HideInInspector] public bool isTalking = false;

    [SerializeField] float charPerSecond;    // ��� ��� �ӵ�
    private string typeDialogue;    // Ÿ������ ���� ���ڿ� �ӽ� ���� ����
    private bool isTyping = false;    // ��� Ÿ���� ����

    private int textIdx;    // ��� index
    private int charIdx;    // ��� ���� index

    // ��� ������
    private string[] dialogue;    

    // ������ ����
    private bool isChoice = false;

    // ����Ʈ ����/Ŭ���� ���, �̺�Ʈ �߻� ��� ����
    private bool startQuest = false;
    private bool endQuest = false;
    private bool isEvent = false;

    // ��� �ڵ� ��� ������Ʈ ����� ����
    private GameObject autoTalker;

    public void Awake()
    {
        // Singleton ���� ����
        if (null == instance) {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else {
            Destroy(this.gameObject);
        }

        // Script Component �ʱ�ȭ
        _IOController = FindObjectOfType<IOController>();
        _ChoiceController = FindObjectOfType<ChoiceController>();
        _QuestController = FindObjectOfType<QuestController>();
        _QuestActivator = FindObjectOfType<QuestActivator>();
        
        // ȭ�� ����
        if (type == talkPlayerType.Player)
            talkPlayer = "�ҴϾ�";
        else
            talkPlayer = "����";

        textIdx = 0;
    }

    // Talk Panel �ʱ�ȭ
    public void InitTalkPanel(GameObject obj)
    {
        isTyping = true;
        isTalking = true;

        // �ܺ� ��� ������ import
        TalkObjData tObjData = obj.GetComponent<TalkObjData>();
        dialogue = _IOController.LoadDialogue(tObjData.objID, tObjData.dialogueID);

        // NPC�� �ƴ� ��� ���� ��Ȱ��ȭ
        if (!tObjData.isNPC)
            npcImage.enabled = false;

        // ��� �ڵ� ���� ���� Ȯ��
        if (tObjData.autoStart)
            autoTalker = tObjData.gameObject;

        // ��� �г� �� �÷��̾� �̹��� Ȱ��ȭ, �ؽ�Ʈ ��� ��ġ ���� 
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

        // ��� �Ľ� (�̸�-����-��� �и�)
        string[] splitDialogue;
        splitDialogue = dialogue[0+questCommandSkip].Split('&');

        talkName.text = splitDialogue[0];

        switch (splitDialogue.Length) {
            // ȿ����, �ý��� ���� (�̸� x, ���� x, ��� o)
            case 1:
                // �ʿ���� �̹��� ��Ȱ��ȭ
                playerImage.enabled = false;
                npcImage.enabled = false;
                talkName.enabled = false;
                nameLabel.enabled = false;

                talkText.rectTransform.anchoredPosition = new Vector2(395.0f, 50.0f);    // �ؽ�Ʈ ��ġ �缳��
                TypeEffectStart(splitDialogue[0]);
                break;
            // ���� (�̸� o, ���� x, ��� o)
            case 2:
                // NPC ���� ��Ȱ��ȭ
                npcImage.enabled = false;
                playerImage.color = new Color(0.4f, 0.4f, 0.4f);
                TypeEffectStart(splitDialogue[1]);
                break;
            // �Ϲ� ��� (�̸� o, ���� o, ��� o)
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

    // ��ȭ ��ȣ�ۿ�
    public void Talk()
    {
        // Ÿ���� ȿ�� ���� ����
        if(isTyping) {
            isTyping = false;
        }
        // ��� ���
        else {
            // ������ ��� ����
            if (isChoice) {
                // ���� ���� ����
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

            // �̸�, ���� Ȱ��ȭ, �ؽ�Ʈ ��ġ �缳��
            playerImage.enabled = true;
            talkName.enabled = true;
            nameLabel.enabled = true;
            talkText.rectTransform.anchoredPosition = new Vector2(600.0f, 50.0f);

            // ��� ��� ��� �Ϸ� �� ��ȭ ����
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

                // ��� �ڵ� ���� ������Ʈ ��Ȱ��ȭ (�ߺ� �ν� ����)
                if (autoTalker != null) {
                    if (autoTalker.GetComponent<TalkObjData>().autoStartRemove) {
                        autoTalker.SetActive(false);
                    }
                }    

                return;
            }

            // ��� �Ľ� (�̸�-����-��� �и�)
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
                    // ȿ����, �ý��� ���� (�̸� x, ���� x, ��� o)
                    case 1:
                        // �ʿ���� �̹��� ��Ȱ��ȭ
                        playerImage.enabled = false;
                        npcImage.enabled = false;
                        talkName.enabled = false;
                        nameLabel.enabled = false;

                        talkText.rectTransform.anchoredPosition = new Vector2(395.0f, 50.0f);    // �ؽ�Ʈ ��ġ �缳��
                        TypeEffectStart(splitDialogue[0]);
                        break;
                    // ���� (�̸� o, ���� x, ��� o)
                    case 2:
                        // NPC ���� ��Ȱ��ȭ
                        npcImage.enabled = false;
                        playerImage.color = new Color(0.4f, 0.4f, 0.4f);
                        TypeEffectStart(splitDialogue[1]);
                        break;
                    // �Ϲ� ��� (�̸� o, ���� o, ��� o)
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

    // Ÿ���� ȿ�� �ʱ�ȭ �޼���
    void TypeEffectStart(string curDialogue)
    {
        typeDialogue = curDialogue;
        talkText.text = "";
        charIdx = 0;

        Invoke("TypeEffect", 1 / charPerSecond);
    }

    // Ÿ���� ȿ�� �޼���
    void TypeEffect()
    {
        // Ÿ���� ȿ�� ���� (Ȥ�� ���� ����)
        if (talkText.text.Equals(typeDialogue) || !isTyping) {
            talkText.text = typeDialogue;
            isTyping = false;
            
            // ������ ��� �ʱ�ȭ
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
