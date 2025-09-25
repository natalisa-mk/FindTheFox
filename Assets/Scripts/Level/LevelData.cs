namespace Level
{
    public class LevelData
    {
        public readonly CellType[,] Cells;

        public int Width => Cells.GetLength(0);
        public int Height => Cells.GetLength(1);

        public readonly int CurrentTargetsCount;
        public readonly int CurrentLevel;

        public LevelData(int currentLevel, CellType[,] cells, int currentTargetsCount)
        {
            CurrentLevel = currentLevel;
            Cells = cells;
            CurrentTargetsCount = currentTargetsCount;
        }
    }

    public enum CellType 
    {
        None,
        Empty,
        Target
    }
}