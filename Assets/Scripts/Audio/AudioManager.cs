using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    private void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    private void Start()
    {
        GameEvents.current.OnPlaySFX += Play;
    }

    private void Play (string name)
    {
        Sound s = Array.Find(sounds, sounds => sounds.name == name);

        if (s == null)
        {
            Debug.LogWarning($"Sound {name} not found");
        }
        s.source.Play();
    }

    private void OnDestroy()
    {
        GameEvents.current.OnPlaySFX -= Play;
    }
}