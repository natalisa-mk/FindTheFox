namespace Level
{
    public interface ILevelLoader
    {
        bool TryLoadLevel(int levelId, out LevelData levelData);
    }
}
