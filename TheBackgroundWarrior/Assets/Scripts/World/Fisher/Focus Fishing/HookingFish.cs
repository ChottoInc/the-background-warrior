using System.Collections;
using UnityEngine;

public class HookingFish : MonoBehaviour
{
    private int transparencyAmount = Shader.PropertyToID("_Transparency");

    [Header("Manager")]
    [SerializeField] FocusFishingManager _manager;

    [Header("Movement")]
    [SerializeField] float _riseAcceleration = 22f;   // upward force while holding
    [SerializeField] float _gravity = 14f;            // downward force while released
    [SerializeField] float _maxRiseSpeed = 6f;
    [SerializeField] float _maxFallSpeed = 9f;        // higher = waiting longer hurts more


    [SerializeField] float _riseSpeed = 4f;

    [Header("Bounce")]
    [SerializeField, Range(0f, 1f)] float _bottomBounce = 0.4f;
    [SerializeField, Range(0f, 1f)] float _topBounce = 0f;
    [SerializeField] float _minBounceSpeed = 0.5f;    // below this, just stop instead of bouncing

    [Space(10)]
    [SerializeField] GameObject _hook;

    [Space(10)]
    [SerializeField] float _timerChangeTransparency = 0.35f;

    private bool _isAnimatingRockHit;

    private SpriteRenderer _spriteRenderer;
    private Material _matImageWeapon;

    private bool isInitialized;

    [Header("Pond Bounds")]
    [SerializeField] float _topBound = 3f;
    [SerializeField] float _bottomBound = -3f;

    public bool CanMove { get; private set; }
    public bool CanMoveHook { get; private set; }
    public bool CanMoveWin { get; private set; }

    private bool _isHolding;
    private float _currentVelocityY;

    private Vector3 _startPos;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _startPos = transform.localPosition;

        InitializedIfNeeded();
    }

    private void InitializedIfNeeded()
    {
        if (isInitialized) return;

        // copy material image ui
        _matImageWeapon = new Material(_spriteRenderer.material);
        _spriteRenderer.material = _matImageWeapon;

        isInitialized = true;
    }

    private void Update()
    {
        if (!CanMove)
        {
            if (CanMoveHook)
            {
                // move hook up on fail
                HandleMovementHook();
            }

            if (CanMoveWin)
            {
                // move all fish on win
                HandleMovementWin();
            }
        }
        else
        {
            HandleInput();
            HandleMovement();
            ClampToPondBounds();
        }
    }

    private void HandleInput()
    {
        _isHolding = Input.GetMouseButton(0);
    }

    private void HandleMovement()
    {
        float acceleration = _isHolding ? _riseAcceleration : -_gravity;

        _currentVelocityY += acceleration * Time.deltaTime;
        _currentVelocityY = Mathf.Clamp(_currentVelocityY, -_maxFallSpeed, _maxRiseSpeed);

        transform.localPosition += Vector3.up * _currentVelocityY * Time.deltaTime;

        if (_isHolding)
        {
            AudioManager.Instance.PlayEffectCheckPlaying("ReelingFish");
        }
        else
        {
            AudioManager.Instance.PauseEffect("ReelingFish");
        }
    }

    private void ClampToPondBounds()
    {
        Vector3 pos = transform.localPosition;

        if (pos.y <= _bottomBound)
        {
            pos.y = _bottomBound;
            if (_currentVelocityY < 0f)
            {
                // small bounce off the bottom, like Stardew
                _currentVelocityY = -_currentVelocityY > _minBounceSpeed
                    ? -_currentVelocityY * _bottomBounce
                    : 0f;
            }
        }
        else if (pos.y >= _topBound)
        {
            pos.y = _topBound;
            if (_currentVelocityY > 0f)
            {
                _currentVelocityY = _currentVelocityY > _minBounceSpeed
                    ? -_currentVelocityY * _topBounce
                    : 0f;
            }
        }

        transform.localPosition = pos;
    }

    private void HandleMovementHook()
    {
        _currentVelocityY = _riseSpeed;

        _hook.transform.localPosition += Vector3.up * _currentVelocityY * Time.deltaTime;
    }

    private void HandleMovementWin()
    {
        _currentVelocityY = _riseSpeed;

        transform.localPosition += Vector3.up * _currentVelocityY * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!CanMove) return;

        if (other.CompareTag("FishingRock"))
        {
            AudioManager.Instance.PlayEffect("FishingRockHit");
            if(other.TryGetComponent(out FishingRock fRock))
            {
                fRock.StartDestroy();
            }

            LoseStar();

            if (!_isAnimatingRockHit)
            {
                StartCoroutine(CoFlashSprite());
            }
        }
        else if (other.CompareTag("FishingStar"))
        {
            AudioManager.Instance.PlayEffect("FishingStarHit");
            if (other.TryGetComponent(out FishingRock fRock))
            {
                fRock.StartDestroy();
            }

            AddStar();
        }
    }

    private void LoseStar()
    {
        _manager.LoseStar();
    }

    private void AddStar()
    {
        _manager.AddStar();
    }

    public void SetMove(bool canMove)
    {
        CanMove = canMove;
    }

    public void SetMoveHook(bool canMove)
    {
        CanMoveHook = canMove;
    }

    public void SetMoveWin(bool canMove)
    {
        CanMoveWin = canMove;
    }

    public void ResetFish()
    {
        transform.localPosition = _startPos;

        _hook.transform.localPosition = Vector2.zero;
        
        _isHolding = false;
        _currentVelocityY = 0f;
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
}
