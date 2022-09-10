using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private Cell CellPrefab;
    [SerializeField] private Transform CellsHolder;

    [SerializeField] private GameObject WinPanel;
    [SerializeField] private Text LevelText;
    [SerializeField] private Text LevelOnSceneText;
    [SerializeField] private Text PlayerTotalPointsText;
    [SerializeField] private Text WinPointsText;
    [SerializeField] private Text AnimalOnFieldText;
    
    private CanvasGroup _canvasGroup;

    private const int MaxFieldSize = 10;
    private const int MaxAnimalCount = 30;
    private int _startFieldSize = 5;
    private int _startAnimalCount = 1;

    [HideInInspector] public int AnimalsOnField;

    private Cell[,] cells;

    public int CurLevel
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

    private int lvlPoints;
    public int TotalPlayerPoints
    {
        get => PlayerPrefs.GetInt("PlayerPoints");
        set => PlayerPrefs.SetInt("PlayerPoints", value);
    }

    public int CurFieldSize
    {
        get 
        {
            if (!PlayerPrefs.HasKey("CurFieldSize"))
            {
                PlayerPrefs.SetInt("CurFieldSize", _startFieldSize);
            }
            return PlayerPrefs.GetInt("CurFieldSize");
        }
        set => PlayerPrefs.SetInt("CurFieldSize", value);
    }

    public int CurAnimalCount
    {
        get 
        {
            if (!PlayerPrefs.HasKey("CurAnimalCount"))
            {
                PlayerPrefs.SetInt("CurAnimalCount", _startAnimalCount);
            }
            return PlayerPrefs.GetInt("CurAnimalCount");
        }
        set => PlayerPrefs.SetInt("CurAnimalCount", value);
    }

    public static GameController Instance;

    private void Start()
    {
        Instance = this;

        WinPanel.SetActive(false);
        _canvasGroup = CellsHolder.gameObject.GetComponent<CanvasGroup>();

        cells = new Cell[CurFieldSize, CurFieldSize];

        lvlPoints = CurFieldSize * CurFieldSize;
        AnimalsOnField = CurAnimalCount;
        AnimalOnFieldText.text = AnimalsOnField.ToString();

        PlaceCells();
        PlaceAnimal();

        PlayerTotalPointsText.text = TotalPlayerPoints.ToString();
        LevelOnSceneText.text = "Level: " + CurLevel;
        LevelText.text = "Level " + CurLevel;
    }

    private void PlaceCells()
    {
        var offset = 0.5f;
        if (CurFieldSize % 2 != 0)
        {
            offset = 0;
        }

        for (int x = 0; x < CurFieldSize; x++)
        {
            for (int y = 0; y < CurFieldSize; y++)
            {
                var cell = Instantiate(CellPrefab, CellsHolder);

                cell.transform.position = new Vector3(x - (CurFieldSize / 2 - offset), y - (CurFieldSize / 2 - offset));
                cell.name = $"Cell: {x + 1} {y + 1}";
                cells[x, y] = cell;
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
            var randomCell = cells[Random.Range(0, CurFieldSize), Random.Range(0, CurFieldSize)];

            if (!randomCell.IsAnimal)
            {
                randomCell.IsAnimal = true;
                animalsPlaced++;
            }
        }
    }

    public int FindAnimals(int col, int row)
    {
        var animalsCount = 0;
        for(int x = 0; x < CurFieldSize; x++)
        {
            if(cells[x, row].IsAnimal)
            {
                animalsCount++;
            }
        }

        for (int y = 0; y < CurFieldSize; y++)
        {
            if (cells[col, y].IsAnimal)
            {
                animalsCount++;
            }
        }

        return animalsCount;
    }

    public void AnimalsFound()
    {
        AnimalsOnField -= 1;
        AnimalOnFieldText.text = AnimalsOnField.ToString();

        if (AnimalsOnField <= 0)
        {
            StartCoroutine(WinGame());
        }
    }

    public void CountPoints()
    {
        lvlPoints -= 1;
    }

    private IEnumerator WinGame()
    {
        _canvasGroup.interactable = false;
        yield return new WaitForSeconds(1);

        while (_canvasGroup.alpha > 0)
        {
            _canvasGroup.alpha -= Time.deltaTime;
            yield return null;
        }

        WinPanel.SetActive(true);
        float animTime = 0;
        while(animTime <= 1)
        {
            WinPanel.transform.position = Vector3.Lerp(new Vector3(0, 15, 0), Vector3.zero, animTime);
            animTime += Time.deltaTime;
            yield return null;
        }

        CellsHolder.gameObject.SetActive(false);
        AudioManager.Instance.PlaySound("WinSound");

        CurLevel++;
        TotalPlayerPoints += lvlPoints;
        CurFieldSize++;
        CurAnimalCount++;

        WinPointsText.text = "Points: " + lvlPoints;
        PlayerTotalPointsText.text = TotalPlayerPoints.ToString();

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
        CurFieldSize = _startFieldSize;
        CurAnimalCount = _startAnimalCount;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
