using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyStatusUIController : MonoBehaviour
{
    private enum StatusType { monster, boss };

    public GameObject enemyPanel;

    [HideInInspector] StatusType type;

    [HideInInspector] public GameObject character;
    public TMP_Text enemyName;
    public Image enemyUpshot;

    public GameObject monsterHP;
    public GameObject bossHP;
    public Image monsterHP_MAX;    // HP 게이지
    public Image bossHP_MAX;

    public TMP_Text monsterHPNum;
    public TMP_Text bossHPNum;

    public bool enemyHit = false;

    // Component 선언
    MonsterController MC;
    
    private void Update()
    {
        if (enemyHit) {
            if (type == StatusType.monster) {
                monsterHP_MAX.fillAmount = MC.curHP / MC.maxHP;
                monsterHPNum.text = Mathf.RoundToInt(MC.curHP).ToString();
            }
            else if (type == StatusType.boss) {
                bossHP_MAX.fillAmount = MC.curHP / MC.maxHP;
                bossHPNum.text = Mathf.RoundToInt(MC.curHP).ToString();
            }
        }
    }

    public void InitEnemyStatusPanel(GameObject enemy, bool isBoss)
    {
        enemyPanel.SetActive(true);

        if (isBoss) {
            type = StatusType.boss;
            bossHP.SetActive(true);
            monsterHP.SetActive(false);
        }

        else {
            type = StatusType.monster;
            bossHP.SetActive(false);
            monsterHP.SetActive(true);
        }

        character = enemy;
        string nameParsing;
        nameParsing = enemy.name.Split('(')[0];
        Debug.Log(nameParsing);
        MC = character.GetComponent<MonsterController>();
        enemyUpshot.sprite = Resources.Load<Sprite>("Upshot/" + nameParsing);
        enemyName.text = nameParsing;
        enemyHit = true;
    }
}
