using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{

    public QuestController questController;
    public GameObject player;
    public GameObject optionPanel;

    void Start()
    {
        GameLoad();
    }

    void Update()
    {
        
    }

    public void GameSave()
    {
        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
        PlayerPrefs.SetInt("QuestNum", questController.questNum);
        PlayerPrefs.SetInt("QuestActivated", questController.questActivated);
        PlayerPrefs.Save();

        optionPanel.SetActive(false);
    }

    public void GameLoad()
    {
        if (PlayerPrefs.HasKey("PlayerX")) // ���̺� �̷� üũ -> �̷� ���� �� �ε�x
            return;

        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");
        int questNum = PlayerPrefs.GetInt("QuestNum");
        int questActivated = PlayerPrefs.GetInt("QuestActivated");

        player.transform.position = new Vector3(x, y, 0);
        questController.questNum = questNum;
        questController.questActivated = questActivated;
    }

    public void GameExit()
    {
        Application.Quit();
    }

    /**
    ���� ���̺� ����� questNum�� questActivated�� ����
    -> ����Ʈ ���� ������Ʈ ���� ���� �ʿ� (���庸�ٴ� ��Ȳ�� ���� ���� �ѵ��� ����)

    questController�� ������Ʈ�� ��Ʈ���ϴ� �޼ҵ� ���� ���� �ʿ� (line 61����)
    ex) � ����Ʈ�� ��� ���� 10������ �׿����� -> ���� ���� �� ���� �ڵ� ����
    ����Ʈ �Ϸ� �� ���� -> �ٽ� �ε����� �� ���Ͱ� ������ϴµ� ����

    �ذ�) questNum�� questActivated�� ���� ���� ������Ʈ�� setActive�� ��Ʈ���ϴ� �޼ҵ� �ʿ�
    GameLoad �������� ���������
    */



}
