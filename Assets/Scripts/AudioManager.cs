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

    public bool Loop;

    private AudioSource _source;

    public void SetSource(AudioSource audioSource)
    {
        _source = audioSource;
        _source.clip = Clip;
        _source.loop = Loop;
    }

    public void Play()
    {
        _source.volume = Volume * (1 + Random.Range(-RandomVolume / 2f, RandomVolume / 2f));
        _source.pitch = Pitch * (1 + Random.Range(-RandomPitch / 2f, RandomPitch / 2f));
        _source.Play();
    }

    public void Stop()
    {
        _source.Stop();
    }
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private Sound[] sounds;

    private void Awake()
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

    private void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            var go = new GameObject("Sound_" + i + "_" + sounds[i].Name);
            go.transform.SetParent(this.transform);
            sounds[i].SetSource(go.AddComponent<AudioSource>());
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

    public void PlaySound(string soundName)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].Name == soundName)
            {
                sounds[i].Play();
                return;
            }
        }

        //No sound with soundName.
        Debug.LogWarning("AudioManager: Sound not found in list: " + soundName);
    }

    public void StopSound(string soundName)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].Name == soundName)
            {
                sounds[i].Stop();
                return;
            }
        }

        //No sound with _name.
        Debug.LogWarning("AudioManager: Sound not found in list: " + soundName);
    }
}
