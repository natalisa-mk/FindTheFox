using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OpenLevel : MonoBehaviour
{
    private Button _nextLvlButton;
    private void Start()
    {
        _nextLvlButton = GetComponent<Button>();
        _nextLvlButton.onClick.AddListener(TurnOnNextLevel);
    }

    private void TurnOnNextLevel()
    {
        AudioManager.Instance.PlaySound("PressButton");
        SceneManager.LoadScene("Game");
    }
}
