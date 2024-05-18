using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
    플레이어 status 표시 UI 제어 스크립트
*/

public class StatusUIController : MonoBehaviour
{
    public GameObject character;
    public Image statusHP;    // HP 게이지
    public Image statusUmbrella;    // 우산 게이지
    public TMP_Text statusHPNum;
    public TMP_Text statusUmbrellaNum;

    // Component 선언
    PlayerController PC;

    private void Start()
    {
        PC = character.GetComponent<PlayerController>();
    }

    private void Update()
    {
        statusHP.fillAmount = PC.curHP / PC.maxHP;
        statusUmbrella.fillAmount = PC.curUmbrella / PC.maxUmbrella;

        statusHPNum.text = Mathf.RoundToInt(PC.curHP).ToString();
        statusUmbrellaNum.text = Mathf.RoundToInt(PC.curUmbrella).ToString();
    }
}
