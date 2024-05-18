using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

// 전체 대사 resource
[System.Serializable]
public class DialogueResources
{
    public List<DialogueData> dialogueData;
}

// 특정 오브젝트 대사 data (한 오브젝트의 여러 대사 목록 전부를 의미)
[System.Serializable]
public class DialogueData
{
    public int objID;
    public string objName;
    public List<DialogueList> dialogueList;
}

// 특정 오브젝트의 특정 대사 목록 (한 오브젝트에 여러 대사 목록 중 한 대사 목록을 의미)
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
    // Singleton 패턴 인스턴스
    private static IOController instance = null;

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

    // 외부 대사 데이터 Load
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