using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchLightContactController : MonoBehaviour
{
    private bool eventOnce = false;

    private GameObject player;
    private GameObject contactEventMessage;

    PlayerController _PlayerController;
    SceneData _SceneData;
    SearchLightEventController _SearchLightEventController;

    private void Awake()
    {
        player = GameObject.Find("Player");
        _PlayerController = player.GetComponent<PlayerController>();
        _SceneData = GameObject.Find("SceneData").GetComponent<SceneData>();
        _SearchLightEventController = FindObjectOfType<SearchLightEventController>();

        contactEventMessage = Resources.Load<GameObject>("SearchLightEventMessage");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            Debug.Log(eventOnce);

            if (!_SearchLightEventController.eventOnce) {
                _SearchLightEventController.eventOnce = true;
                Instantiate(contactEventMessage, player.transform.position + new Vector3(player.transform.localScale.x * 2, 0, 0), Quaternion.identity);
                _PlayerController.autoTalking = true;
            }
        }
    }

    public void ContactEvent()
    {
        if (_PlayerController.sceneInitPos == 0) {
            player.transform.position = _SceneData.initPos[0].position;
        }
        else if (_PlayerController.sceneInitPos == 2) {
            player.transform.position = _SceneData.initPos[2].position;
        }

        _SearchLightEventController.eventOnce = false;
    }
}
