using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public string tag;

    public AudioClip clip;
    public AudioMixerGroup group;

    [Range(0f,1f)] public float volume = 0.5f;
    [Range(0f, 10f)] public float pitch = 1f;

    public bool loop;

    [HideInInspector] public AudioSource source;

}
