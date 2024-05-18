using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
    �ʵ� Ŭ���� Ȯ�� �� ���� ��ũ��Ʈ
*/

public class ClearController : MonoBehaviour
{
    // private static ClearController instance = null;

    private enum ClearType { none, objectCollecting, eliminateMonster, searchLight };    // �ʵ� Ŭ���� ���

    [SerializeField] private ClearType type;
    [SerializeField] private int needCollectNum = 5;    // ������Ʈ ���� �ʿ䷮ (������Ʈ ���� ���)
    [SerializeField] private int needEliminateNum = 5;

    [HideInInspector] public int collectCount = 0;
    [HideInInspector] public int eliminateCount = 0;

    public bool stageClear = false;

    // �ʵ� Ŭ���� ���� ǥ�� UI Text
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
            stageCondition.text = "�� �ʵ� Ŭ���� ���� ����";
            stageGoal.text = "";
            stageClear = true;
        }
    }

    private void FixedUpdate()
    {
        // ������Ʈ ���� ���
        if(type == ClearType.objectCollecting) {
            stageGoal.text = "Stage Goal : Collecting (" + collectCount + "/" + needCollectNum + ")";

            if (collectCount == needCollectNum) {
                stageCondition.text = "�� Ŭ����";
                stageGoal.text = "��Ż�� ���� �̵��Ͻʽÿ�.";
                stageClear = true;
            }            
        }

        if(type == ClearType.eliminateMonster) {
            stageCondition.text = "�� �Ϲ� ����";
            stageGoal.text = "";
            // stageCondition.text = "�� �ʵ� Ŭ���� ����";
            // stageGoal.text = "��� ���� óġ�Ͻʽÿ�. (" + eliminateCount + "/" + needEliminateNum + ")";

            stageClear = true;

            /*
            if (eliminateCount == needEliminateNum) {
                stageCondition.text = "�� �ʵ� Ŭ����";
                stageGoal.text = "��Ż�� ���� �̵��Ͻʽÿ�.";
                stageClear = true;
            }
            */
        }

        if(type == ClearType.searchLight) {
            stageCondition.text = "�� Ư�� ����";
            stageGoal.text = "��ġ����Ʈ�� ���� �̵��Ͻʽÿ�.";
            stageClear = true;
        }
    }
}