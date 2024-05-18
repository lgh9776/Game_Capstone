using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkObjData : MonoBehaviour
{
    public int objID;    // 오브젝트 ID
    public int dialogueID;    // 오브젝트 대사 ID
    public bool isNPC;    // NPC 여부
    public bool autoStart;    // 자동 대사 출력 오브젝트 여부
    public bool autoStartRemove;    // 자동 대사 출력 오브젝트 1회 출력 후 비활성화 여부
    public bool forQuest;
    public int forQuestNum;
    // public bool forEpisodeStart;

    // NPC Image 좌우 변환을 위한 변수
    private GameObject player;
    private Transform npcSprite;

    // 퀘스트 제어 스크립트
    QuestController _QuestController;

    PlayerController _PlayerController;

    private void Awake()
    {
        _QuestController = GameObject.Find("QuestController").GetComponent<QuestController>();
        _PlayerController = GameObject.Find("Player").GetComponent<PlayerController>();

        // NPC Image 및 대상 Player 초기화
        if (isNPC) {
            player = GameObject.Find("Player");
            npcSprite = transform.parent;
        }

        // 자동 수락 퀘스트 대사의 재등장 방지
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
        // Episode 자동 시작 오브젝트
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
        FlipNPC();    // 플레이어 방향으로 바라보는 방향 전환

        // 대화 상호작용 가능 표시 UI 활성화/비활성화
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
