using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBard : Player
{
    [Header("Movement")]
    [SerializeField] Animator _animator;
    [SerializeField] ParticleSystem _notesVFX;

    private bool _isSongPlaying;
    private float _timer1Sec;

    private float _counterSong;
    private float _currentSongLength;

    public event Action OnChangeMusic;


    public PlayerBardData PlayerData { get; private set; }


    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (_isSongPlaying)
        {
            // add timer before closing
            PlayerData.AddPlayedTime(_counterSong);

            // add timer to buff on closing
            Buff inspirationBuff = new Buff(UtilsBuffs.BuffType.Inspiration, _counterSong);
            PlayerManager.Instance.PlayerBuffsData.AddBuff(inspirationBuff);
        }
    }

    public void Setup(PlayerBardData playerData)
    {
        PlayerData = playerData;

        _counterSong = 0;
        _currentSongLength = 0;

        StartMusic();
    }

    protected override void Update()
    {
        base.Update();

        if (_isSongPlaying)
        {
            if(_timer1Sec <= 0)
            {
                _timer1Sec = UtilsGeneral.TIMER_1SECONDS;

                _counterSong += 1f;
            }
            else
            {
                _timer1Sec -= Time.unscaledDeltaTime;
            }
        }
    }

    public void StartMusic()
    {
        // plays first music
        string firstSong = AudioManager.Instance.GetCurrentMusicName();
        StartCoroutine(CoDelayMusic(2f, firstSong));
    }

    private IEnumerator CoDelayMusic(float timer, string songName)
    {
        // add timer before changing song
        PlayerData.AddPlayedTime(_counterSong);

        // add timer to buff after each song
        Buff inspirationBuff = new Buff(UtilsBuffs.BuffType.Inspiration, _counterSong);

        PlayerManager.Instance.PlayerBuffsData.AddBuff(inspirationBuff);

        yield return new WaitForSecondsRealtime(timer);

        AudioManager.Instance.PlayMusic(songName);
        _currentSongLength = AudioManager.Instance.GetMusicLength(songName);

        OnChangeMusic?.Invoke();

        // reset counter
        _counterSong = 0;

        // set timer for counting
        _timer1Sec = UtilsGeneral.TIMER_1SECONDS;

        // restore counter when playing music
        _isSongPlaying = true;

        StartCoroutine(CoWaitForEndSong(_currentSongLength));
    }

    private IEnumerator CoWaitForEndSong(float timer)
    {
        yield return new WaitForSecondsRealtime(timer);

        NextSong();
    }

    public void ForceNextSong()
    {
        StopMusic();

        NextSong(0.5f);
    }

    private void NextSong(float delay = 3f)
    {
        // stop timer to add time when music is stopped
        _isSongPlaying = false;

        string nextSong = string.Empty;

        if (PlayerData.IsSingleLoop)
        {
            nextSong = AudioManager.Instance.GetCurrentMusicName();
        }
        else
        {
            if (PlayerData.IsRandom)
            {
                nextSong = AudioManager.Instance.GetRandomMusicName();
            }
            else
            {
                nextSong = AudioManager.Instance.GetNextOrderMusicName();
            }
        }

        StartCoroutine(CoDelayMusic(delay, nextSong));
    }

    public void ForcePlay(string songName)
    {
        StopMusic();

        StartCoroutine(CoDelayMusic(1.5f, songName));
    }

    public void PauseMusic()
    {
        AudioManager.Instance.PauseMusic();

        StopAllCoroutines();

        _isSongPlaying = false;

        //_notesVFX.Stop();
    }

    public void StopMusic()
    {
        AudioManager.Instance.StopMusic();

        StopAllCoroutines();

        _isSongPlaying = false;
    }

    public void ResumeMusic()
    {
        AudioManager.Instance.ResumeMusic();

        // restore counter when playing music
        _isSongPlaying = true;

        //_notesVFX.Play();

        // calc difference btw already listened song
        StartCoroutine(CoWaitForEndSong(_currentSongLength - _counterSong));
    }

    #region SAVE

    public void SaveBardData()
    {
        PlayerManager.Instance.UpdateBardData(PlayerData);
        PlayerManager.Instance.SaveBardData();
    }

    #endregion
}
