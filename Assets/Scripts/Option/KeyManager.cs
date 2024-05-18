using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 0 1 2 3 4 5 6 7(KEYCOUNT = 키 사용 개수)
//조작키 추가 시 default, 버튼UI 생성해야함
public enum KeyAction { UP, DOWN, LEFT, RIGHT, Interaction, Stance, Attack, Defense, KEYCOUNT };

public static class KeySet {
    public static Dictionary<KeyAction, KeyCode> keys = new Dictionary<KeyAction, KeyCode>();
}

public class KeyManager : MonoBehaviour
{
    private static KeyManager instance = null;

    //초기 설정
    KeyCode[] defaultKey = new KeyCode[] { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.Space, KeyCode.Q, KeyCode.Z, KeyCode.X };

    public void Awake()
    {
        if (null == instance) {
            instance = this;

            for (int i = 0; i < (int)KeyAction.KEYCOUNT; i++)
                KeySet.keys.Add((KeyAction)i, defaultKey[i]);

            DontDestroyOnLoad(this.gameObject);
        }
        else {
            Destroy(this.gameObject);
        }

        /*
        for (int i = 0; i < (int)KeyAction.KEYCOUNT; i++)
            KeySet.keys.Add((KeyAction)i, defaultKey[i]);
        */
    }

    private void OnGUI()
    {
        Event keyEvent = Event.current;
        if (keyEvent.isKey)
        {
            KeySet.keys[(KeyAction)key] = keyEvent.keyCode;
            key = -1;
        }
    }

    int key = -1;
    public void ChangeKey(int n)
    {
        key = n;
    }

}
