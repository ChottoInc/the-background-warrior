using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] Sound[] effects;

    [Space(10)]
    [SerializeField] AudioMixer mixer;

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
    }
    
    public void PlayEffect(string name)
    {
        Sound s = Array.Find(effects, sound => sound.name == name);

        if (s == null)
            return;
        
        s.source.Play();
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
