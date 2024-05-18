using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    플레이어 제어 스크립트
*/

public class PlayerController : BaseController
{
    // Singleton 패턴 인스턴스
    private static PlayerController instance = null;

    // 플레이어 이동 벡터
    private Vector3 playerMovingVector;
    private int horizontal;
    private int vertical;

    public bool autoTalking = false;

    private bool uStance = false;    // 우산 스탠스 상태
    private bool onHit = false;    // 피격 상태

    private bool isSeriesAttack = false;    // 연속 공격 상태
    private bool onAttack = false;    // 현재 공격 상태
    private int seriesAttackNum = 0;    // 연속 공격 타수

    public bool onDefense = false;

    // 대화창 구현을 위한 필드
    public TalkController talkController;
    public GameObject scanObj;
    Vector3 dirVec = new Vector3(1, 0, 0);

    public float maxUmbrella = 100.0f;
    public float curUmbrella = 0.0f;
    private bool recoveryUmbrella = true;

    public bool cantMove = false;

    private RaycastHit hit;

    OptionController _OptionController;
    GameOverManager _GameOverManager;

    [HideInInspector] private GameObject attackRange1;
    [HideInInspector] private GameObject attackRange2;
    [HideInInspector] private GameObject attackRange3;
    [HideInInspector] public int sceneInitPos = 0;
    [SerializeField] private Transform attackPos;

    // Player 이동 벡터 property
    public Vector3 pMovingVector
    {
        get { return playerMovingVector; }
    }

    protected override void Awake()
    {
        base.Awake();

        if (null == instance) {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else {
            Destroy(this.gameObject);
        }

        _OptionController = FindObjectOfType<OptionController>();
        _GameOverManager = FindObjectOfType<GameOverManager>();
    }

    protected override void Start()
    {
        base.Start();
        attackRange1 = Resources.Load<GameObject>("AttackRange/AttackRange_1");
        attackRange2 = Resources.Load<GameObject>("AttackRange/AttackRange_2");
        attackRange3 = Resources.Load<GameObject>("AttackRange/AttackRange_3");

        if(attackRange1.transform.localScale.x < 0)
        {
            Debug.Log("negative!");
        }

        curUmbrella = maxUmbrella;
    }

    void Update()
    {
        if(curHP <= 0) {
            if(!_GameOverManager.isGameOver)
                _GameOverManager.GameOver();
            else {
                Time.timeScale = 0f;
            }
        }

        // Rayacst 진행 표시 (확인용)
        Debug.DrawRay(transform.position, dirVec * 2.0f, Color.red);

        /* Save scan object using Ray */
        if (Physics.Raycast(transform.position, dirVec, out hit, 2.0f) && hit.collider.gameObject.layer == 9) // 2.0f = ray distance
        {
            scanObj = hit.collider.gameObject;
        }
        else {
            scanObj = null;
        }

        if (_OptionController.onOption || cantMove) return;

        // 플레이어 스탠스 전환
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (uStance)
            {
                anim.SetBool("isStance", false);
                anim.SetBool("isAttack", false);
            }
            else
            {
                anim.SetBool("isStance", true);
            }
            uStance = !uStance;
        }
            
        // 우산 사용 스탠스일 시 산성비 데미지 적용
        if (uStance)
            if(curHP > maxHP * 0.01)
                curHP -= curHP * 0.002f * Time.deltaTime;

        /* Input.GetKey(KeySet.keys[KeyAction.UP]
         * 이후 개발 시 Input.GetKey(~)할 경우 위처럼 KeyManager의 딕셔너리,enum 활용
		*/
        /* 조작키 Log 테스트
        if (Input.GetKey(KeySet.keys[KeyAction.UP]))
            Debug.Log("UP");
        else if (Input.GetKey(KeySet.keys[KeyAction.DOWN]))
            Debug.Log("DOWN");
        else if (Input.GetKey(KeySet.keys[KeyAction.LEFT]))
            Debug.Log("LEFT");
        else if (Input.GetKey(KeySet.keys[KeyAction.RIGHT]))
            Debug.Log("RIGHT");
        else if (Input.GetKey(KeySet.keys[KeyAction.SPACE]))
            Debug.Log("SPACE");
        else if (Input.GetKey(KeySet.keys[KeyAction.RUN]))
            Debug.Log("RUN");
        else if (Input.GetKey(KeySet.keys[KeyAction.MODE]))
            Debug.Log("MODE"); */


        // Object Scan 후 해당 Obj에 대한 대화창 생성
        if ((autoTalking || Input.GetKeyDown(KeySet.keys[KeyAction.Interaction])) && scanObj != null) {
            Debug.Log("Auto Talking ON");
            if (!talkController.isTalking)
                talkController.InitTalkPanel(scanObj);
            else
                talkController.Talk();

            autoTalking = false;
        }

        // 플레이어 공격
        if (Input.GetKeyDown(KeySet.keys[KeyAction.Attack]) && uStance)
        {
            if (!onAttack)
            {
                seriesAttackNum = 0;
                rb.AddForce(new Vector3(10 * transform.localScale.x, 0, 0), ForceMode.Impulse);
                Instantiate(attackRange1, attackPos);
                anim.SetBool("isAttack", true);
                // generateAttackRange = true;
                onAttack = true;
            }
            else
            {
                isSeriesAttack = true;
            }
        }

        if (Input.GetKey(KeySet.keys[KeyAction.Defense]) && uStance)
        {
            if (curUmbrella < 0) {
                curUmbrella = 0;
            }

            if (curUmbrella >= 30) {
                curUmbrella -= maxUmbrella * 0.0005f;
                anim.SetBool("isDefense", true);
                onDefense = true;
                recoveryUmbrella = false;
            }
        }
        if (Input.GetKeyUp(KeySet.keys[KeyAction.Defense]) || curUmbrella < 30)
        {
            anim.SetBool("isDefense", false);
            anim.SetBool("isAttack", false);
            onDefense = false;
            onAttack = false;
            StartCoroutine(RecoveryUmbrella());
        }
        if(!onDefense && recoveryUmbrella && curUmbrella <= maxUmbrella) {
            if (uStance) {
                Debug.Log("Recovery");
                curUmbrella += maxUmbrella * 0.0005f;
            }
            else {
                curUmbrella += maxUmbrella * 0.0016f;
            }

            if(curUmbrella > maxUmbrella) {
                curUmbrella = maxUmbrella;
            }
        }
    }

    private void FixedUpdate()
    {
        if (_OptionController.onOption || cantMove) return;

        /*
        // Player 이동 - 대화 시 이동불가 (isAction 판단)
        float horizontal = talkController.isTalking ? 0 : Input.GetAxis("Horizontal");
        float vertical = talkController.isTalking ? 0 : Input.GetAxis("Vertical");
        */

        horizontal = 0;
        vertical = 0;

        if (Input.GetKey(KeySet.keys[KeyAction.UP])) {
            vertical = 1;
        }
        if (Input.GetKey(KeySet.keys[KeyAction.DOWN])) {
            vertical = -1;
        }
        if(Input.GetKey(KeySet.keys[KeyAction.UP]) && Input.GetKey(KeySet.keys[KeyAction.DOWN])) {
            vertical = 0;
        }

        if (Input.GetKey(KeySet.keys[KeyAction.LEFT])) {
            horizontal = -1;
        }
        if (Input.GetKey(KeySet.keys[KeyAction.RIGHT])) {
            horizontal = 1;
        }
        if (Input.GetKey(KeySet.keys[KeyAction.LEFT]) && Input.GetKey(KeySet.keys[KeyAction.RIGHT])) {
            horizontal = 0;
        }

        if (talkController.isTalking) {
            horizontal = 0;
            vertical = 0;
        }

        // 플레이어 이동 입력이 없거나 공격 상태인 경우 이동 불가
        if (!(Input.GetKey(KeySet.keys[KeyAction.UP]) || Input.GetKey(KeySet.keys[KeyAction.LEFT]) || Input.GetKey(KeySet.keys[KeyAction.DOWN]) || Input.GetKey(KeySet.keys[KeyAction.RIGHT])) || onAttack || onDefense) {
            playerMovingVector = Vector3.zero;
            rb.velocity = playerMovingVector;
            horizontal = 0;
            vertical = 0;
        }

        // 입력 값으로 플레이어 이동 벡터 값 연산
        playerMovingVector = new Vector3(horizontal, 0, vertical);

        // 플레이어 이동
        if (horizontal != 0 || vertical != 0) {
            rb.velocity = playerMovingVector * speed;

            // 플레이어 이동 방향에 따라 방향 전환
            if (horizontal < 0 && facingRight) {
                SetFlipX();
				dirVec = Vector3.left;
            }
            else if (horizontal > 0 && !facingRight) {
                SetFlipX();
				dirVec = Vector3.right;
            }
            anim.SetBool("isRun", true);
        }

        else {
            anim.SetBool("isRun", false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeySet.keys[KeyAction.Interaction])) {
            if (other.gameObject.CompareTag("CollectingObject")) {
                GameObject.FindObjectOfType<ClearController>().collectCount++;
                Destroy(other.gameObject);
            }
        }

        if (other.gameObject.CompareTag("Monster") && !onHit && curHP > 0) {
            curHP -= other.gameObject.GetComponent<MonsterController>().damage;

            /*
            Vector3 colDir = transform.position - other.transform.position;
            colDir.Normalize();
            rb.AddForce(colDir * bounceForce, ForceMode.Impulse);
            */

            if (curHP > 0) {
                onHit = true;
                StartCoroutine("InvincibleTime");
            }
        }

        if(other.gameObject.CompareTag("BossEffect") && !onHit && curHP > 0) {
            curHP -= 10.0f;

            if(curHP > 0) {
                onHit = true;
                StartCoroutine("InvincibleTime");
            }
        }
    }

    public void EndOfAttackTMP()
    {
        seriesAttackNum++;

        if(seriesAttackNum == 3)
        {
            seriesAttackNum = 0;
        }

        rb.AddForce(new Vector3(10 * transform.localScale.x, 0, 0), ForceMode.Impulse);

        if (isSeriesAttack)
        {
            switch (seriesAttackNum)
            {
                case 0:
                    Instantiate(attackRange1, attackPos);
                    break;
                case 1:
                    Instantiate(attackRange2, attackPos);
                    break;
                case 2:
                    Instantiate(attackRange3, attackPos);
                    break;
            }

            isSeriesAttack = false;
        }

        else {
            anim.SetBool("isAttack", false);
            onAttack = false;
        }
    }

    // 캐릭터 피격 무적 시간 Coroutine
    IEnumerator InvincibleTime()
    {
        int countTime = 0;

        while(countTime < 10) {
            if (countTime % 2 == 0) {
                sr.color = new Color(255, 255, 255, 0.75f);
            }
            else {
                sr.color = new Color(255, 255, 255, 1.0f);
            }

            yield return new WaitForSecondsRealtime(0.25f);
            countTime++;
        }
        sr.color = new Color(255, 255, 255, 1.0f);
        onHit = false;

        yield return null;
    }

    IEnumerator RecoveryUmbrella()
    {
        int t = 0;

        while(t < 3) {
            yield return new WaitForSecondsRealtime(1.0f);
            t++;
        }
        recoveryUmbrella = true;
    }
}