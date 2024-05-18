using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestController : MonoBehaviour
{
    // Singleton 패턴 인스턴스
    private static QuestController instance = null;

    public TMP_Text questField;
    public TMP_Text questState;

    public GameObject questWindow;
    public bool changeQuestWindow = true;

    /* Quest Activated : 0[진행 중] / 1[완료 후] */
    public int questNum = 0;
    public int questActivated = 0;

    [HideInInspector] public bool episode1Start = true;

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
    }

    private void Update()
    {
        switch (questNum) {
            case 1:
                if (questActivated == 0) {
                    questField.text = "▶ 스탠드 은신처";
                    questState.text = "은신처에서 여분의 허가증을 가져오십시오.";
                    if (changeQuestWindow) {
                        questWindow.GetComponent<OptionPanelManager>().WindowIn();
                        changeQuestWindow = false;
                    }

                }
                else {
                    questField.text = "";
                    questState.text = "";
                    if (!changeQuestWindow) {
                        questWindow.GetComponent<OptionPanelManager>().WindowOut();
                        changeQuestWindow = true;
                    }
                }
                break;
            case 2:
                if (questActivated == 0) {
                    questField.text = "▶ 입국심사장";
                    questState.text = "허가증을 호이트에게 가져다 주십시오.";
                    if (changeQuestWindow) {
                        questWindow.GetComponent<OptionPanelManager>().WindowIn();
                        changeQuestWindow = false;
                    }
                }
                else {
                    questField.text = "";
                    questState.text = "";
                    if (!changeQuestWindow) {
                        questWindow.GetComponent<OptionPanelManager>().WindowOut();
                        changeQuestWindow = true;
                    }
                }
                break;
            case 3:
                if (questActivated == 0) {
                    questField.text = "▶ 게네하토 공터";
                    questState.text = "몰려드는 모든 적을 모두 처치하고 스탠드 일행과 합류하십시오.";
                    if (changeQuestWindow) {
                        questWindow.GetComponent<OptionPanelManager>().WindowIn();
                        changeQuestWindow = false;
                    }
                }
                else {
                    questField.text = "";
                    questState.text = "";
                    if (!changeQuestWindow) {
                        questWindow.GetComponent<OptionPanelManager>().WindowOut();
                        changeQuestWindow = true;
                    }
                }
                break;
            case 4:
                if(questActivated == 0) {
                    questField.text = "▶ 게네하토 하수 처리장";
                    questState.text = "거대화된 슬라임 '킹베르크'를 처치하십시오.";
                    if (changeQuestWindow) {
                        questWindow.GetComponent<OptionPanelManager>().WindowIn();
                        changeQuestWindow = false;
                    }
                }
                else {
                    questField.text = "";
                    questState.text = "";
                    if (!changeQuestWindow) {
                        questWindow.GetComponent<OptionPanelManager>().WindowOut();
                        changeQuestWindow = true;
                    }
                }
                break;
            default:
                questState.text = "";
                break;
        }
    }
}
