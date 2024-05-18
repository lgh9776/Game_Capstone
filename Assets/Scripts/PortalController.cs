using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    ��Ż�� Scene �̵� ���� ��ũ��Ʈ   
*/

public class PortalController : MonoBehaviour
{
    private enum PortalDirection { left, right, up, down };

    ClearController _ClearController;
    PlayerController _PlayerController;
    QuestController _QuestController;

    // ��Ż�� �̵���ų ���� Scene
    [SerializeField] private int nextScene;

    // ��Ż�� �÷��̾ �̵���ų �ʵ� ����
    [SerializeField] private PortalDirection dir;

    // ��Ż�� Ȱ��ȭ��Ű�� ����Ʈ ���� ��Ȳ
    [SerializeField] private int activated;

    // ����Ʈ ��Ȳ�� ���� ��Ż�� ���/�ر��� �ʿ��� ���
    [SerializeField] private bool needLock;
    [SerializeField] private int quest;
    [SerializeField] [Tooltip("����Ʈ �Ϸ� �� �ر� [0], ����Ʈ �Ϸ� �� ���[1]")] private int lockNum;

    // ���θ����� �� �޽��� ��� ����
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
        // ����Ʈ ��Ȳ�� ���� ��Ż ���/�ر�
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

    // ��Ż �̵�
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && _ClearController.stageClear) {
            if(activated <= _QuestController.questNum) {
                // ��Ż ���⿡ ���� Scene ��ȯ ���� �÷��̾� ��ġ �ʱ�ȭ
                if (dir == PortalDirection.down)
                    _PlayerController.sceneInitPos = 0;
                else if (dir == PortalDirection.up)
                    _PlayerController.sceneInitPos = 1;
                else if (dir == PortalDirection.right)
                    _PlayerController.sceneInitPos = 2;
                else if (dir == PortalDirection.left)
                    _PlayerController.sceneInitPos = 3;

                // Scene ��ȯ
                FindObjectOfType<FadeEffectController>().SceneChange(nextScene);
                // SceneManager.LoadScene(nextScene);
            }
            else if (isBlockMessage) {
                _PlayerController.autoTalking = true;
            }
        }
    }
}
