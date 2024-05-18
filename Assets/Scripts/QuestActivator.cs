using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestActivator : MonoBehaviour
{
    // Singleton 패턴 인스턴스
    private static QuestActivator instance = null;

    // 퀘스트 번호 지정 변수
    public int activator = 1;

    private bool isActivating = false;

    QuestController _QuestController;
    PlayerController _PlayerController;

    private void Awake()
    {
        // Singleton 패턴 구현
        if (null == instance) {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else {
            Destroy(this.gameObject);
        }

        _QuestController = GameObject.Find("QuestController").GetComponent<QuestController>();
        _PlayerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
       /*
           [Quest 1 활성화]
           허가증 찾으러 가기
           (입국심사장 진입 시 자동으로 퀘스트 수락)
       */
       if(SceneManager.GetActiveScene().name == "ImmigrationCenter" && activator == 1 && !isActivating) {
            isActivating = true;
            _PlayerController.autoTalking = true;    // 대사 자동 출력 퀘스트
       }
       
       /*
           [Quest 2 활성화]
           호이트에게 허가증 가져다 주기
           (허가증 습득 시 자동으로 퀘스트 수락)
       */
       if(SceneManager.GetActiveScene().name == "StandShelter" && activator == 2 && _QuestController.questActivated == 1 && !isActivating) {
            isActivating = true;
            QuestActivate();
       }

       /*
           [Quest 3 활성화]
           몬스터 웨이브 막기
           (게네하토 공터 진입 시 자동으로 퀘스트 수락)
       */
       if(SceneManager.GetActiveScene().name == "GenehatoVecantLot" && activator == 3 && !isActivating) {
            Debug.Log("Quest Activated");
            isActivating = true;
            QuestActivate();
       }

       /*
           [Quest 4 활성화]
           지역1 보스 처치
           (게네하토 하수 처리장 진입 시 자동으로 퀘스트 수락)
       */
       if(SceneManager.GetActiveScene().name == "GenehatoSewagePlant" && activator == 4 && !isActivating) {
            Debug.Log("Quest Activated");
            isActivating = true;
            _PlayerController.autoTalking = true;    // 대사 자동 출력 퀘스트
        }
    }

    public void QuestActivate()
    {
        _QuestController.questNum = activator;
        _QuestController.questActivated = 0;
        activator++;
        isActivating = false;
    }
}
