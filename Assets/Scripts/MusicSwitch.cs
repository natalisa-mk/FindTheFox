using UnityEngine;
using UnityEngine.UI;

public class MusicSwitch : MonoBehaviour
{
    private Toggle _musicToggle;

    private void Start()
    {
        _musicToggle = GetComponent<Toggle>();

        if(PlayerPrefs.GetInt("Music") == 1)
        {
            _musicToggle.isOn = true;
        }
        else
        {
            _musicToggle.isOn = false;
        }

        _musicToggle.onValueChanged.AddListener(SwitchMusic);

    }

    private void SwitchMusic(bool isOn)
    {
        if(!isOn)
        {
            AudioManager.Instance.StopSound("Music");
            PlayerPrefs.SetInt("Music", 0);
        }
        else
        {
            AudioManager.Instance.PlaySound("Music");
            PlayerPrefs.SetInt("Music", 1);
        }
        
    }
}
