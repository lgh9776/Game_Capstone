using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkObjData : MonoBehaviour
{
    public int objID;    // ������Ʈ ID
    public int dialogueID;    // ������Ʈ ��� ID
    public bool isNPC;    // NPC ����
    public bool autoStart;    // �ڵ� ��� ��� ������Ʈ ����
    public bool autoStartRemove;    // �ڵ� ��� ��� ������Ʈ 1ȸ ��� �� ��Ȱ��ȭ ����
    public bool forQuest;
    public int forQuestNum;
    // public bool forEpisodeStart;

    // NPC Image �¿� ��ȯ�� ���� ����
    private GameObject player;
    private Transform npcSprite;

    // ����Ʈ ���� ��ũ��Ʈ
    QuestController _QuestController;

    PlayerController _PlayerController;

    private void Awake()
    {
        _QuestController = GameObject.Find("QuestController").GetComponent<QuestController>();
        _PlayerController = GameObject.Find("Player").GetComponent<PlayerController>();

        // NPC Image �� ��� Player �ʱ�ȭ
        if (isNPC) {
            player = GameObject.Find("Player");
            npcSprite = transform.parent;
        }

        // �ڵ� ���� ����Ʈ ����� ����� ����
        if (forQuest) {
            if(_QuestController.questNum >= forQuestNum) {
                gameObject.SetActive(false);
            }
            /*
                switch (objID) {
                    case 1:
                        if (_QuestController.questNum >= forQuestNum)
                            gameObject.SetActive(false);
                        break;
            }
            */
        }

        /*
        // Episode �ڵ� ���� ������Ʈ
        if(!forQuest && autoStart && forEpisodeStart) {
            if (!_QuestController.episode1Start) {
                gameObject.SetActive(false);
            }
            else {
                _PlayerController.autoTalking = true;
            }
        }
        */
    }

    private void Update()
    {
        FlipNPC();    // �÷��̾� �������� �ٶ󺸴� ���� ��ȯ

        // ��ȭ ��ȣ�ۿ� ���� ǥ�� UI Ȱ��ȭ/��Ȱ��ȭ
        if (transform.parent.childCount >= 2) {
            if (_PlayerController.scanObj == gameObject) {
                transform.parent.GetChild(1).gameObject.SetActive(true);
            }
            else {
                transform.parent.GetChild(1).gameObject.SetActive(false);
            }
        }
    }

    private void FlipNPC()
    {
        if (isNPC) {
            if (player.transform.position.x < npcSprite.position.x)
                npcSprite.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            else
                npcSprite.gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
    }
}
