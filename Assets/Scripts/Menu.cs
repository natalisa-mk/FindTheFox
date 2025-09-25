using Context;
using Level;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button _playButton;

    private void Start()
    {
        _playButton.onClick.AddListener(LoadGameScene);
    }

    private void LoadGameScene()
    {
        AudioManager.Instance.PlaySound("PressButton");

        ILevelStarter levelStarter = ProjectContext.Services.Get<CoreLevelStarter>();
        levelStarter.StartLevel(1); //Заглушка, яка запускає level
    }

   
}
