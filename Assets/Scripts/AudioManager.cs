using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Sound
{
    public string Name;
    public AudioClip Clip;

    [Range(0f, 1f)]
    public float Volume = 0.7f;
    [Range(0.5f, 1.5f)]
    public float Pitch = 1f;

    [Range(0f, 0.5f)]
    public float RandomVolume = 0.1f;
    [Range(0f, 0.5f)]
    public float RandomPitch = 0.1f;

    public bool Loop = false;

    private AudioSource source;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = Clip;
        source.loop = Loop;
    }

    public void Play()
    {
        source.volume = Volume * (1 + Random.Range(-RandomVolume / 2f, RandomVolume / 2f));
        source.pitch = Pitch * (1 + Random.Range(-RandomPitch / 2f, RandomPitch / 2f));
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField]
    Sound[] sounds;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].Name);
            _go.transform.SetParent(this.transform);
            sounds[i].SetSource(_go.AddComponent<AudioSource>());
        }

        if (PlayerPrefs.HasKey("Music"))
        {
            if (PlayerPrefs.GetInt("Music") == 1)
            {
                PlaySound("Music");
            }
        }
        else
        {
            PlaySound("Music");
            PlayerPrefs.SetInt("Music", 1);
        }
        
    }

    public void PlaySound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].Name == _name)
            {
                sounds[i].Play();
                return;
            }
        }

        //No sound with _name.
        Debug.LogWarning("AudioManager: Sound not found in list: " + _name);
    }

    public void StopSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].Name == _name)
            {
                sounds[i].Stop();
                return;
            }
        }

        //No sound with _name.
        Debug.LogWarning("AudioManager: Sound not found in list: " + _name);
    }
}
