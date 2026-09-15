using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMage : Player
{
    [Header("Movement")]
    [SerializeField] Animator animator;

    [Header("Casting")]
    [SerializeField] Image _imageLearningSpell;

    [Space(10)]
    [SerializeField] Transform dummyPosition;
    [SerializeField] Transform castPosition;
    [SerializeField] float baseCooldownCast = 10f;

    [Space(10)]
    [SerializeField] GenericBar barSpell;

    [Space(10)]
    [SerializeField] GenericBar _barCooldown;

    private float _finalCooldownCast;

    private SpellData _currentSpell;
    private GameObject _spellPrefab;
    private float _timerCast;


    public event Action<int, int> OnStatChange;




    public PlayerMageData PlayerData { get; private set; }


    protected override void Awake()
    {
        base.Awake();

        OnStatChange += CheckSpellBar;
    }

    private void Start()
    {
        _buffsToCheckTypes = new List<UtilsBuffs.BuffType>()
        {
            UtilsBuffs.BuffType.Greed,
            UtilsBuffs.BuffType.Veteran,
            UtilsBuffs.BuffType.Arcanist,
            UtilsBuffs.BuffType.Inspiration,
        };
    }


    protected override void OnDestroy()
    {   
        base.OnDestroy();

        OnStatChange -= CheckSpellBar;

        if (PlayerData != null)
        {
            PlayerData.OnLevelUp -= LevelUp;

            PlayerData.OnStatChange -= OnStatChangeMage;
        }
    }

    public void Setup(PlayerMageData playerData)
    {
        PlayerData = playerData;

        if (playerData != null)
        {
            playerData.OnLevelUp += LevelUp;

            playerData.OnStatChange += OnStatChangeMage;

            RefreshSpell();
        }
    }

    public void RefreshSpell()
    {
        if (PlayerData.CurrentLearningSpell != UtilsMage.MageSpellType.None)
        {
            // get spell and assign prefab to ast
            _currentSpell = PlayerData.GetSpellByType(PlayerData.CurrentLearningSpell);
            _spellPrefab = _currentSpell.SpellSO.Prefab;

            // update learning points ui
            UpdateSpellBar();

            // update timers
            UpdateCooldownCast();
            _timerCast = _finalCooldownCast;

            _barCooldown.Setup(_finalCooldownCast, 0f);

            // update current spell sprite icon
            _imageLearningSpell.gameObject.SetActive(true);
            _imageLearningSpell.sprite = _currentSpell.SpellSO.Sprite;
        }
        else
        {
            _imageLearningSpell.gameObject.SetActive(false);
        }
    }

    protected override void Update()
    {
        base.Update();

        if (_spellPrefab == null) return;

        if(_timerCast <= 0)
        {
            CastSpell();

            UpdateCooldownCast();
            _timerCast = _finalCooldownCast;
        }
        else
        {
            _timerCast -= Time.deltaTime;
            UpdateCooldowBar();
        }
    }

    private void CastSpell()
    {
        // animator
        animator.SetTrigger("Attack");

        // add points and update player data
        int finalPointsToAdd = 1;

        // check if player has arcanist buff
        if (PlayerManager.Instance.PlayerBuffsData.HasBuff(UtilsBuffs.BuffType.Arcanist))
        {
            finalPointsToAdd *= 2;
        }
        _currentSpell.AddPoints(finalPointsToAdd);

        PlayerData.UpdateSpellData(_currentSpell);
        PlayerData.AddExp(UtilsMage.GetSpellCastExp(_currentSpell.SpellSO.SpellType));

        // uppdate bar ui
        barSpell.SetCurrentValue(_currentSpell.CurrentLearnPoints);

        // save
        SaveMageData();
    }

    public void ExternalAttack()
    {
        // cast spell
        SpawnSpell();
    }

    private void SpawnSpell()
    {
        GameObject spawned = Instantiate(_spellPrefab, transform.position, Quaternion.identity);
        if (spawned.TryGetComponent(out SpellMage spellMage))
        {
            if (spellMage.DoesMove)
            {
                spellMage.SetPositions(castPosition.position, new Vector2(dummyPosition.position.x, castPosition.position.y));
            }
            else
            {
                spellMage.SetPositions(new Vector2(dummyPosition.position.x, castPosition.position.y), new Vector2(dummyPosition.position.x, castPosition.position.y));
            }
            spellMage.Perform();
        }
    }

    private void UpdateCooldownCast()
    {
        _finalCooldownCast =
            baseCooldownCast -
            Mathf.FloorToInt(baseCooldownCast * PlayerManager.Instance.PlayerMageData.CurrentCastSpeed);
    }

    private void UpdateSpellBar()
    {
        barSpell.Setup(_currentSpell.RequiredPointsToNextRank, _currentSpell.CurrentLearnPoints);
    }

    private void UpdateCooldowBar()
    {
        _barCooldown.SetCurrentValue(_finalCooldownCast - _timerCast);
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

    public void SaveMageData()
    {
        PlayerManager.Instance.UpdateMageData(PlayerData);
        PlayerManager.Instance.SaveMageData();
    }

    #endregion

    #region HANDLE EVENTS FROM MAGE DATA

    protected override void LevelUp()
    {
        base.LevelUp();

        SaveMageData();
    }

    private void OnStatChangeMage(int id, int value)
    {
        OnStatChange?.Invoke(id, value);
    }

    private void CheckSpellBar(int id, int value)
    {
        if(id == UtilsPlayer.ID_MAGE_INSIGHT)
        {
            UpdateSpellBar();
        }
    }

    #endregion
}
