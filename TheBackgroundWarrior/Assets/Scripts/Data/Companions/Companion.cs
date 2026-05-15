using System.Collections;
using UnityEngine;

public class Companion : MonoBehaviour
{
    [Header("Sprite")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] bool faceRight;
    [SerializeField] float speed = 5f;
    [SerializeField] float cooldownIdle = 1.5f;

    [Space(10)]
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip attackClip;

    [Space(10)]
    [SerializeField] Transform checkEnemyPoint;
    [SerializeField] LayerMask enemyLayer;

    [Header("UI")]
    [SerializeField] GameObject imageBefriended;
    [SerializeField] GameObject imageNotBefriended;


    private CompanionSO tempSOBefriend;


    private CompanionData companionData;


    // --------- MOVEMENT VARS

    private bool canInitialMove;

    private Vector3 startScale;

    private float currentTarget;
    private Vector2 currentDirection;

    private bool isWalking;

    private bool isIdling;
    private float timerIdle;

    // setup vars
    private bool isGoingToCrop;
    private int slotCropIndex;
    private bool isRandomWalking;

    // disappear vars
    private bool isWalkingAway;

    private Rigidbody2D rb;

    // --------- ATTACK VARS


    private bool canAttack;
    private bool isAttacking;

    private float CooldownAttack => 1f / companionData.CurrentAtkSpd;
    private float timerAttack;

    private bool isEnemyDetected;
    private Enemy currentEnemy;


    public CompanionData CurrentCompanionData => companionData;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        startScale = spriteRenderer.transform.localScale;

        StartCoroutine(CoSpawned());
    }


    private void Update()
    {
        CheckAttack();
    }

    private void FixedUpdate()
    {
        if (isGoingToCrop || isRandomWalking || isWalkingAway)
        {
            HandleMovement();
        }
        
        // when in fight scene, check for enemy when not attacking
        if (!isAttacking)
        {
            CheckForEnemy();
        }
    }

    private IEnumerator CoSpawned()
    {
        yield return new WaitForSeconds(2f);

        // change rb type
        rb.bodyType = RigidbodyType2D.Kinematic;

        // enable movement
        canInitialMove = true;

        // enable attack
        canAttack = true;

        //GenerateNewTarget();
    }

    private void HandleMovement()
    {
        if (!canInitialMove) return;

        float distance = Mathf.Abs(transform.position.x - currentTarget);

        if (distance > 0.1f && !isIdling)
        {
            // if not on target and moving, set to walk animator
            if (!isWalking)
            {
                isWalking = true;
                animator.SetBool("isWalking", isWalking);
            }

            // get target dir
            currentDirection = new Vector2(currentTarget - transform.position.x, 0).normalized;

            // move
            transform.position += speed * Time.fixedDeltaTime * (Vector3)currentDirection;

            CheckFlip();
        }
        else
        {
            // if is going to crop, can't be walking away
            if (isGoingToCrop)
            {
                HandleTargetCrop();
            }
            else if (isWalkingAway)
            {
                HandleWalkAway();
            }
            else
            {
                HandleStandardWalk();
            }
        }
    }

    private void HandleTargetCrop()
    {
        if (!isIdling)
        {
            timerIdle = 5f;
            isIdling = true;

            isWalking = false;
            animator.SetBool("isWalking", isWalking);

            //Debug.Log("arrived at crop");
        }
        else
        {
            if (timerIdle <= 0)
            {
                // remove crop from plot
                ResetCrop();
                

                isGoingToCrop = false;

                // take action befriended or not
                bool success = UtilsGeneral.GetRandomSuccessFromValue(PlayerManager.Instance.PlayerFarmerData.CurrentLuck);
                if (success)
                {
                    // Animation
                    imageBefriended.SetActive(true);

                    // add exp, might tweak
                    PlayerManager.Instance.PlayerFarmerData.AddExp(500);

                    PlayerManager.Instance.OnBefriendedCompanion(tempSOBefriend.Id);

                    if (PlayerManager.Instance.PlayerFarmerData.HasCompanion(tempSOBefriend))
                    {
                        // TODO: handle if companion is already befriended, dismantle it, for now give bits?
                        PlayerManager.Instance.Inventory.AddBits(2);
                        PlayerManager.Instance.SaveInventoryData();
                    }
                    else
                    {
                        // add to companions
                        PlayerManager.Instance.PlayerFarmerData.AddCompanion(tempSOBefriend);
                    }

                    PlayerManager.Instance.SaveFarmerData();

                    // walk away
                    SetTargetOutsideScreen();

                    //Debug.Log("Companion befriended");
                }
                else
                {
                    // add exp, might tweak
                    PlayerManager.Instance.PlayerFarmerData.AddExp(50);
                    PlayerManager.Instance.SaveFarmerData();

                    // Animation
                    imageNotBefriended.SetActive(true);

                    // set the companion to walk away from the screen
                    SetTargetOutsideScreen();

                    //Debug.Log("Companion not befriended");
                }

                isIdling = false;
            }
            else
            {
                timerIdle -= Time.fixedDeltaTime;
            }
        }
    }

    private void ResetCrop()
    {
        CropData currentCrop = null;

        switch (slotCropIndex)
        {
            case 0: currentCrop = PlayerManager.Instance.PlayerFarmerData.Slot1CropData; break;
            case 1: currentCrop = PlayerManager.Instance.PlayerFarmerData.Slot2CropData; break;
            case 2: currentCrop = PlayerManager.Instance.PlayerFarmerData.Slot3CropData; break;
            case 3: currentCrop = PlayerManager.Instance.PlayerFarmerData.Slot4CropData; break;
        }

        if (currentCrop != null)
        {
            currentCrop.ResetGrowth();
            CropsPlantManager.Instance.SetCrop(slotCropIndex, currentCrop, false);
        }
    }

    private void HandleWalkAway()
    {
        Destroy(gameObject);
    }

    private void HandleStandardWalk()
    {
        // handles idling timer before move again when random walking
        if (!isIdling)
        {
            timerIdle = cooldownIdle;
            isIdling = true;

            isWalking = false;
            animator.SetBool("isWalking", isWalking);
        }
        else
        {
            if (timerIdle <= 0)
            {
                GenerateNewTarget();
                isIdling = false;
            }
            else
            {
                timerIdle -= Time.fixedDeltaTime;
            }
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
            if(CombatManager.Instance.CurrentEnemy == null)
            {
                if (!enemy.IsDead && timerAttack <= 0)
                {
                    HandleAttack(enemy);
                }
            }
            else
            {
                if ((enemy.EnemyIndex != CombatManager.Instance.CurrentEnemy.EnemyIndex) &&
                !enemy.IsDead && timerAttack <= 0)
                {
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

        // disable walking
        isRandomWalking = false;

        isWalking = false;
        animator.SetBool("isWalking", isWalking);

        // set the current enemy
        currentEnemy = enemy;

        PerformAttack();
    }

    private void CheckAttack()
    {
        if (timerAttack <= 0) return;

        // keep decreasing the timer for the attack
        timerAttack -= Time.deltaTime;
    }

    private void PerformAttack()
    {
        // animate
        animator.SetTrigger("Attack");
        animator.ResetTrigger("Stop");

        // stop enemy movement to synch animation
        if(currentEnemy != null)
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
            // damage enemy once
            currentEnemy.EnemyData.TakeDamage(companionData);
        }
    }

    private IEnumerator CoStopAttack()
    {
        yield return new WaitForSeconds(attackClip.length);

        // animate stop
        animator.SetTrigger("Stop");
        animator.ResetTrigger("Attack");

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

        // re enable walking
        isRandomWalking = true;

        isWalking = true;
        animator.SetBool("isWalking", isWalking);
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

    public void SetTargetOutsideScreen()
    {
        isRandomWalking = false;

        float x;
        if (Random.value < 0.5f)
        {
            x = InitializerManager.Instance.GetScreenOffsetBound() - 200f;
        }
        else
        {
            x = InitializerManager.GetScreenWidth() - InitializerManager.Instance.GetScreenOffsetBound() + 200f;
        }

        currentTarget = Camera.main.ScreenToWorldPoint(new Vector2(x, 0)).x;

        isWalkingAway = true;
    }

    public void SetupBefriend(CompanionSO so, Vector2 cropPos, int slotIndex)
    {
        tempSOBefriend = so;

        // set going to crop
        isGoingToCrop = true;
        slotCropIndex = slotIndex;

        // set crop target
        currentTarget = cropPos.x;

        //Debug.Log("Setup befriend");
    }

    public void SetupRandomWalk()
    {
        // set going to crop
        isRandomWalking = true;

        GenerateNewTarget();
    }

    public void SetupFight(CompanionData data)
    {
        // set data from farmer save
        companionData = data;

        // new direction
        GenerateNewTarget();

        isRandomWalking = true;
    }
}
