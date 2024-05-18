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
        talkPlayer = "�ҴϾ�";
        secondTitle.SetActive(true);
        prologueControlObject.GetComponent<PrologueTalkObjData>().dialogueID = 1;
    }

    private void PrologueDialogueStart()
    {
        isTyping = true;
        isTalking = true;

        // �ܺ� ��� ������ import
        PrologueTalkObjData tObjData = prologueControlObject.GetComponent<PrologueTalkObjData>();
        dialogue = _IOController.LoadDialogue(tObjData.objID, tObjData.dialogueID);

        // ��� �г� �� �÷��̾� �̹��� Ȱ��ȭ, �ؽ�Ʈ ��� ��ġ ���� 
        talkPanel.SetActive(isTalking);
        if(talkPlayer.Equals("???"))
            playerImage.sprite = Resources.Load<Sprite>("Upshot/" + "someone" + "_silhouette");    // !! ȭ�� ���� ������ ������ �� !!
        else if(talkPlayer.Equals("�ҴϾ�"))
            playerImage.sprite = Resources.Load<Sprite>("Upshot/" + "�ҴϾ�" + "_blank");    // !! ȭ�� ���� ������ ������ �� !!

        npcImage.enabled = false;
        talkText.rectTransform.anchoredPosition = new Vector2(600.0f, 50.0f);

        // ��� �Ľ� (�̸�-����-��� �и�)
        string[] splitDialogue;
        splitDialogue = dialogue[0].Split('&');

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
                if (splitDialogue[0].Equals(talkPlayer)) {    // !! ȭ�� ���� ������ ������ �� !!
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
        // Ÿ���� ȿ�� ���� ����
        if (isTyping) {
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
                textIdx = 0;
                isTalking = false;
                npcImage.enabled = false;
                talkPanel.SetActive(false);
                if (talkPlayer.Equals("???"))
                    Invoke("PrologueSecondStart", 2.0f);
                else if (talkPlayer.Equals("�ҴϾ�")) {
                    FindObjectOfType<FadeEffectController>().SceneChange(1);
                }
                return;
            }

            // ��� �Ľ� (�̸�-����-��� �и�)
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
                        if (splitDialogue[0].Equals(talkPlayer)) {    // !! ȭ�� ���� ������ ������ �� !!
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

    // Ÿ���� ȿ�� �ʱ�ȭ �޼���
    void TypeEffectStart(string curDialogue)
    {
        typeDialogue = curDialogue;
        talkText.text = "";
        charIdx = 0;

        Invoke("TypeEffect", 1 / charPerSecond);
    }

    void TypeEffect()
    {
        // Ÿ���� ȿ�� ���� (Ȥ�� ���� ����)
        if (talkText.text.Equals(typeDialogue) || !isTyping) {
            talkText.text = typeDialogue;
            isTyping = false;

            // ������ ��� �ʱ�ȭ
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
