using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

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
    
    [HideInInspector] public int AnimalsOnField;
    
    private const int MaxFieldSize = 9;
    private const int MaxAnimalCount = 30;
    private const int StartFieldSize = 5;
    private const int StartAnimalCount = 1;
    private const float ShowAnimDuration = 1f;
    private const float WinDelay = 5f;
    
    private Cell[,] _cells;
    
    private CanvasGroup _canvasGroup;
    
    private static int CurLevel
    {
        get 
        {
            if (!PlayerPrefs.HasKey("CurLevel"))
            {
                PlayerPrefs.SetInt("CurLevel", 1);
            }
            return PlayerPrefs.GetInt("CurLevel"); 
        }
        set => PlayerPrefs.SetInt("CurLevel", value);
    }

    private int _lvlPoints;

    private static int TotalPlayerPoints
    {
        get => PlayerPrefs.GetInt("PlayerPoints");
        set => PlayerPrefs.SetInt("PlayerPoints", value);
    }

    private static int CurFieldSize
    {
        get 
        {
            if (!PlayerPrefs.HasKey("CurFieldSize"))
            {
                PlayerPrefs.SetInt("CurFieldSize", StartFieldSize);
            }
            return PlayerPrefs.GetInt("CurFieldSize");
        }
        set => PlayerPrefs.SetInt("CurFieldSize", value);
    }

    private static int CurAnimalCount
    {
        get 
        {
            if (!PlayerPrefs.HasKey("CurAnimalCount"))
            {
                PlayerPrefs.SetInt("CurAnimalCount", StartAnimalCount);
            }
            return PlayerPrefs.GetInt("CurAnimalCount");
        }
        set => PlayerPrefs.SetInt("CurAnimalCount", value);
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
        _cells = new Cell[CurFieldSize, CurFieldSize];
        _lvlPoints = CurFieldSize * CurFieldSize;
        
        AnimalsOnField = CurAnimalCount;
        animalOnFieldText.text = AnimalsOnField.ToString();
        
        playerTotalPointsText.text = TotalPlayerPoints.ToString();
        winPanelLevelText.text = "Level " + CurLevel;
        
        levelOnSceneText.transform.localScale = Vector3.zero;
        levelOnSceneText.DOFade(0, 0);
        levelOnSceneText.text = "Level " + CurLevel;

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
        sequence.AppendCallback(PlaceAnimal);
        sequence.Append(_canvasGroup.DOFade(1f, ShowAnimDuration));
    }

    private void PlaceCells()
    {
        gridLayoutGroup.constraintCount = CurFieldSize;

        for (int x = 0; x < CurFieldSize; x++)
        {
            for (int y = 0; y < CurFieldSize; y++)
            {
                var cell = Instantiate(cellPrefab, cellsHolder);
                
                cell.name = $"Cell: {x + 1} {y + 1}";
                _cells[x, y] = cell;
                cell.X = x;
                cell.Y = y;
            }
        }
    }

    private void PlaceAnimal()
    {
        var animalsPlaced = 0;

        while(animalsPlaced < AnimalsOnField)
        {
            var randomCell = _cells[Random.Range(0, CurFieldSize), Random.Range(0, CurFieldSize)];

            if (randomCell.IsAnimal) continue;
            randomCell.IsAnimal = true;
            animalsPlaced++;
        }
    }

    public int FindAnimals(int col, int row)
    {
        var animalsCount = 0;
        for(int x = 0; x < CurFieldSize; x++)
        {
            if(_cells[x, row].IsAnimal)
            {
                animalsCount++;
            }
        }

        for (int y = 0; y < CurFieldSize; y++)
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
        AnimalsOnField -= 1;
        animalOnFieldText.text = AnimalsOnField.ToString();

        if (AnimalsOnField <= 0)
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
        CurLevel++;
        if ((CurLevel % 2) != 0)
        {
            CurFieldSize++;
        }
        TotalPlayerPoints += _lvlPoints;
        CurAnimalCount++;

        winPointsText.text = "Points: " + _lvlPoints;
        playerTotalPointsText.text = TotalPlayerPoints.ToString();

        if(CurFieldSize > MaxFieldSize)
        {
            CurFieldSize = MaxFieldSize;
        }

        if(CurAnimalCount > MaxAnimalCount)
        {
            CurAnimalCount = MaxAnimalCount;
        }
    }

    public void ResetProgress()
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
    }
}
