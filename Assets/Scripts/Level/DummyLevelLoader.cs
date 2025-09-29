using UnityEngine;

namespace Level
{
    public class DummyLevelLoader : ILevelLoader
    {
        private int _targetsCount = 4; // Потім буде записуватися з png
        private int _fieldWidth = 5;
        private int _fieldHeight = 5;
    
        private LevelData _levelData;
    
        public bool TryLoadLevel(int levelId, out LevelData levelData)
        {
            GenerateLevelData();
        
            levelData = _levelData;
        
            return true;
        }
    
        private void GenerateLevelData()
        {
            var fieldArray = GenerateFieldArray();
            PlaceTargets(fieldArray);
            _levelData = new LevelData(1, fieldArray, _targetsCount );
            CrossScenesData.CurrentLevelData = _levelData;
        }

        private CellType[,] GenerateFieldArray()
        {
            var cells = new CellType[_fieldWidth, _fieldHeight];
        
            for (var x = 0; x < _fieldWidth; x++)
            {
                for (var y = 0; y < _fieldHeight; y++)
                {
                    cells[x, y] = CellType.Empty;
                }
            }

            cells[2, 2] = CellType.None; // Создала пустую
        
            return cells;
        }
    
        private void PlaceTargets(CellType[,] cells)
        {
            var animalsPlaced = 0;

            while(animalsPlaced < _targetsCount)
            {
                var x = Random.Range(0, _fieldWidth);
                var y = Random.Range(0, _fieldHeight);
            
                var randomCell = cells[x, y];
                //Має бути перевірка на відсутність клітини
                if (randomCell == CellType.Target)
                {
                    continue;
                }
            
                cells[x, y] = CellType.Target;
                animalsPlaced++;
            }
        }
    }
}

