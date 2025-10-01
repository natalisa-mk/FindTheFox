using Context;
using UnityEngine;
using UnityEngine.UI;

namespace Audio
{
    [RequireComponent(typeof(Button))]
    public class ButtonSound : MonoBehaviour
    {
        [SerializeField] private TypeOfSound soundType;
    
        private Button _button;
        private IAudioManager _audioManager;
    
        private void Start()
        {
            _audioManager = ProjectContext.Services.Get<IAudioManager>();
        
            _button = GetComponent<Button>();
            _button.onClick.AddListener(PlaySound);
        }

        private void PlaySound()
        {
            _audioManager.PlaySound(soundType);
        }
    }
}
