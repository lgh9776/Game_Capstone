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
        textData.Add(100, new string[] {"1. cube ��ȭ�մϴ�.", "2. cube ��ȭ�մϴ�.", "������ �����Դϴ�."});
        textData.Add(200, new string[] {"1. circle ��ȭ�մϴ�.", "2. circle ��ȭ�մϴ�.", "������ �����Դϴ�."});

    }

    string[] GetDialouge(int objID, int dialogueID)
    {
        return null;
    }

    public string GetText(int id, int textIdx)
    {
        if (textIdx == textData[id].Length) //text Index�� ���� �� ��
            return null; //���� ���� ���� ���
        else
            return textData[id][textIdx];
    }
}
