using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] int maxMultipleSound = 5;

    private SoundData _soundData;
    private Dictionary<string, float> volume = new Dictionary<string, float>();
    private Dictionary<string, AudioClip> clip = new Dictionary<string, AudioClip>();

    private List<AudioSource> audioSources = new List<AudioSource>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _soundData = Resources.Load<SoundData>("SoundData");
    }

    private void Start()
    {
        Initiate();
    }

    private void Initiate()
    {
        foreach (SoundClip soundClip in _soundData.SoundClips)
        {
            if (!volume.ContainsKey(soundClip.Name))
                volume.Add(soundClip.Name, soundClip.Volume);

            if (!clip.ContainsKey(soundClip.Name))
                clip.Add(soundClip.Name, soundClip.Clip);
        }
        for (int i = 0; i < maxMultipleSound; i++)
        {
            CreateNewAudioSource();
        }
    }

    private AudioSource CreateNewAudioSource()
    {
        GameObject go = new GameObject($"AudioSource {audioSources.Count}");
        go.transform.parent = transform;
        AudioSource source = go.AddComponent<AudioSource>();
        audioSources.Add(source);
        return source;
    }

    public void Play(string name)
    {
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                SetupAndPlay(source, name, false);
                return;
            }
        }

        SetupAndPlay(CreateNewAudioSource(), name, false);
    }

    public void PlayLoop(string name)
    {
        foreach (AudioSource source in audioSources)
        {
            if (source.isPlaying && source.loop && source.clip == clip[name])
                return;
        }

        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                SetupAndPlay(source, name, true);
                return;
            }
        }

        SetupAndPlay(CreateNewAudioSource(), name, true);
    }

    public void StopLoop(string name)
    {
        foreach (AudioSource source in audioSources)
        {
            if (source.isPlaying && source.clip == clip[name] && source.loop)
            {
                source.Stop();
                source.loop = false;
            }
        }
    }

    private void SetupAndPlay(AudioSource source, string name, bool loop)
    {
        source.clip = clip[name];
        source.volume = volume[name];
        source.loop = loop;
        source.Play();
    }

    public void PlayOnIncrease(string name, float coefficient)
    {
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                source.clip = clip[name];
                source.volume = 0f;
                source.loop = false;
                StartCoroutine(IncreaseVolume(source, coefficient, volume[name]));
                return;
            }
        }

        AudioSource newSource = CreateNewAudioSource();
        newSource.clip = clip[name];
        newSource.volume = 0f;
        StartCoroutine(IncreaseVolume(newSource, coefficient, volume[name]));
    }

    public void PlayOnDecrease(string name, float coefficient)
    {
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                source.clip = clip[name];
                source.volume = volume[name];
                StartCoroutine(DecreaseVolume(source, coefficient));
                return;
            }
        }

        AudioSource newSource = CreateNewAudioSource();
        newSource.clip = clip[name];
        newSource.volume = volume[name];
        StartCoroutine(DecreaseVolume(newSource, coefficient));
    }

    private IEnumerator IncreaseVolume(AudioSource audioSource, float coefficient, float targetVolume)
    {
        audioSource.Play();
        float currentVolume = 0f;
        while (currentVolume < targetVolume)
        {
            currentVolume += Time.deltaTime * coefficient;
            audioSource.volume = Mathf.Clamp(currentVolume, 0f, targetVolume);
            yield return null;
        }
        audioSource.volume = targetVolume;
    }

    private IEnumerator DecreaseVolume(AudioSource audioSource, float coefficient)
    {
        audioSource.Play();
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0f)
        {
            audioSource.volume -= Time.deltaTime * coefficient;
            yield return null;
        }
        audioSource.volume = 0f;
    }
}
