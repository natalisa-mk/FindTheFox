using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSwitch : MonoBehaviour
{
    private Toggle musicToggle;

    private void Start()
    {
        musicToggle = GetComponent<Toggle>();

        if(PlayerPrefs.GetInt("Music") == 1)
        {
            musicToggle.isOn = true;
        }
        else
        {
            musicToggle.isOn = false;
        }

        musicToggle.onValueChanged.AddListener(SwitchMusic);

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
