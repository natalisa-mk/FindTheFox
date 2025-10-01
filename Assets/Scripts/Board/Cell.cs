using System;
using Context;
using UnityEngine;
using UnityEngine.UI;

namespace Board
{
    public class Cell : MonoBehaviour
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        [SerializeField] private Text targetsCountText;
        [SerializeField] private Image targetImage;
    
        private Button _button;
        private Action<Cell> _onClicked;
        
        private IAudioManager _audioManager;

        public void Init(int x, int y, Action<Cell> onClicked)
        {
            X = x;
            Y = y;
        
            _onClicked = onClicked;
        }
    
        private void Start()
        {
            _audioManager = ProjectContext.Services.Get<IAudioManager>();
            
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClicked);
        }

        private void OnClicked()
        {
            _onClicked?.Invoke(this);
            _button.onClick.RemoveAllListeners();
        }

        public void DisplayFoundTarget()
        {
            _audioManager.PlaySound(TypeOfSound.TargetFound);
            targetImage.gameObject.SetActive(true);

            _button.enabled = false;
        }
        
        public void DisplayHiddenTargets(int targetsCount)
        {
            _audioManager.PlaySound(TypeOfSound.CellClick);
            targetsCountText.text = targetsCount.ToString();
            
            _button.enabled = false;
        }
    }
}
