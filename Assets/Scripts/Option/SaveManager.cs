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
        if (PlayerPrefs.HasKey("PlayerX")) // 세이브 이력 체크 -> 이력 없을 시 로드x
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
    현재 세이브 기능은 questNum과 questActivated만 저장
    -> 퀘스트 관련 오브젝트 따로 저장 필요 (저장보다는 상황에 따라 끄고 켜도록 설정)

    questController에 오브젝트를 컨트롤하는 메소드 따로 설정 필요 (line 61참조)
    ex) 어떤 퀘스트의 경우 몬스터 10마리를 죽여야함 -> 게임 시작 시 몬스터 자동 생성
    퀘스트 완료 후 저장 -> 다시 로드했을 때 몬스터가 없어야하는데 있음

    해결) questNum과 questActivated에 따라 몬스터 오브젝트를 setActive로 컨트롤하는 메소드 필요
    GameLoad 마지막에 실행시켜줌
    */



}
