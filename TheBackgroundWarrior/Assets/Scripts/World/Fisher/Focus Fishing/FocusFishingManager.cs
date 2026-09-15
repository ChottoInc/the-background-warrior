using System.Collections;
using TMPro;
using UnityEngine;

public class FocusFishingManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject _panelCountdown;
    [SerializeField] TMP_Text _textCountdown;

    private float _timerCountdown;
    private int _countdown = 3;

    [SerializeField] UIFishingStarPrefab[] _uiStars;

    private int _collectedStars;

    [Header("Fish")]
    [SerializeField] HookingFish _fish;

    [Header("Rocks")]
    [SerializeField] FishingRockSpawner _spawner;

    [Header("Bubbles")]
    [SerializeField] FishingRockSpawner _bubbleSpawner;

    [Header("Stars")]
    [SerializeField] FishingRockSpawner _starSpawner;

    public bool CanCount { get; private set; }
    public bool HasGameStarted { get; private set; }

    private void Update()
    {
        if(CanCount && !HasGameStarted)
        {
            if(_timerCountdown <= 0)
            {
                _countdown--;

                _timerCountdown = UtilsGeneral.TIMER_1SECONDS;

                if(_countdown <= 0)
                {
                    _panelCountdown.SetActive(false);

                    CanCount = false;
                    HasGameStarted = true;

                    _fish.SetMove(true);
                }
                else
                {
                    _textCountdown.text = _countdown.ToString();
                }
            }
            else
            {
                _timerCountdown -= Time.deltaTime;
            }
        }
    }

    public void Open()
    {
        gameObject.SetActive(true);

        StartCoroutine(CoWaitFrame());
    }

    private IEnumerator CoWaitFrame()
    {
        yield return new WaitForEndOfFrame();

        Initialize();
    }

    private void Initialize()
    {
        _panelCountdown.SetActive(true);
        _countdown = 3;
        _textCountdown.text = _countdown.ToString();
        _timerCountdown = UtilsGeneral.TIMER_1SECONDS;

        _fish.ResetFish();

        _spawner.StartCoroutine(_spawner.CoSpawnLoop(_countdown));

        _bubbleSpawner.StartCoroutine(_bubbleSpawner.CoSpawnLoop(_countdown));

        _starSpawner.StartCoroutine(_starSpawner.CoSpawnLoop(_countdown));

        _collectedStars = 0;
        foreach (var star in _uiStars)
        {
            star.ResetStar();
        }

        CanCount = true;
    }

    public void LoseStar()
    {
        _collectedStars--;

        if(_collectedStars < 0)
        {
            // lost
            // stop now rock spawner and star spawner
            _spawner.StopAllCoroutines();
            _starSpawner.StopAllCoroutines();

            // stop fish
            _fish.SetMove(false);

            _fish.SetMoveHook(true);

            StartCoroutine(CoLose());

            AudioManager.Instance.StopEffect("ReelingFish");

            //Debug.Log("Lose");
        }
        else
        {
            _uiStars[_collectedStars].LostStar();
        }
    }

    private IEnumerator CoLose()
    {
        yield return new WaitForSeconds(2f);

        _bubbleSpawner.StopAllCoroutines();

        _fish.SetMoveHook(false);

        Close();
    }

    private IEnumerator CoWin()
    {
        yield return new WaitForSeconds(2f);

        _bubbleSpawner.StopAllCoroutines();

        _fish.SetMoveWin(false);

        Close();
    }

    public void AddStar()
    {
        _uiStars[_collectedStars].CollectStar();

        _collectedStars++;

        if (_collectedStars >= 5)
        {
            // win
            // stop now rock spawner and star spawner
            _spawner.StopAllCoroutines();
            _starSpawner.StopAllCoroutines();

            // stop fish and move it up
            _fish.SetMove(false);

            _fish.SetMoveWin(true);

            StartCoroutine(CoWin());

            AudioManager.Instance.StopEffect("ReelingFish");

            //Debug.Log("Win");
        }
    }

    private void Close()
    {
        HasGameStarted = false;
        gameObject.SetActive(false);
    }
}
