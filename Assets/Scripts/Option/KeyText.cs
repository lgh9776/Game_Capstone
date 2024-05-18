using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyText : MonoBehaviour
{
    // public List<TextMeshPro> keyText = new List<TextMeshPro>();
    public Text[] keyText;

    void Start()
    {
        for(int i = 0; i < keyText.Length; i++)
        {
            keyText[i].text = KeySet.keys[(KeyAction)i].ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < keyText.Length; i++)
        {
            keyText[i].text = KeySet.keys[(KeyAction)i].ToString();
        }
    }
}
