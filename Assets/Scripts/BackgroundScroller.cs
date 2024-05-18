using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    �İ� ���� ��ũ�� ��� ��ũ��Ʈ (�İ� ���ٰ� �ο�)
*/

public class BackgroundScroller : MonoBehaviour
{
    // �İ� ���� ��ũ�� �ӵ�
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

    // �÷��̾� ���� ī�޶�
    public Camera playerCamera;

    // Scene �̵� �� ��ũ�ѷ� �ʱ�ȭ ����
    [HideInInspector] public bool initScroller = false;

    private float intervalX = 0;    // �÷��̾� ���� ī�޶��� �̵� �Ÿ�
    private float beforeX;    // intervalX ������ ���� �ӽ� ����

    private void FixedUpdate()
    {
        /* 
            �÷��̾ �̵� ���¿� ������ ������Ʈ�� ���θ��� ������ �̵����� �ʴ� ��츦
            �Ǵ��ϱ� ���� �÷��̾� ���� ī�޶��� �̵� �Ÿ��� ����Ͽ� �İ��� ��ũ��
        */
        layer0 = GameObject.Find("Layer0");
        layer1 = GameObject.Find("Layer1");
        layer2 = GameObject.Find("Layer2");
        layer3 = GameObject.Find("Layer3");
        layer4 = GameObject.Find("Layer4");
        layer5 = GameObject.Find("Layer5");

        // Scene �̵� �� �߾����� ��ġ �ʱ�ȭ
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
        // GameObject.Find("Background_Back").transform.Translate(intervalX * scrollSpeed, 0, 0);    // ���� ��ũ�� �ӵ��� ���� �İ� ��ũ��
        */
        
        beforeX = playerCamera.transform.position.x; 
    }
}
