using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

/*
    몬스터 AI 제어 스크립트
*/

public class MonsterController : BaseController
{
    private enum MonsterType { slime, kingBerk, boss };    // 몬스터 유형

    [SerializeField] private MonsterType mType;
    [SerializeField] private bool isBoss;

    [SerializeField] private float patrolRadius = 5;
    [SerializeField] private float pursueRange = 0.5f;    // 몬스터 추적 범위
    // [SerializeField] private float attackRange = 0.1f;    // 몬스터 공격 범위

    private GameObject tornado;
    private GameObject cloud;
    private GameObject fog;
    private GameObject danger;
    private GameObject icicle;

    // public GameObject player;
    private GameObject player;

    private bool onHit = false;
    private bool isDead = false;
    private int probCount = 1;
    private int readyCount = 0;
    private int requireCount = -1;
    private bool attackDelay = true;
    public bool actionDelay = false;
    private bool stopChase = true;
    private bool movingRight = false;    // 몬스터가 바라보는 방향
    Vector3 nextPos;

    private bool monsterAttackDelay = false;
    private Vector3 posBeforeAttack;

    NavMeshAgent nav;
    EnemyStatusUIController _EnemyStatusUIController;
    ClearController _ClearController;
    PlayerController PC;
    GameOverManager _GameOverManager;

    protected override void Start()
    {
        curHP = maxHP;
        _EnemyStatusUIController = FindObjectOfType<EnemyStatusUIController>();
        _ClearController = FindObjectOfType<ClearController>();
        _GameOverManager = FindObjectOfType<GameOverManager>();
        player = GameObject.Find("Player");
        PC = player.GetComponent<PlayerController>();
        nav = GetComponent<NavMeshAgent>();
        nextPos = transform.position;

        tornado = Resources.Load<GameObject>("Effect/Boss_ACTION_tornado");
        cloud = Resources.Load<GameObject>("Effect/Boss_ACTION_cloud");
        fog = Resources.Load<GameObject>("Effect/Boss_ACTION_fog");
        danger = Resources.Load<GameObject>("Effect/Boss_ACTION_danger");
        icicle = Resources.Load<GameObject>("Effect/Boss_ACTION_icicle");
    }

    private void FixedUpdate()
    {
        if(curHP <= 0 && !isDead) {
            _EnemyStatusUIController.enemyHit = false;
            _EnemyStatusUIController.enemyPanel.SetActive(false);
            _ClearController.eliminateCount++;
            nav.ResetPath();
            isDead = true;
            anim.SetBool("isHit", true);
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            StartCoroutine(FadeOutOnDead());
            Destroy(gameObject, 1.0f);
        }

        if (!isDead) {
            // 좌우 방향 전환
            if (!movingRight && facingRight) {
                SetFlipX();
            }
            else if (movingRight && !facingRight) {
                SetFlipX();
            }

            // 플레이어와의 거리
            float distToPlayer = DistanceToPlayer();

            // 슬라임 AI
            if (mType == MonsterType.slime) {
                SlimeAI(distToPlayer);
            }

            else if(mType == MonsterType.kingBerk) {
                KingBerkAI(distToPlayer);
            }

            else if (mType == MonsterType.boss) {
                BossAI(distToPlayer);
            }
        }
    }

    // 플레이어-몬스터 간 거리 연산
    private float DistanceToPlayer()
    {
        float distance;
        distance = Vector3.Distance(player.transform.position, gameObject.transform.position);

        return distance;
    }

    // 슬라임 AI
    private void SlimeAI(float distToPlayer)
    {
        // 배회 상태 (추적 범위 바깥)
        if (distToPlayer > pursueRange) {
            movingRight = GetMovingRight(nextPos);

            if (!stopChase) {
                stopChase = true;
                nextPos = RandomPointGenerator(transform.position, patrolRadius);
                nav.SetDestination(nextPos);
            }

            if (Vector3.Distance(nextPos, transform.position) <= 1.5f) {
                nextPos = RandomPointGenerator(transform.position, patrolRadius);
                nav.SetDestination(nextPos);
            }
        }

        else if (distToPlayer <= pursueRange) {
            movingRight = GetMovingRight(player.transform.position);
            stopChase = false;

            /*
            Vector3 closeAttackPoint;
            
            if(transform.position.x > player.transform.position.x) {
                closeAttackPoint = GetCloseAttackPoint(player.transform.position + new Vector3(gameObject.transform.localScale.x * 3 / 2, 0, 0));
            }
            else {
                closeAttackPoint = GetCloseAttackPoint(player.transform.position - new Vector3(gameObject.transform.localScale.x * 3 / 2, 0, 0));
            }
            */

            
            Vector3 playerVec = new Vector3(player.transform.position.x, 0, player.transform.position.z);
            Vector3 monsterVec = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 closeAttackPoint = playerVec + (monsterVec - playerVec).normalized * 2.0f;

            if(Vector3.Distance(closeAttackPoint, transform.position) > transform.position.y + 0.1f) {
                nav.SetDestination(closeAttackPoint);
            }
            else if(!onHit) {
                if (!monsterAttackDelay) {
                    nav.ResetPath();
                    monsterAttackDelay = true;
                    posBeforeAttack = transform.position;
                    StartCoroutine(CloseAttack());
                    StartCoroutine(MonsterAttackDelay());
                }
            }
            else {
                nav.ResetPath();
            }

            /*
            if (Vector3.Distance(playerVec, monsterVec) > 2.5f) {
                closeAttackPoint = playerVec + (monsterVec - playerVec).normalized * 1.5f;
                nav.SetDestination(closeAttackPoint);
            }
           
            else {
                if (!monsterAttackDelay) {
                    nav.ResetPath();
                    monsterAttackDelay = true;
                    posBeforeAttack = transform.position;
                    // rb.AddForce(new Vector3(-15.0f, 0, 0), ForceMode.Impulse);
                    StartCoroutine(CloseAttack(closeAttackPoint));
                    StartCoroutine(MonsterAttackDelay());
                }
            }
            */

            /*
            if (onHit) {
                nav.ResetPath();
            }
            */

            /*
            if (Vector3.Distance(gameObject.transform.position, closeAttackPoint) < 3.0f && !monsterAttackDelay) {
                nav.ResetPath();
                monsterAttackDelay = true;
                posBeforeAttack = transform.position;
                rb.AddForce(new Vector3(-15.0f, 0, 0), ForceMode.Impulse);
                // Invoke("BackToBeforePos", 0.5f);
                // StartCoroutine(CloseAttack());
                StartCoroutine(MonsterAttackDelay());
            }
            */
            /*
            else {
                if (onHit)
                    nav.ResetPath();
                else
                    nav.SetDestination(closeAttackPoint);
            }
            */
            // nav.SetDestination(player.transform.position);
        }
    }

    private void KingBerkAI(float distToPlayer)
    {
        rb.AddForce(0, 10, 0, ForceMode.Impulse);

        // 배회 상태 (추적 범위 바깥)
        if (distToPlayer > pursueRange) {
            movingRight = GetMovingRight(nextPos);

            if (!stopChase) {
                stopChase = true;
                nextPos = RandomPointGenerator(transform.position, patrolRadius);
                nav.SetDestination(nextPos);
            }

            if (Vector3.Distance(nextPos, transform.position) <= 1.5f) {
                nextPos = RandomPointGenerator(transform.position, patrolRadius);
                nav.SetDestination(nextPos);
            }
        }

        else if (distToPlayer <= pursueRange) {
            movingRight = GetMovingRight(player.transform.position);
            stopChase = false;

            Vector3 playerVec = new Vector3(player.transform.position.x, 0, player.transform.position.z);
            Vector3 monsterVec = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 closeAttackPoint = playerVec + (monsterVec - playerVec).normalized * 2.0f;

            if (Vector3.Distance(closeAttackPoint, transform.position) > transform.position.y + 0.1f) {
                nav.SetDestination(closeAttackPoint);
            }
            else if (!onHit) {
                if (!monsterAttackDelay) {
                    nav.ResetPath();
                    monsterAttackDelay = true;
                    posBeforeAttack = transform.position;
                    StartCoroutine(CloseAttack());
                    StartCoroutine(MonsterAttackDelay());
                }
            }
            else {
                nav.ResetPath();
            }
        }
    }

    private void BossAI(float distToPlayer)
    {
        if (!actionDelay) {
            nextPos = RandomPointGenerator(transform.position, patrolRadius);
            int actionProb = Random.Range(1, 101);
            
            if (actionProb <= 25 * probCount && !attackDelay) {
                actionDelay = true;
                int patternProb = Random.Range(1, 101);
                anim.SetBool("isReady", true);

                requireCount = GetBossRequireCount(distToPlayer, patternProb);

                probCount = 1;
            }
            else {
                actionDelay = true;
                movingRight = GetMovingRight(nextPos);
                anim.SetBool("isIdle", true);
                Debug.Log("IDLE");
                Invoke("BossMove", 3.0f);
            }
        }

        else if(Vector3.Distance(transform.position, nextPos) <= 2.0f + Mathf.Epsilon) {
            anim.SetBool("isMove", false);
            probCount *= 2;
            actionDelay = false;
            attackDelay = false;
            Debug.Log("MOVE FIN");
        }
    }

    // 몬스터가 바라보는 방향 반환
    private bool GetMovingRight(Vector3 targetVector)
    {
        if (targetVector.x > transform.position.x)
            return true;
        else if (targetVector.x < transform.position.x)
            return false;

        return movingRight;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("AttackRange")){
            onHit = true;
            if(curHP > 0)
                _EnemyStatusUIController.InitEnemyStatusPanel(gameObject, isBoss);

            anim.SetBool("isHit", true);
            nav.ResetPath();

            if (movingRight) {
                rb.AddForce(new Vector3(-15, 0, 0), ForceMode.Impulse);
            }
            else {
                rb.AddForce(new Vector3(15, 0, 0), ForceMode.Impulse);
            }

            /*
            if (!isBoss)
            {
                anim.SetBool("isHit", true);
                nav.ResetPath();

                if (movingRight)
                {
                    rb.AddForce(new Vector3(-15, 0, 0), ForceMode.Impulse);
                }
                else
                {
                    rb.AddForce(new Vector3(15, 0, 0), ForceMode.Impulse);
                }
            }
            */

            curHP -= PC.damage;
        }
    }

    private List<Vector3> PatternPointGeneratorByRadius(int pointNum, float radius)
    {
        List<Vector3> patternPointList = new List<Vector3>();

        float x;
        float z;
        int toMinus;
        float tmp;        

        for(int i=0; i < pointNum; i++) {
            x = Random.Range(-1.0f, 1.0f);
            tmp = Mathf.Pow(1.0f, 2) - Mathf.Pow(x, 2);
            toMinus = Random.Range(0, 2) * 2 - 1;
            z = Mathf.Sqrt(tmp) * toMinus;

            patternPointList.Add(new Vector3(x, 0.0f, z) * Random.Range(0.0f, radius) + new Vector3(transform.position.x, 0.0f, transform.position.z));
        }

        return patternPointList;
    }

    public static Vector3 RandomPointGenerator(Vector3 StartPos, float Radius)
    {
        Vector3 Dir = Random.insideUnitSphere * Radius;
        Dir += StartPos;
        NavMeshHit Hit_;
        Vector3 FinalPos = Vector3.zero;
        if(NavMesh.SamplePosition(Dir, out Hit_, Radius, 1)) {
            FinalPos = Hit_.position;
        }
        return FinalPos;
    }

    private Vector3 GetCloseAttackPoint(Vector3 targetAttackPoint)
    {
        /*
        if (player.GetComponent<PlayerController>().facingRight)
        {
            if (player.GetComponent<PlayerController>().onDefense)
                return targetAttackPoint + new Vector3(1.5f, 0, 0);
            else
                return targetAttackPoint + new Vector3(0.65f, 0, 0);
        }
        else
        {
            if (player.GetComponent<PlayerController>().onDefense)
                return targetAttackPoint + new Vector3(-1.5f, 0, 0);
            else
                return targetAttackPoint + new Vector3(-0.65f, 0, 0);
        }
        */
        Vector3 playerVector = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        Vector3 attackPointVector = new Vector3(targetAttackPoint.x, 0, targetAttackPoint.z);
        Vector3 calVec = playerVector + (playerVector - attackPointVector) * 3.0f;

        return new Vector3(calVec.x, transform.position.y, calVec.z);
    }

    public void FinishHitAnimation()
    {
        if(curHP > 0) {
            anim.SetBool("isHit", false);
            onHit = false;
        }
    }

    public void BossMove()
    {
        nav.SetDestination(nextPos);
        anim.SetBool("isIdle", false);
        anim.SetBool("isMove", true);
        Debug.Log("IDLE FIN");
        Debug.Log("MOVE");
    }

    private int GetBossRequireCount(float distance, float prob)
    {
        if (distance <= 5) {
            if (prob <= 40)
                return 1;
            else if (prob <= 50)
                return 2;
            else if (prob <= 60)
                return 3;
            else if (prob <= 100)
                return 4;
        }
        else if (distance <= 10) {
            if (prob <= 20)
                return 1;
            else if (prob <= 50)
                return 2;
            else if (prob <= 80)
                return 3;
            else if (prob <= 100)
                return 4;
        }
        else if (distance <= 15) {
            if (prob <= 20)
                return 1;
            else if (prob <= 50)
                return 2;
            else if (prob <= 100)
                return 3;
        }
        return -1;
    }

    public void FinishReadyAnimation()
    {
        List<Vector3> randomPatternPoint;
        int i;

        readyCount++;

        if (readyCount == requireCount) {
            switch (requireCount) {
                case 1:
                    randomPatternPoint = PatternPointGeneratorByRadius(5, 20.0f);
                    for(i=0; i<5; i++) {
                        Instantiate(fog, randomPatternPoint[i] + new Vector3(0, 3, 0), Quaternion.identity);
                    }
                    break;
                case 2:
                    Instantiate(cloud, transform.position + new Vector3(2 * transform.localScale.x, 2, 0), Quaternion.identity);
                    break;
                case 3:
                    randomPatternPoint = PatternPointGeneratorByRadius(10, 50.0f);
                    for(i=0; i<10; i++) {
                        Instantiate(danger, randomPatternPoint[i] + new Vector3(0, 1.15f, 0), Quaternion.Euler(90, 0, 0));
                        StartCoroutine(GenerateIcicle(randomPatternPoint[i] + new Vector3(0.0f, 8.5f, 0.0f), 3.0f));
                    }
                    break;
                case 4:
                	Instantiate(tornado, transform);
                    break;
                default:
                    Debug.Log("Ready Count Error");
                    break;
            }
            
			anim.SetBool("isReady", false);
            readyCount = 0;
            attackDelay = true;
            actionDelay = false;
        }
	}

    private IEnumerator GenerateIcicle(Vector3 genPos, float genTime)
    {
        float k = genTime;

        while (k > 0) {
            k -= Time.deltaTime;
            yield return null;
        }

        Instantiate(icicle, genPos, Quaternion.identity);
    }

    private IEnumerator MonsterAttackDelay()
    {
        float k = 2.0f;

        while (k > 0) {
            k -= Time.deltaTime;
            yield return null;
        }

        monsterAttackDelay = false;
    }

    private void BackToBeforePos()
    {
        transform.position = posBeforeAttack;
    }


    private IEnumerator CloseAttack()
    {
        Vector3 attackDir = new Vector3(player.transform.position.x - transform.position.x, 0, player.transform.position.z - transform.position.z);

        rb.AddForce(attackDir * 10f, ForceMode.Impulse);
        yield return new WaitForSeconds(0.15f);
        rb.AddForce(-attackDir * 10f, ForceMode.Impulse);
        /*
        if(transform.position.x < player.transform.position.x) {
            rb.AddForce(new Vector3(player.transform.position.x - attackPoint.x, 0, (player.transform.position.z - attackPoint.z) * 5.0f) * -30.0f);
        }
        else {
            rb.AddForce(new Vector3(player.transform.position.x - attackPoint.x, 0, (player.transform.position.z - attackPoint.z) * 5.0f) * 30.0f);
        }
        
        // rb.AddForce(new Vector3(-15.0f, 0, 0), ForceMode.Impulse);
        
        // rb.AddForce(new Vector3(15.0f, 0, 0), ForceMode.Impulse);
        if(transform.position.x < player.transform.position.x) {
            rb.AddForce(new Vector3(attackPoint.x - player.transform.position.x, 0, (attackPoint.z - player.transform.position.z) * 5.0f) * -30.0f);
        }
        else {
            rb.AddForce(new Vector3(attackPoint.x - player.transform.position.x, 0, (attackPoint.z - player.transform.position.z) * 5.0f) * 30.0f);
        }
        */
    }

    private IEnumerator FadeOutOnDead()
    {
        float alpha = 1.0f;

        while (alpha > 0) {
            sr.color = new Color(255, 255, 255, alpha);
            yield return new WaitForSecondsRealtime(0.025f);
            alpha -= 0.025f;
        }
    }
}