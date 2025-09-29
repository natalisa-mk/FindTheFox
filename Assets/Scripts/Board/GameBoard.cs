using System;
using Level;
using UnityEngine;
using UnityEngine.UI;

namespace Board
{
    public class GameBoard : MonoBehaviour
    {
        public event Action<int> OnHiddenTargetsChanged;
    
        [SerializeField] private Cell cellPrefab;
        [SerializeField] private GameObject invisibleCellPrefab;
        [SerializeField] private Transform cellsHolder;
        [SerializeField] private GridLayoutGroup gridLayoutGroup;

        private Cell[,] _cells;
    
        private LevelData _levelData;
        private int _hiddenTargets;

        private void Awake()
        {
            _levelData = CrossScenesData.CurrentLevelData;
        
            _hiddenTargets = _levelData.CurrentTargetsCount;
        
            CreateCells();
        }
    
        private void CreateCells()
        {
            gridLayoutGroup.constraintCount = _levelData.Width; 
            _cells = new Cell[_levelData.Width, _levelData.Height];
        
            for (var x = 0; x < _levelData.Width; x++)
            {
                for (var y = 0; y < _levelData.Height; y++)
                {
                    if (_levelData.Cells[x, y] == CellType.None)
                    {
                        Instantiate(invisibleCellPrefab, cellsHolder);
                    }
                    else
                    {
                        var cell = Instantiate(cellPrefab, cellsHolder);
                
                        cell.name = $"Cell: {x + 1} {y + 1}";
                        _cells[x, y] = cell;
                        cell.Init(x, y, FindTargetsInCross);
                    }
                
                }
            }
        }
    
        private void FindTargetsInCross(Cell cell)
        {
            if (CrossScenesData.CurrentLevelData.Cells[cell.X, cell.Y] == CellType.Target)
            {
                cell.DisplayFoundTarget();
                GameController.Instance.AnimalsFound();
                return;
            }
            
            var targetsCount = 0;
        
            for (var x = cell.X - 1; x >= 0; x--)
            {
                if (CanCheckTargets(x, cell.Y, ref targetsCount) == false)
                {
                    break;
                }
            }
        
            for (var x = cell.X + 1; x < _levelData.Width; x++)
            {
                if (CanCheckTargets(x, cell.Y, ref targetsCount) == false)
                {
                    break;
                }
            }
        
            for (var y = cell.Y - 1; y >= 0; y--)
            {
                if (CanCheckTargets(cell.X, y, ref targetsCount) == false)
                {
                    break;
                }
            }
        
            for (var y = cell.Y + 1; y < _levelData.Height; y++)
            {
                if (CanCheckTargets(cell.X, y, ref targetsCount) == false)
                {
                    break;
                }
            }
        
            cell.DisplayHiddenTargets(targetsCount);
            
            GameController.Instance.DecreasePoints();
        }

        private bool CanCheckTargets(int x, int y, ref int foundTargets)
        {
            if (_levelData.Cells[x, y] == CellType.None)
                return false;

            if (_levelData.Cells[x, y] == CellType.Target)
                foundTargets++;

            return true;
        }
    }
}
