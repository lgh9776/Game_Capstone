using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    포탈의 Scene 이동 제어 스크립트   
*/

public class PortalController : MonoBehaviour
{
    private enum PortalDirection { left, right, up, down };

    ClearController _ClearController;
    PlayerController _PlayerController;
    QuestController _QuestController;

    // 포탈이 이동시킬 다음 Scene
    [SerializeField] private int nextScene;

    // 포탈이 플레이어를 이동시킬 필드 방향
    [SerializeField] private PortalDirection dir;

    // 포탈을 활성화시키는 퀘스트 진행 상황
    [SerializeField] private int activated;

    // 퀘스트 상황에 따라 포탈의 잠금/해금이 필요한 경우
    [SerializeField] private bool needLock;
    [SerializeField] private int quest;
    [SerializeField] [Tooltip("퀘스트 완료 시 해금 [0], 퀘스트 완료 시 잠금[1]")] private int lockNum;

    // 가로막혔을 때 메시지 출력 여부
    [SerializeField] private bool isBlockMessage;

    public GameObject chain;

    private void Awake()
    {
        _ClearController = GameObject.Find("ClearController").GetComponent<ClearController>();
        _PlayerController = GameObject.Find("Player").GetComponent<PlayerController>();
        _QuestController = GameObject.Find("QuestController").GetComponent<QuestController>();
    }

    private void Update()
    {
        // 퀘스트 상황에 따른 포탈 잠금/해금
        if (needLock && _QuestController.questNum <= quest && _QuestController.questNum > 0) {
            if (_QuestController.questActivated == lockNum) {
                activated = 99;
            }
            else {
                activated = _QuestController.questNum;
            }
        }

        if (activated != 99 && _ClearController.stageClear)
            chain.SetActive(false);
        else
            chain.SetActive(true);
    }

    // 포탈 이동
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && _ClearController.stageClear) {
            if(activated <= _QuestController.questNum) {
                // 포탈 방향에 따른 Scene 전환 시의 플레이어 위치 초기화
                if (dir == PortalDirection.down)
                    _PlayerController.sceneInitPos = 0;
                else if (dir == PortalDirection.up)
                    _PlayerController.sceneInitPos = 1;
                else if (dir == PortalDirection.right)
                    _PlayerController.sceneInitPos = 2;
                else if (dir == PortalDirection.left)
                    _PlayerController.sceneInitPos = 3;

                // Scene 전환
                FindObjectOfType<FadeEffectController>().SceneChange(nextScene);
                // SceneManager.LoadScene(nextScene);
            }
            else if (isBlockMessage) {
                _PlayerController.autoTalking = true;
            }
        }
    }
}
