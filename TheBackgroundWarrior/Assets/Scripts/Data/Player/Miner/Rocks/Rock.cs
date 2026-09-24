using System.Collections;
using UnityEngine;

public class Rock : MonoBehaviour, IPoolObject
{
    private int transparencyAmount = Shader.PropertyToID("_Transparency");


    [Header("Sprite")]
    [SerializeField] SpriteRenderer spriteRenderer;

    [Space(10)]
    [SerializeField] float _timerHit = 0.35f;

    private bool _isAnimatingRockHit;

    private Material _matImageWeapon;

    private bool isInitialized;

    [Header("Scale")]
    [SerializeField] float _scaleX = 0.95f;
    [SerializeField] float _scaleY = 1.15f;

    [Header("Death")]
    [SerializeField] ParticleSystem smashVFX;

    [Header("UI")]
    [SerializeField] GenericBar durabilityBar;

    private bool isDurabilityBarActive;



    private float smashVFXDuration;
    private float timerSmashVFX;

    private RockData rockData;


    private int rockIndex;

    private Vector3 _startScale;


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

        _startScale = transform.localScale;

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

        if(_isAnimatingRockHit)
        {
            // reset all
            StopAllCoroutines();

            ResetFlash();
            ResetScale();

            _isAnimatingRockHit = false;
        }

        // flash and scale at every hit
        StartCoroutine(CoFlashSprite());
        StartCoroutine(CoScaleSprite());
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
        while (elapsedTime < _timerHit)
        {
            elapsedTime += Time.unscaledDeltaTime;

            lerpedTransparency = Mathf.Lerp(1f, 0f, elapsedTime / _timerHit);

            _matImageWeapon.SetFloat(transparencyAmount, lerpedTransparency);

            yield return null;
        }

        _isAnimatingRockHit = false;
    }

    private void ResetFlash()
    {
        _matImageWeapon.SetFloat(transparencyAmount, 0f);
    }

    private IEnumerator CoScaleSprite()
    {
        float elapsedTime = 0;

        transform.localScale = new Vector3(_startScale.x * _scaleX, _startScale.y * _scaleY, _startScale.z);

        // lerp from 0 to 1
        while (elapsedTime < _timerHit)
        {
            elapsedTime += Time.unscaledDeltaTime;

            float xScale = Mathf.Lerp(transform.localScale.x, _startScale.x, elapsedTime / _timerHit);
            float yScale = Mathf.Lerp(transform.localScale.y, _startScale.y, elapsedTime / _timerHit);

            transform.localScale = new Vector3(xScale, yScale, _startScale.z);

            yield return null;
        }
    }

    private void ResetScale()
    {
        transform.localScale = _startScale;
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
