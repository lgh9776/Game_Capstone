using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ī�޶� ���� ��ũ��Ʈ
*/

public class CameraController : MonoBehaviour
{
    // Singleton ���� �ν��Ͻ�
    private static CameraController instance = null;

    public Transform targetPlayer;    // ���� ��� �÷��̾�
    public float limitX;    // ī�޶� �¿� �ִ� �̵� ����

    // ī�޶� �ʱ� ��ġ offset
    [SerializeField] private float offsetY = 0f;
    [SerializeField] private float offsetZ = 0f;

    private void Awake()
    {
        // Singleton ���� ����
        if (null == instance) {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else {
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(targetPlayer.position.x, offsetY, offsetZ);

        targetPos.x = Mathf.Clamp(targetPos.x, limitX * -1, limitX);    // x ���� �¿� �ִ� �̵� ������ ����
        transform.position = targetPos;    // �÷��̾� ����
    }
}