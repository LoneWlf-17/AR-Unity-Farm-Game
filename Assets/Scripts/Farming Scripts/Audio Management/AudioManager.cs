using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Audio[] audios;

    public static AudioManager instance {  get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(this);
            return;
        }

        for (int i = 0; i < audios.Length; i++)
        {
            audios[i].source = gameObject.AddComponent<AudioSource>();
            audios[i].source.clip = audios[i].clip;
            audios[i].source.volume = audios[i].volume;
            audios[i].source.pitch = audios[i].pitch;
            audios[i].source.loop = audios[i].loop;
        }
    }

    private void Start()
    {
        instance.play("ThemeSong");
    }

    public void play(string clipName)
    {
        Audio audio = Array.Find(audios, a => a.audioName == clipName);

        if (audio != null)
        {
            audio.source.Play();
        }
        else
        {
            Debug.LogWarning("Sound " + clipName + "not found");
        }
    }
}
