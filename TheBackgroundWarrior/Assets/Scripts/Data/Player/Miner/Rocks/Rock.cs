using System.Collections;
using UnityEngine;

public class Rock : MonoBehaviour, IPoolObject
{
    private int transparencyAmount = Shader.PropertyToID("_Transparency");


    [Header("Sprite")]
    [SerializeField] SpriteRenderer spriteRenderer;

    [Space(10)]
    [SerializeField] float _timerChangeTransparency = 0.35f;

    private bool _isAnimatingRockHit;

    private Material _matImageWeapon;

    private bool isInitialized;

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

        InitializedIfNeeded();
    }

    private void InitializedIfNeeded()
    {
        if (isInitialized) return;

        // copy material image ui
        _matImageWeapon = new Material(spriteRenderer.material);
        spriteRenderer.material = _matImageWeapon;

        isInitialized = true;
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

        if(!_isAnimatingRockHit)
            StartCoroutine(CoFlashSprite());
    }

    private void UpdateDurabilityUI()
    {
        durabilityBar.SetCurrentValue(rockData.CurrentDurability);
    }


    private IEnumerator CoFlashSprite()
    {
        _isAnimatingRockHit = true;

        _matImageWeapon.SetFloat(transparencyAmount, 1);

        float elapsedTime = 0;

        float lerpedTransparency = 0;

        // lerp from 0 to 1
        while (elapsedTime < _timerChangeTransparency)
        {
            elapsedTime += Time.unscaledDeltaTime;

            lerpedTransparency = Mathf.Lerp(1f, 0f, elapsedTime / _timerChangeTransparency);

            _matImageWeapon.SetFloat(transparencyAmount, lerpedTransparency);

            yield return null;
        }

        _isAnimatingRockHit = false;
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
