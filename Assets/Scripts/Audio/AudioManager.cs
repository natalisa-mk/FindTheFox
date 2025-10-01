using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;


[Serializable]
public class Sound
{
    public TypeOfSound type;
    public AudioClip[] audioClips;

    [Range (0f, 1f)]
    public float volume = 0.7f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;

    [Range(0f, 0.5f)]
    public float randomVolume = 0.1f;
    [Range(0f, 0.5f)]
    public float randomPitch = 0.1f;

    public bool loop = false;

    [HideInInspector] public AudioSource source;

    public void SetSource(AudioSource audioSource)
    {
        source = audioSource;
        source.loop = loop;
    }

    public void Play()
    {
        source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
        source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }
}

public class AudioManager : MonoBehaviour, IAudioManager
{
    [SerializeField] private AudioMixerGroup soundMixer;
    [SerializeField] private AudioMixerGroup musicMixer;
    [SerializeField] private Sound[] soundsList;
    [SerializeField] private Sound[] musicList;

    private static AudioSource _musicSource;
    private static TypeOfSound _curMusic;
    
    private void Awake()
    {
        for (var i = 0; i < soundsList.Length; i++)
        {
            var go = new GameObject("Sound_" + i + "_" + soundsList[i].type);
            go.transform.SetParent(transform);
            
            var source = go.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = soundMixer;
            soundsList[i].SetSource(source);
        }
        
        _musicSource = gameObject.AddComponent<AudioSource>();
        _musicSource.outputAudioMixerGroup = musicMixer;
    }

    private void Start()
    {
        AudioSettings.OnSoundVolumeChange += ChangeSoundsVolume;
        AudioSettings.OnMusicVolumeChange += ChangeMusicVolume;
        ChangeSoundsVolume();
        ChangeMusicVolume();
    }

    public void PlaySound(TypeOfSound soundType)
    {
        for (var i = 0; i < soundsList.Length; i++)
        {
            if (soundsList[i].type == soundType)
            {
                if(soundsList[i].source.isPlaying) return;
                
                soundsList[i].source.clip = soundsList[i].audioClips[Random.Range(0, soundsList[i].audioClips.Length)];
                soundsList[i].Play();
                return;
            }
        }
        
        Debug.LogWarning("AudioManager: Sound not found in list: " + soundType);
    }
    
    public void StopSound(TypeOfSound soundType)
    {
        for (int i = 0; i < soundsList.Length; i++)
        {
            if (soundsList[i].type == soundType)
            {
                soundsList[i].Stop();
                return;
            }
        }
        
        Debug.LogWarning("AudioManager: Sound not found in list: " + soundType);
    }

    public void PlayMusic(TypeOfSound musicType)
    {
        if (_curMusic == musicType) return;
        _curMusic = musicType;
        
        for (var i = 0; i < musicList.Length; i++)
        {
            if (musicList[i].type == musicType) {
                _musicSource.DOFade(0, 1f).SetUpdate(true).OnComplete(() => {
                    musicList[i].SetSource(_musicSource);
                    _musicSource.clip = musicList[i].audioClips[Random.Range(0, musicList[i].audioClips.Length)];
                    musicList[i].Play();
                    _musicSource.DOFade(0, 1f).From().SetUpdate(true);
                });
                return;
            }
        }
    }
    public void StopMusic()
    {
        _musicSource.DOFade(0, 0.3f);
        _curMusic = default;
    }
    
    private void ChangeSoundsVolume()
    {
        soundMixer.audioMixer.SetFloat(soundMixer.name, Mathf.Log10(Mathf.Clamp(AudioSettings.SoundsVolume, 0.0001f, 1)) * 20);
    }
    
    private void ChangeMusicVolume()
    {
        musicMixer.audioMixer.SetFloat(musicMixer.name, Mathf.Log10(Mathf.Clamp(AudioSettings.MusicVolume, 0.0001f, 1)) * 20);
    }
}

public enum TypeOfSound
{
    TargetFound,
    CellClick,
    PressButton,
    Win,
    FantasyMusic,
    OpenPanel,
    ClosePanel
}