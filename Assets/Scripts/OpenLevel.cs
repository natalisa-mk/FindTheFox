using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OpenLevel : MonoBehaviour
{
    private Button nextLvlButton;
    private void Start()
    {
        nextLvlButton = GetComponent<Button>();
        nextLvlButton.onClick.AddListener(TurnOnNextLevel);
    }

    private void TurnOnNextLevel()
    {
        AudioManager.Instance.PlaySound("PressButton");
        SceneManager.LoadScene("Game");
    }
}
