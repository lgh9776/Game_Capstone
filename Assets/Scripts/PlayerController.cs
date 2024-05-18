using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    �÷��̾� ���� ��ũ��Ʈ
*/

public class PlayerController : BaseController
{
    // Singleton ���� �ν��Ͻ�
    private static PlayerController instance = null;

    // �÷��̾� �̵� ����
    private Vector3 playerMovingVector;
    private int horizontal;
    private int vertical;

    public bool autoTalking = false;

    private bool uStance = false;    // ��� ���Ľ� ����
    private bool onHit = false;    // �ǰ� ����

    private bool isSeriesAttack = false;    // ���� ���� ����
    private bool onAttack = false;    // ���� ���� ����
    private int seriesAttackNum = 0;    // ���� ���� Ÿ��

    public bool onDefense = false;

    // ��ȭâ ������ ���� �ʵ�
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

    // Player �̵� ���� property
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

        // Rayacst ���� ǥ�� (Ȯ�ο�)
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

        // �÷��̾� ���Ľ� ��ȯ
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
            
        // ��� ��� ���Ľ��� �� �꼺�� ������ ����
        if (uStance)
            if(curHP > maxHP * 0.01)
                curHP -= curHP * 0.002f * Time.deltaTime;

        /* Input.GetKey(KeySet.keys[KeyAction.UP]
         * ���� ���� �� Input.GetKey(~)�� ��� ��ó�� KeyManager�� ��ųʸ�,enum Ȱ��
		*/
        /* ����Ű Log �׽�Ʈ
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


        // Object Scan �� �ش� Obj�� ���� ��ȭâ ����
        if ((autoTalking || Input.GetKeyDown(KeySet.keys[KeyAction.Interaction])) && scanObj != null) {
            Debug.Log("Auto Talking ON");
            if (!talkController.isTalking)
                talkController.InitTalkPanel(scanObj);
            else
                talkController.Talk();

            autoTalking = false;
        }

        // �÷��̾� ����
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
        // Player �̵� - ��ȭ �� �̵��Ұ� (isAction �Ǵ�)
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

        // �÷��̾� �̵� �Է��� ���ų� ���� ������ ��� �̵� �Ұ�
        if (!(Input.GetKey(KeySet.keys[KeyAction.UP]) || Input.GetKey(KeySet.keys[KeyAction.LEFT]) || Input.GetKey(KeySet.keys[KeyAction.DOWN]) || Input.GetKey(KeySet.keys[KeyAction.RIGHT])) || onAttack || onDefense) {
            playerMovingVector = Vector3.zero;
            rb.velocity = playerMovingVector;
            horizontal = 0;
            vertical = 0;
        }

        // �Է� ������ �÷��̾� �̵� ���� �� ����
        playerMovingVector = new Vector3(horizontal, 0, vertical);

        // �÷��̾� �̵�
        if (horizontal != 0 || vertical != 0) {
            rb.velocity = playerMovingVector * speed;

            // �÷��̾� �̵� ���⿡ ���� ���� ��ȯ
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

    // ĳ���� �ǰ� ���� �ð� Coroutine
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