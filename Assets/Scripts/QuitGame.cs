using UnityEngine;
using UnityEngine.UI;


public class QuitGame : MonoBehaviour
{
    private Button _quitButton;

    private void Start()
    {
        _quitButton = GetComponent<Button>();
        _quitButton.onClick.AddListener(GameQuit);
    }

    private void GameQuit()
    {
        AudioManager.Instance.PlaySound("PressButton");
        Application.Quit();
    }
        
}
