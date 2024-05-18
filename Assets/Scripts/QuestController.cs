using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestController : MonoBehaviour
{
    // Singleton ���� �ν��Ͻ�
    private static QuestController instance = null;

    public TMP_Text questField;
    public TMP_Text questState;

    public GameObject questWindow;
    public bool changeQuestWindow = true;

    /* Quest Activated : 0[���� ��] / 1[�Ϸ� ��] */
    public int questNum = 0;
    public int questActivated = 0;

    [HideInInspector] public bool episode1Start = true;

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
    }

    private void Update()
    {
        switch (questNum) {
            case 1:
                if (questActivated == 0) {
                    questField.text = "�� ���ĵ� ����ó";
                    questState.text = "����ó���� ������ �㰡���� �������ʽÿ�.";
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
                    questField.text = "�� �Ա��ɻ���";
                    questState.text = "�㰡���� ȣ��Ʈ���� ������ �ֽʽÿ�.";
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
                    questField.text = "�� �Գ����� ����";
                    questState.text = "������� ��� ���� ��� óġ�ϰ� ���ĵ� ����� �շ��Ͻʽÿ�.";
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
                    questField.text = "�� �Գ����� �ϼ� ó����";
                    questState.text = "�Ŵ�ȭ�� ������ 'ŷ����ũ'�� óġ�Ͻʽÿ�.";
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
