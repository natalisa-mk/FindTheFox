using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuitGame : MonoBehaviour
{
    private Button quitButton;

    private void Start()
    {
        quitButton = GetComponent<Button>();
        quitButton.onClick.AddListener(GameQuit);
    }

    private void GameQuit()
    {
        AudioManager.Instance.PlaySound("PressButton");
        Application.Quit();
    }
        
}
