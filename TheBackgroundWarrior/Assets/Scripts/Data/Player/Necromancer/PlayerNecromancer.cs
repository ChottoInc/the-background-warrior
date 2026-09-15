using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNecromancer : Player
{
    [Header("Movement")]
    [SerializeField] Animator _animator;


    public bool IsSummonAnimationPlaying { get; private set; }


    [Space(10)]
    [SerializeField] NecromancerFightingSpot[] _fightSpots;

    private int _maxFightSpots;


    public event Action<int, int> OnStatChange;




    public PlayerNecromancerData PlayerData { get; private set; }


    protected override void Awake()
    {
        base.Awake();

        //OnStatChange += CheckSpellBar;
    }

    private void Start()
    {
        _buffsToCheckTypes = new List<UtilsBuffs.BuffType>()
        {
            UtilsBuffs.BuffType.Greed,
            UtilsBuffs.BuffType.Veteran,
            UtilsBuffs.BuffType.Inspiration,
        };
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();

        //OnStatChange -= CheckSpellBar;

        if (PlayerData != null)
        {
            PlayerData.OnLevelUp -= LevelUp;

            PlayerData.OnStatChange -= OnStatChangeNecromancer;
        }
    }

    public void Setup(PlayerNecromancerData playerData)
    {
        PlayerData = playerData;

        if (playerData != null)
        {
            playerData.OnLevelUp += LevelUp;

            playerData.OnStatChange += OnStatChangeNecromancer;

            // summon animation and summon first fighters
            StartCoroutine(CoSummon(2f));
        }
    }

    private void InitializeFightingSpots()
    {
        _maxFightSpots = (int)PlayerData.CurrentAptitude + 1;

        for (int i = 0; i < _maxFightSpots; i++)
        {
            if (!_fightSpots[i].IsInitialized)
            {
                _fightSpots[i].Initialize();
            }
        }
    }

    protected override void Update()
    {
        base.Update();
    }



    public void GiveExp()
    {
        long baseExp = 500;
        long finalExp = Mathf.RoundToInt((float)baseExp * (1f + PlayerData.CurrentLuck));
        PlayerData.AddExp(finalExp);
        SaveNecromancerData();
    }

    private IEnumerator CoSummon(float timer)
    {
        yield return new WaitForSeconds(timer);

        InitializeFightingSpots();
        SummonAnimation();
    }

    public void SummonAnimation()
    {
        if (IsSummonAnimationPlaying) return;

        _animator.SetTrigger("Summon");
        IsSummonAnimationPlaying = true;
    }

    public void ExternalEndAnimation()
    {
        IsSummonAnimationPlaying = false;
        _animator.SetTrigger("Idle");
    }




    public override IBasePlayerData GetPlayerData()
    {
        return PlayerData;
    }

    public override long GetCurrenExp()
    {
        return PlayerData.CurrentExp;
    }

    public override long GetExpToNextLevel()
    {
        return PlayerData.ExpToNextLevel;
    }

    #region SAVE

    public void SaveNecromancerData()
    {
        PlayerManager.Instance.UpdateNecromancerData(PlayerData);
        PlayerManager.Instance.SaveNecromancerData();
    }

    #endregion

    #region HANDLE EVENTS FROM NECROMANCER DATA

    protected override void LevelUp()
    {
        base.LevelUp();

        SaveNecromancerData();
    }

    private void OnStatChangeNecromancer(int id, int value)
    {
        // check for new fighting spots when in scene
        StartCoroutine(CoSummon(0.25f));

        OnStatChange?.Invoke(id, value);
    }

    #endregion
}
