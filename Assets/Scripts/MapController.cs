using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapController : MonoBehaviour
{
    public List<GameObject> map1;
    public List<GameObject> map1Link;
    public GameObject curMapPos;

    private int sceneNum;

    QuestController _QuestController;

    private void Awake()
    {
        _QuestController = FindObjectOfType<QuestController>();
    }

    private void Update()
    {
        sceneNum = SceneManager.GetActiveScene().buildIndex - 3;
        curMapPos.GetComponent<RectTransform>().anchoredPosition = map1[sceneNum].GetComponent<RectTransform>().anchoredPosition;
        
        if(_QuestController.questNum == 1) {
            map1Link[0].SetActive(false);
            map1Link[1].SetActive(true);
            map1Link[2].SetActive(true);
            map1Link[3].SetActive(false);
            map1Link[4].SetActive(true);
            map1Link[5].SetActive(false);
        }

        if (_QuestController.questNum == 2 && _QuestController.questActivated == 1) {
            map1Link[0].SetActive(true);
            map1Link[1].SetActive(false);
            map1Link[2].SetActive(false);
            map1Link[3].SetActive(true);
            map1Link[4].SetActive(false);
            map1Link[5].SetActive(true);
            map1Link[6].SetActive(true);
            map1Link[7].SetActive(false);
        }

        if(_QuestController.questNum == 3 && _QuestController.questActivated == 1) {
            map1Link[8].SetActive(true);
            map1Link[9].SetActive(false);
        }
    }
}
