using UnityEngine;
using UnityEngine.UI;

public class UIPanelBaitFisher : MonoBehaviour
{
    [SerializeField] GameObject _content;
    //[SerializeField] Image _imageBait;
    [SerializeField] Image _imageDayMoment;

    [Header("Sprites")]
    [SerializeField] Sprite _spriteMorning;
    [SerializeField] Sprite _spriteAfternoon;
    [SerializeField] Sprite _spriteNight;

    private bool _isInit;

    private PlayerFisher _playerFisher;

    private void Awake()
    {
        if (_playerFisher == null) 
            _playerFisher = FindFirstObjectByType<PlayerFisher>();
    }

    private void OnDestroy()
    {
        if(_playerFisher != null)
            _playerFisher.OnBaitChange -= Refresh;
    }

    private void Update()
    {
        if(!_isInit && _playerFisher != null && _playerFisher.PlayerData != null)
        {
            _isInit = true;

            _playerFisher.OnBaitChange += Refresh;

            Refresh();
        }
    }

    private void Refresh()
    {
        _content.SetActive(!(_playerFisher.PlayerData.ActiveBait == null));
       

        if (_playerFisher.PlayerData.ActiveBait != null)
        {
            //Debug.Log("active bait");
            _imageDayMoment.sprite = GetMomentIcon(_playerFisher.PlayerData.ActiveBait.AttractsMoment);
        }
    }

    private Sprite GetMomentIcon(UtilsGeneral.DayMoment moment)
    {
        switch(moment)
        {
            default: return null;
            case UtilsGeneral.DayMoment.Morning: return _spriteMorning;
            case UtilsGeneral.DayMoment.Afternoon: return _spriteAfternoon;
            case UtilsGeneral.DayMoment.Night: return _spriteNight;
        }
    }
}
