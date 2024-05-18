using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
    필드 클리어 확인 및 제어 스크립트
*/

public class ClearController : MonoBehaviour
{
    // private static ClearController instance = null;

    private enum ClearType { none, objectCollecting, eliminateMonster, searchLight };    // 필드 클리어 방식

    [SerializeField] private ClearType type;
    [SerializeField] private int needCollectNum = 5;    // 오브젝트 수집 필요량 (오브젝트 수집 방식)
    [SerializeField] private int needEliminateNum = 5;

    [HideInInspector] public int collectCount = 0;
    [HideInInspector] public int eliminateCount = 0;

    public bool stageClear = false;

    // 필드 클리어 조건 표시 UI Text
    private TMP_Text stageCondition;
    private TMP_Text stageGoal;

    private void Awake()
    {
        /*
        if (null == instance) {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else {
            Destroy(this.gameObject);
        }
        */

        stageCondition = GameObject.Find("FieldCondition").GetComponent<TMP_Text>();
        stageGoal = GameObject.Find("FieldClearState").GetComponent<TMP_Text>();

        if (type == ClearType.none) {
            stageCondition.text = "▶ 필드 클리어 조건 없음";
            stageGoal.text = "";
            stageClear = true;
        }
    }

    private void FixedUpdate()
    {
        // 오브젝트 수집 방식
        if(type == ClearType.objectCollecting) {
            stageGoal.text = "Stage Goal : Collecting (" + collectCount + "/" + needCollectNum + ")";

            if (collectCount == needCollectNum) {
                stageCondition.text = "▶ 클리어";
                stageGoal.text = "포탈을 통해 이동하십시오.";
                stageClear = true;
            }            
        }

        if(type == ClearType.eliminateMonster) {
            stageCondition.text = "▶ 일반 구역";
            stageGoal.text = "";
            // stageCondition.text = "▶ 필드 클리어 조건";
            // stageGoal.text = "모든 적을 처치하십시오. (" + eliminateCount + "/" + needEliminateNum + ")";

            stageClear = true;

            /*
            if (eliminateCount == needEliminateNum) {
                stageCondition.text = "▶ 필드 클리어";
                stageGoal.text = "포탈을 통해 이동하십시오.";
                stageClear = true;
            }
            */
        }

        if(type == ClearType.searchLight) {
            stageCondition.text = "▶ 특수 구역";
            stageGoal.text = "서치라이트를 피해 이동하십시오.";
            stageClear = true;
        }
    }
}