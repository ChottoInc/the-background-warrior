using UnityEngine;

public class Rock : MonoBehaviour, IPoolObject
{
    [Header("Sprite")]
    [SerializeField] SpriteRenderer spriteRenderer;

    [Header("Death")]
    [SerializeField] ParticleSystem smashVFX;

    [Header("UI")]
    [SerializeField] GenericBar durabilityBar;

    private bool isDurabilityBarActive;



    private float smashVFXDuration;
    private float timerSmashVFX;

    private RockData rockData;


    private int rockIndex;


    // ------- DEATH

    private bool isSmashVFXPlaying;




    public RockData RockData => rockData;

    public bool IsSmashed => rockData.CurrentDurability <= 0;


    public int RockIndex => rockIndex;



    private void OnDestroy()
    {
        if(rockData != null)
            rockData.OnTakeDamage -= OnTakeDamage;
    }



    private void Start()
    {
        smashVFXDuration = smashVFX.main.duration;
    }

    private void Update()
    {
        if (isSmashVFXPlaying)
        {
            CheckSmashVFX();
        }
    }

    private void CheckSmashVFX()
    {
        if (timerSmashVFX <= 0)
        {
            isSmashVFXPlaying = false;
            HideAfterSmash();
        }
        else
        {
            timerSmashVFX -= Time.deltaTime;
        }
    }

    public void Setup(RockData rockData, int index)
    {
        this.rockData = rockData;

        rockIndex = index;

        spriteRenderer.sprite = rockData.RockSO.Sprite;
        spriteRenderer.sortingOrder = rockIndex;

        durabilityBar.Setup(rockData.MaxDurability, rockData.CurrentDurability);

        rockData.OnTakeDamage += OnTakeDamage;
    }

    private void HideSprite(bool hide)
    {
        // save initial color
        Color spriteColor = spriteRenderer.color;

        if (hide)
            spriteRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 0);
        else
            spriteRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 1);
    }

    public void PlayDeath(bool setSmashed)
    {
        if (setSmashed && rockData != null)
            rockData.SetSmashed();

        HideSprite(true);

        durabilityBar.gameObject.SetActive(false);
        isDurabilityBarActive = false;

        //rockData = null;

        // play vfx
        smashVFX.Play();
        timerSmashVFX = smashVFXDuration;
        isSmashVFXPlaying = true;
    }

    private void HideAfterSmash()
    {
        smashVFX.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        Die();
    }

    public void OnSpawn()
    {
        HideSprite(false);
    }

    public void OnDespawn()
    {
        if (rockData != null)
        {
            rockData = null;
        }
    }

    public void Die()
    {
        if(RockSpawnManager.Instance != null)
            RockSpawnManager.Instance.RemoveFromCurrentRocksList(this);

        PoolManager.Instance.Return(gameObject, "rock");
    }


    private void OnTakeDamage()
    {
        // active durability ui when damaged
        if (!isDurabilityBarActive)
        {
            isDurabilityBarActive = true;
            durabilityBar.gameObject.SetActive(true);
        }

        UpdateDurabilityUI();
    }

    private void UpdateDurabilityUI()
    {
        durabilityBar.SetCurrentValue(rockData.CurrentDurability);
    }




    public override bool Equals(object other)
    {
        Rock otherEnemy = other as Rock;
        return rockIndex == otherEnemy.rockIndex;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
