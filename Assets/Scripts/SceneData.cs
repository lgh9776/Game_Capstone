using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData : MonoBehaviour
{
    [SerializeField] private float cameraLimit;
    public List<Transform> initPos;

    private GameObject player;

    CameraController _CameraController;
    PlayerController _PlayerController;
    BackgroundScroller _BackgroundScroller;

    private void Awake()
    {
        player = GameObject.Find("Player");
        _PlayerController = player.GetComponent<PlayerController>();
        _CameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
        _BackgroundScroller = GameObject.Find("Main Camera").GetComponent<BackgroundScroller>();
        _CameraController.limitX = cameraLimit;

        _BackgroundScroller.initScroller = false;

        switch (_PlayerController.sceneInitPos) {
            case 0:
                player.transform.position = initPos[0].position;
                break;
            case 1:
                player.transform.position = initPos[1].position;
                break;
            case 2:
                player.transform.position = initPos[2].position;
                break;
            case 3:
                player.transform.position = initPos[3].position;
                break;
        }
    }
}
