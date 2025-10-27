using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public AudioSource source;

    [Range(0f, 1f)]
    public float volume;
    [Range(0f, 1f)]
    public float pitch;
}