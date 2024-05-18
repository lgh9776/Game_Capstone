using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    [SerializeField] private float destroyTime;

    void Awake()
    {
        Destroy(gameObject, destroyTime);
    }

    public void DestroyBossEffect()
    {
        Debug.Log("Destroy Effect");

        if (GameObject.Find("Boss")) {
            GameObject.Find("Boss").GetComponent<MonsterController>().actionDelay = false;
        }
            
        Destroy(gameObject);
    }
}
