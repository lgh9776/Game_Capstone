using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    후경 지연 스크롤 기능 스크립트 (후경 원근감 부여)
*/

public class BackgroundScroller : MonoBehaviour
{
    // 후경 지연 스크롤 속도
    [SerializeField] private float scrollSpeedLayer0 = 0.3f;
    [SerializeField] private float scrollSpeedLayer1 = 0.3f;
    [SerializeField] private float scrollSpeedLayer2 = 0.3f;
    [SerializeField] private float scrollSpeedLayer3 = 0.3f;
    [SerializeField] private float scrollSpeedLayer4 = 0.3f;
    [SerializeField] private float scrollSpeedLayer5 = 0.3f;

    public GameObject layer0;
    public GameObject layer1;
    public GameObject layer2;
    public GameObject layer3;
    public GameObject layer4;
    public GameObject layer5;

    // 플레이어 추적 카메라
    public Camera playerCamera;

    // Scene 이동 시 스크롤러 초기화 여부
    [HideInInspector] public bool initScroller = false;

    private float intervalX = 0;    // 플레이어 추적 카메라의 이동 거리
    private float beforeX;    // intervalX 연산을 위한 임시 변수

    private void FixedUpdate()
    {
        /* 
            플레이어가 이동 상태에 있지만 오브젝트에 가로막혀 실제로 이동하지 않는 경우를
            판단하기 위해 플레이어 추적 카메라의 이동 거리를 사용하여 후경을 스크롤
        */
        layer0 = GameObject.Find("Layer0");
        layer1 = GameObject.Find("Layer1");
        layer2 = GameObject.Find("Layer2");
        layer3 = GameObject.Find("Layer3");
        layer4 = GameObject.Find("Layer4");
        layer5 = GameObject.Find("Layer5");

        // Scene 이동 시 중앙으로 위치 초기화
        if (!initScroller) {
            beforeX = playerCamera.transform.position.x;
            initScroller = true;
        }
        intervalX = playerCamera.transform.position.x - beforeX;
        layer0.transform.position = new Vector3(playerCamera.transform.position.x * scrollSpeedLayer0, 0, 0);
        layer1.transform.position = new Vector3(playerCamera.transform.position.x * scrollSpeedLayer1, 0, 0);
        layer2.transform.position = new Vector3(playerCamera.transform.position.x * scrollSpeedLayer2, 0, 0);
        layer3.transform.position = new Vector3(playerCamera.transform.position.x * scrollSpeedLayer3, 0, 0);
        layer4.transform.position = new Vector3(playerCamera.transform.position.x * scrollSpeedLayer4, 0, 0);
        layer5.transform.position = new Vector3(playerCamera.transform.position.x * scrollSpeedLayer5, 0, 0);

        /*
        layer0.transform.Translate(intervalX * scrollSpeedLayer0, 0, 0);
        layer1.transform.Translate(intervalX * scrollSpeedLayer1, 0, 0);
        layer2.transform.Translate(intervalX * scrollSpeedLayer2, 0, 0);
        layer3.transform.Translate(intervalX * scrollSpeedLayer3, 0, 0);
        layer4.transform.Translate(intervalX * scrollSpeedLayer4, 0, 0);
        layer5.transform.Translate(intervalX * scrollSpeedLayer5, 0, 0);
        // GameObject.Find("Background_Back").transform.Translate(intervalX * scrollSpeed, 0, 0);    // 지연 스크롤 속도에 따라 후경 스크롤
        */
        
        beforeX = playerCamera.transform.position.x; 
    }
}
