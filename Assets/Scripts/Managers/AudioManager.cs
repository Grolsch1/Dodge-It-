using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;

        [Range(0f, 1f)] public float volume;
        [Range(0.1f, 3f)] public float pitch;

        public bool loop;

        [HideInInspector] public AudioSource source;
    }

    public Sound[] sounds;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    private Dictionary<string, Sound> soundDictionary;

    private void Awake()
    {
        instance = this;

        soundDictionary = new Dictionary<string, Sound>();

        foreach (Sound s in sounds)
        {
            soundDictionary.Add(s.name, s);

            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void PlaySFX(string name)
    {
        if (soundDictionary.TryGetValue(name, out Sound s))
        {
            s.source.pitch = Random.Range(0.6f, 1.1f);
            s.source.PlayOneShot(s.clip);
        }
        else
        {
            Debug.LogWarning("Sound not found: " + name);
        }
    }

    public void PlayMusic(string name)
    {
        if (soundDictionary.TryGetValue(name, out Sound s))
        {
            musicSource.clip = s.clip;
            musicSource.volume = s.volume;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}