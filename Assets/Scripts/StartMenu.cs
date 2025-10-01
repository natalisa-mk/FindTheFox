using Context;
using Level;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    
    private IAudioManager _audioManager;

    private void Start()
    {
        playButton.onClick.AddListener(LoadGameScene);
        
        _audioManager = ProjectContext.Services.Get<IAudioManager>();
        _audioManager.PlayMusic(TypeOfSound.FantasyMusic);
    }

    private void LoadGameScene()
    {
        _audioManager.PlaySound(TypeOfSound.PressButton);

        ILevelStarter levelStarter = ProjectContext.Services.Get<CoreLevelStarter>();
        levelStarter.StartLevel(1); //Заглушка, яка запускає level
    }

   
}
