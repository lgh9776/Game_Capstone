using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWaveController : MonoBehaviour
{
    public int totalMonsterCount;
    public float waveDelayTime;
    public Transform spawnPos;

    private int monsterCount = 0;
    private bool onWaveDelay = false;

    private GameObject monster;

    ClearController _ClearController;
    QuestController _QuestController;

    private void Awake()
    {
        monster = Resources.Load<GameObject>("Prefab/ΩΩ∂Û¿”");
        _ClearController = FindObjectOfType<ClearController>();
        _QuestController = FindObjectOfType<QuestController>();
    }

    private void Update()
    {
        if(monsterCount != totalMonsterCount && !onWaveDelay) {
            onWaveDelay = true;
            Instantiate(monster, new Vector3(spawnPos.position.x, 1.8f, spawnPos.position.z), Quaternion.identity);
            monsterCount++;
            StartCoroutine(WaveDelayTime());
        }
    }

    IEnumerator WaveDelayTime()
    {
        int countTime = 0;

        while (countTime < waveDelayTime) {
            yield return new WaitForSecondsRealtime(1.0f);
            countTime++;
        }

        onWaveDelay = false;

        yield return null;
    }
}
