using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    캐릭터 및 몬스터 공통 스크립트
*/

public class BaseController : MonoBehaviour
{
    [SerializeField] protected float speed = 1.0f;    // 이동 속도
    
    public bool facingRight = true;    // 바라보는 방향
    public float maxHP = 100.0f;    // 최대 체력
    public float curHP;    // 현재 체력
    public float damage = 5.0f;    // 공격 데미지

    // Component 선언
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public SpriteRenderer sr;
    [HideInInspector] public Animator anim;

    virtual protected void Awake()
    {
        // Component 초기화
        rb = GetComponent<Rigidbody>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    virtual protected void Start()
    {
        // 체력 초기화
        curHP = maxHP;
    }

    // 캐릭터 방향 전환
    protected void SetFlipX()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }
}