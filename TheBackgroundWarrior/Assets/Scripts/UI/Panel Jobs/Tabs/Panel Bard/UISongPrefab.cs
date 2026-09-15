using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISongPrefab : MonoBehaviour
{
    [SerializeField] UITabJobBard _tabBard;

    [Space(10)]
    [SerializeField] string _songName;

    [Space(10)]
    [SerializeField] TMP_Text _textTitle;

    [Space(10)]
    [SerializeField] Image _imagePlay;
    [SerializeField] Image _imagePause;
    [SerializeField] Color _selectedColor;

    private bool _isStopped;

    private void Start()
    {
        _textTitle.text = _songName;
    }

    public void OnButtonPlay()
    {
        if (_tabBard.Player == null) return;

        if (AudioManager.Instance.GetCurrentMusicName() == _songName && _isStopped)
        {
            // tell player to resume
            _isStopped = false;

            _tabBard.Player.ResumeMusic();

            _imagePlay.color = _selectedColor;
            _imagePause.color = Color.white;
        }
        else if (AudioManager.Instance.GetCurrentMusicName() != _songName)
        {
            // force play
            _tabBard.Player.ForcePlay(_songName);

            _imagePlay.color = _selectedColor;
            _imagePause.color = Color.white;

            _tabBard.ChangeSelected(this);
        }
    }

    public void OnButtonPause()
    {
        if (_tabBard.Player == null) return;

        if (_isStopped) return;

        if(AudioManager.Instance.GetCurrentMusicName() == _songName)
        {
            // tell player to stop music
            _isStopped = true;

            _tabBard.Player.PauseMusic();

            _imagePlay.color = Color.white;
            _imagePause.color = _selectedColor;
        }
    }

    public void ResetSelected()
    {
        _imagePlay.color = Color.white;
        _imagePause.color = Color.white;
    }

    public void SetSelected()
    {
        _imagePlay.color = _selectedColor;
        _imagePause.color = Color.white;
    }
}
