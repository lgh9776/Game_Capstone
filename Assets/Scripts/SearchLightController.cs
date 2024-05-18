using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchLightController : MonoBehaviour
{
    public Transform center;

    [SerializeField] private float moveRadius;
    [SerializeField] private float moveSpeed;

    private Vector3 randomPos;

    private void Start()
    {
        /*
        transform.position = center.position;
        randomPos = Random.insideUnitSphere * moveRadius;
        randomPos += center.position;
        randomPos = new Vector3(randomPos.x, 1.15f, randomPos.z);
        */
    }

    private void Update()
    {
        /*
        if(Vector3.Distance(randomPos, transform.position) <= 2.0f) {
            randomPos = Random.insideUnitSphere * moveRadius;
            randomPos += center.position;
            randomPos = new Vector3(randomPos.x, 1.15f, randomPos.z);
        }

        transform.position = Vector3.MoveTowards(transform.position, randomPos, moveSpeed * Time.deltaTime);
        */
    }
}
