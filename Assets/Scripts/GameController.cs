using UnityEngine;
using DG.Tweening;
using Level;
using Context;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Transform cellsHolder;
    [SerializeField] private Transform winPanel;
    [SerializeField] private TextMeshProUGUI winPanelLevelText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI targetCountText;
    
    private LevelData _levelData;
    private IAudioManager _audioManager;

    private const float ShowAnimDuration = 1f;
    private const float WinDelay = 5f;
    
    private int _hiddenTargets;

    private static int TotalPlayerPoints
    {
        get => PlayerPrefs.GetInt("PlayerPoints");
        set => PlayerPrefs.SetInt("PlayerPoints", value);
    }
    
    public static GameController Instance;

    private void Start()
    {
        _audioManager = ProjectContext.Services.Get<IAudioManager>();
        Instance = this;
        
        SetStartValues();
        StartGame();
    }

    private void SetStartValues()
    {
        _levelData = CrossScenesData.CurrentLevelData;
        
        targetCountText.text = _levelData.CurrentTargetsCount.ToString();
        _hiddenTargets = _levelData.CurrentTargetsCount;
        
        winPanelLevelText.text = "Level " + _levelData.CurrentLevel;
        
        levelText.transform.localScale = Vector3.zero;
        levelText.DOFade(0, 0);
        levelText.text = "Level " + _levelData.CurrentLevel;

        winPanel.gameObject.SetActive(false);
        winPanel.transform.position = new Vector3(0, 15, 0);
        
    }

    private void StartGame()
    {
        canvasGroup.alpha = 0;
        
        var sequence = DOTween.Sequence();
        
        sequence.Append(levelText.DOFade(1, ShowAnimDuration / 2f));
        sequence.Join(levelText.transform.DOScale(Vector3.one, ShowAnimDuration));

        sequence.AppendInterval(ShowAnimDuration);
        sequence.Append(levelText.DOFade(0, ShowAnimDuration / 2f));
        
        sequence.Append(canvasGroup.DOFade(1f, ShowAnimDuration));
    }

    
    public void AnimalsFound()
    {
        _hiddenTargets-= 1;
        targetCountText.text = _hiddenTargets.ToString();

        if (_hiddenTargets <= 0)
        {
            WinGame();
        }
    }
    
    private void WinGame()
    {
        canvasGroup.interactable = false;

        var sequence = DOTween.Sequence();
        sequence.AppendInterval(WinDelay);
        sequence.Append(canvasGroup.DOFade(0, ShowAnimDuration / 2f));

        sequence.AppendCallback(()=>winPanel.gameObject.SetActive(true));
        sequence.Append(winPanel.DOMove(Vector3.zero, ShowAnimDuration)).SetEase(Ease.OutBack);

        sequence.AppendCallback(() =>
                {
                    cellsHolder.gameObject.SetActive(false);
                    _audioManager.PlaySound(TypeOfSound.Win);
                }
            );
    }

    
    /*public void ResetProgress()
    {
        CurLevel = 1;
     
        CurFieldSize = StartFieldSize;
        CurAnimalCount = StartAnimalCount;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }*/
}
