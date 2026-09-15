using UnityEngine;
using UnityEngine.UI;

public class UITabJobBard : UITabWindow
{
    [Space(10)]
    [SerializeField] UITabPlayerJob panelJob;

    [Space(10)]
    [SerializeField] UISongPrefab[] _songPrefabs;

    private UISongPrefab _currentSongPrefab;

    [Space(10)]
    [SerializeField] Color _selectedColor;
    [SerializeField] GameObject _singleIcon;
    [SerializeField] Image _iconRandomButton;

    private bool _isRandom;
    private bool _isSingleLoop;



    public PlayerBard Player { get; private set; }

    private void OnDestroy()
    {
        if (Player != null)
        {
            Player.OnChangeMusic -= OnChangeMusic;
        }
    }

    public override void Open()
    {
        base.Open();

        if (Player == null)
        {
            Player = FindFirstObjectByType<PlayerBard>();

            if(Player != null)
                Player.OnChangeMusic += OnChangeMusic;
        }

        OnChangeMusic();

        PlayerBardData data = PlayerManager.Instance.PlayerBardData;

        // get data from save
        _isRandom = data.IsRandom;
        _isSingleLoop = data.IsSingleLoop;

        // set ui
        _singleIcon.SetActive(_isSingleLoop);
        _iconRandomButton.color = _isRandom ? _selectedColor : Color.white;

        panelJob.ChangeCurrentTab(this, UtilsPlayer.PlayerJob.Bard);
    }

    private void OnChangeMusic()
    {
        if (Player != null)
        {
            _currentSongPrefab = _songPrefabs[AudioManager.Instance.CurrentPlayingSongIndex];

            foreach (var song in _songPrefabs)
            {
                song.ResetSelected();
            }

            _currentSongPrefab.SetSelected();
        }
    }

    public void OnButtonBack()
    {
        AudioManager.Instance.PlayClickUI();

        Close();
        panelJob.ChangeCurrentTab(null, UtilsPlayer.PlayerJob.None);
    }

    public void OnButtonLoop()
    {
        _isSingleLoop = !_isSingleLoop;

        PlayerBardData data = PlayerManager.Instance.PlayerBardData;
        data.SetIsSingleLoop(_isSingleLoop);

        _singleIcon.SetActive(_isSingleLoop);
    }

    public void OnButtonRandom()
    {
        _isRandom = !_isRandom;

        PlayerBardData data = PlayerManager.Instance.PlayerBardData;
        data.SetIsRandom(_isRandom);

        _iconRandomButton.color = _isRandom ? _selectedColor : Color.white;
    }

    public void OnButtonNext()
    {
        if (Player == null) return;

        Player.ForceNextSong();
    }

    public void ChangeSelected(UISongPrefab next)
    {
        if(_currentSongPrefab != null)
        {
            _currentSongPrefab.ResetSelected();
        }

        _currentSongPrefab = next;

        if (_currentSongPrefab != null)
        {
            _currentSongPrefab.SetSelected();
        }
    }


    public void OnButtonPlay()
    {
        if (Player != null) return;
        
        panelJob.OnButtonClose(false);

        LastSceneSettings settings = new LastSceneSettings();
        settings.lastSceneName = "BardScene";
        settings.lastSceneType = SceneLoaderManager.SceneType.Bard;

        SceneLoaderManager.Instance.LoadScene(settings);
    }
}
