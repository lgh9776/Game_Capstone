using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestActivator : MonoBehaviour
{
    // Singleton ���� �ν��Ͻ�
    private static QuestActivator instance = null;

    // ����Ʈ ��ȣ ���� ����
    public int activator = 1;

    private bool isActivating = false;

    QuestController _QuestController;
    PlayerController _PlayerController;

    private void Awake()
    {
        // Singleton ���� ����
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
           [Quest 1 Ȱ��ȭ]
           �㰡�� ã���� ����
           (�Ա��ɻ��� ���� �� �ڵ����� ����Ʈ ����)
       */
       if(SceneManager.GetActiveScene().name == "ImmigrationCenter" && activator == 1 && !isActivating) {
            isActivating = true;
            _PlayerController.autoTalking = true;    // ��� �ڵ� ��� ����Ʈ
       }
       
       /*
           [Quest 2 Ȱ��ȭ]
           ȣ��Ʈ���� �㰡�� ������ �ֱ�
           (�㰡�� ���� �� �ڵ����� ����Ʈ ����)
       */
       if(SceneManager.GetActiveScene().name == "StandShelter" && activator == 2 && _QuestController.questActivated == 1 && !isActivating) {
            isActivating = true;
            QuestActivate();
       }

       /*
           [Quest 3 Ȱ��ȭ]
           ���� ���̺� ����
           (�Գ����� ���� ���� �� �ڵ����� ����Ʈ ����)
       */
       if(SceneManager.GetActiveScene().name == "GenehatoVecantLot" && activator == 3 && !isActivating) {
            Debug.Log("Quest Activated");
            isActivating = true;
            QuestActivate();
       }

       /*
           [Quest 4 Ȱ��ȭ]
           ����1 ���� óġ
           (�Գ����� �ϼ� ó���� ���� �� �ڵ����� ����Ʈ ����)
       */
       if(SceneManager.GetActiveScene().name == "GenehatoSewagePlant" && activator == 4 && !isActivating) {
            Debug.Log("Quest Activated");
            isActivating = true;
            _PlayerController.autoTalking = true;    // ��� �ڵ� ��� ����Ʈ
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
