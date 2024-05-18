using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueChanger : MonoBehaviour
{
    QuestController _QuestController;
    IOController _IOController;
    TalkObjData _TalkObjData;

    private void Awake()
    {
        _QuestController = GameObject.Find("QuestController").GetComponent<QuestController>();
        _IOController = GameObject.Find("IOController").GetComponent<IOController>();
        _TalkObjData = gameObject.GetComponent<TalkObjData>();

    }


    private void Update()
    {
        int changedObjID = ObjIDChange(_TalkObjData.objID);

        if (changedObjID != -1 && _QuestController.questNum != 0) {
            int dID = _IOController.LoadDialogueIDByQuest(changedObjID, _QuestController.questNum, _QuestController.questActivated);
            if (dID != -1)
                _TalkObjData.dialogueID = dID;
        }
    }

    private int ObjIDChange(int objID)
    {
        switch (objID) {
            // ���̵�
            case 4:
                return 0;
            // �Ŵ���M
            case 5:
                return 1;
            // �̳�
            case 6:
                return 2;
            // �̳��� å��
            case 7:
                return 3;
            // ȣ��Ʈ
            case 8:
                return 4;            
            default:
                return -1;
        }
    }
}
