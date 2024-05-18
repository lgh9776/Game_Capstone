using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
    �÷��̾� status ǥ�� UI ���� ��ũ��Ʈ
*/

public class StatusUIController : MonoBehaviour
{
    public GameObject character;
    public Image statusHP;    // HP ������
    public Image statusUmbrella;    // ��� ������
    public TMP_Text statusHPNum;
    public TMP_Text statusUmbrellaNum;

    // Component ����
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
