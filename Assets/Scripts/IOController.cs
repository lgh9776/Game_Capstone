using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

// ��ü ��� resource
[System.Serializable]
public class DialogueResources
{
    public List<DialogueData> dialogueData;
}

// Ư�� ������Ʈ ��� data (�� ������Ʈ�� ���� ��� ��� ���θ� �ǹ�)
[System.Serializable]
public class DialogueData
{
    public int objID;
    public string objName;
    public List<DialogueList> dialogueList;
}

// Ư�� ������Ʈ�� Ư�� ��� ��� (�� ������Ʈ�� ���� ��� ��� �� �� ��� ����� �ǹ�)
[System.Serializable]
public class DialogueList
{
    public int dialogueID;
    public string[] dialogue;
}

[System.Serializable]
public class DialogueIDByQuest
{
    public List<DialogueIDChanges> dialogueChanges;
}

[System.Serializable]
public class DialogueIDChanges
{
    public int objID;
    public string objName;
    public List<QuestList> questList;
}

[System.Serializable]
public class QuestList
{
    public int questNum;
    public List<DialogueIDByActivated> dialogueIDByActivated;
}

[System.Serializable]
public class DialogueIDByActivated
{
    public int questActivated;
    public int dialogueID;
}

public class IOController : MonoBehaviour
{
    // Singleton ���� �ν��Ͻ�
    private static IOController instance = null;

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

    // �ܺ� ��� ������ Load
    public string[] LoadDialogue(int objID, int dialogueID)
    {
        TextAsset textAsset = Resources.Load<TextAsset>("dialogue");
        DialogueResources dialogueResources = JsonUtility.FromJson<DialogueResources>(textAsset.text);

        return dialogueResources.dialogueData[objID].dialogueList[dialogueID].dialogue;
    }

    public int LoadDialogueIDByQuest(int objID, int questNum, int questActivated)
    {
        TextAsset textAsset = Resources.Load<TextAsset>("DialogueIDByQuest");
        DialogueIDByQuest dialogueIDByQuest = JsonUtility.FromJson<DialogueIDByQuest>(textAsset.text);

        if(dialogueIDByQuest.dialogueChanges.Count > objID) {
            if(dialogueIDByQuest.dialogueChanges[objID].questList.Count > questNum - 1) {
                if(dialogueIDByQuest.dialogueChanges[objID].questList[questNum-1].dialogueIDByActivated.Count > questActivated) {
                    return dialogueIDByQuest.dialogueChanges[objID].questList[questNum - 1].dialogueIDByActivated[questActivated].dialogueID;
                }
            }
        }

        return -1;
    }
}