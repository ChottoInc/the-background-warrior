using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] Sound[] effects;
    [SerializeField] Sound[] musics;

    [Space(10)]
    [SerializeField] AudioMixer mixer;

    public int CurrentPlayingSongIndex { get; private set; }

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (var sound in effects)
        {
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.outputAudioMixerGroup = sound.group;
        }

        foreach (var sound in musics)
        {
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.outputAudioMixerGroup = sound.group;
        }
    }

    public void PlayMusic(string name)
    {
        int index = Array.FindIndex(musics, sound => sound.name == name);
        Sound s = musics[index];

        if (s == null)
            return;

        CurrentPlayingSongIndex = index;
        s.source.Play();
    }

    public string GetRandomMusicName()
    {
        int randIndex = UnityEngine.Random.Range(0, musics.Length);
        return musics[randIndex].name;
    }

    public string GetNextOrderMusicName()
    {
        int nextIndex = CurrentPlayingSongIndex + 1;

        if (nextIndex < musics.Length) return musics[nextIndex].name;
        else return musics[0].name;
    }

    public string GetCurrentMusicName()
    {
        return musics[CurrentPlayingSongIndex].name;
    }

    public float GetMusicLength(string name)
    {
        Sound s = Array.Find(musics, sound => sound.name == name);

        if (s == null)
            return -1f;

        return s.source.clip.length;
    }

    public void PauseMusic()
    {
        Sound s = musics[CurrentPlayingSongIndex];

        if (s == null)
            return;

        s.source.Pause();
    }

    public void StopMusic()
    {
        Sound s = musics[CurrentPlayingSongIndex];

        if (s == null)
            return;

        s.source.Stop();
    }

    public void ResumeMusic()
    {
        Sound s = musics[CurrentPlayingSongIndex];

        if (s == null)
            return;

        s.source.Play();
    }

    public void PlayEffect(string name)
    {
        Sound s = Array.Find(effects, sound => sound.name == name);

        if (s == null)
            return;
        
        s.source.Play();
    }

    public void PlayEffectCheckPlaying(string name)
    {
        Sound s = Array.Find(effects, sound => sound.name == name);

        if (s == null)
            return;

        // if the sound is already playing, don't play
        if (s.source.isPlaying) return;

        s.source.Play();
    }

    public void PauseEffect(string name)
    {
        Sound s = Array.Find(effects, sound => sound.name == name);

        if (s == null)
            return;

        // if the sound is already playing, don't play
        if (!s.source.isPlaying) return;

        s.source.Pause();
    }

    public void StopEffect(string name)
    {
        Sound s = Array.Find(effects, sound => sound.name == name);

        if (s == null)
            return;

        s.source.Stop();
    }

    public void PlayClickUI(bool randPitch = true)
    {
        string name = "Click";

        Sound s = Array.Find(effects, sound => sound.name == name);

        if (s == null)
            return;

        if (randPitch)
        {
            float randPitchValue = UnityEngine.Random.Range(1f, 2f);
            s.source.pitch = randPitchValue;
        }
        
        s.source.Play();
    }

    public void SetMasterVolume(float volume)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }
}
