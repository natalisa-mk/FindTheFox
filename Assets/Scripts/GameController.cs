using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Level;
using TMPro.EditorUtilities;

public class GameController : MonoBehaviour
{
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private Transform cellsHolder;
    [SerializeField] private GridLayoutGroup gridLayoutGroup;

    [SerializeField] private Transform winPanel;
    [SerializeField] private Text winPanelLevelText;
    [SerializeField] private Text levelOnSceneText;
    [SerializeField] private Text playerTotalPointsText;
    [SerializeField] private Text winPointsText;
    [SerializeField] private Text animalOnFieldText;
    
    private LevelData _levelData;

    private const float ShowAnimDuration = 1f;
    private const float WinDelay = 5f;
    
    private Cell[,] _cells;
    
    private CanvasGroup _canvasGroup;
    
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
        
        _canvasGroup = cellsHolder.gameObject.GetComponent<CanvasGroup>();

        SetStartValues();
        StartGame();
    }

    private void SetStartValues()
    {
        _levelData = CrossScenesData.CurrentLevelData;
        
        _cells = new Cell[_levelData.Width, _levelData.Height];
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

        sequence.AppendCallback(PlaceCells);
        sequence.Append(_canvasGroup.DOFade(1f, ShowAnimDuration));
    }

    private void PlaceCells()
    {
        gridLayoutGroup.constraintCount = _levelData.Width; // Не впевнена, що тут має бути ширина

        for (int x = 0; x < _levelData.Width; x++)
        {
            for (int y = 0; y < _levelData.Height; y++)
            {
                //Має бути перевірка на відсутність клітини
                
                var cell = Instantiate(cellPrefab, cellsHolder);
                
                cell.name = $"Cell: {x + 1} {y + 1}";
                _cells[x, y] = cell;
                cell.X = x;
                cell.Y = y;

                if (_levelData.Cells[x, y] == CellType.Target)
                {
                    cell.IsAnimal = true;
                }
            }
        }
    }
    

    public int FindAnimals(int col, int row)
    {
        var animalsCount = 0;
        for(int x = 0; x < _levelData.Width; x++)
        {
            if(_cells[x, row].IsAnimal)
            {
                animalsCount++;
            }
        }

        for (int y = 0; y < _levelData.Height; y++)
        {
            if (_cells[col, y].IsAnimal)
            {
                animalsCount++;
            }
        }

        return animalsCount;
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

    public void CountPoints()
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
