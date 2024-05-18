using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEffectController : MonoBehaviour
{
    private enum effectType { tornado, cloud, icicle, fog, danger };

    private GameObject player;

    private bool isDestroy = false;

    [SerializeField] effectType type;
    [SerializeField] float effectTimer;
    [SerializeField] private float cloudSpeed = 1.0f;

    private void Start()
    {
        player = GameObject.Find("Player");
        StartCoroutine(EffectTimer());

        if (transform.position.z > 6.0f || transform.position.z < -12.0f) {
            Destroy(gameObject);
        }

        if (type == effectType.icicle) {
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, -1, 0) * 1.0f, ForceMode.Force);
        }
    }

    private void Update()
    {
        if (type == effectType.cloud)
        {
            transform.Translate((player.transform.position + new Vector3(0, 2, 0) - transform.position).normalized * cloudSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (type == effectType.cloud && other.gameObject.CompareTag("Player") && !isDestroy)
        {
            isDestroy = true;
            transform.Find("Boss_ACTION_lightning").gameObject.SetActive(true);
            Destroy(gameObject, 0.5f);
        }

        if(type == effectType.icicle) {
            if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Platform")) {
                Debug.Log("Icicle Hit!");
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator EffectTimer()
    {
        float timer = effectTimer;

        while(timer > 0) {
            timer -= Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
