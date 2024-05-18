using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectActivator : MonoBehaviour
{
    // 활성화/비활성화 대상 오브젝트 List
    public List<GameObject> objList = new List<GameObject>();

    QuestController _QuestController;
    ClearController _ClearController;

    private void Awake()
    {
        _QuestController = FindObjectOfType<QuestController>();
        _ClearController = FindObjectOfType<ClearController>();
    }

    private void Update()
    {
        // 활성화/비활성화 대상이 없는 경우
        if(objList.Count == 0) {
            return;
        }

        /* Quest 1/2 은신처 NPC 비활성화 */
        if(SceneManager.GetActiveScene().name == "StandShelter"){
            if (!_QuestController.episode1Start) {
                objList[3].SetActive(false);
                objList[4].SetActive(false);
            }
                

            if(_QuestController.questNum == 1 || (_QuestController.questNum == 2 && _QuestController.questActivated == 0)){
                foreach (GameObject obj in objList) {
                    obj.SetActive(false);
                }
            }
            if ((_QuestController.questNum == 2 && _QuestController.questActivated == 1) || _QuestController.questNum > 2) {
                objList[1].SetActive(true);
            }
        }

        /* Quest 2 입국심사장 NPC 비활성화 */
        if(SceneManager.GetActiveScene().name == "ImmigrationCenter" && _QuestController.questNum == 2 && _QuestController.questActivated == 1) {
            foreach(GameObject obj in objList) {
                obj.SetActive(false);
            }
        }

        /* Quest 3 게네하토 공터 NPC 활성화 */
        if(SceneManager.GetActiveScene().name == "GenehatoVecantLot" && _QuestController.questNum == 3 && _QuestController.questActivated == 0 && _ClearController.stageClear) {
            foreach(GameObject obj in objList) {
                obj.SetActive(true);
            }
        }
        /* Quest 4 게네하토 공터 NPC 비활성화 */
        if(SceneManager.GetActiveScene().name == "GenehatoVecantLot" && _QuestController.questNum == 3 && _QuestController.questActivated == 1 && _ClearController.stageClear) {
            objList[1].SetActive(false);
        }
    }
}
