using System.Collections;
using UnityEngine;

public class Summon : MonoBehaviour
{
    [Header("Sprite")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] bool faceRight;
    [SerializeField] float speed = 5f;

    [Space(10)]
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip attackClip;

    [Space(10)]
    [SerializeField] Transform checkEnemyPoint;
    [SerializeField] LayerMask enemyLayer;

    [Space(10)]
    [SerializeField] GameObject _explosionPrefab;

    // --------- MOVEMENT VARS

    private bool canInitialMove;

    private Vector3 startScale;

    private float currentTarget;
    private Vector2 currentDirection;

    private bool isWalking;

    private Rigidbody2D rb;

    // --------- ATTACK VARS


    private bool canAttack;
    private bool isAttacking;

    private float CooldownAttack => 1f / Data.CurrentAtkSpd;
    private float timerAttack;

    private bool isEnemyDetected;
    private Enemy currentEnemy;

    // --------- DEATH VARS

    private float _timer1Sec;

    private bool _requestedDeath;
    private bool isDying;


    //public event Action OnDespawn;



    public SummonData Data { get; private set; }


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void Start()
    {
        _timer1Sec = UtilsGeneral.TIMER_1SECONDS;

        startScale = spriteRenderer.transform.localScale;


        StartCoroutine(CoSpawned());
    }


    private void Update()
    {
        CheckAttack();

        HandleLifetime();

        HandleDeath();
    }

    private void FixedUpdate()
    {
        // always tries to move
        HandleMovement();

        // when in fight scene, check for enemy when not attacking
        if (!isAttacking)
        {
            CheckForEnemy();
        }
    }

    public void Setup(SummonData data)
    {
        Data = data;
        //Debug.Log("atk perc: " + data.CurrentAtkPerc);
    }

    private IEnumerator CoSpawned()
    {
        //Debug.Log("Summon animation...");
        // summon animation into idle, never returns to idle
        animator.SetTrigger("Summon");

        yield return new WaitForSeconds(2f);

        // change rb type
        rb.bodyType = RigidbodyType2D.Kinematic;

        // enable movement
        canInitialMove = true;

        // enable attack
        canAttack = true;

        //Debug.Log("Moving.");

        GenerateNewTarget();
    }

    private void HandleLifetime()
    {
        // decrease life every second
        if (_timer1Sec <= 0)
        {
            if (Data != null)
            {
                Data.DecreaseHp(1);

                // check for death
                if (Data.IsDead)
                {
                    _requestedDeath = true;
                }
            }
            _timer1Sec = UtilsGeneral.TIMER_1SECONDS;
        }
        else
        {
            _timer1Sec -= Time.deltaTime;
        }
    }

    private void HandleDeath()
    {
        // don't acll death animation if already dying
        if (isDying) return;

        // check on death if requested and dies if attack is done
        if (_requestedDeath && !isAttacking)
        {
            isDying = true;
            _requestedDeath = false;

            // stops attack and movement
            canAttack = false;
            canInitialMove = false;

            //Debug.Log("Death animation.");

            // animate death
            animator.SetTrigger("Death");

            // check explosion 
            PlayerNecromancerData necroData = PlayerManager.Instance.PlayerNecromancerData;
            if (necroData.IsAfterlifeRitualUnlocked)
            {
                // make expolsion
                Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            }

            StartCoroutine(CoDespawn());
        }
    }

    private void HandleMovement()
    {
        if (!canInitialMove) return;

        if (isAttacking) return;

        float distance = Mathf.Abs(transform.position.x - currentTarget);

        if (distance > 0.1f)
        {
            // if not on target and moving, set to walk animator
            if (!isWalking)
            {
                isWalking = true;
                animator.SetTrigger("Walk");
            }

            // get target dir
            currentDirection = new Vector2(currentTarget - transform.position.x, 0).normalized;

            // move
            transform.position += speed * Time.fixedDeltaTime * (Vector3)currentDirection;

            CheckFlip();
        }
        else
        {
            // when arriving at point, don't idle, just change direction
            GenerateNewTarget();
        }
    }

    private void CheckForEnemy()
    {
        if (!canAttack) return;

        // if already engaged an enemy don't do anything
        if (isEnemyDetected) return;

        // find the enemy
        if (CheckEnemyAtPoint(checkEnemyPoint.position, 0.5f, enemyLayer, out Collider2D hit))
        {
            Enemy enemy = hit.GetComponent<Enemy>();

            // if the enemy is not dead and can actually attack, perform attack
            if (CombatManager.Instance.CurrentEnemy == null)
            {
                if (!enemy.IsDead && timerAttack <= 0)
                {
                    //Debug.Log("Found enemy...");
                    HandleAttack(enemy);
                }
            }
            else
            {
                if ((enemy.EnemyIndex != CombatManager.Instance.CurrentEnemy.EnemyIndex) &&
                !enemy.IsDead && timerAttack <= 0)
                {
                    //Debug.Log("Found enemy...");
                    HandleAttack(enemy);
                }
            }

        }
    }

    private void HandleAttack(Enemy enemy)
    {
        // set detected enemy and is attacking so the companion doesn't look for another one
        isEnemyDetected = true;
        isAttacking = true;

        isWalking = false;

        // set the current enemy
        currentEnemy = enemy;

        PerformAttack();
    }

    private void CheckAttack()
    {
        // only scale timer when attack is not occuring
        if (isAttacking) return;

        if (timerAttack <= 0) return;

        // keep decreasing the timer for the attack
        timerAttack -= Time.deltaTime;
    }

    private void PerformAttack()
    {
        //Debug.Log("Attack animation...");
        // animate
        animator.SetTrigger("Attack");

        // stop enemy movement to synch animation
        if (currentEnemy != null)
        {
            currentEnemy.SetMove(false);
        }

        // wait and start resets
        StartCoroutine(CoStopAttack());
    }

    /// <summary>
    /// Called from the animation to align animation and damages
    /// </summary>
    public void ExternalAttack()
    {
        if (currentEnemy != null)
        {
            //Debug.Log("Actual hit on enemy...");

            // damage enemy once
            currentEnemy.EnemyData.TakeDamage(Data);

            if (currentEnemy.IsDead && !currentEnemy.IsAttacking)
            {
                if (CombatManager.Instance != null)
                {
                    CombatManager.Instance.HandleEnemyDeath(currentEnemy);
                }
            }
        }
    }

    private IEnumerator CoStopAttack()
    {
        yield return new WaitForSeconds(attackClip.length);

        // reset enemy vars
        isEnemyDetected = false;
        isAttacking = false;

        // restart enemy movement
        if (currentEnemy != null)
        {
            currentEnemy.SetMove(true);
        }

        currentEnemy = null;

        // reset attack cooldown
        timerAttack = CooldownAttack;

        // random movement in direction
        GenerateNewTarget();

        isWalking = true;
        animator.SetTrigger("Walk");
    }

    public bool CheckEnemyAtPoint(Vector2 point, float radius, LayerMask enemyMask, out Collider2D hitEnemy)
    {
        hitEnemy = Physics2D.OverlapCircle(point, radius, enemyMask);
        return hitEnemy != null;
    }

    private void CheckFlip()
    {
        // check sprite flip
        float vx = currentDirection.x;
        if (vx > 0.01f && faceRight)
        {
            spriteRenderer.transform.localScale = startScale;
        }
        else if (vx > 0.01f && !faceRight)
        {
            spriteRenderer.transform.localScale = new Vector3(-startScale.x, startScale.y, startScale.z);
        }
        else if (vx < -0.01f && faceRight)
        {
            spriteRenderer.transform.localScale = new Vector3(-startScale.x, startScale.y, startScale.z);
        }
        else if (vx < -0.01f && !faceRight)
        {
            spriteRenderer.transform.localScale = startScale;
        }
    }

    private void GenerateNewTarget()
    {
        currentTarget = UnityEngine.Random.Range(InitializerManager.Instance.GetScreenOffsetBound(), InitializerManager.GetScreenWidth() - InitializerManager.Instance.GetScreenOffsetBound());
        currentTarget = Camera.main.ScreenToWorldPoint(new Vector2(currentTarget, 0)).x;
    }

    /// <summary>
    /// Called when dies
    /// </summary>
    private IEnumerator CoDespawn()
    {
        yield return new WaitForSeconds(3f);

        // derease horde number
        FindObjectsByType<PlayerWarriorRituals>(FindObjectsSortMode.None)[0].DecreaseHorde();

        Destroy(gameObject);
    }
}
