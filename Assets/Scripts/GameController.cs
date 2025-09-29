using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Level;

public class GameController : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Transform cellsHolder;
    [SerializeField] private Transform winPanel;
    [SerializeField] private Text winPanelLevelText;
    [SerializeField] private Text levelOnSceneText;
    [SerializeField] private Text playerTotalPointsText;
    [SerializeField] private Text winPointsText;
    [SerializeField] private Text animalOnFieldText;
    
    private LevelData _levelData;

    private const float ShowAnimDuration = 1f;
    private const float WinDelay = 5f;
    
    private int _lvlPoints;
    private int _hiddenTargets;

    private static int TotalPlayerPoints
    {
        get => PlayerPrefs.GetInt("PlayerPoints");
        set => PlayerPrefs.SetInt("PlayerPoints", value);
    }
    
    public static GameController Instance;

    private void Start()
    {
        Instance = this;
        
        SetStartValues();
        StartGame();
    }

    private void SetStartValues()
    {
        _levelData = CrossScenesData.CurrentLevelData;
        
       
        _lvlPoints = _levelData.Width * _levelData.Height;

        animalOnFieldText.text = _levelData.CurrentTargetsCount.ToString();
        _hiddenTargets = _levelData.CurrentTargetsCount;
        
        playerTotalPointsText.text = TotalPlayerPoints.ToString();
        winPanelLevelText.text = "Level " + _levelData.CurrentLevel;
        
        levelOnSceneText.transform.localScale = Vector3.zero;
        levelOnSceneText.DOFade(0, 0);
        levelOnSceneText.text = "Level " + _levelData.CurrentLevel;

        winPanel.gameObject.SetActive(false);
        winPanel.transform.position = new Vector3(0, 15, 0);
        
    }

    private void StartGame()
    {
        _canvasGroup.alpha = 0;
        
        var sequence = DOTween.Sequence();
        
        sequence.Append(levelOnSceneText.DOFade(1, ShowAnimDuration / 2f));
        sequence.Join(levelOnSceneText.transform.DOScale(Vector3.one, ShowAnimDuration));

        sequence.AppendInterval(ShowAnimDuration);
        sequence.Append(levelOnSceneText.DOFade(0, ShowAnimDuration / 2f));

        //sequence.AppendCallback(PlaceCells);
        sequence.Append(_canvasGroup.DOFade(1f, ShowAnimDuration));
    }

    

    public void AnimalsFound()
    {
        _hiddenTargets-= 1;
        animalOnFieldText.text = _hiddenTargets.ToString();

        if (_hiddenTargets <= 0)
        {
            WinGame();
        }
    }

    public void DecreasePoints()
    {
        _lvlPoints -= 1;
    }

    private void WinGame()
    {
        _canvasGroup.interactable = false;

        var sequence = DOTween.Sequence();
        sequence.AppendInterval(WinDelay);
        sequence.Append(_canvasGroup.DOFade(0, ShowAnimDuration / 2f));

        sequence.AppendCallback(()=>winPanel.gameObject.SetActive(true));
        sequence.Append(winPanel.DOMove(Vector3.zero, ShowAnimDuration)).SetEase(Ease.OutBack);

        sequence.AppendCallback(() =>
                {
                    cellsHolder.gameObject.SetActive(false);
                    AudioManager.Instance.PlaySound("WinSound");

                    CalculateWinValues();
                }
            );
    }

    private void CalculateWinValues()
    {
        //CurLevel++;
        /*if ((CurLevel % 2) != 0)
        {
            CurFieldSize++;
        }*/
        TotalPlayerPoints += _lvlPoints;
        //CurAnimalCount++;

        winPointsText.text = "Points: " + _lvlPoints;
        playerTotalPointsText.text = TotalPlayerPoints.ToString();

        /*if(CurFieldSize > MaxFieldSize)
        {
            CurFieldSize = MaxFieldSize;
        }

        if(CurAnimalCount > MaxAnimalCount)
        {
            CurAnimalCount = MaxAnimalCount;
        }*/
    }

    /*public void ResetProgress()
    {
        CurLevel = 1;
        TotalPlayerPoints = 0;
        CurFieldSize = StartFieldSize;
        CurAnimalCount = StartAnimalCount;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }*/
}
