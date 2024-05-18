using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ĳ���� �� ���� ���� ��ũ��Ʈ
*/

public class BaseController : MonoBehaviour
{
    [SerializeField] protected float speed = 1.0f;    // �̵� �ӵ�
    
    public bool facingRight = true;    // �ٶ󺸴� ����
    public float maxHP = 100.0f;    // �ִ� ü��
    public float curHP;    // ���� ü��
    public float damage = 5.0f;    // ���� ������

    // Component ����
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public SpriteRenderer sr;
    [HideInInspector] public Animator anim;

    virtual protected void Awake()
    {
        // Component �ʱ�ȭ
        rb = GetComponent<Rigidbody>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    virtual protected void Start()
    {
        // ü�� �ʱ�ȭ
        curHP = maxHP;
    }

    // ĳ���� ���� ��ȯ
    protected void SetFlipX()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }
}