using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    Dictionary<int, string[]> textData;

    void Awake()
    {
        textData = new Dictionary<int, string[]>();
        CreateData();
    }

    void CreateData()
    {
        textData.Add(100, new string[] {"1. cube 대화합니다.", "2. cube 대화합니다.", "마지막 문장입니다."});
        textData.Add(200, new string[] {"1. circle 대화합니다.", "2. circle 대화합니다.", "마지막 문장입니다."});

    }

    string[] GetDialouge(int objID, int dialogueID)
    {
        return null;
    }

    public string GetText(int id, int textIdx)
    {
        if (textIdx == textData[id].Length) //text Index와 문장 수 비교
            return null; //남은 문장 없을 경우
        else
            return textData[id][textIdx];
    }
}
