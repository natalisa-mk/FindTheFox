using System;
using Context;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public static event Action OnSoundVolumeChange;
    public static event Action OnMusicVolumeChange;
    
    public static float SoundsVolume {
        get => 1 - PlayerPrefs.GetFloat("SoundVolume");
        private set => PlayerPrefs.SetFloat("SoundVolume", 1 - value);
    }
    
    
    public static float MusicVolume {
        get => 1 - PlayerPrefs.GetFloat("MusicVolume");
        private set => PlayerPrefs.SetFloat("MusicVolume", 1 - value);
    }
    
    [SerializeField] private Toggle soundToggle;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Slider musicSlider;
    
    private IAudioManager _audioManager;
    private bool _ignoreEvents;
    
    private void Awake()
    {
        _audioManager = ProjectContext.Services.Get<IAudioManager>();
        
        soundSlider.value = SoundsVolume;
        musicSlider.value = MusicVolume;
        
        soundToggle.isOn = SoundsVolume > 0;
        musicToggle.isOn = MusicVolume > 0;
        
        soundToggle.graphic.enabled = true;
        musicToggle.graphic.enabled = true;
        
        soundSlider.onValueChanged.AddListener(ControlSoundsVolume);
        musicSlider.onValueChanged.AddListener(ControlMusicVolume);
        
        soundToggle.onValueChanged.AddListener(SwitchToggleSounds);
        musicToggle.onValueChanged.AddListener(SwitchToggleMusic);
    }

    private void OnEnable()
    {
        _audioManager.PlaySound(TypeOfSound.OpenPanel);
    }

    private void OnDisable()
    {
        _audioManager.PlaySound(TypeOfSound.ClosePanel);
    }

    private void SwitchToggleSounds(bool isOn)
    {
        if (_ignoreEvents) return;
        
        _audioManager.PlaySound(TypeOfSound.PressButton);
        
        if(!isOn)
        {
            soundSlider.value = 0;
            soundToggle.graphic.enabled = false;
        }
        else
        {
            soundSlider.value = 1;
            soundToggle.graphic.enabled = true;
        }
    }

    private void ControlSoundsVolume(float soundValue)
    {
        SoundsVolume = soundValue;
        
        _ignoreEvents = true;
        soundToggle.isOn = SoundsVolume > 0;
        soundToggle.graphic.enabled = true;
        _ignoreEvents = false;
        
        OnSoundVolumeChange?.Invoke();
    }
    
    private void SwitchToggleMusic(bool isOn)
    {
        if (_ignoreEvents) return;
        
        _audioManager.PlaySound(TypeOfSound.PressButton);
        
        if(!isOn) {
            musicSlider.value = 0;
            musicToggle.graphic.enabled = true;
        }
        else
        {
            musicSlider.value = 1;
            musicToggle.graphic.enabled = true;
        }
    }
    
    private void ControlMusicVolume(float musicValue)
    {
        MusicVolume = musicValue;
        
        _ignoreEvents = true;
        musicToggle.isOn = MusicVolume > 0;
        musicToggle.graphic.enabled = true;
        _ignoreEvents = false;
        
        OnMusicVolumeChange?.Invoke();
    }
}
