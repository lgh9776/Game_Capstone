using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    카메라 제어 스크립트
*/

public class CameraController : MonoBehaviour
{
    // Singleton 패턴 인스턴스
    private static CameraController instance = null;

    public Transform targetPlayer;    // 추적 대상 플레이어
    public float limitX;    // 카메라 좌우 최대 이동 범위

    // 카메라 초기 위치 offset
    [SerializeField] private float offsetY = 0f;
    [SerializeField] private float offsetZ = 0f;

    private void Awake()
    {
        // Singleton 패턴 구현
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

        targetPos.x = Mathf.Clamp(targetPos.x, limitX * -1, limitX);    // x 값을 좌우 최대 이동 범위로 제한
        transform.position = targetPos;    // 플레이어 추적
    }
}