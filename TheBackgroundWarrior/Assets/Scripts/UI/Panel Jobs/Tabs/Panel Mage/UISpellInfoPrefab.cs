using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISpellInfoPrefab : MonoBehaviour
{
    [SerializeField] UITabJobMage tabMage;

    [Space(10)]
    [SerializeField] UtilsMage.MageSpellType _spellType;

    [Header("Icon")]
    [SerializeField] Image imageSpell;

    [Header("Name")]
    [SerializeField] TMP_Text textName;

    [Header("Rank")]
    [SerializeField] Sprite spriteRankUnlocked;
    [SerializeField] Image[] rankStars;

    [Space(10)]
    [SerializeField] GenericBar barLearn;

    [Header("Description")]
    [SerializeField] TMP_Text textDesc;

    [Header("Cooldowns")]
    [SerializeField] TMP_Text _textCooldowns;

    [Header("Buttons")]
    [SerializeField] Button buttonLearn;
    [SerializeField] Button buttonEquip;
    [SerializeField] TMP_Text textButtonEquip;


    private SpellData _spellData;

    private bool _isEquipped;

    public bool IsUnlocked { get; private set; }


    private ScrollRect _scroll;



    public void Refresh()
    {
        _scroll = tabMage.ScollSpells;

        _spellData = PlayerManager.Instance.PlayerMageData.GetSpellByType(_spellType);

        IsUnlocked = _spellData.IsUnlocked;

        // set sprite
        imageSpell.sprite = _spellData.SpellSO.Sprite;
        imageSpell.color = IsUnlocked ? Color.white : Color.black;

        // set name
        textName.text = IsUnlocked ? _spellData.SpellSO.SpellName : UtilsText.AllText[UtilsText.text_job_mage_spell_locked];

        // set rank images, by default they have locked sprite
        for (int i = 0; i < _spellData.CurrentRank; i++)
        {
            if(i < rankStars.Length)
            {
                rankStars[i].sprite = spriteRankUnlocked;
            }
        }

        // set bar points
        barLearn.Setup(_spellData.RequiredPointsToNextRank, _spellData.CurrentLearnPoints);

        // set desc
        textDesc.text = IsUnlocked ? UtilsMage.GetSpellDescription(_spellData) : UtilsText.AllText[UtilsText.text_job_mage_spell_locked];

        // set warrior cooldown text
        _textCooldowns.text = IsUnlocked 
            ? string.Format(UtilsText.AllText[UtilsText.text_job_mage_spell_warriorcooldown], UtilsGeneral.FormatDecimal(_spellData.CooldownCast)) 
            : UtilsText.AllText[UtilsText.text_job_mage_spell_locked];

        // set button interactable
        buttonLearn.interactable = IsUnlocked;

        buttonEquip.interactable = IsUnlocked;

        _isEquipped = PlayerManager.Instance.PlayerMageData.IsSpellEquipped(_spellData);
        textButtonEquip.text = _isEquipped ? UtilsText.AllText[UtilsText.text_button_unequip] : UtilsText.AllText[UtilsText.text_button_equip];
    }

    public void OnButtonLearn()
    {
        PlayerManager.Instance.PlayerMageData.SetLearningSpell(_spellType);

        tabMage.OnSpellSelected();
    }

    public void OnButtonEquip()
    {
        if (_isEquipped)
        {
            int idSlot = PlayerManager.Instance.PlayerMageData.GetEquippedSlot(_spellData);
            if(idSlot != -1)
            {
                PlayerManager.Instance.PlayerMageData.UnequipFromSlot(idSlot);
                tabMage.Open();
                PlayerManager.Instance.SaveMageData();
            }
        }
        else
        {
            int idSlot = PlayerManager.Instance.PlayerMageData.GetFirstEmptySlot();
            if (idSlot != -1)
            {
                PlayerManager.Instance.PlayerMageData.EquipToSlot(idSlot, _spellData);
                tabMage.Open();
                PlayerManager.Instance.SaveMageData();
            }
        }
    }



    public void OnBeginDrag(BaseEventData data)
    {
        if (_scroll != null)
        {
            PointerEventData pointerData = (PointerEventData)data;
            _scroll.OnBeginDrag(pointerData);
        }
    }

    public void OnDrag(BaseEventData data)
    {
        if (_scroll != null)
        {
            PointerEventData pointerData = (PointerEventData)data;
            _scroll.OnDrag(pointerData);
        }
    }

    public void OnEndDrag(BaseEventData data)
    {
        if (_scroll != null)
        {
            PointerEventData pointerData = (PointerEventData)data;
            _scroll.OnEndDrag(pointerData);
        }
    }
}
